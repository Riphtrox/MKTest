using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    //Creating a class to define each pool
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public Transform prefab;
        public int size;
    }

    //Singleton
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }


    public List<Pool> pools;                                        //List of pools
    public Dictionary<string, Queue<Transform>> poolDictionary;     //Dictionary for separating and selecting pools


    //Start is called before the first frame update
    void Start()
    {

        poolDictionary = new Dictionary<string, Queue<Transform>>();

        //Creating all the required objects for each pool defined
        foreach(Pool pool in pools)
        {
            Queue<Transform> objectPool = new Queue<Transform>();

            //Populating the pool
            for(int i = 0; i < pool.size; i++)
            {
                Transform obj = Instantiate(pool.prefab);

                //Setting the objects inactive so they don't interact with the game yet
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    public Transform SpawnPooledObject(string tag, Vector3 position, Quaternion rotation)
    {

        //Checking if the key exists to avoid errors
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag:" + tag + "does not exist");
            return null;
        }

        //Taking the object to be used out of the queue
        Transform target = poolDictionary[tag].Dequeue();

        target.gameObject.SetActive(true);

        //Ensuring that all collected coins will spawn again
        for(int i = 0; i < target.childCount; i++)
        {
            target.GetChild(i).gameObject.SetActive(true);
        }
        
        //Moving the level part to the required location
        target.position = position;
        target.rotation = rotation;

        //Adding the object to the queue ready to be re-used later
        poolDictionary[tag].Enqueue(target);

        return target;
    }
}
