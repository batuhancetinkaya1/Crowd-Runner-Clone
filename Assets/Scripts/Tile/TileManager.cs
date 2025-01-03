using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private Tile[] tilePrefabs;
    [SerializeField] private Tile fightTilePrefab;
    [SerializeField] private int maxRegularTiles = 5;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private List<Tile> activeTiles = new List<Tile>();

    [SerializeField] private Tile assignedFightTile = null;

    [SerializeField] public bool isShuttingDown = false;
    [SerializeField] private int regularTileCount = 0;

    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    public void Start()
    {
        InitialSpawn();
    }

    private void InitialSpawn()
    {
        for (int i = 0; i < maxRegularTiles; i++)
        {
            SpawnRegularTile();
        }
        SpawnFightTile();
    }

    public void TileSpawner(bool spawnFightTile)
    {
        if (isShuttingDown) return;

        if (spawnFightTile)
        {
            SpawnFightTile();
        }
        else
        {
            SpawnRegularTile();
        }
    }

    private void SpawnRegularTile()
    {
        if (regularTileCount >= maxRegularTiles) return;

        Tile tileToCreate = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        UpdateSpawnPosition(tileToCreate);

        Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
        newTile.tileManager = this;
        activeTiles.Add(newTile);

        UpdateSpawnPosition(tileToCreate);
        regularTileCount++;
    }

    private void SpawnFightTile()
    {
        Tile tileToCreate = fightTilePrefab;
        UpdateSpawnPosition(tileToCreate);

        Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
        newTile.tileManager = this;

        UpdateSpawnPosition(tileToCreate);
        regularTileCount = 0; // Reset regular tile count after a fight tile

        assignedFightTile = newTile;
    }

    private void UpdateSpawnPosition(Tile tile)
    {
        spawnPosition.z += tile.GetChunkLength() / 2;
    }

    public void RemoveRegularTile(Tile tile)
    {
        if (!isShuttingDown && activeTiles.Contains(tile))
        {
            activeTiles.Remove(tile);
            TileSpawner(false);
        }
    }

    public void RemoveFightTile(Tile tile)
    {
        if (!isShuttingDown)
        {
            assignedFightTile = null;
            TileSpawner(true);
        }
    }

    public List<Tile> GetActiveTiles()
    {
        return activeTiles;
    }

    public void ClearTiles()
    {
        isShuttingDown = true;

        // Clear regular tiles
        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            Tile tile = activeTiles[i];
            if (tile != null)
            {
                tile.transform.SetParent(null);
                Destroy(tile.gameObject);
            }
        }

        // Clear fight tile
        if (assignedFightTile != null)
        {
            isShuttingDown = true;
            assignedFightTile.transform.SetParent(null);
            Destroy(assignedFightTile.gameObject);
        }

        activeTiles.Clear();
        regularTileCount = 0;
        spawnPosition = Vector3.zero;
    }
}