using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] projectilePrefab;
    public Transform firePoint;

    private int weaponNumber = 0;
    private int weaponCount;

    private float repeaterCooldown= 0.05f;
    private float lastShootTime;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        weaponCount = projectilePrefab.Length;
        lastShootTime = Time.time;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (weaponNumber == 2)
            {
                if (Time.time - lastShootTime >= repeaterCooldown)
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            WeaponChange();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab[weaponNumber], firePoint.position, Quaternion.LookRotation(direction));
        lastShootTime = Time.time;
    }

    void WeaponChange()
    {
        weaponNumber++;

        if (weaponNumber == weaponCount)
        {
            weaponNumber = 0;
        }
    }
}
