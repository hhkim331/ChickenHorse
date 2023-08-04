using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public StageObjectData objectData;

    public Transform cursor;    //현재 나를 잡고 있는 커서
    bool isPlace = false;   //배치되었는지 여부

    // Start is called before the first frame update
    void Start()
    {
        if (objectData.objectType == StageObjectData.ObjectType.Fixed)
        {
            if (objectData.objectTileList.Count == 0)
            {
                for (int i = 0; i < objectData.objectSize.x; i++)
                    for (int j = 0; j < objectData.objectSize.y; j++)
                        objectData.objectTileList.Add(new Vector2(i, j));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectData.objectType == StageObjectData.ObjectType.Fixed) return;

        if (isPlace == false)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.X))   //배치
            {
                isPlace = true;
                Place();
            }

            if (Input.GetKeyDown(KeyCode.C))    //회전
            {
                switch (objectData.objectRotType)
                {
                    case StageObjectData.ObjectRotType.Spin:
                        Spin();
                        break;
                    case StageObjectData.ObjectRotType.FlipY:
                        FlipY();
                        break;
                }
            }
        }
    }

    public void Move()  //이동
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursor.position);
        Vector2 newPos;

        //소숫점이 0.5이상이면 올림, 0.5미만이면 내림
        if (cursorPos.x - (int)cursorPos.x >= 0.5f)
            newPos.x = (int)cursorPos.x + 1;
        else
            newPos.x = (int)cursorPos.x;

        if (cursorPos.y - (int)cursorPos.y >= 0.5f)
            newPos.y = (int)cursorPos.y + 1;
        else
            newPos.y = (int)cursorPos.y;

        transform.position = newPos - objectData.objectPickUpPos;
    }

    public void Spin()  //회전
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            //회전축을 기준으로 좌표를 오른쪽 90도 회전 좌표로 변경
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            //tilePos를 오른쪽 90도 회전
            Vector2 newTilePos = new Vector2(tilePos.y, -tilePos.x);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void FlipY() //Y축 뒤집기
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            //회전축을 기준으로 좌표를 오른쪽 90도 회전 좌표로 변경
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            //tilePos를 오른쪽 90도 회전
            Vector2 newTilePos = new Vector2(-tilePos.x, tilePos.y);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    //오브젝트 배치
    public void Place()
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = new Vector2(transform.position.x + objectData.objectLeftBottomPos.x + objectData.objectTileList[i].x,
                transform.position.y + objectData.objectLeftBottomPos.y + objectData.objectTileList[i].y);
            MapManager.instance.mapObjectDic[tilePos] = this;
            Debug.Log(tilePos);
        }
    }
}
