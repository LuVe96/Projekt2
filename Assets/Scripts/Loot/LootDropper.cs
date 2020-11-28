using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{

    //public PlayerMovement[] lootObjects;

    [Serializable]
    public struct LootObject
    {
        public GameObject obj;
        [Range(0, 100)]
        public int probability;
    }

    [SerializeField]
    private LootObject[] lootObjects;

    [Range(0, 100)]
    public int mainProbability;

    public float spawnHeight = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropLoot()
    {
        SpawnLootObj(lootObjects[0].obj);
    }

    void SpawnLootObj(GameObject obj )
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);

        var lo = Instantiate(obj);
        lo.transform.position = spawnPos;
    }


}

