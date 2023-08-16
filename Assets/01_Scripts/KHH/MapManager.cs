using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Header("맵 데이터")]
    public Vector2 mapSize; //맵 크기
    public StageObject startBlock;  //고정 타일 데이터
    public StageObject endBlock;    //고정 타일 데이터

    [Header("고정 타일")]
    public StageObject[] fixedObjects; //고정 타일 데이터

    //현재 타일 데이터
    public Dictionary<Vector2, StageObject> mapObjectDic = new Dictionary<Vector2, StageObject>();  //각 타일이 가지고 있는 오브젝트 데이터
    List<StageObject> objectList = new List<StageObject>();    //현재 배치되어있는 오브젝트의 리스트

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //맵 데이터를 기반으로 초반 배치
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                mapObjectDic.Add(new Vector2(i, j), null);
            }
        }

        startBlock.FixedSet();
        startBlock.FixedPlace();
        endBlock.FixedSet();
        endBlock.FixedPlace();
        for (int i = 0; i < fixedObjects.Length; i++)
        {
            fixedObjects[i].FixedSet();
            fixedObjects[i].FixedPlace();
            objectList.Add(fixedObjects[i]);
        }

        //mapData.fixedObjectDatas.Length
    }

    // Update is called once per frame
    void Update()
    {

    }
}
