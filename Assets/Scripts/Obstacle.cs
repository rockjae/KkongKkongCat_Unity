using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;
    private Spawner spawner;
    private GameObject originalPrefab;

    public void Setup(Spawner spawner, GameObject prefab)
    {
        this.spawner = spawner;
        this.originalPrefab = prefab;
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            spawner.ReturnToPool(originalPrefab, this.gameObject);
        }
    }

}
