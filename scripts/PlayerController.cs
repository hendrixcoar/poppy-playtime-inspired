using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float groundDrag = 5f;
    
    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airDrag = 2f;
    private bool readyToJump = true;
    
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    
    private float speedToUse;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found!");
        }
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        GetInput();
        ApplyDrag();
        LimitVelocity();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
        {
            Jump();
        }
        
        speedToUse = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * speedToUse * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * speedToUse * 10f * 0.4f, ForceMode.Force);
    }

    private void Jump()
    {
        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void ApplyDrag()
    {
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void LimitVelocity()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (flatVel.magnitude > speedToUse)
        {
            Vector3 limitedVel = flatVel.normalized * speedToUse;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
