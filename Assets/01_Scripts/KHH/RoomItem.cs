using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    Button button;
    string roomName;
    UnityAction<string> joinAction;

    [SerializeField] TextMeshProUGUI numText;
    [SerializeField] TextMeshProUGUI regionText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI matchText;
    [SerializeField] TextMeshProUGUI modeText;
    [SerializeField] TextMeshProUGUI tagText;
    [SerializeField] TextMeshProUGUI hostText;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI roundText;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(JoinRoom);
    }

    public void SetInfo(int roomNum, int users, string match, string hostName, UnityAction<string> action)
    {
        numText.text = roomNum.ToString() + ".";
        regionText.text = "한국";
        playerText.text = users.ToString() + "/4";
        matchText.text = match;
        modeText.text = "파티";
        tagText.text = "재미";
        hostText.text = hostName;
        pointText.text = "25";
        roundText.text = "없음";

        roomName = hostName;
        joinAction = action;
    }

    void JoinRoom()
    {
        joinAction?.Invoke(roomName);
    }
}
