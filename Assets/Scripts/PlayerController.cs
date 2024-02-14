using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    private float hor;
    private float vert;

    private Vector3 _moveDirection;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(hor, 0, vert);
        _moveDirection.Normalize();

        rb.MovePosition(transform.position + _moveDirection * _speed * Time.deltaTime);
        //transform.Translate(_moveDirection * _speed * Time.deltaTime, Space.World);

        if (_moveDirection != Vector3.zero)
        {
            animator.SetBool("IsRunning", true);

            Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotSpeed);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }
}
