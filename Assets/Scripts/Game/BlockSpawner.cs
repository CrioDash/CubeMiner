using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CubeToSpawn;
    void Start()
    {
        StartCoroutine(SpawnFruitsRoutine());
    }

    private IEnumerator SpawnFruitsRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);
        while (true)
        {
            GameObject gm = Instantiate(CubeToSpawn, transform.position, Quaternion.identity);
            gm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-40, 40), 0));
            yield return wait;
        }
    }
}
