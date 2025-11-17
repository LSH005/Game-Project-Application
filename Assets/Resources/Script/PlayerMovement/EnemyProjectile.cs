using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : MonoBehaviour
{
    public float lifeTime = 3;
    public float speed = 8;
    public float damage = 1;

    Vector3 movedir;


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector3 dir)
    {
        movedir = dir.normalized;
    }

    void Update()
    {
        transform.position += movedir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement damageScript = other.GetComponent<PlayerMovement>();
            //damageScript.AddDamage(damage);
            Destroy(gameObject);
        }
    }
}
