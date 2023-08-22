using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CubeToSpawn;

    private Camera cam;
    

    private void Awake()
    {
        cam = Camera.main;
        
    }

    void Start()
    {
        StartCoroutine(SpawnFruitsRoutine());
    }

    private IEnumerator SpawnFruitsRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);
        while (true)
        {
            int mult = Random.Range(0, 2) == 0 ? -1 : 1;
            Vector3 spawnPos = new Vector3(cam.orthographicSize * cam.aspect * mult, cam.orthographicSize+ 1);
            GameObject gm = Instantiate(CubeToSpawn, spawnPos, Quaternion.identity);
            gm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(75, 150)*mult*-1, 50));
            yield return wait;
        }
    }
}
