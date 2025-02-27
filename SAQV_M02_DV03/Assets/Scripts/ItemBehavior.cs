using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameBehaviour gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameBehaviour>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(this.transform.parent.gameObject);
            UnityEngine.Debug.Log("Item collected!");
            gameManager.Items += 1;
            gameManager.PrintLootReport();
        }
    }
}