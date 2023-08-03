using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    [Header("����ũ��")]
    public int width;
    [Header("����ũ��")]
    public int height;
    [Header("�����Ÿ����ġ")]
    public Vector2 startTilePos;
    [Header("�����Ÿ��ũ��")]
    public Vector2 startTileSize;
    [Header("�����Ÿ����ġ")]
    public Vector2 endTilePos;
    [Header("�����Ÿ��ũ��")]
    public Vector2 endTileSize;
}
