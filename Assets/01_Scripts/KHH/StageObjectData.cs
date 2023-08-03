using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageObjectData
{
    //Ÿ��
    public enum ObjectType
    {
        Fixed,  //����������Ʈ
        Normal, //��ġ���� ������Ʈ
        Cover,  //�⺻ ������Ʈ�� ������ ������Ʈ
        Top,    //�⺻ ������Ʈ ���� ��ġ�Ǵ� ������Ʈ
    }
    public ObjectType objectType = ObjectType.Fixed;

    //ȸ��
    public enum ObjectRotType
    {
        None,
        Spin,
        FlipY,
    }
    public ObjectRotType objectRotType = ObjectRotType.None;

    public Vector2 objectSize;

    public List<Vector2> objectTileList = new List<Vector2>(); //Ÿ�� ��ġ
    public Vector2 objectLeftBottomPos; //������Ʈ�� ���� �ϴ�
    public Vector2 objectPickUpPos; //Ÿ���� ��� ��� ��ġ
    public Vector2 objectRotAxis; //Ÿ���� ȸ�� �߽���
}
