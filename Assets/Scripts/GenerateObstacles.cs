using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : MonoBehaviour {

    public GameObject obstacle;
    private float[] offsets = { -5, -2, 2, 5 };
    private int yOffset = 30;
    private int previousIndex = 0;

    // Use this for initialization
    void Start () {

        Spawn();
	}

    private void Spawn()
    {
        for (int i = 0; i < 35; i++)
        {
            GameObject obs;
            if (i == 0) obs = Instantiate(obstacle, new Vector3(RandomOffset(), 1, yOffset), Quaternion.identity);
            else obs = Instantiate(obstacle, new Vector3(RandomOffset(), 1, yOffset += 10), Quaternion.identity);
            obs.transform.parent = transform;
        }
    }

    private float RandomOffset()
    {
        int index = Random.Range(0, 4);

        while (previousIndex == index)
        {
            index = Random.Range(0, 4);
        }

        previousIndex = index;
        return offsets[index];
    }

    public void Reset()
    {
        yOffset = 30;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Spawn();
    }
	
}
