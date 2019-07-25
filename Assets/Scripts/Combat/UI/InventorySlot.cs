using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Text TextName;
    public Button button;

	public void SetName(string name)
    {
        TextName.text = name;
    }

    public void EnableSlot(Spell spell, CanvasController canvas)
    {
        TextName.text = spell.name;
        button.onClick.AddListener(() => canvas.SetSpell(spell));
    }

    public void EnableSlot(Item item, int stackNo, CanvasController canvas)
    {
        TextName.text = item.name + " x"+ stackNo;
        button.onClick.AddListener(() => canvas.SetItem(item));
    }

	public void DisableSlot()
    {
        TextName.text = null;
        button.onClick.RemoveAllListeners();
    }
}
