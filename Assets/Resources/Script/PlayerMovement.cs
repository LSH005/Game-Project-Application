using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;

    private CharacterController chrCont;
    private Vector3 velocity;
    public bool isGrounded = true;

    void Awake()
    {
        chrCont = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = chrCont.isGrounded;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);
        chrCont.Move(move * moveSpeed * Time.deltaTime);

        if (isGrounded)
        {
            velocity.y = 0f;
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = jumpPower;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        chrCont.Move(velocity*Time.deltaTime);
    }
}
