using System.Collections;
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

    [Header("미리보기 칸")]
    public GameObject previewBlock;

    [Header("미리보기 재질")]
    public float previewBlockAlpha;
    public Material grassBlock;
    public Material dirtBlock;
    public Material goldBlock;
    public Material coalBlock;
    public Material waterBlock;

    private Renderer previewRenderer;

    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();

        previewRenderer = previewBlock.GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
        {
            Harvest();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Place();
        }

        PreviewHandler();
    }

    void Harvest()
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

    void Place()
    {
        InventorySlot slot = InventoryManager.instance.GetSelectedInventorySlot();
        if (slot == null) return;
        if (slot.isEmptySlot) return;

        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 화면 중앙
        if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
        {
            var block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                Vector3 blockPos = AdjacentCellOnHitFace(hit);


                PerlinNoise.instance.SetBlockVector3(blockPos, slot.currentBlockType);
                slot.AddItemCount(-1);
            }
        }
    }

    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position; // 맞춘 블록의 중심(정수 좌표(x,y,z)
        Vector3 adjCenter = baseCenter + hit.normal; // 그 면의 바깥쪽으로 정확히 한 칸 이동
        return Vector3Int.RoundToInt(adjCenter);
    }

    void PreviewHandler()
    {
        InventorySlot slot = InventoryManager.instance.GetSelectedInventorySlot();
        if (slot == null)
        {
            TogglePreviewBlock(false);
            return;
        }
        if (slot.isEmptySlot)
        {
            TogglePreviewBlock(false);
            return;
        }

        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
        {
            var block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                Vector3 blockPos = AdjacentCellOnHitFace(hit);

                previewBlock.transform.position = blockPos;
                SetPriviewBlockMaterial(slot);
                TogglePreviewBlock(true);
            }
        }
        else
        {
            TogglePreviewBlock(false);
        }
    }

    void TogglePreviewBlock(bool enable)
    {
        previewBlock.transform.localScale = enable ? Vector3.one : Vector3.zero;
    }

    void SetPriviewBlockMaterial(InventorySlot slot)
    {
        Material targetMat = null;

        switch (slot.currentBlockType)
        {
            case BlockType.Grass:
                targetMat = grassBlock;
                break;
            case BlockType.Dirt:
                targetMat = dirtBlock;
                break;
            case BlockType.Water:
                targetMat = waterBlock;
                break;
            case BlockType.Coal:
                targetMat = coalBlock;
                break;
            case BlockType.Gold:
                targetMat = goldBlock;
                break;
        }

        previewRenderer.material = targetMat;
    }
}