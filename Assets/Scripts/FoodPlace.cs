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



        // playerýn positionýný new player posa eþitle animle

        other.transform.DOMove(NewPlayerPosition.position, 2f);

        // eðer varsa playerýn animini oynat (animatora siti ekle)
        if (HasPlayerAnimation)
        {
            Animator animator = other.GetComponent<Animator>();
            animator.SetBool("IsSitting", true);
        }

        // karakterin yüzünü oturduðu yerden yemeðe doðru çevir (new player pos la FoodGO nin pos u arasýnda bir vector oluþtur ve karakterin yüzünü o vektör yap)

        //var lookDirection = FoodGO.transform.position - NewFoodTransform.position;

        //transform.rotation = Quaternion.LookRotation(lookDirection);

        //other.transform.DOLookAt(new Vector3(FoodGO.transform.position.x, NewPlayerPosition.position.y, FoodGO.transform.position.z), 2f);


        // foodGO pos unu karakterin elinin posa ekle animle
        FoodGO.transform.DOMove(NewFoodTransform.position, 2f).SetDelay(2f).OnComplete(() =>
        {
            // yukarýdaki anim bittikten sonra yemek yeme animi oynat
            // ÖYLE BÝ ANÝM YOK
            DOVirtual.DelayedCall(1f, () =>
            {

                // yemek yeme animi bittikten sonra foodGO destroyla foodGO null la (emin deðilim)
                Destroy(FoodGO);
                FoodGO = null;

                // playerýn pos old player posa eþitle
                other.transform.DOMove(OldPlayerPosition, 1f);
                other.GetComponent<Animator>().SetBool("IsSitting", false);

                // security ve waiterý setle
                Color color = Color.red;
                color.a = 0.3f;

                _rangeMaterial.color = color;

                // capsule colliderý aç
                capsuleCollider.isTrigger = false;

            });

        });

    }
}
