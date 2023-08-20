using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBox : MonoBehaviour
{
    TotalStageObjectData totalStageObjectData;
    List<StageObject> stageObjects;

    [SerializeField] GameObject box;

    [Header("자리배치")]
    [SerializeField] Transform[] fourSeats;
    [SerializeField] Transform[] fiveSeats;
    [SerializeField] Transform[] sixSeats;

    [Header("연출용")]
    [SerializeField] Transform topLeft;
    [SerializeField] Transform topRight;
    [SerializeField] Transform topBlankLeft;
    [SerializeField] Transform topBlankRight;

    Animator animator;

    public void Init()
    {
        totalStageObjectData = Resources.Load<TotalStageObjectData>("ScriptableObject/TotalStageObjectData");
        stageObjects = new List<StageObject>();
        animator = GetComponentInChildren<Animator>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //}

    public void SetBox()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int randCount = Random.Range(4, 7);
            for (int i = 0; i < randCount; i++)
            {
                int randItem = Random.Range(0, totalStageObjectData.stageObjectDataList.Count);
                GameObject go = PhotonNetwork.Instantiate(totalStageObjectData.stageObjectDataList[randItem].name, Vector3.zero, Quaternion.identity);

                Vector3 newPos = Vector3.zero;
                //자리배치
                if (randCount == 4)
                    newPos = fourSeats[i].position;
                else if (randCount == 5)
                    newPos = fiveSeats[i].position;
                else if (randCount == 6)
                    newPos = sixSeats[i].position;

                StageObject stageObject = go.GetComponent<StageObject>();
                stageObject.Set(newPos);
                stageObjects.Add(stageObject);
            }
        }

        Open();
    }

    void Open()
    {
        box.SetActive(true);
        topLeft.gameObject.SetActive(false);
        topRight.gameObject.SetActive(false);
        topBlankLeft.gameObject.SetActive(false);
        topBlankRight.gameObject.SetActive(false);

        //애니메이션
        animator.SetTrigger("Open");
    }

    public void BoxShake()
    {
        SoundManager.Instance.PlaySFX("Shake");
    }

    public void ActiveCursor()
    {
        MainGameManager.instance.MyCursor.Active(true);
    }

    public void Close()
    {
        animator.SetTrigger("Close");
    }

    /// <summary>
    /// 선택된 아이템 리스트에서 제거
    /// </summary>
    public void RemoveItem(StageObject stageObject)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            stageObjects.Remove(stageObject);
    }

    /// <summary>
    /// 리스트 전체 제거
    /// </summary>
    void RemoveItemAll()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            foreach (var item in stageObjects)
            {
                PhotonNetwork.Destroy(item.gameObject);
            }
            stageObjects.Clear();
        }

        box.SetActive(false);
    }
}
