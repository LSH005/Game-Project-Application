using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20;
    public float lifeTime = 2;
    public float damage = 1;
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyLove"))
        {
            Enemy damageScript = other.GetComponent<Enemy>();
            damageScript.AddDamage(damage);
            Destroy(gameObject);
        }
    }
}
