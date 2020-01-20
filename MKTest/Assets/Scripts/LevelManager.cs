using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour

{

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 60f;     //The distance from the character at which level parts spawn

    [SerializeField] private Transform levelStart;                  //The starting location
    [SerializeField] private Transform[] levelPartList;             //The list of level parts
    [SerializeField] private GameObject player;                     //The player

    public ObjectPooler objectPool;                                 //The object pooler

    private List<GameObject> spawnedParts;                          //Preparing to save spawned parts
    private Vector3 prevPartPos;                                    //The position of the previous level part

    private void Awake()
    {
        //Initialize variables
        spawnedParts = new List<GameObject>();
        prevPartPos = new Vector3 (levelStart.position.x + 20, levelStart.position.y, levelStart.position.z);
        objectPool = ObjectPooler.instance;        
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
            Transform prevPartLoc = Instantiate(levelStart, levelStart.position, Quaternion.identity);
            spawnedParts.Add(prevPartLoc.gameObject);
        }
        else
        {
            //Getting the random number to select the level part to spawn
            int partIndex = Random.Range(0, levelPartList.Length);

            //Using the index to set up the tag for the pool dictionary
            string tag = "area" + (partIndex + 1);

            //Saving the transform of the new part
            Transform randomPart = levelPartList[partIndex];

            //Spawn the new part
            Transform prevPartLoc = SpawnLevelPart(tag, randomPart, prevPartPos);
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

    private Transform SpawnLevelPart(string tag, Transform levelPart, Vector3 spawnPosition)
    {
        //Get the part from the pool
        Transform newPart = objectPool.SpawnPooledObject(tag, spawnPosition, Quaternion.identity);
        return newPart;
    }

    private void DestroyPart()
    {

        //Disable a level part that is no longer used
        spawnedParts[0].SetActive(false);
        spawnedParts.RemoveAt(0);
    }
}
