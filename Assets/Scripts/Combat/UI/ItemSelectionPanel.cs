using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionPanel : MonoBehaviour {

    public InventorySlot[] slots;

    void OnEnable()
    {
        slots = this.gameObject.GetComponentsInChildren<InventorySlot>();
    }

    public void SetButtons(List<Spell> spells, CanvasController canvas)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i >= spells.Count)
            {
                slots[i].DisableSlot();
            }
            else
            {
                slots[i].EnableSlot(spells[i], canvas);
            }
        }
        canvas.eventsystem.SetSelectedGameObject(slots[0].GetComponentInChildren<Button>().gameObject);
    }

    public void SetButtons(ItemBag itemBag, CanvasController canvas)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= itemBag.GetBag().Count)
            {
                slots[i].DisableSlot();
            }
            else
            {
                Item item = itemBag.GetBag()[i];
                slots[i].EnableSlot(item,itemBag.GetStackNo(item), canvas);
            }
        }
        canvas.eventsystem.SetSelectedGameObject(slots[0].GetComponentInChildren<Button>().gameObject);
    }

    public void DisableButtons()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].DisableSlot();
        }
    }
}
