using Core;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour, IManager
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public void Init()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                var parent = pool.parent != null ? pool.parent : transform;
                GameObject obj = Instantiate(pool.prefab, parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public void StartUp()
    {

    }

    public void Cleanup()
    {
        poolDictionary.Clear();
    }

    public GameObject SpawnFromPool(string tag)
    {
        GameObject objSpawn = poolDictionary[tag].Dequeue();
        objSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objSpawn);
        return objSpawn;
    }

    public T SpawnFromPool<T>(string tag)
    {
        GameObject objSpawn = poolDictionary[tag].Dequeue();
        objSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objSpawn);
        return objSpawn.GetComponent<T>();
    }

    public List<T> GetAllActive<T>(string tag)
    {
        List<T> result = new List<T>();
        if (!poolDictionary.ContainsKey(tag)) return new List<T>();
        foreach (GameObject item in poolDictionary[tag])
        {
            if (item.activeSelf) result.Add(item.GetComponent<T>());
        }
        return result;
    }

    public List<GameObject> GetAllActive(string tag)
    {
        List<GameObject> result = new List<GameObject>();
        if (!poolDictionary.ContainsKey(tag)) return new List<GameObject>();
        foreach (GameObject item in poolDictionary[tag])
        {
            if (item.activeSelf) result.Add(item);
        }
        return result;
    }
}