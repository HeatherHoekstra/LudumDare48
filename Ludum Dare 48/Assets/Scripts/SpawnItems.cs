using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] items;
    [SerializeField] private float spawnDistance =15;
    private int spawnPointIndex;
    private int itemIndex;

    private float lastSpawnPositionY;
    private float currentPositionY;


    // Start is called before the first frame update
    void Start()
    {
        lastSpawnPositionY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentPositionY = player.transform.position.y;

        if(lastSpawnPositionY - currentPositionY > spawnDistance)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        //pick number of items to be spawned
        float numberOfItems = Random.Range(0, 4);
        
        while(numberOfItems > 0)
        {
            numberOfItems--;

            //Pick a spawnlocation

            spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject spawnPoint = spawnPoints[spawnPointIndex];
            SpawnPoint script = spawnPoint.GetComponent<SpawnPoint>();
            if (script.full)
            {
                //if this spot is already taken, try again
                numberOfItems++;
            }
            else
            {
                //spawn an item in this spot
                script.full = true;
                itemIndex = Random.Range(0, items.Length);
                GameObject item = items[itemIndex];

                GameObject newItem = Instantiate(item);
                newItem.transform.position = spawnPoint.transform.position;

                StartCoroutine(newItem.GetComponent<DestroySelf>().WaitToDestroy());
            }                  
                        
        }

        lastSpawnPositionY = currentPositionY;
        foreach(GameObject spawnpoint in spawnPoints)
        {
           spawnpoint.GetComponent<SpawnPoint>().full = false;
        }
    }    
}
