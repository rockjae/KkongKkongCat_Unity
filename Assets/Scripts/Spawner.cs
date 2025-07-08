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
    private int initialPoolSize = 2; // 각 프리팹 당 초기에 생성할 오브젝트 수

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
            // count 개수만큼 장애물의 isUnlocked 상태를 true로 변경합니다.
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
        // 1. 해금된 장애물 목록과 전체 스폰 확률 계산
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

        // 해금된 장애물이 없으면 스폰하지 않음
        if (unlockedObjects.Count == 0)
        {
            Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
            return;
        }

        // 2. 해금된 장애물 중에서 무작위로 하나를 선택
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
            else // 풀이 비어있다면 새로 생성 (비상시 대비)
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