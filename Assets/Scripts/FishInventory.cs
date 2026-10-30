using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    public List<float> inventory = new List<float>();

    public void FishCaught(float exampleVar1/*, float exampleVar2*/) //second variable removed, in final can calculate fish value in fish script then add value to inventory
    {
        Debug.Log("Added new fish to inventory");
        inventory.Add(exampleVar1/*, exampleVar2*/);
        Debug.Log("Inventory Values:" + string.Join(", ", inventory));
    }
}