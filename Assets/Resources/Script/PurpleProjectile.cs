using UnityEngine;

public class PurpleProjectile : MonoBehaviour
{
    public float speed = 20;
    public float lifeTime = 2;

    private Transform firstChildTransform;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        firstChildTransform = transform.GetChild(0);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        firstChildTransform.rotation = Quaternion.LookRotation(randomDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyLove"))
        {
            Destroy(other.gameObject);
        }
    }
}
