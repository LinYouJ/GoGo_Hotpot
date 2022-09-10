using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManger : MonoBehaviour
{
    [SerializeField] GameObject[] floorPrefabs;
    public void spawnFloor(){
        int r = Random.Range(0, floorPrefabs.Length);
        GameObject floor = Instantiate(floorPrefabs[r], transform);
        floor.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), -6f, 0f);
    }
}
