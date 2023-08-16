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
    public Camera cursorCamera;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    StageObject myObject;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -15);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X") * Time.deltaTime * 100;
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * 100;

        //마우스 좌표로 커서 이동
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = -15;
        transform.position = new Vector3(transform.position.x + x, transform.position.y + y, -15);
        if (MainGameManager.instance.state == MainGameManager.GameState.Select && !isSelect)
        {
            Vector3 screenPoint = cursorCamera.WorldToScreenPoint(transform.position);
            Vector3 rayPoint = partyBoxCamera.ScreenToWorldPoint(screenPoint);

            //현재 마우스가 올라와 있는 오브젝트를 가져옴
            RaycastHit hit;
            Physics.Raycast(rayPoint, Vector3.forward, out hit);
            if (hit.collider != null)
            {
                StageObject hitObject = hit.collider.GetComponentInParent<StageObject>();
                if (hitObject != null)
                {
                    if (!hitObject.IsPlace)
                    {
                        if (myObject != hitObject)
                            myObject?.Focus(false);
                        myObject = hitObject;
                        myObject.Focus(true);
                    }
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
        else if (MainGameManager.instance.state == MainGameManager.GameState.Place && !isPlace)
        {
            myObject.Move(new Vector2(transform.position.x, transform.position.y));
            if (myObject.CanPlace)
                spriteRenderer.sprite = sprites[0];
            else
                spriteRenderer.sprite = sprites[1];


            if (Input.GetMouseButtonDown(0) && myObject.CanPlace)   //배치
            {
                myObject.Place();
                MainGameManager.instance.AddStageObject(myObject);
                myObject = null;
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
        if (MainGameManager.instance.state == MainGameManager.GameState.Select)
            transform.localScale = Vector3.one * 7.5f;
        else if (MainGameManager.instance.state == MainGameManager.GameState.Place)
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
