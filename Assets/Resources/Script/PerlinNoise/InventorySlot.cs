using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public RawImage itemIcon;
    public TextMeshProUGUI itemAmount;
    public BlockType currentBlockType;
    public bool isEmptySlot = true;

    int count = 0;

    void Start()
    {
        itemIcon.texture = InventoryManager.instance.airImage;
        ClearSlot();
        itemAmount.gameObject.SetActive(false);
    }

    public void SetBlockType(BlockType type)
    {
        currentBlockType = type;
        
        if (type == BlockType.Air)
        {
            itemIcon.texture = InventoryManager.instance.airImage;
        }
    }

    public void AddItemCount(int amount)
    {
        count += amount;
        if (count <= 0)
        {
            ClearSlot();
            return;
        }
        else
        {
            itemAmount.gameObject.SetActive(true);
            isEmptySlot = false;
        }
            

        itemAmount.text = count.ToString();
    }

    public void SetItemCount(int number)
    {
        if (number <= 0)
        {
            ClearSlot();
            return;
        }
        else
        {
            itemAmount.gameObject.SetActive(true);
            isEmptySlot = false;
        }

        count = number;
        itemAmount.text = count.ToString();
    }
    

    public void ClearSlot()
    {
        count = 0;
        itemAmount.gameObject.SetActive(false);
        SetBlockType(BlockType.Air);
        isEmptySlot = true;
    }

    public void SetIcon(Texture2D icon)
    {
        itemIcon.texture = icon;
    }
}
