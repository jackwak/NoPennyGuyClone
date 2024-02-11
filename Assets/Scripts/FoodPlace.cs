using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodPlace : MonoBehaviour
{
    [HideInInspector]public GameObject FoodGO;

    public Transform NewFoodTransform;
    public Transform NewPlayerPosition;
    [HideInInspector] public Transform OldPlayerPosition;

    public Animation PlayerAnimation;

    private void Start()
    {
        FoodGO = transform.GetChild(0)?.gameObject;
    }

    public void GiveTheFoodToCustomer(Collider other)
    {
        FoodTaker foodTaker = other.GetComponent<FoodTaker>();

        if (foodTaker.IsFoodOnHand && FoodGO == null)
        {
            FoodGO = foodTaker.WaiterFoodPosition.GetChild(0).gameObject;

            FoodGO.transform.SetParent(gameObject.transform);
            FoodGO.transform.localPosition = Vector3.zero;
        }
    }

    public void EatTheFood(Collider other)
    {
        if (FoodGO == null) return;

        OldPlayerPosition.position = other.transform.position;


        // playerýn positionýný new player posa eþitle animle


        // eðer varsa playerýn animini oynat (animatora siti ekle)
        if (PlayerAnimation != null)
        {
            other.GetComponent<Animator>().Play(PlayerAnimation.name);
        }


        // foodGO pos unu karakterin elinin posa ekle animle
        FoodGO.transform.position = NewFoodTransform.position;


        // yukarýdaki anim bittikten sonra yemek yeme animi oynat


        // yemek yeme animi bittikten sonra foodGO destroyla foodGO null la (emin deðilim)


        // playerýn pos old player posa eþitle


        // security ve waiterý setle

    }


}
