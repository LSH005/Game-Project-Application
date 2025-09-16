using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 15;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;
    public GameObject runEffect;

    public CinemachineVirtualCamera virtualCam;

    private CharacterController chrCont;
    private Vector3 velocity;
    private bool isGrounded = true;
    private bool isPressShift = false;
    private bool wasPressShift = false;

    private CinemachinePOV pov;
    private CinemachineSwitcher switcher;


    void Awake()
    {
        chrCont = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
        switcher = GetComponent<CinemachineSwitcher>();
    }

    void Update()
    {
        isGrounded = chrCont.isGrounded;
        float x = 0;
        float z = 0;

        if (!switcher.usingFreeLook)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        isPressShift = Input.GetKey(KeyCode.LeftShift);

        if (isGrounded)
        {
            velocity.y = 0f;
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = jumpPower;
            }

            if (isPressShift)
            {
                Vector3 randomInstantiatePos = Random.insideUnitSphere;
                randomInstantiatePos.y = 0;

                GameObject newEffect = Instantiate(runEffect, transform.position + (randomInstantiatePos * (GetRandomFloatValue(true) * 3)), Quaternion.identity);
                float randomScale = GetRandomFloatValue(true);
                newEffect.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                Rigidbody rb = newEffect.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Vector3 randomDirection = Random.insideUnitSphere;
                    randomDirection.y = Mathf.Abs(randomDirection.y) * 2;

                    rb.AddForce(randomDirection * GetRandomFloatValue(true), ForceMode.Impulse);

                    Vector3 randomTorqueDirection = Random.insideUnitSphere;

                    // 토크를 가해 오브젝트 회전시키기
                    rb.AddTorque(randomTorqueDirection * GetRandomFloatValue(false) * 10, ForceMode.Impulse);
                }
                else Debug.LogError("없?");

                Destroy(newEffect, GetRandomFloatValue(true) * 5f);
            }
        }

        Vector3 camForward = virtualCam.transform.forward;
        Vector3 camRight = virtualCam.transform.right;
        // virtualCam.transform 은 CinemachineVirtualCamera 의 "LookAT" 인스턴스 (Transform 데이터타입)에 기반함
        camForward.y = camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * z + camRight * x).normalized;
        chrCont.Move(move * (isPressShift ? runSpeed : moveSpeed) * Time.deltaTime);

        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);


        chrCont.Move(velocity * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;


        if (isPressShift != wasPressShift)
        {
            if (isPressShift)
            {
                switcher.SetFov(80, 0.05f);
            }
            else
            {
                switcher.SetFov(60, 0.2f);
            }

            wasPressShift = isPressShift;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    float GetRandomFloatValue(bool isABS)
    {
        return isABS ? Random.Range(0.01f, 0.1f) : Random.Range(-0.1f, 0.1f);
    }
}
