using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag {

    private List<Item> Bag = new List<Item>();
    private List<int> StackTotal = new List<int>();

    public void AddItem(Item item)
    {
        int index = Bag.IndexOf(item);
        if (index == -1)
        {
            Bag.Add(item);
            StackTotal.Add(0);
        }
        else
        {
            StackTotal[index] += 1;
        }
    }

    public void RemoveItem(Item item, int no)
    {
        int index = Bag.IndexOf(item);
        if (index == -1)
        {
            Debug.Log("Tried to remove an item that doesn't exist");
            return;
        }
        StackTotal[index] -= 1;
        if (StackTotal[index] <= 0)
        {
            StackTotal.RemoveAt(index);
            Bag.RemoveAt(index);
        }
    }

    public List<Item> GetBag()
    {
        return Bag;
    }

    public int GetStackNo(Item item)
    {
        int index = Bag.IndexOf(item);
        if (index == -1)
        {
            return -1;
        }
        return StackTotal[index];
    }
}
