using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPool : MonoBehaviour
{
    public int birdPoolSizePerType = 5;

    public GameObject[] birds;
    private List<GameObject> birdsInstanced = new List<GameObject>();

    private static Transform myTransform;

    private void Start()
    {
        myTransform = transform;

        //Spawn birdPoolSizePerType of each
        foreach (GameObject bird in birds)
        {
            for (int i = 0; i < birdPoolSizePerType; i++)
            {
                GameObject birdInstance = Instantiate(bird, myTransform.position, Quaternion.identity, myTransform);
                birdsInstanced.Add(birdInstance);
            }
        }
    }

    public GameObject GetPooledBird()
    {
        // Filter the list of pooled object and put all the inactive ones into a new list
        List<GameObject> inactiveObjects = birdsInstanced.FindAll(go => !go.activeInHierarchy);
        
        // Check if the list created above has elements
        // If so, pick a random one, return null otherwise
        return inactiveObjects.Count > 0 ?
            inactiveObjects[Random.Range(0, inactiveObjects.Count)] :
            null;

        //Position and setActive will be defined on the Spawn
    }

    public static void ReturnToPoolPos(Transform _bird)
    {
        _bird.position = myTransform.position;
        _bird.gameObject.SetActive(false);
    }
}
