using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Tile[] tilePrefab;
    [SerializeField] int maxTileCount = 5;
    [SerializeField] Vector3 spawnPosition = Vector3.zero;
    [SerializeField] List<Tile> tiles;

    void Start()
    {
        InitialSpawn();
    }

    private void InitialSpawn()
    {
        for (int i = 0; i < maxTileCount; i++)
        {
            Tile tileToCreate = tilePrefab[Random.Range(0, tilePrefab.Length)];

            if (i > 0)
            {
                spawnPosition.z += tileToCreate.GetChunkLength() / 2;
            }

            Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
            newTile.tileManager = this; // Referans� ata
            tiles.Add(newTile);

            spawnPosition.z += tileToCreate.GetChunkLength() / 2;
        }
    }

    public void TileSpawner()
    {
        if (tiles.Count < maxTileCount)
        {
            Tile tileToCreate = tilePrefab[Random.Range(0, tilePrefab.Length)];

            spawnPosition.z += tileToCreate.GetChunkLength() / 2;

            Tile newTile = Instantiate(tileToCreate, spawnPosition, Quaternion.identity, transform);
            newTile.tileManager = this; // Referans� ata
            tiles.Add(newTile);

            spawnPosition.z += tileToCreate.GetChunkLength() / 2;
        }
    }

    public void RemoveTile(Tile tile)
    {
        if (tiles.Count > 0 && tiles[0] == tile) // Sadece listenin ilk eleman�n� kontrol et
        {
            tiles.RemoveAt(0); //Sadece listedeki ilk tile yok edilecek oyun �izgisel akt���ndan bu sorunumuzu ��z�yor.

            // Yeni bir Tile spawn et
            TileSpawner();
        }
    }
}
