using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCursor : MonoBehaviourPun, IPunObservable
{
    bool isActive;
    public bool IsActive { get { return isActive; } }
    bool isSelect = false;
    public bool IsSelect { get { return isSelect; } }
    bool isPlace = false;
    public bool IsPlace { get { return isPlace; } }

    Camera mainCamera;
    Camera partyBoxCamera;
    Camera cursorCamera;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;
    StageObject myObject;

    //네트워크
    Vector3 myPosition;

    public void Init(Camera main, Camera partyBox, Camera cursor)
    {
        mainCamera = main;
        partyBoxCamera = partyBox;
        cursorCamera = cursor;

        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -15);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        if (photonView.IsMine)
        {
            float x = Input.GetAxis("Mouse X") * Time.deltaTime * 100;
            float y = Input.GetAxis("Mouse Y") * Time.deltaTime * 100;
            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, -15);

            //마우스 좌표로 커서 이동
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mousePos.z = -15;
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
                    myObject.Select(transform, photonView.Owner);
                    isSelect = true;
                    Active(false);
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
                    Active(false);
                }

                if (Input.GetMouseButtonDown(1))    //회전
                {
                    myObject.Rotate();
                }
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, myPosition, Time.deltaTime * 10);
        }
    }

    public void Set()
    {
        isSelect = false;
        isPlace = false;
        myObject = null;
    }

    //커서 활성화
    public void Active(bool active)
    {
        if (active)
        {
            isActive = true;
            spriteRenderer.gameObject.SetActive(true);
            if (MainGameManager.instance.state == MainGameManager.GameState.Select)
                transform.localScale = Vector3.one * 7.5f;
            else if (MainGameManager.instance.state == MainGameManager.GameState.Place)
            {
                transform.localScale = Vector3.one * 5f;
                myObject.Active(true);
            }
        }
        else
        {
            isActive = false;
            spriteRenderer.gameObject.SetActive(false);
        }

        photonView.RPC(nameof(ActiveRPC), RpcTarget.Others, active);
    }

    [PunRPC]
    void ActiveRPC(bool active)
    {
        if (active)
        {
            isActive = true;
            spriteRenderer.gameObject.SetActive(true);
            if (MainGameManager.instance.state == MainGameManager.GameState.Select)
                transform.localScale = Vector3.one * 7.5f;
            else if (MainGameManager.instance.state == MainGameManager.GameState.Place)
                transform.localScale = Vector3.one * 5f;
        }
        else
        {
            isActive = false;
            spriteRenderer.gameObject.SetActive(false);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)   //데이터를 보내는 중
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(isActive);
            stream.SendNext(isSelect);
            stream.SendNext(isPlace);
        }
        else    //데이터를 받는 중
        {
            myPosition = (Vector3)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            isActive = (bool)stream.ReceiveNext();
            isSelect = (bool)stream.ReceiveNext();
            isPlace = (bool)stream.ReceiveNext();
        }
    }
}
