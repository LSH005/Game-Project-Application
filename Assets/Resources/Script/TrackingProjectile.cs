using UnityEngine;

public class TrackingProjectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float trackingIntensity = 5f;
    public float lifeTime = 4;

    private Transform target;
    private Vector3 targetPos;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        transform.rotation = Quaternion.LookRotation(randomDirection);

        FindClosestMyLove();

        if (target == null)
        {
            Destroy(gameObject);
            this.enabled = false;
        }
    }

    void Update()
    {
        if (target != null) targetPos = target.position;

        Vector3 direction = targetPos - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingIntensity * Time.deltaTime);

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(targetPos, transform.position) <= (trackingIntensity * Time.deltaTime * 2))
        {
            Destroy(gameObject);
        }
    }

    void FindClosestMyLove()
    {
        GameObject[] myLoves = GameObject.FindGameObjectsWithTag("MyLove");
        float closestDistance = Mathf.Infinity;
        GameObject closestMyLove = null;

        foreach (GameObject myLove in myLoves)
        {
            float distance = Vector3.Distance(transform.position, myLove.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMyLove = myLove;
            }
        }

        if (closestMyLove != null)
        {
            target = closestMyLove.transform;
            targetPos = target.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyLove"))
        {
            Enemy damageScript = other.GetComponent<Enemy>();
            damageScript.AddDamage(1f);
            Destroy(gameObject);
        }
    }
}
