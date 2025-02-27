using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InventoryList<T> where T : class
{
    private T _item;
    public T item
    {
        get { return _item; }
    }

    public InventoryList()
    {
        UnityEngine.Debug.Log("Generic list initalized...");
    }
    
    public void SetItem(T newItem)
    {
        _item = newItem;
        UnityEngine.Debug.Log("New item added...");
    }
}