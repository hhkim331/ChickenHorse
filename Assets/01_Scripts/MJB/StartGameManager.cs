using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//타이머 UI
//시간이 지남에 따라 텍스트 UI 바뀜
//플레이어가 나가면 시간 0으로 초기화
public class StartGameManager : MonoBehaviourPun
{
    //시간을 증가시키는 카운트 다운을 만들고 싶다.
    public TextMeshProUGUI timerUI;

    //3초 시간이 제한
    private readonly float MAX_TIME = 3.4f, LIMIT_TIME = 0.7f;

    private readonly int NEXT_SCENE_NUMBER = 2;
    private readonly string START_GAME_TEXT = "게임 시작까지\n", HORSE_TAG_NAME = "Horse", CHICKEN_TAG_NAME = "Chicken", DEMICAL_POINT_TRUNCATION = "0";

    //3초 시간이 되었다는 체크, 플레이어가 들어갔나요?
    private bool hasLimit, hasPlayer;

    //시간에 대한 접근
    private float currentTime;

    // 딕셔너리를 사용하여 플레이어를 추가한다.
    private Dictionary<string, GameObject> playerTag = new Dictionary<string, GameObject>();

    private bool load = false;

    private void Start()
    {
        //씬이동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
        //나 자신을 끈다.
        timerUI.enabled = false;
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
        //딕셔너리에 내가 원하는 플레이어의 키가 있다면 UI를 실행시킨다.
        if (playerTag.ContainsKey(HORSE_TAG_NAME) && playerTag.ContainsKey(CHICKEN_TAG_NAME)) ReadyToPlay(true);

        //딕셔너리에 플레이어 키가 없다면 UI를 끈다.
        if (!playerTag.ContainsKey(HORSE_TAG_NAME) || !playerTag.ContainsKey(CHICKEN_TAG_NAME))
        {
            ReadyToPlay(false);
            //시간을 3초로 초기화 한다.
            currentTime = MAX_TIME;
        }
    }

    private void ReadyToPlay(bool isStarting)
    {
        hasPlayer = isStarting;
        //시간UI 비활성화 한다.
        timerUI.enabled = isStarting;
    }

    private void SetCountDown()
    {
        //플레이어가 있을때만 실행시킨다.
        if (hasPlayer)
        {
            //시간을 증가시켜 문자열로 변환한다.
            currentTime -= Time.deltaTime;
            //만약에 시간이 1초가 되었다면?
            if (currentTime < LIMIT_TIME)
            {
                //시간이 지났다는 체크를 한다.
                hasLimit = true;
                timerUI.enabled = false;
            }

            SetTimerText();
        }
    }

    private void SetTimerText()
    {
        string numberText = currentTime.ToString(DEMICAL_POINT_TRUNCATION);
        //UI에 시간을 넣는다.
        timerUI.text = START_GAME_TEXT + numberText;
    }

    private void ChangeScene()
    {
        load = true;
        //만약에 일정시간이 된다면 다음씬으로 이동한다.
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.LoadLevel(NEXT_SCENE_NUMBER);
        //SceneManager.LoadScene(NEXT_SCENE_NUMBER);
        //시간 UI 비활성화 한다.
        timerUI.enabled = false;
    }

    // 말 플레이어가 태그 되었다면 그 플레이어를 넣는다.
    // 치킨 플레이어가 태그 되었다면 그 플레이어를 넣는다.

    private void OnTriggerEnter(Collider other)
    {
        //말이 들어갔다면 말을 넣는다.
        if (other.CompareTag(HORSE_TAG_NAME)) playerTag[HORSE_TAG_NAME] = other.gameObject;
        //닭이 들어갔다면 닭을 넣는다.
        if (other.CompareTag(CHICKEN_TAG_NAME)) playerTag[CHICKEN_TAG_NAME] = other.gameObject;
    }

    //플레이어가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        //말이 나갔다면 말을 뺀다.
        if (other.CompareTag(HORSE_TAG_NAME)) playerTag.Remove(HORSE_TAG_NAME);
        //닭이 나갔다면 닭을 뺀다.
        if (other.CompareTag(CHICKEN_TAG_NAME)) playerTag.Remove(CHICKEN_TAG_NAME);
    }
}