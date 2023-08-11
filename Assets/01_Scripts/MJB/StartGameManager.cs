using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//타이머 UI
//시간이 지남에 따라 텍스트 UI 바뀜
//플레이어가 나가면 시간 0으로 초기화
public class StartGameManager : MonoBehaviour
{
    //시간을 증가시키는 카운트 다운을 만들고 싶다.
    public TextMeshProUGUI timerUI;

    //3초 시간이 제한
    private readonly float limitTime = 0.5f;

    //3초 시간이 되었다는 체크
    //플레이어가 들어갔나요?
    private bool hasLimit, hasPlayer;

    //시간에 대한 접근
    private float currentTime;

    private void Start()
    {
        //나 자신을 끈다.
        timerUI.enabled = false;
        // 처음 시간을 초기화
        currentTime = 3.4f;
    }

    private void Update()
    {
        //플레이어가 있을때만 실행시킨다.
        if (hasPlayer)
        {
            //시간을 증가시켜 문자열로 변환한다.
            currentTime -= Time.deltaTime;
            //만약에 시간이 1초가 되었다면?
            if (currentTime < limitTime)
            {
                //시간이 지났다는 체크를 한다.
                hasLimit = true;
                //시간을 3초로 초기화하고
                currentTime = 3.4f;
            }

            SetTimerText();
        }
        //일정 시간이 지난다면 씬을 바꾼다.
        if (hasLimit) ChangeScene();
    }

    private void SetTimerText()
    {
        //UI에 시간을 넣는다.
        timerUI.text = "게임 시작까지\n" + currentTime.ToString("0");
    }

    public void ChangeScene()
    {
        //만약에 일정시간이 된다면 다음씬으로 이동한다.
        SceneManager.LoadScene(1);
        //시간 UI 비활성화 한다.
        timerUI.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어 태그가 닿았다면
        if (other.CompareTag("Player"))
        {
            //플레이어가 들어가있다.
            hasPlayer = true;
            //시간 UI를 활성화 한다.
            timerUI.enabled = true;
        }
    }

    //플레이어가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        //나 자신을 끈다.
        if (other.CompareTag("Player"))
        {
            //플레이어가 없다.
            hasPlayer = false;
            //시간UI 비활성화 한다.
            timerUI.enabled = false;
            //시간을 3초로 초기화 한다.
            currentTime = 3.4f;
        }
    }
}