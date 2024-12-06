using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Tile[] tilePrefab;
    [SerializeField] private int maxTileCount = 5;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private List<Tile> tiles;

    [Header("Elements")]
    [SerializeField] private Tile fightTile;
    [SerializeField] private int fightCountDown = 5;

    // Flag to prevent spawning during scene shutdown
    private bool isShuttingDown = false;

    private void OnApplicationQuit()
    {
        // Set flag to prevent further tile spawning
        isShuttingDown = true;
    }

    void Start()
    {
        InitialSpawn();
    }

    private void InitialSpawn()
    {
        for (int i = 0; i < maxTileCount; i++)
        {
            SpawnRegularTile();
        }
    }

    public void TileSpawner()
    {
        if (tiles.Count < maxTileCount && !isShuttingDown && fightCountDown > 0)
        {
            SpawnRegularTile();
        }
        else if(!isShuttingDown && fightCountDown > 0)
        {
            SpawnFightTile(); //bunu kapatmak gerekebilir
        }
    }

    private void SpawnRegularTile()
    {
        Tile tileToCreate = tilePrefab[Random.Range(0, tilePrefab.Length)];

        if (tiles.Count > 0)
        {
            spawnPosition.z += tileToCreate.GetChunkLength() / 2;
        }

        Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
        newTile.tileManager = this;

        spawnPosition.z += tileToCreate.GetChunkLength() / 2;
        fightCountDown--;
    }

    private void SpawnFightTile()
    {
        Tile tileToCreate = fightTile;

        spawnPosition.z += tileToCreate.GetChunkLength() / 2;


        Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
        newTile.tileManager = this;
        tiles.Add(newTile);

        spawnPosition.z += tileToCreate.GetChunkLength() / 2;
        fightCountDown = 5;
    }

    public void RemoveTile(Tile tile)
    {
        if (!isShuttingDown && tiles.Count > 0 && tiles[0] == tile)
        {
            tiles.RemoveAt(0);
            TileSpawner();
        }
    }
}