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

    private int selectedIndex = -1;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        for (int i = 0; i < 7; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }

        if (Input.GetKey(KeyCode.Keypad1))
        {
            AddItem(BlockType.Grass, 5);
        }
    }

    void SetSelectedIndex(int index)
    {
        if (index != selectedIndex)
        {
            selectedIndex = index;

            foreach (InventorySlot slot in slots)
            {
                slot.SetColor(Color.gray);
            }

            slots[index].SetColor(Color.red);
        }
        else
        {
            ClearSelect();
        }
    }

    public InventorySlot GetSelectedInventorySlot()
    {
        if (selectedIndex >= 0) return slots[selectedIndex];
        else return null;
    }

    void ClearSelect()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.SetColor(Color.gray);
        }

        selectedIndex = -1;
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
