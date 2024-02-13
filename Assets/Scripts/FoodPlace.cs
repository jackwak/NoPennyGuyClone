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


        // player�n position�n� new player posa e�itle animle

        other.transform.DOMove(NewFoodTransform.position, 0.2f).OnComplete(() =>
        {
            // e�er varsa player�n animini oynat (animatora siti ekle)
            if (PlayerAnimation != null)
            {
                Animator animator = other.GetComponent<Animator>();
                animator.Play(PlayerAnimation.name);
                float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

                DOVirtual.DelayedCall(animationDuration + 0.2f, () =>
                {
                    KeepGoingAfterPlayerAnim();
                });
            }
            KeepGoingAfterPlayerAnim();
        });
    }

    void KeepGoingAfterPlayerAnim()
    {

        // foodGO pos unu karakterin elinin posa ekle animle
        FoodGO.transform.position = NewFoodTransform.position;


        // yukar�daki anim bittikten sonra yemek yeme animi oynat


        // yemek yeme animi bittikten sonra foodGO destroyla foodGO null la (emin de�ilim)


        // player�n pos old player posa e�itle


        // security ve waiter� setle
    }
}
