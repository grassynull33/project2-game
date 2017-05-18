using UnityEngine;
using System.Collections;

public class UI_ItemSpawner : MonoBehaviour {

    public UI_ItemsList itemList;

    public Transform spawnPos; //the transform which indicates the possition of the item which will be spawned

    public int[] possibleItems; //the id's of the items that can be spawned on this spawner
    public int[] chanceOfItems; //the chance of each item to be spawned

    public float minSpawnDelay = 50, maxSpawnDelay = 500; //the spawn delay min & max values

    public bool spawnOnce = false; //whether to spawn only one item or not

    public GameObject spawnedObj; //the spawned object (leave it null)

    bool cooldown = false;
    
    void Update()
    {
        if (cooldown == false && spawnedObj == null)
        {
            StartCoroutine("Timer");
            cooldown = true;
        }
    }

    IEnumerator Timer()
    {
        float cdTime = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(cdTime);
        GenerateRandomItem();
    }

    void GenerateRandomItem()
    {
        int rolled = Random.Range(0, 100);
        int id = -1;
        bool found = false;
        for (int i=0; i<possibleItems.Length; i++)
        {
            if (found == false)
            {
                if (rolled <= chanceOfItems[i])
                {
                    id = possibleItems[i];
                    found = true;
                }
            }
        }
        if (id != -1)
        {
            GameObject tmp = itemList.allItems[id];
            GameObject newObj = (GameObject)Instantiate(tmp, spawnPos.position, tmp.transform.rotation);
            spawnedObj = newObj;
        }
        else
            spawnedObj = null;

        cooldown = false;
    }
}
