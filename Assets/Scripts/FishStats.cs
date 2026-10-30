using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FishStats : MonoBehaviour

{
    public string fishName;

    float fishHealth = 100f;
    float hookDamage = 25f; // Get from upgrades when M has coded them
    float currentHealth;


    //fish value
    public float weight;
    public float rarityMultiplier;
    public float sellValue;

    private GameObject Hook;
    private FishInventory fishInventory;


    private  WeightedMode 


    void Start()
    {
        //transform.position = new Vector3(1.53f, -8.39f, 0); // Temporary starting position while M works on fish spawning
        currentHealth = fishHealth;
        fishInventory = FindObjectOfType<FishInventory>();
        GenerateFishStats();
        Debug.Log("FishInventory found: " + fishInventory);


    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            //Attach to hook until hook i
            transform.position = new Vector3(Hook.transform.position.x, Hook.transform.position.y, 0.0f);
            if (transform.position == new Vector3(0.0f, 0.0f, 0.0f))
            {
                fishInventory.FishCaught(sellValue);
                Debug.Log("called fish caught");
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHook>() && currentHealth > 0)
        {
            Hook = collision.gameObject;
            currentHealth -= hookDamage;
            Debug.Log("Fish Health:" + currentHealth);
        }
    }

    private void CalculateValue()
    {
        sellValue = weight * rarityMultiplier;

        Debug.Log(fishName + " is worth $" + sellValue);
    }




    private void GenerateFishStats()
    {
        weight = Random.Range(0.5f, 5f);

        int rarityRoll = Random.Range(1, 101);

        if (rarityRoll <= 60)
        {
            rarityMultiplier = 1f;
        }
        else if (rarityRoll <= 85)
        {
            rarityMultiplier = 2f;
        }
        else if (rarityRoll <= 97)
        {
            rarityMultiplier = 4f;
        }
        else
        {
            rarityMultiplier = 10f;
        }

        CalculateValue();
    }
}