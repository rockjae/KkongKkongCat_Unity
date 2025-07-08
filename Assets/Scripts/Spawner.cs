using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance; 
        [HideInInspector]
        public bool isUnlocked;
    }

    public SpawnableObject[] objects;
    private float minSpawnRate = 1f;
    private float maxSpawnRate = 2f;
    private int initialPoolSize = 2; // �� ������ �� �ʱ⿡ ������ ������Ʈ ��

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (var objInfo in objects)
        {
            var objectPool = new Queue<GameObject>();

            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject newObj = Instantiate(objInfo.prefab);
                newObj.SetActive(false);
                objectPool.Enqueue(newObj);
            }
            
            poolDictionary.Add(objInfo.prefab, objectPool);
        }
    }
    public void SetUnlockedCount(int count)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            // count ������ŭ ��ֹ��� isUnlocked ���¸� true�� �����մϴ�.
            objects[i].isUnlocked = i < count;
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        // 1. �رݵ� ��ֹ� ��ϰ� ��ü ���� Ȯ�� ���
        var unlockedObjects = new List<SpawnableObject>();
        float totalSpawnChance = 0f;

        foreach (var objInfo in objects)
        {
            if (objInfo.isUnlocked)
            {
                unlockedObjects.Add(objInfo);
                totalSpawnChance += objInfo.spawnChance;
            }
        }

        // �رݵ� ��ֹ��� ������ �������� ����
        if (unlockedObjects.Count == 0)
        {
            Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
            return;
        }

        // 2. �رݵ� ��ֹ� �߿��� �������� �ϳ��� ����
        float randomValue = Random.Range(0f, totalSpawnChance);

        foreach (var objInfo in unlockedObjects)
        {
            if (randomValue <= objInfo.spawnChance)
            {
                GameObject pooledObject = GetFromPool(objInfo.prefab);

                pooledObject.transform.position = transform.position;
                pooledObject.SetActive(true);

                Obstacle obstacle = pooledObject.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    obstacle.Setup(this, objInfo.prefab);
                }
                break;
            }

            randomValue -= objInfo.spawnChance;
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> objectPool))
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Dequeue();
                return obj;
            }
            else // Ǯ�� ����ִٸ� ���� ���� (���� ���)
            {
                GameObject newObj = Instantiate(prefab);
                return newObj;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> objectPool))
        {
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }
}