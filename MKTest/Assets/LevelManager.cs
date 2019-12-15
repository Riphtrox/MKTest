using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour

{

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 60f;

    [SerializeField] private Transform levelStart;
    [SerializeField] private Transform[] levelPartList;
    [SerializeField] private GameObject player;

    private List<GameObject> spawnedParts; //Preparing to save spawned parts
    private Vector3 prevPartPos;

    private void Awake()
    {
        spawnedParts = new List<GameObject>();
        prevPartPos = new Vector3 (levelStart.position.x + 20, levelStart.position.y, levelStart.position.z);
        int startingSpawnLevelParts = 5;
        for (int i = 0; i < startingSpawnLevelParts; i++)
        {
            SpawnLevelPart();
        }
    }

    private void Update()
    {
        //Check player distance
        if(Vector3.Distance(player.GetComponent<Transform>().position, prevPartPos) < PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            //Spawn another level part
            SpawnLevelPart();
        }

        //Check player distance
        if (Vector3.Distance(player.GetComponent<Transform>().position, prevPartPos) > -PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            //Spawn another level part
            
        }
    }

    private void SpawnLevelPart()
    {
        if (spawnedParts.Count <= 0)
        {
            Transform prevPartLoc = SpawnLevelPart(levelStart, levelStart.position);
            spawnedParts.Add(prevPartLoc.gameObject);
        }
        else
        {
            Transform randomPart = levelPartList[Random.Range(0, levelPartList.Length)];
            Transform prevPartLoc = SpawnLevelPart(randomPart, prevPartPos);
            spawnedParts.Add(prevPartLoc.gameObject);

            if (spawnedParts.Count >= 6)
            {
                DestroyPart();
            }
            prevPartPos = new Vector3(prevPartLoc.position.x + 20, prevPartLoc.position.y, prevPartLoc.position.z);
        }
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {
        Transform levelPartTrans = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        return levelPartTrans;
    }

    private void DestroyPart()
    {
        Destroy(spawnedParts[0]);
        spawnedParts.RemoveAt(0);
    }
}
