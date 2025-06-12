using System.Collections;
using UnityEngine;

public enum SIDE { Left, Mid, Right }

public class PlayerController : MonoBehaviour
{
    public Transform coinTargetPoint;
    public static PlayerController Instance;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float laneDistance = 2.5f;

    [Header("Jump & Roll")]
    [SerializeField] private float jumpForce = 10f;
    private bool isGrounded = false;

    private Rigidbody rb;
    private Animator ani;

    private SIDE currentSide = SIDE.Mid;
    private float targetX = 0f;

    // Swipe detection
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool swipeDetected = false;
    private float swipeThreshold = 50f;

    private bool isKnockedBack = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
        transform.position = Vector3.zero;
    }

    void Update()
    {
        if (isKnockedBack) return;

#if UNITY_EDITOR
        HandleKeyboardInput(); // Hỗ trợ test bằng bàn phím trong Editor
#endif
        HandleSwipe();
    }

    void FixedUpdate()
    {
        if (isKnockedBack) return;

        Vector3 currentPosition = rb.position;
        Vector3 forwardMove = transform.forward * moveSpeed * Time.fixedDeltaTime;

        Vector3 targetPosition = new Vector3(targetX, currentPosition.y, currentPosition.z);
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition + forwardMove);
    }

    // ==========================
    //          INPUT
    // ==========================

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) SwipeLeft();
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) SwipeRight();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
    }

    void HandleSwipe()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                swipeDetected = true;
            }
            else if (touch.phase == TouchPhase.Ended && swipeDetected)
            {
                endTouchPosition = touch.position;
                Vector2 swipeDelta = endTouchPosition - startTouchPosition;

                if (swipeDelta.magnitude > swipeThreshold)
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        if (swipeDelta.x < 0) SwipeLeft();
                        else SwipeRight();
                    }
                    else
                    {
                        if (swipeDelta.y > 0 && isGrounded) Jump();
                       
                    }
                }

                swipeDetected = false;
            }
        }
    }

    // ==========================
    //        MOVEMENT
    // ==========================

    void SwipeLeft()
    {
        if (currentSide == SIDE.Mid)
        {
            currentSide = SIDE.Left;
            targetX = -laneDistance;
        }
        else if (currentSide == SIDE.Right)
        {
            currentSide = SIDE.Mid;
            targetX = 0f;
        }
    }

    void SwipeRight()
    {
        if (currentSide == SIDE.Mid)
        {
            currentSide = SIDE.Right;
            targetX = laneDistance;
        }
        else if (currentSide == SIDE.Left)
        {
            currentSide = SIDE.Mid;
            targetX = 0f;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        ani?.SetTrigger("Jump");
    }
    void Knockback()
    {
        Vector3 knockDirection = -transform.forward + Vector3.up * 0.5f;
        rb.AddForce(knockDirection.normalized * 700f);
        ani?.SetBool("Fall", true);
        moveSpeed = 0f;
        isKnockedBack = true;
       
    }

    // ==========================
    //        COLLISION
    // ==========================

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Trap"))
    //    {
    //        Knockback();
    //        StartCoroutine(GameOverAfterDelay(2f));
    //    }
    //}

    //private IEnumerator GameOverAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    GameManager.Instance.GameOver();
    //    Time.timeScale = 0;
    //}
}
