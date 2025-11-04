using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState {Idle, Trace, Attack, RUUUUUUUNNNNNNNNN };
    public EnemyState state = EnemyState.Idle;
    public float MaxHealth = 5;

    public GameObject projectilePrefabs;
    public Transform firePoint;
    public GameObject HealthUi;

    public float moveSpeed = 2;
    public float traceRange = 15f;
    public float attackRange = 7f;
    public float attackCooldown = 1.75f;

    public float normalizedHP;

    private Transform player;
    private float lastShootTime;
    private float DangerHP;
    private float health;

    private bool isAtRiskEnoughToReceiveMoreLivelihoodSupportFunds;  // 내가 민생지원금을 더 받을 정도로 위험한가?

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Destroy(gameObject);
        }

        health = MaxHealth;
        DangerHP = MaxHealth * 0.2f;
    }

    private void Start()
    {
        GameObject myUI = Instantiate(HealthUi);
        EnemyHealthUi myUiScript = myUI.GetComponent<EnemyHealthUi>();
        myUiScript.enemyScript = this;
        myUiScript.targetObj = this.gameObject;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        
        isAtRiskEnoughToReceiveMoreLivelihoodSupportFunds = health <= DangerHP;

        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                {
                    if (!isAtRiskEnoughToReceiveMoreLivelihoodSupportFunds) // 지금 상태로는 민생지원금을 10만원 받는가?
                    {
                        state = EnemyState.Trace;
                    }
                    else    // 아님 말고
                    {
                        state = EnemyState.RUUUUUUUNNNNNNNNN;
                    }
                }

                break;
            case EnemyState.Trace:
                if (dist < attackRange)
                {
                    state = EnemyState.Attack;
                }
                else if (dist > traceRange)
                {
                    state = EnemyState.Idle;
                }
                else
                {
                    TracePlayer();
                }

                if (isAtRiskEnoughToReceiveMoreLivelihoodSupportFunds)
                {
                    state = EnemyState.RUUUUUUUNNNNNNNNN;
                }

                break;
            case EnemyState.Attack:
                
                if (dist > attackRange)
                {
                    state = EnemyState.Trace;
                }
                else
                {
                    AttackPlayer();
                }

                if (isAtRiskEnoughToReceiveMoreLivelihoodSupportFunds)
                {
                    state = EnemyState.RUUUUUUUNNNNNNNNN;
                }


                break;

            case EnemyState.RUUUUUUUNNNNNNNNN:
                if (dist < traceRange)
                {
                    RunAway();
                }
                else // 민생지원금 감소
                {
                    state = EnemyState.Idle;
                }

                break;

        }

        normalizedHP = health / MaxHealth;
    }

    void TracePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    void RunAway()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + transform.position - player.position);
    }

    void AttackPlayer()
    {
        if (Time.time >= lastShootTime + attackCooldown)
        {
            lastShootTime = Time.time;
            ShootPorjectile();
        }
    }

    void ShootPorjectile()
    {
        if (projectilePrefabs == null || firePoint == null) return;

        transform.LookAt(player.position);
        GameObject Proj = Instantiate(projectilePrefabs, firePoint.position, firePoint.rotation);
        EnemyProjectile ep = Proj.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            Vector3 dir = (player.position - firePoint.position).normalized;
            ep.SetDirection(dir);
        }
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
