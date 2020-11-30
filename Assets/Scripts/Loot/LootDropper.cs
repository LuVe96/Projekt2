using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public float spawnHeight = 0.2f;
    [Range(0, 100)]
    public int mainProbability;

    [Serializable]
    public struct LootObject
    {
        public LootItem lootItem;
        [Range(0, 100)]
        public int probability;
    }

    [SerializeField]
    private LootObject[] lootObjects;

    private bool allreadyCalled = false;

    public void DropLoot()
    {
        if (!allreadyCalled)
        {
            SpawnLootObj(CalcRandomObj());
            allreadyCalled = true;
        }

    }

    private GameObject CalcRandomObj()
    {
        int randomValueBetween0And99 = UnityEngine.Random.Range(0, 100);
        if (randomValueBetween0And99 > mainProbability)
        {
            return null;
        }


        int sumProp = 0;
        foreach (var l in lootObjects)
        {
            sumProp += l.probability;
        }
        int randomSumPropValue = UnityEngine.Random.Range(0, sumProp) +1;

        int calcProp = 0;
        foreach (var l in lootObjects)
        {
            calcProp += l.probability;
            if(calcProp >= randomSumPropValue)
            {
                return l.lootItem.gameObject;
            }
        }

        Debug.Log("Propability Error");
        return null;
    }

    void SpawnLootObj(GameObject obj )
    {

        if (obj == null) return;

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);

        var lo = Instantiate(obj);
        lo.transform.position = spawnPos;
    }


}

