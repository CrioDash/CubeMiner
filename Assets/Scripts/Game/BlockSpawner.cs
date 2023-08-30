using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Fruit;
using TMPro;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject dynamitePrefab;
    
    [SerializeField] private float baseSpawnTime;
    [SerializeField] private int baseFallSpeed;
    [SerializeField] private int baseBlockGoal;

    public static int currentBlockGoal;
    public static int currentFallSpeed;
    public static float currentSpawnTime;
    public static Variables.BlockType currentType;
        
    private const float MinSpawnTime = 0.05f;
    private const int MaxFallSpeed = 1000;
    
    private Camera _cam;
   

    

    private List<Variables.BlockType> _types = new List<Variables.BlockType>(); 

    private void Awake()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        _types = Variables.BlockInfo.Keys.ToList();
        
        currentType = _types[0];
        _types.Remove(currentType);
        
        currentBlockGoal = baseBlockGoal;
        currentSpawnTime = baseSpawnTime;
        
        StartCoroutine(SpawnFruitsRoutine());
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventBus.EventType.CHANGE_BLOCK,ChangleBlock);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventBus.EventType.CHANGE_BLOCK, ChangleBlock);
    }

    private void ChangleBlock()
    {
        currentSpawnTime -= 0.05f;
        currentSpawnTime = Mathf.Clamp(currentSpawnTime, MinSpawnTime, float.MaxValue);

        currentFallSpeed += 25;
        currentFallSpeed = Mathf.Clamp(currentFallSpeed, baseFallSpeed, MaxFallSpeed);

        currentType = _types[Random.Range(0, _types.Count)];
        _types.Remove(currentType);

        currentBlockGoal = (int)Mathf.Round(currentBlockGoal * 0.7f + Random.Range(currentBlockGoal * 0.5f, currentBlockGoal) /
            Variables.BlockInfo[currentType].Health);

        currentBlockGoal = Mathf.Clamp(currentBlockGoal, 10, Int32.MaxValue);

        if (_types.Count == 0)
        {
            _types = Variables.BlockInfo.Keys.ToList();
            _types.Remove(currentType);
        }
}

    private IEnumerator SpawnFruitsRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(baseSpawnTime);
        while (true)
        {
            int mult = Random.Range(0, 2) == 0 ? -1 : 1;
            
            Vector3 spawnPos = new Vector3(_cam.orthographicSize * _cam.aspect * mult, _cam.orthographicSize+ 1);

            if (Variables.BlocksCut >= currentBlockGoal)
            {
                Variables.BlocksCut -= currentBlockGoal;
                Vector3 pos = spawnPos;
                pos.x *= -1;
                GameObject gm = Instantiate(dynamitePrefab, pos, Quaternion.identity);
                gm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(35, 70)*mult, -currentFallSpeed));
                
                EventBus.Publish(EventBus.EventType.SPAWN_DYNAMITE);
            }
            
            Block block = Instantiate(cubePrefab, spawnPos, Quaternion.identity).GetComponent<Block>();
            block.SetStats(currentType);
            
            block.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(35, 70)*mult*-1, -currentFallSpeed));
            
            yield return wait;
        }
    }
}
