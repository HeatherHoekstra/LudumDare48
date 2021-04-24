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
        //print(lastSpawnPositionY - currentPositionY);

        if(lastSpawnPositionY - currentPositionY > spawnDistance)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        //pick nr tussen 0 en 3 (aantal dat spawnt)(while loop)
        float numberOfItems = Random.Range(0, 4);
        
        while(numberOfItems > 0)
        {
            numberOfItems--;

            spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject spawnPoint = spawnPoints[spawnPointIndex];
            SpawnPoint script = spawnPoint.GetComponent<SpawnPoint>();
            if (script.full)
            {
                numberOfItems++;
            }
            else
            {
                script.full = true;
                itemIndex = Random.Range(0, items.Length);
                GameObject item = items[itemIndex];

                GameObject newItem = Instantiate(item);
                newItem.transform.position = spawnPoint.transform.position;
            }                  
                        
        }

        lastSpawnPositionY = currentPositionY;
        foreach(GameObject spawnpoint in spawnPoints)
        {
           spawnpoint.GetComponent<SpawnPoint>().full = false;
        }
    }    
}
