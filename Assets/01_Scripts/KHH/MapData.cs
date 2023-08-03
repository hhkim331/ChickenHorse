using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    [Header("가로크기")]
    public int width;
    [Header("세로크기")]
    public int height;
    [Header("출발점타일위치")]
    public Vector2 startTilePos;
    [Header("출발점타일크기")]
    public Vector2 startTileSize;
    [Header("결승점타일위치")]
    public Vector2 endTilePos;
    [Header("결승점타일크기")]
    public Vector2 endTileSize;
}
