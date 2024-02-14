using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodPlace : MonoBehaviour
{
    [HideInInspector]public GameObject FoodGO;

    public Transform NewFoodTransform;
    public Transform NewPlayerPosition;
    [HideInInspector] public Vector3 OldPlayerPosition;
    private Material _rangeMaterial;

    public bool HasPlayerAnimation;

    private void Start()
    {
        FoodGO = transform.GetChild(transform.childCount - 1)?.gameObject;

        _rangeMaterial = (Material)Resources.Load("Range", typeof(Material));

        Color color = Color.white;
        color.a = 0.3f;

        _rangeMaterial.color = color;

        
    }

    public void GiveTheFoodToCustomer(Collider other)
    {
        FoodTaker foodTaker = other.GetComponent<FoodTaker>();

        if (foodTaker.IsFoodOnHand && FoodGO == null)
        {
            FoodGO = foodTaker.WaiterFoodPosition.GetChild(0).gameObject;

            FoodGO.transform.SetParent(gameObject.transform);
            FoodGO.transform.localPosition = Vector3.zero;

            foodTaker.IsFoodOnHand = false;
        }
    }

    public void EatTheFood(Collider other)
    {
        if (FoodGO == null) return;

        OldPlayerPosition = other.transform.position;


        CapsuleCollider capsuleCollider = other.gameObject.GetComponent<CapsuleCollider>();

        capsuleCollider.isTrigger = true;



        // player�n position�n� new player posa e�itle animle

        other.transform.DOMove(NewPlayerPosition.position, 2f);

        // e�er varsa player�n animini oynat (animatora siti ekle)
        if (HasPlayerAnimation)
        {
            Animator animator = other.GetComponent<Animator>();
            animator.SetBool("IsSitting", true);
        }

        // karakterin y�z�n� oturdu�u yerden yeme�e do�ru �evir (new player pos la FoodGO nin pos u aras�nda bir vector olu�tur ve karakterin y�z�n� o vekt�r yap)

        //var lookDirection = FoodGO.transform.position - NewFoodTransform.position;

        //transform.rotation = Quaternion.LookRotation(lookDirection);

        //other.transform.DOLookAt(new Vector3(FoodGO.transform.position.x, NewPlayerPosition.position.y, FoodGO.transform.position.z), 2f);


        // foodGO pos unu karakterin elinin posa ekle animle
        FoodGO.transform.DOMove(NewFoodTransform.position, 2f).SetDelay(2f).OnComplete(() =>
        {
            // yukar�daki anim bittikten sonra yemek yeme animi oynat
            // �YLE B� AN�M YOK
            DOVirtual.DelayedCall(1f, () =>
            {

                // yemek yeme animi bittikten sonra foodGO destroyla foodGO null la (emin de�ilim)
                Destroy(FoodGO);
                FoodGO = null;

                // player�n pos old player posa e�itle
                other.transform.DOMove(OldPlayerPosition, 1f);
                other.GetComponent<Animator>().SetBool("IsSitting", false);

                // security ve waiter� setle
                Color color = Color.red;
                color.a = 0.3f;

                _rangeMaterial.color = color;

                // capsule collider� a�
                capsuleCollider.isTrigger = false;

            });

        });

    }
}
