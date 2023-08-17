using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviourPun, IPunObservable
{
    public StageObjectData objectData;

    public Transform cursor;    //현재 나를 잡고 있는 커서
    bool isFocus = false;   //현재 커서가 나를 잡고 있는지 여부

    Vector2 moveCurPos = Vector2.zero;
    bool canPlace = false;  //배치 가능 여부
    public bool CanPlace { get { return canPlace; } }
    bool isPlace = false;   //배치되었는지 여부
    public bool IsPlace { get { return isPlace; } }
    bool isPlay = false;   //배치되었는지 여부
    public bool IsPlay { get { return isPlay; } }

    [SerializeField] Animator[] animators;
    public Transform rendererTransform; //스케일 조정을 위한 현재 오브젝트의 메쉬
    Vector3 rendererDefaultScale;   //메쉬의 기본 스케일

    public void FixedSet()
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
        //if (rendererTransform != null)
        //    rendererDefaultScale = rendererTransform.localScale;
    }

    public void Set(Vector3 newPos)
    {
        photonView.RPC(nameof(SetRPC), RpcTarget.All, newPos);
    }

    [PunRPC]
    void SetRPC(Vector3 newPos)
    {
        transform.localScale = Vector3.one * 0.75f;
        transform.position = newPos;

        if (rendererTransform != null)
            rendererDefaultScale = rendererTransform.localScale;

        //해당 오브젝트의 모든 레이어를 PartyBox로 변경
        foreach (Transform item in GetComponentsInChildren<Transform>())
            item.gameObject.layer = LayerMask.NameToLayer("PartyBox");
    }

    /// <summary>
    /// 포커스
    /// </summary>
    public void Focus(bool focus)
    {
        if (objectData.objectType == StageObjectData.ObjectType.Fixed) return;
        if (isFocus == focus) return;
        isFocus = focus;
        photonView.RPC(nameof(FocusRPC), RpcTarget.All, focus);
    }

    [PunRPC]
    void FocusRPC(bool focus)
    {
        isFocus = focus;
        if (focus)
        {
            transform.DOKill();
            if (rendererTransform != null)
                rendererTransform.DOScale(rendererDefaultScale * 1.2f, 0.3f);

            if (animators.Length > 0)
            {
                foreach (var anim in animators)
                {
                    anim.enabled = true;
                }
            }
        }
        else
        {
            transform.DOKill();
            if (rendererTransform != null)
                rendererTransform.DOScale(rendererDefaultScale, 0.3f);

            if (animators.Length > 0)
            {
                foreach (var anim in animators)
                {
                    anim.Rebind();
                    anim.enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// 선택
    /// </summary>
    public void Select(Transform cursorTr)
    {
        if (objectData.objectType == StageObjectData.ObjectType.Fixed) return;
        //photonView.RPC(nameof(SelectRPC), RpcTarget.All, cursorTr);
    }

    [PunRPC]
    void SelectRPC(Transform cursorTr)
    {
        cursor = cursorTr;
        transform.parent = null;
        transform.localScale = Vector3.one;
        rendererTransform.DOKill();
        rendererTransform.localScale = rendererDefaultScale;
        gameObject.SetActive(false);

        //isFocus = false;
        //크기 애니메이션 상태 초기화

        //해당 오브젝트의 모든 레이어를 Default로 변경
        foreach (Transform item in GetComponentsInChildren<Transform>())
            item.gameObject.layer = LayerMask.NameToLayer("Default");

        MainGameManager.instance.partyBox.RemoveItem(this);

        if (animators.Length > 0)
        {
            foreach (var anim in animators)
            {
                anim.Rebind();
                anim.enabled = false;
            }
        }
    }

    /// <summary>
    /// 이동
    /// </summary>
    public void Move(Vector2 cursorPos)
    {
        Vector2 moveNewPos;
        //소숫점이 0.5이상이면 올림, 0.5미만이면 내림
        if (cursorPos.x - (int)cursorPos.x >= 0.5f)
            moveNewPos.x = (int)cursorPos.x + 1;
        else
            moveNewPos.x = (int)cursorPos.x;

        if (cursorPos.y - (int)cursorPos.y >= 0.5f)
            moveNewPos.y = (int)cursorPos.y + 1;
        else
            moveNewPos.y = (int)cursorPos.y;

        if (moveCurPos != moveNewPos)
        {
            moveCurPos = moveNewPos;
            transform.position = moveNewPos - objectData.objectPickUpPos;
            //배치 가능 체크
            if (CheckCanPlace())
            {
                canPlace = true;
                if (rendererTransform != null)
                {
                    MeshRenderer[] meshRenderers = rendererTransform.GetComponentsInChildren<MeshRenderer>();
                    if (meshRenderers != null)
                        foreach (MeshRenderer meshRenderer in meshRenderers)
                            meshRenderer.material.color = Color.white;
                    SpriteRenderer[] spriteRenderers = rendererTransform.GetComponentsInChildren<SpriteRenderer>();
                    if (spriteRenderers != null)
                        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                            spriteRenderer.material.color = Color.white;
                }
            }
            else
            {
                canPlace = false;
                if (rendererTransform != null)
                {
                    MeshRenderer meshRenderer = rendererTransform.GetComponent<MeshRenderer>();
                    if (meshRenderer != null) meshRenderer.material.color = Color.red;
                    SpriteRenderer spriteRenderer = rendererTransform.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null) spriteRenderer.material.color = Color.red;
                }
            }
        }
    }

    bool CheckCanPlace()
    {
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = new Vector2(transform.position.x + objectData.objectLeftBottomPos.x + objectData.objectTileList[i].x,
                transform.position.y + objectData.objectLeftBottomPos.y + objectData.objectTileList[i].y);

            if (tilePos.x < 0 || tilePos.x >= MapManager.instance.mapSize.x) return false;
            if (tilePos.y < 0 || tilePos.y >= MapManager.instance.mapSize.y) return false;
            if (MapManager.instance.mapObjectDic[tilePos] != null) return false;
        }

        return true;
    }

    /// <summary>
    /// 회전
    /// </summary>
    public void Rotate()
    {
        photonView.RPC(nameof(RotateRPC), RpcTarget.All);
    }

    [PunRPC]
    void RotateRPC()
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

    public void FixedPlace()
    {
        isPlace = true;
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = new Vector2(transform.position.x + objectData.objectLeftBottomPos.x + objectData.objectTileList[i].x,
                transform.position.y + objectData.objectLeftBottomPos.y + objectData.objectTileList[i].y);
            MapManager.instance.mapObjectDic[tilePos] = this;
        }
    }

    /// <summary>
    /// 오브젝트 배치
    /// </summary>
    public void Place()
    {
        photonView.RPC(nameof(PlaceRPC), RpcTarget.All, transform.position);
    }

    [PunRPC]
    void PlaceRPC(Vector3 pos)
    {
        isPlace = true;
        transform.position = pos;
        for (int i = 0; i < objectData.objectTileList.Count; i++)
        {
            Vector2 tilePos = new Vector2(pos.x + objectData.objectLeftBottomPos.x + objectData.objectTileList[i].x,
                pos.y + objectData.objectLeftBottomPos.y + objectData.objectTileList[i].y);
            MapManager.instance.mapObjectDic[tilePos] = this;
        }

        //해당 오브젝트의 모든 레이어를 Default로 변경
        foreach (Transform item in GetComponentsInChildren<Transform>())
            item.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    /// <summary>
    /// 오브젝트 동작
    /// </summary>
    public void Play()
    {
        photonView.RPC(nameof(PlayRPC), RpcTarget.All);
    }

    [PunRPC]
    void PlayRPC()
    {
        isPlay = true;
        if (animators.Length > 0)
        {
            foreach (var anim in animators)
            {
                anim.enabled = true;
            }
        }
    }

    /// <summary>
    /// 오브젝트 동작 정지
    /// </summary>
    public void Stop()
    {
        photonView.RPC(nameof(StopRPC), RpcTarget.All);
    }

    [PunRPC]
    void StopRPC()
    {
        isPlay = false;
        if (animators.Length > 0)
        {
            foreach (var anim in animators)
            {
                anim.Rebind();
                anim.enabled = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //데이터를 보내는 중
        {
            stream.SendNext(transform.position);
        }
        else //데이터를 받는 중
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
