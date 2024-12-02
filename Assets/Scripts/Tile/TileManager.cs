using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Tile[] tilePrefab;
    [SerializeField] int maxTileCount = 5;
    [SerializeField] Vector3 spawnPosition = Vector3.zero;
    [SerializeField] List<Tile> tiles;

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
            SpawnTile();
        }
    }

    public void TileSpawner()
    {
        if (tiles.Count < maxTileCount && !isShuttingDown)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        Tile tileToCreate = tilePrefab[Random.Range(0, tilePrefab.Length)];

        if (tiles.Count > 0)
        {
            spawnPosition.z += tileToCreate.GetChunkLength() / 2;
        }

        Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
        newTile.tileManager = this;
        tiles.Add(newTile);

        spawnPosition.z += tileToCreate.GetChunkLength() / 2;
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