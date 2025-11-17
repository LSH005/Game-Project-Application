using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    // 채집 가능 거리
    public float rayDistance = 5f;
    // 가능한 레이어 전부 다
    public LayerMask hitMask = ~0;
    // 타격 데미지
    public int toolDamage = 1;

    // 연타 간격
    public float hitCooldown = 0.15f;
    private float _nextHitTime;

    private Camera _cam;
    public Inventory inventory; // 플레이어 인벤토리 (없으면 자동 부착)

    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
        {
            _nextHitTime = Time.time + hitCooldown;

            Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 화면 중앙
            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
            {
                var block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    block.Hit(toolDamage, inventory);
                }
            }
        }
    }
}