using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class House : MonoBehaviour
{
    [SerializeField] private int _houseIndex;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private int _levelCount = 5;


    public int GetHouseIndex { get => _houseIndex; }


    private void Start()
    {
        camera = transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    }

    public void IncreaseCameraPriority()
    {
        camera.Priority++;
    }

    public void DecreaseCameraPriority()
    {
        camera.Priority--;
    }
}
