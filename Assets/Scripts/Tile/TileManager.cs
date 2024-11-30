using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Tile[] tilePrefab;
    [SerializeField] int maxTileCount;
    [SerializeField] Vector3 spawnPosition = Vector3.zero;

    void Start()
    {
        InitialSpawn();
    }

    private void InitialSpawn()
    {
        for(int i = 0; i < maxTileCount; i++)
        {
            Tile tileToCreate = tilePrefab[Random.Range(0, tilePrefab.Length)];

            if (i > 0)
            {
                spawnPosition.z += tileToCreate.GetLength() / 2;
            }

            Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);

            

            spawnPosition.z += tileToCreate.GetLength() / 2;
        }
    }

    public void TileSpawner()
    {
        Debug.Log("tile spawning from tilemanager");
    }

}
