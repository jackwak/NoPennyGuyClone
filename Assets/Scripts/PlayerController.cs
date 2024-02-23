using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
        hor = Input.GetAxis("Horizontal") * Time.deltaTime;
        vert = Input.GetAxis("Vertical") * Time.deltaTime;

        _moveDirection = new Vector3(hor, 0, vert);
        _moveDirection.Normalize();

        rb.MovePosition(transform.position + _moveDirection * _speed * Time.deltaTime);

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

    public void CharacterRotate(Vector3 rotateVector)
    {
        transform.eulerAngles = rotateVector;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            if (LevelManager.Instance.IsTasksCompleted())
            {
                StopAllCoroutines();

                Time.timeScale = 0f;

                // open panel. you escaped. next level or go to street button
                SaveManager.Instance.LastOpenedHouseIndex++;


                LevelManager.Instance.OpenEscapePanel();

                SaveManager.Instance.SaveMoney();
                LevelManager.Instance.ClearVariables();
            }


        }
    }
}
