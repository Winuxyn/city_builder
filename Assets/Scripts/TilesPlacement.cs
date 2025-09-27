using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Tilemaps;

public class TilesPlacement : MonoBehaviour
{
    public Tilemap tilemap;

    private Vector3 mousePos = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    private TileBase GetCell()
    {
        // Récupère la position de la souris en coordonnées écran
        Vector3 screenMousePos = Input.mousePosition;
        // Convertit la position écran en coordonnées monde
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));

       return tilemap.GetTile(new Vector3Int((int)mousePos.x, (int)mousePos.y, 0));
    }

    // Update is called once per frame
    private void Update()
    {
        TileBase tile = GetCell();

        tile.GetTileData;

    }
}