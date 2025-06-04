
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    private float horizontalInput;
    [SerializeField] float jumpForce = 10f;
    private bool isGrounded = false;
  
    Animator ani;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        Vector3 forwordMove = transform.forward * moveSpeed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + horizontalMove + forwordMove);
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    void Jump()
    {
  
        rb.AddForce(Vector3.up * jumpForce);
        ani.SetTrigger("Jump");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            Knockback();  // Gọi hàm hất ngược
        }
    }
    private void Knockback()
    {
        // Hướng ngược với hướng di chuyển
        Vector3 knockDirection = -transform.forward + Vector3.up * 0.5f;
        rb.AddForce(knockDirection.normalized * 700f); 
        ani.SetBool("Fall", true);
        moveSpeed = 0f;
        
        

    }

}
