using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCursor : MonoBehaviour
{
    bool isSelect = false;
    public bool IsSelect { get { return isSelect; } }
    bool isPlace = false;
    public bool IsPlace { get { return isPlace; } }

    public Camera mainCamera;
    public Camera partyBoxCamera;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    StageObject myObject;


    // Update is called once per frame
    void Update()
    {
        //마우스 좌표로 커서 이동
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -15;
        transform.position = mousePos;
        if (KHHGameManager.instance.state == KHHGameManager.GameState.Select && !isSelect)
        {
            //현재 마우스가 올라와 있는 오브젝트를 가져옴
            RaycastHit hit;
            Physics.Raycast(partyBoxCamera.ScreenPointToRay(Input.mousePosition), out hit);
            StageObject hitObject = null;
            if (hit.collider != null)
            {
                hitObject = hit.collider.GetComponentInParent<StageObject>();
                if (!hitObject.IsPlace)
                {
                    if (myObject != hitObject)
                        myObject?.Focus(false);
                    myObject = hitObject;
                    myObject.Focus(true);
                }
            }
            else
            {
                if (myObject != null)
                {
                    myObject.Focus(false);
                    myObject = null;
                }
            }

            //선택
            if (myObject != null && Input.GetMouseButtonDown(0))
            {
                myObject.Select(transform);
                isSelect = true;
                Deactive();
            }
        }
        else if (KHHGameManager.instance.state == KHHGameManager.GameState.Place && !isPlace)
        {
            myObject.Move(new Vector2(transform.position.x, transform.position.y));
            if (myObject.CanPlace)
                spriteRenderer.sprite = sprites[0];
            else
                spriteRenderer.sprite = sprites[1];


            if (Input.GetMouseButtonDown(0) && myObject.CanPlace)   //배치
            {
                myObject.Place();
                isPlace = true;
                Deactive();
            }

            if (Input.GetMouseButtonDown(1))    //회전
            {
                myObject.Rotate();
            }
        }
    }

    public void Set()
    {
        isSelect = false;
        isPlace = false;
        myObject = null;
    }

    //커서 활성화
    public void Active()
    {
        gameObject.SetActive(true);
        if (KHHGameManager.instance.state == KHHGameManager.GameState.Select)
            transform.localScale = Vector3.one * 7.5f;
        else if (KHHGameManager.instance.state == KHHGameManager.GameState.Place)
        {
            transform.localScale = Vector3.one * 5f;
            myObject.gameObject.SetActive(true);
        }
    }

    //커서 비활성화
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
