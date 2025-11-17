using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }
    public InventorySlot[] slots;

    [Header("공기")]
    public Texture2D airImage;
    [Header("블록")]
    public Texture2D grassBlock;
    public Texture2D dirtBlock;
    public Texture2D coalBlock;
    public Texture2D goldBlock;
    public Texture2D waterBlock;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(BlockType blockType, int itemCount)
    {
        InventorySlot slot = FindSlot(blockType);

        if (slot != null)
        {
            if (slot.isEmptySlot)
            {
                slot.SetBlockType(blockType);
                slot.AddItemCount(itemCount);
                slot.SetIcon(GetImageFromBlockType(blockType));
            }
            else
            {
                slot.AddItemCount(itemCount);
            }
        }
    }
    
    InventorySlot FindSlot(BlockType type)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];

            if (slot.isEmptySlot) continue;

            if (slot.currentBlockType == type)
            {
                return slot;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];

            if (slot.isEmptySlot)
            {
                return slot;
            }
        }

        return null;
    }

    Texture2D GetImageFromBlockType(BlockType type)
    {
        switch (type)
        {
            case BlockType.Grass:
                return grassBlock;
            case BlockType.Dirt:
                return dirtBlock;
            case BlockType.Water:
                return waterBlock;
            case BlockType.Coal:
                return coalBlock;
            case BlockType.Gold:
                return goldBlock;
        }

        return airImage;
    }
}
