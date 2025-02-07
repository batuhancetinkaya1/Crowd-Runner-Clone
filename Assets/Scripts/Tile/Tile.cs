using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Chunk chunk;
    public TileManager tileManager; // TileManager referansý

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            DestroyTile();        
        }
    }
    public void DestroyTile()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (tileManager == null) return;

        // Check if this is a fight tile or regular tile
        if (gameObject.CompareTag("TileFight"))
        {
            tileManager.RemoveFightTile(this);
        }
        else
        {
            tileManager.RemoveRegularTile(this);
        }
    }

    public float GetChunkLength()
    {
        return chunk.GetLength();
    }
}
