using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour

{

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 60f;     //The distance from the character at which level parts spawn

    [SerializeField] private Transform levelStart;                  //The starting location
    [SerializeField] private Transform[] levelPartList;             //The list of level parts
    [SerializeField] private GameObject player;                     //The player

    private List<GameObject> spawnedParts;                          //Preparing to save spawned parts
    private Vector3 prevPartPos;                                    //The position of the previous level part

    private void Awake()
    {
        //Initialize variables
        spawnedParts = new List<GameObject>();
        prevPartPos = new Vector3 (levelStart.position.x + 20, levelStart.position.y, levelStart.position.z);

        //Spawn the initial level parts
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

    }

    private void SpawnLevelPart()
    {

        //Check if spawning the first part - this will always be the starting platform
        if (spawnedParts.Count <= 0)
        {
            Transform prevPartLoc = SpawnLevelPart(levelStart, levelStart.position);
            spawnedParts.Add(prevPartLoc.gameObject);
        }
        else
        {

            //Select a random level part to spawn next
            Transform randomPart = levelPartList[Random.Range(0, levelPartList.Length)];
            Transform prevPartLoc = SpawnLevelPart(randomPart, prevPartPos);
            spawnedParts.Add(prevPartLoc.gameObject);

            //Check if the initial level parts have finished spawning
            if (spawnedParts.Count >= 6)
            {
                DestroyPart();
            }

            //Set the position to the latest spawn
            prevPartPos = new Vector3(prevPartLoc.position.x + 20, prevPartLoc.position.y, prevPartLoc.position.z);
        }
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {

        //Instantiate a clone of the level part
        Transform levelPartTrans = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        return levelPartTrans;
    }

    private void DestroyPart()
    {

        //Destroy a level part that is no longer used
        Destroy(spawnedParts[0]);
        spawnedParts.RemoveAt(0);
    }
}
