using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public StageObjectData objectData;

    public Transform cursor;    //현재 나를 잡고 있는 커서
    bool isFocus = false;   //현재 커서가 나를 잡고 있는지 여부
    bool isPlace = false;   //배치되었는지 여부

    public Transform meshTransform; //스케일 조정을 위한 현재 오브젝트의 메쉬
    Vector3 meshDefaultScale;   //메쉬의 기본 스케일

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

        if (meshTransform != null)
            meshDefaultScale = meshTransform.localScale;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (objectData.objectType == StageObjectData.ObjectType.Fixed) return;

    //    if (KHHGameManager.instance.state == KHHGameManager.GameState.Select)
    //    {
    //        //Select();
    //    }
    //    else if (KHHGameManager.instance.state == KHHGameManager.GameState.Place && !isPlace)
    //    {
    //        //Place();
    //        if (isPlace == false)
    //        {
    //            Move();
    //        }
    //    }
    //}

    /// <summary>
    /// 포커스
    /// </summary>
    public void Focus(bool isFocus)
    {
        this.isFocus = isFocus;
        if (isFocus)
        {
            transform.DOKill();
            if (meshTransform != null)
                meshTransform.DOScale(meshDefaultScale * 1.2f, 0.3f);
        }
        else
        {
            transform.DOKill();
            if (meshTransform != null)
                meshTransform.DOScale(meshDefaultScale, 0.3f);
        }
    }

    /// <summary>
    /// 선택
    /// </summary>
    public void Select(Transform cursorTr)
    {
        cursor = cursorTr;
        transform.parent = null;
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);

        isFocus = false;
        //크기 애니메이션 상태 초기화

        KHHGameManager.instance.partyBox.RemoveItem(this);
    }

    /// <summary>
    /// 이동
    /// </summary>
    public void Move(Vector2 cursorPos)
    {
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

    /// <summary>
    /// 회전
    /// </summary>
    public void Rotate()
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

    /// <summary>
    /// 오른쪽 90도 회전
    /// </summary>
    void Spin()
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

    /// <summary>
    /// Y축 뒤집기
    /// </summary>
    void FlipY()
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            Vector2 newTilePos = new Vector2(-tilePos.x, tilePos.y);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        //y축 180도 회전
        transform.Rotate(new Vector3(0, 180, 0));
    }

    /// <summary>
    /// 오브젝트 배치
    /// </summary>
    public void Place()
    {
        isPlace = true;
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = new Vector2(transform.position.x + objectData.objectLeftBottomPos.x + objectData.objectTileList[i].x,
                transform.position.y + objectData.objectLeftBottomPos.y + objectData.objectTileList[i].y);
            MapManager.instance.mapObjectDic[tilePos] = this;
        }
    }
}
