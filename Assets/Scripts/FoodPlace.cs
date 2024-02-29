using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class FoodPlace : MonoBehaviour
{
    public GameObject FoodGO;

    public Transform NewFoodTransform;
    public Transform NewPlayerPosition;
     public Vector3 OldPlayerPosition;
    private Material _rangeMaterial;
    [SerializeField] private Vector3 playerRotation;

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
            FoodGO.transform.DOLocalMove(Vector3.zero, 0.5f);

            foodTaker.IsFoodOnHand = false;
        }
    }

    public void EatTheFood(Collider other)
    {
        if (FoodGO == null) return;

        LevelManager.Instance.TickTheFoodOnUI(FoodGO.name);

        OldPlayerPosition = other.transform.position;

        

        CapsuleCollider capsuleCollider = other.gameObject.GetComponent<CapsuleCollider>();
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        Animator animator = other.GetComponent<Animator>();

        animator.SetBool("IsRunning", false);
        playerController.enabled = false;
        capsuleCollider.isTrigger = true;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        // player�n position�n� new player posa e�itle animle
        sequence.Append(other.transform.DOMove(NewPlayerPosition.position, .5f));

        // e�er varsa player�n animini oynat
        if (HasPlayerAnimation)
        {
            animator.SetBool("IsSitting", true);
        }

        // karakterin y�z�n� oturdu�u yerden yeme�e do�ru �evir (new player pos la FoodGO nin pos u aras�nda bir vector olu�tur ve karakterin y�z�n� o vekt�r yap)
        other.transform.DORotate(playerRotation, .5f).OnComplete(() =>
        {
            //customer cry animation set
            //transform.Find("Customer").GetComponent<Animator>().SetTrigger("Cry");
        });        


        // foodGO pos unu karakterin elinin posa ekle animle
        sequence.AppendCallback(()=>FoodGO.transform.DOMove(NewFoodTransform.position, 1f).OnComplete(() =>
        {
            // yukar�daki anim bittikten sonra yemek yeme animi oynat
            FoodGO.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            DOVirtual.DelayedCall(2f, () =>
            {
                CollectMoney();

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

                //disenable to convex range meshes for when range collider is convex it collider is being rectangle 
                LevelManager.Instance.DisenableToConvexRangeMeshes();

                // capsule collider� a�
                capsuleCollider.isTrigger = false;
                playerController.enabled = true;


            });

        }));



    }


    public void CollectMoney()
    {
        string foodName = FoodGO.name;
        foodName = foodName.Replace("(Clone)", "");

        Food food;
        food = Resources.Load<Food>(foodName);

        int miniMoneyCount = food.Price / 10;

        LevelManager.Instance.MoneyAnimation(miniMoneyCount);

    }
}
