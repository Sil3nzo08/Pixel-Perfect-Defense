using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScreenCalculator : MonoBehaviour
{
    public Tilemap tilemap;
    public new Camera camera;
    private void Start()
    {
        var (center, orthoSize) = CalculateOrthoSize();
        camera.transform.position = center;
        camera.orthographicSize =orthoSize;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        tilemap.CompressBounds();

        var bounds = tilemap.localBounds;
        Debug.Log(bounds);

        var vertical = bounds.size.y * 2;
        var horizontal = bounds.size.x * 2 * camera.pixelHeight / camera.pixelWidth;

        var orthoSize = Mathf.Max(vertical, horizontal) * 0.5f;
        var center = bounds.center + new Vector3(0, 0, -10);

        return (center, orthoSize);
    }
}
