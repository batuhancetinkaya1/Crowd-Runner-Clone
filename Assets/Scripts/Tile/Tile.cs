using UnityEditor.UI;
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
    private void DestroyTile()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (tileManager != null && !ReferenceEquals(tileManager, null) && !gameObject.CompareTag("TileFight"))
        {
            tileManager.RemoveRegularTile(this);
        }
        else if(tileManager != null && !ReferenceEquals(tileManager, null) && gameObject.CompareTag("TileFight"))
        {
            tileManager.RemoveFightTile(this);
        }
    }

    public float GetChunkLength()
    {
        return chunk.GetLength();
    }
}
