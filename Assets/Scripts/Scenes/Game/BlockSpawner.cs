using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fruit;
using UnityEngine;
using EventBus = Utilities.EventBus;
using Random = UnityEngine.Random;
using Variables = Data.Variables;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private GameObject chestPrefab;

    [SerializeField] private int chestSpawnChance;
    
    [SerializeField] private float baseSpawnTime;
    [SerializeField] private float baseFallSpeed;
    [SerializeField] private int baseBlockGoal;

    public static int BlocksCut = 0;
    public static int currentBlockGoal;
    public static float currentFallSpeed;
    public static float currentSpawnTime;
    public static Variables.BlockType currentType;
        
    private const float MinSpawnTime = 0.05f;
    private const float MaxFallSpeed = 1000;
    
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

        currentFallSpeed += 10f;
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
        WaitForSeconds wait = new WaitForSeconds(currentSpawnTime);
        while (true)
        {
            int mult = Random.Range(0, 2) == 0 ? -1 : 1;
            
            Vector3 spawnPos = new Vector3(_cam.orthographicSize * _cam.aspect * mult, _cam.orthographicSize+ 1);

            if (BlocksCut >= currentBlockGoal)
            {
                BlocksCut -= currentBlockGoal;
                Vector3 pos = spawnPos;
                pos.x *= -1;
                pos.z = -1;
                GameObject gm = Instantiate(dynamitePrefab, pos, Quaternion.identity);
                gm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(35, 70)*mult, -currentFallSpeed));
                gm.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
                
                EventBus.Publish(EventBus.EventType.SPAWN_DYNAMITE);
            }

            if (Random.Range(1, 100/chestSpawnChance + 1) == 1)
            {
                Vector3 pos = spawnPos;
                pos.x = 0;
                pos.z = -1;
                GameObject gm = Instantiate(chestPrefab, pos, Quaternion.identity);
                gm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(15, 40)*mult, -currentFallSpeed));
                gm.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
            }

            Variables.BlocksFall++;
            
            Block block = Instantiate(cubePrefab, spawnPos, Quaternion.identity).GetComponent<Block>();
            block.SetStats(currentType);
            
            block.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(35, 70)*mult*-1, -currentFallSpeed));
            block.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);

            if (Variables.Score > 50000 && Random.Range(0,10) < 4)
            {
                spawnPos.x *= -1;
                Variables.BlocksFall++;
            
                block = Instantiate(cubePrefab, spawnPos, Quaternion.identity).GetComponent<Block>();
                block.SetStats(currentType);
            
                block.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(35, 70)*mult*-1, -currentFallSpeed));
                block.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
            }
            
            yield return wait;
        }
    }
}
