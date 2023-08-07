using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBox : MonoBehaviour
{
    TotalStageObjectData totalStageObjectData;
    List<StageObject> stageObjects;

    [Header("자리배치")]
    [SerializeField] Transform[] fourSeats;
    [SerializeField] Transform[] fiveSeats;
    [SerializeField] Transform[] sixSeats;

    [Header("연출용")] //나중에 애니메이터로 바꿔야함
    [SerializeField] Transform topLeft;
    [SerializeField] Transform topRight;
    [SerializeField] Transform topBlankLeft;
    [SerializeField] Transform topBlankRight;

    public void Init()
    {
        totalStageObjectData = Resources.Load<TotalStageObjectData>("ScriptableObject/TotalStageObjectData");
        stageObjects = new List<StageObject>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void SetBox()
    {
        int randCount = Random.Range(4, 7);
        for (int i = 0; i < randCount; i++)
        {
            int randItem = Random.Range(0, totalStageObjectData.stageObjectDataList.Count);
            GameObject go = Instantiate(totalStageObjectData.stageObjectDataList[randItem], transform);
            go.transform.localScale = Vector3.one * 0.5f;
            //자리배치
            if (randCount == 4)
                go.transform.localPosition = fourSeats[i].localPosition;
            else if (randCount == 5)
                go.transform.localPosition = fiveSeats[i].localPosition;
            else if (randCount == 6)
                go.transform.localPosition = sixSeats[i].localPosition;

            StageObject stageObject = go.GetComponent<StageObject>();
            stageObject.Set();
            stageObjects.Add(stageObject);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        topLeft.gameObject.SetActive(false);
        topRight.gameObject.SetActive(false);
        topBlankLeft.gameObject.SetActive(false);
        topBlankRight.gameObject.SetActive(false);

        KHHGameManager.instance.cursor.Active();
        //이건 애니메이터로 하는게 맞는듯
    }

    public void Close()
    {
        gameObject.SetActive(false);

        //이건 애니메이터로 하는게 맞는듯

        //남은 오브젝트 제거
        RemoveItemAll();
    }

    /// <summary>
    /// 선택된 아이템 리스트에서 제거
    /// </summary>
    public void RemoveItem(StageObject stageObject)
    {
        stageObjects.Remove(stageObject);
    }

    /// <summary>
    /// 리스트 전체 제거
    /// </summary>
    void RemoveItemAll()
    {
        foreach (var item in stageObjects)
        {
            Destroy(item.gameObject);
        }
        stageObjects.Clear();
    }
}
