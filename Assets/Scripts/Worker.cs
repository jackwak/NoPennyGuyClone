using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Worker : MonoBehaviour
{
    private MeshCollider _meshCollider;

    private void Start()
    {
        _meshCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Security.PlayerCatched();
        }
    }

}
