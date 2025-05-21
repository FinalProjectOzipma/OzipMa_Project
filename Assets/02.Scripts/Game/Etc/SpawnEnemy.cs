using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private float spawnY;

    private void Awake()
    {
        spawnY = -5.0f;

        float randomX = UnityEngine.Random.Range(-2.0f, 2.0f);

        this.gameObject.transform.position = new Vector2(randomX, spawnY);
    }



}
