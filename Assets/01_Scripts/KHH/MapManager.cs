using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Header("�� ������")]
    public Vector2 mapSize; //�� ũ��
    public StageObject startBlock;  //���� Ÿ�� ������
    public StageObject endBlock;    //���� Ÿ�� ������

    [Header("���� Ÿ��")]
    public StageObject[] fixedObjects; //���� Ÿ�� ������

    //���� Ÿ�� ������
    public Dictionary<Vector2, StageObject> mapObjectDic = new Dictionary<Vector2, StageObject>();  //�� Ÿ���� ������ �ִ� ������Ʈ ������
    List<StageObject> objectList = new List<StageObject>();    //���� ��ġ�Ǿ��ִ� ������Ʈ�� ����Ʈ

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�� �����͸� ������� �ʹ� ��ġ
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                mapObjectDic.Add(new Vector2(i, j), null);
            }
        }

        startBlock.Place();
        endBlock.Place();
        for (int i = 0; i < fixedObjects.Length; i++)
        {
            fixedObjects[i].Place();
            objectList.Add(fixedObjects[i]);
        }

        //mapData.fixedObjectDatas.Length
    }

    // Update is called once per frame
    void Update()
    {

    }
}
