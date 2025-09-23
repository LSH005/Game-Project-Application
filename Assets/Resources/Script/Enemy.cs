using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2;
    private Transform player;
    private float health = 5;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    public void AddDamage(float value)
    {
        health -= value;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
