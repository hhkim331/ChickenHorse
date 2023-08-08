using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public StageObjectData objectData;

<<<<<<< Updated upstream
    public Transform cursor;    //ÇöÀç ³ª¸¦ Àâ°í ÀÖ´Â Ä¿¼­
    bool isPlace = false;   //¹èÄ¡µÇ¾ú´ÂÁö ¿©ºÎ
=======
    public Transform cursor;    //í˜„ìž¬ ë‚˜ë¥¼ ìž¡ê³  ìžˆëŠ” ì»¤ì„œ
    private bool isFocus = false;   //í˜„ìž¬ ì»¤ì„œê°€ ë‚˜ë¥¼ ìž¡ê³  ìžˆëŠ”ì§€ ì—¬ë¶€
    private bool isPlace = false;   //ë°°ì¹˜ë˜ì—ˆëŠ”ì§€ ì—¬ë¶€

    public Transform meshTransform; //ìŠ¤ì¼€ì¼ ì¡°ì •ì„ ìœ„í•œ í˜„ìž¬ ì˜¤ë¸Œì íŠ¸ì˜ ë©”ì‰¬
    Vector3 meshDefaultScale;   //ë©”ì‰¬ì˜ ê¸°ë³¸ ìŠ¤ì¼€ì¼
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
            if (Input.GetKeyDown(KeyCode.X))   //¹èÄ¡
            {
                isPlace = true;
                Place();
            }

            if (Input.GetKeyDown(KeyCode.C))    //È¸Àü
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

    public void Move()  //ÀÌµ¿
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursor.position);
        Vector2 newPos;

        //¼Ò¼ýÁ¡ÀÌ 0.5ÀÌ»óÀÌ¸é ¿Ã¸², 0.5¹Ì¸¸ÀÌ¸é ³»¸²
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

    public void Spin()  //È¸Àü
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            //È¸ÀüÃàÀ» ±âÁØÀ¸·Î ÁÂÇ¥¸¦ ¿À¸¥ÂÊ 90µµ È¸Àü ÁÂÇ¥·Î º¯°æ
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            //tilePos¸¦ ¿À¸¥ÂÊ 90µµ È¸Àü
            Vector2 newTilePos = new Vector2(tilePos.y, -tilePos.x);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void FlipY() //YÃà µÚÁý±â
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            //È¸ÀüÃàÀ» ±âÁØÀ¸·Î ÁÂÇ¥¸¦ ¿À¸¥ÂÊ 90µµ È¸Àü ÁÂÇ¥·Î º¯°æ
            Vector2 tilePos = objectData.objectTileList[i] - objectData.objectRotAxis;

            //tilePos¸¦ ¿À¸¥ÂÊ 90µµ È¸Àü
            Vector2 newTilePos = new Vector2(-tilePos.x, tilePos.y);
            objectData.objectTileList[i] = newTilePos + objectData.objectRotAxis;
        }

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    //¿ÀºêÁ§Æ® ¹èÄ¡
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
