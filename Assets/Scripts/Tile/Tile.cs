using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Chunk chunk;
    public TileManager tileManager; // TileManager referansý

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (tileManager != null && !ReferenceEquals(tileManager, null))
        {
            tileManager.RemoveTile(this);
        }
    }

    public float GetChunkLength()
    {
        return chunk.GetLength();
    }
}
