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

    private State state;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
                hor = Input.GetAxis("Horizontal");
                vert = Input.GetAxis("Vertical");

                _moveDirection = new Vector3(hor * Time.deltaTime, 0, vert * Time.deltaTime);
                _moveDirection.Normalize();


                if (_moveDirection != Vector3.zero)
                {
                    animator.SetBool("IsRunning", true);

                    Quaternion toRotation = Quaternion.LookRotation(_moveDirection * Time.deltaTime, Vector3.up);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotSpeed * Time.deltaTime);
                }
                else
                {
                    animator.SetBool("IsRunning", false);
                }
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + _moveDirection * _speed);

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


                LevelManager.Instance.OpenEscapePanel();

                SaveManager.Instance.SaveMoney();
                LevelManager.Instance.ClearVariables();

                LevelManager.Instance.IncreaseLastOpenedLevelIndex();
            }
        }

        if (collision.collider.gameObject.CompareTag("Range"))
        {
            state = State.CATCH;
            animator.SetBool("IsRunning", false);
            Worker.PlayerCatched(this.transform);
        }
    }

    public enum State
    {
        PATROL,
        CATCH
    }
}
