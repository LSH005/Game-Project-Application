using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 3f;

    float xRotation = 0;
    CharacterController characCont;
    Transform cam;
    Vector3 velocity;
    bool isGrounded;

    void Awake()
    {
        characCont = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        UpdateState();
        HandleMove();
        HandleLook();
    }

    void UpdateState()
    {
        isGrounded = characCont.isGrounded;
    }

    void HandleMove()
    {
        isGrounded = characCont.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        characCont.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        characCont.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (cam != null)
        {
            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
