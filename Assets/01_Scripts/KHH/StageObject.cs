using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public StageObjectData objectData;

<<<<<<< Updated upstream
    public Transform cursor;    //���� ���� ��� �ִ� Ŀ��
    bool isPlace = false;   //��ġ�Ǿ����� ����
=======
    public Transform cursor;    //현재 나를 잡고 있는 커서
    private bool isFocus = false;   //현재 커서가 나를 잡고 있는지 여부
    private bool isPlace = false;   //배치되었는지 여부

    public Transform meshTransform; //스케일 조정을 위한 현재 오브젝트의 메쉬
    Vector3 meshDefaultScale;   //메쉬의 기본 스케일
>>>>>>> Stashed changes

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
            if (Input.GetKeyDown(KeyCode.X))   //��ġ
            {
                isPlace = true;
                Place();
            }

            if (Input.GetKeyDown(KeyCode.C))    //ȸ��
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

    public void Move()  //�̵�
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursor.position);
        Vector2 newPos;

        //�Ҽ����� 0.5�̻��̸� �ø�, 0.5�̸��̸� ����
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

    public void Spin()  //ȸ��
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            //ȸ������ �������� ��ǥ�� ������ 90�� ȸ�� ��ǥ�� ����
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            //tilePos�� ������ 90�� ȸ��
            Vector2 newTilePos = new Vector2(tilePos.y, -tilePos.x);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void FlipY() //Y�� ������
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            //ȸ������ �������� ��ǥ�� ������ 90�� ȸ�� ��ǥ�� ����
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            //tilePos�� ������ 90�� ȸ��
            Vector2 newTilePos = new Vector2(-tilePos.x, tilePos.y);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    //������Ʈ ��ġ
    public void Place()
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = new Vector2(transform.position.x + objectData.objectLeftBottomPos.x + objectData.objectTileList[i].x,
                transform.position.y + objectData.objectLeftBottomPos.y + objectData.objectTileList[i].y);
            MapManager.instance.mapObjectDic[tilePos] = this;
        }
    }
}
