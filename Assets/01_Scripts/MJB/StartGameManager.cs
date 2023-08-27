using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//타이머 UI
//시간이 지남에 따라 텍스트 UI 바뀜
//플레이어가 나가면 시간 0으로 초기화
public class StartGameManager : MonoBehaviourPun
{
    //시간을 증가시키는 카운트 다운을 만들고 싶다.
    public TextMeshProUGUI timerUI;

    //Canvas group 컴포넌트를 public으로 가져오고 싶다.
    public CanvasGroup canvasGroup;

    //3초 시간이 제한
    private readonly float MAX_TIME = 3.4f, LIMIT_TIME = 0f;

    private readonly int NEXT_SCENE_NUMBER = 2;
    private readonly string START_GAME_TEXT = "파티 게임\n", DEMICAL_POINT_TRUNCATION = "0";

    //3초 시간이 되었다는 체크, 플레이어가 들어갔나요?
    private bool hasLimit, hasPlayer;

    //시간에 대한 접근
    private float currentTime;

    // 딕셔너리를 사용하여 플레이어를 추가한다.
    private int playerCount = 0;

    private bool load = false;

    private void Start()
    {
        //씬이동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SerializationRate = 20;
        //나 자신을 끈다.
        canvasGroup.alpha = 0;
        // 처음 시간을 초기화
        currentTime = MAX_TIME;
    }

    private void Update()
    {
        SetCountDown();
        //일정 시간이 지난다면 씬을 바꾼다.
        if (hasLimit && !load) ChangeScene();
        StartingGame();
    }

    private void StartingGame()
    {
        if (playerCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            ReadyToPlay(true, 1, 0.5f);
        }
        if (playerCount < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            ReadyToPlay(false, 0, 0.5f);
            //sfx 사운드를 끈다.
            SoundManager.Instance.StopSFX("CountStart");
            //시간을 3초로 초기화 한다.
            currentTime = MAX_TIME;
        }
    }

    private void ReadyToPlay(bool isStarting, float endValue, float duration)
    {
        hasPlayer = isStarting;
        //시간UI 비활성화 한다.
        canvasGroup.DOFade(endValue, duration);
    }

    private void SetCountDown()
    {
        //플레이어가 있을때만 실행시킨다.
        if (hasPlayer)
        {
            //시간을 증가시켜 문자열로 변환한다.
            currentTime -= Time.deltaTime;
            SoundManager.Instance.PlaySFXOnce("CountStart");

            if (currentTime < LIMIT_TIME)
            {
                //시간이 지났다는 체크를 한다.
                hasLimit = true;
                canvasGroup.alpha = 0;
            }

            SetTimerText();
        }
    }

    private void SetTimerText()
    {
        //문자열에 변수를 넣고 싶다면 ""앞에 $를 붙인다.
        string numberText = $"<size=270>{currentTime.ToString(DEMICAL_POINT_TRUNCATION)}</size>";

        //UI에 시간을 넣는다.
        timerUI.text = START_GAME_TEXT + numberText;
    }

    private void ChangeScene()
    {
        load = true;
        //만약에 일정시간이 된다면 다음씬으로 이동한다.
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            foreach (var player in PhotonNetwork.PlayerList)
                PhotonNetwork.RemoveRPCs(player);
            PhotonNetwork.LoadLevel(NEXT_SCENE_NUMBER);
        }
    }

    // 말 플레이어가 태그 되었다면 그 플레이어를 넣는다.
    // 치킨 플레이어가 태그 되었다면 그 플레이어를 넣는다.

    private void OnTriggerEnter(Collider other)
    {
        //말이 들어갔다면 말을 넣는다.
        if (other.CompareTag("Player")) playerCount++;
    }

    //플레이어가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerCount--;
    }
}