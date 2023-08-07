using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScoreManager;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public enum ScoreType
    {
        Goal,
        Solo,
        Death,
        Trap,
        Coin,
        Comeback,
        First,
    }

    const int maxScore = 25;
    const int goal = 5;
    const int solo = 3;
    const int death = 2;
    const int trap = 1;
    const int coin = 3;
    const int comeback = 4;
    const int first = 1;

    // 플레이어 점수
    Dictionary<GameObject, int> playerScore = new Dictionary<GameObject, int>();
    // 스테이지 플레이어별 계산 점수
    Dictionary<GameObject, List<ScoreType>> playerScoreDic = new Dictionary<GameObject, List<ScoreType>>();

    //테스트용 플레이어
    public GameObject playerTest;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //플레이어 수에 맞게 초기화
        List<ScoreType> scoreList = new List<ScoreType>();
        playerScoreDic.Add(playerTest, scoreList);
    }

    public void GetScore(GameObject player, ScoreType scoreType)
    {
        playerScoreDic[player].Add(scoreType);
    }

    //점수 정산
    public void ScoreCalc()
    {
        StartCoroutine(ScoreCalcCoroutine());
    }

    IEnumerator ScoreCalcCoroutine()
    {
        yield return null;
        //각각의 state에 맞게 점수계산
        //게임이 종료된 경우
        if (CheckGameEnd())
            KHHGameManager.instance.ChangeState(KHHGameManager.GameState.End);
        else
            KHHGameManager.instance.ChangeState(KHHGameManager.GameState.Select);
    }

    bool CheckGameEnd()
    {
        //점수가 25점 이상인 플레이어가 있는지 체크
        foreach (var item in playerScore)
        {
            if (item.Value >= maxScore)
            {
                return true;
            }
        }

        return false;
    }
}
