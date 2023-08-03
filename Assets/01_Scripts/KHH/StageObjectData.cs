using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageObjectData
{
    //타입
    public enum ObjectType
    {
        Fixed,  //고정오브젝트
        Normal, //배치가능 오브젝트
        Cover,  //기본 오브젝트에 덮어씌우는 오브젝트
        Top,    //기본 오브젝트 위에 배치되는 오브젝트
    }
    public ObjectType objectType = ObjectType.Fixed;

    //회전
    public enum ObjectRotType
    {
        None,
        Spin,
        FlipY,
    }
    public ObjectRotType objectRotType = ObjectRotType.None;

    public Vector2 objectSize;

    public List<Vector2> objectTileList = new List<Vector2>(); //타일 위치
    public Vector2 objectLeftBottomPos; //오브젝트의 좌측 하단
    public Vector2 objectPickUpPos; //타일을 잡는 장소 위치
    public Vector2 objectRotAxis; //타일의 회전 중심축
}
