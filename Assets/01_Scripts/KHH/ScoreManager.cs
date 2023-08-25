using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Point
{
    public enum PointType
    {
        Goal,
        Solo,
        Death,
        Trap,
        Coin,
        Comeback,
        First,
        //Second,
        //Third,
        //Fourth,
        Length
    }

    public int point;
    public Color color;
    public string name;

    public Point(int point, Color color, string name)
    {
        this.point = point;
        this.color = color;
        this.name = name;
    }
}

public class ScoreManager : MonoBehaviourPun
{
    readonly int maxScore = 25;

    readonly int goal = 5;
    readonly int solo = 3;
    readonly int death = 2;
    readonly int trap = 1;
    readonly int coin = 3;
    readonly int comeback = 4;
    readonly int first = 1;

    Color goalColor = new Color(0f, 0f, 0.5f);
    Color soloColor = Color.blue;
    Color deathColor = Color.black;
    Color trapColor = new Color(1f, 0.5f, 0f);
    Color coinColor = Color.yellow;
    Color comebackColor = new Color(1f, 0f, 1f);
    Color firstColor = Color.green;

    //종류별 점수
    Dictionary<Point.PointType, Point> points = new Dictionary<Point.PointType, Point>();

    // 플레이어 점수
    Dictionary<int, int> playerScore = new Dictionary<int, int>();
    // 스테이지 각 점수를 획득한 플레이어 확인
    Dictionary<Point.PointType, List<int>> playerScoreDic = new Dictionary<Point.PointType, List<int>>();

    CharacterData characterData;

    [Header("UI")]
    [Header("ScorePaper")]
    [SerializeField] RectTransform scorePaper;
    [SerializeField] RectTransform noPointPaper;
    [SerializeField] RectTransform playerInfoParent;
    [SerializeField] GameObject playerInfoFactory;
    [SerializeField] GameObject pointAreaFactory;

    [Header("ScorePaper")]
    [SerializeField] GameObject winnerObj;
    [SerializeField] TextMeshProUGUI winnerCharacterText;

    Dictionary<int, PlayerInfo> playerInfos = new Dictionary<int, PlayerInfo>();

    ////테스트용 플레이어
    //public GameObject playerTest;

    int goalNum = 0;
    public int winner = -1;

    private void Awake()
    {
        characterData = Resources.Load<CharacterData>("ScriptableObject/CharacterData");
    }

    void Start()
    {
        points.Add(Point.PointType.Goal, new Point(goal, goalColor, "골"));
        points.Add(Point.PointType.Solo, new Point(solo, soloColor, "솔로"));
        points.Add(Point.PointType.Death, new Point(death, deathColor, "사후"));
        points.Add(Point.PointType.Trap, new Point(trap, trapColor, "덫"));
        points.Add(Point.PointType.Coin, new Point(coin, coinColor, "코인"));
        points.Add(Point.PointType.Comeback, new Point(comeback, comebackColor, "컴백"));
        points.Add(Point.PointType.First, new Point(first, firstColor, "첫 도착자"));
    }

    public void Init(List<(int, string)> actors)
    {
        //플레이어의 수만큼 유저인포 생성

        for (int i = 0; i < actors.Count; i++)
        {
            PlayerInfo info = Instantiate(playerInfoFactory, playerInfoParent).GetComponent<PlayerInfo>();
            info.Set(actors[i].Item2, PlayerData.instance.PlayerCharacterDic[actors[i].Item1]);
            playerInfos.Add(actors[i].Item1, info);
            playerScore.Add(actors[i].Item1, 0);
        }
    }

    public void AddScore(Point.PointType scoreType, int playerNum)
    {
        if (playerScoreDic.ContainsKey(scoreType) == false)
            playerScoreDic.Add(scoreType, new List<int>());
        playerScoreDic[scoreType].Add(playerNum);

        if (scoreType == Point.PointType.Goal)
        {
            switch (goalNum)
            {
                case 0:
                    playerScoreDic.Add(Point.PointType.First, new List<int>());
                    playerScoreDic[Point.PointType.First].Add(playerNum);
                    break;
                    //case 1:
                    //    playerScoreDic.Add(Point.PointType.Second, new List<int>());
                    //    playerScoreDic[Point.PointType.Second].Add(playerNum);
                    //    break;
                    //case 2:
                    //    playerScoreDic.Add(Point.PointType.Third, new List<int>());
                    //    playerScoreDic[Point.PointType.Third].Add(playerNum);
                    //    break;
                    //case 3:
                    //    playerScoreDic.Add(Point.PointType.Fourth, new List<int>());
                    //    playerScoreDic[Point.PointType.Fourth].Add(playerNum);
                    //    break;
            }
            goalNum++;
        }
    }

    //public void Score()
    //{
    //    photonView.RPC(nameof(ScoreRPC), RpcTarget.All);
    //}

    //[PunRPC]
    //void ScoreRPC()
    //{
    //    ScoreCalc();
    //}

    //점수 정산
    public void ScoreCalc()
    {
        if (playerScoreDic.Count > 0)
        {
            if (playerScoreDic.ContainsKey(Point.PointType.Goal))
            {
                if (playerScoreDic[Point.PointType.Goal].Count == 1)
                {
                    playerScoreDic.Add(Point.PointType.Solo, new List<int>());
                    playerScoreDic[Point.PointType.Solo].Add(playerScoreDic[Point.PointType.Goal][0]);
                    playerScoreDic[Point.PointType.First].Remove(playerScoreDic[Point.PointType.Goal][0]);
                    playerScoreDic.Remove(Point.PointType.First);
                }
            }
        }

        StartCoroutine(ScoreCalcCoroutine());
    }

    IEnumerator ScoreCalcCoroutine()
    {
        //계산할 점수를 가지고 있는 유저가 있는지 확인
        bool needCalc = false;
        if (playerScoreDic.Count > 0)
            needCalc = true;

        yield return new WaitForSeconds(1f);
        //플레이어 비활성화
        MainGameManager.instance.PlayerInactive();

        //ui등장
        scorePaper.DOLocalMoveY(0, 0.5f).From(-1200);
        if (needCalc == false)
        {
            noPointPaper.DOLocalMove(new Vector3(-80, 0, 0), 0.5f);
            noPointPaper.DOLocalRotate(new Vector3(0, 0, 10), 0.5f);
        }
        SoundManager.Instance.PlaySFX("Paper");

        yield return new WaitForSeconds(0.5f);

        //계산할 점수가 있는지 확인
        for (int i = 0; i < (int)Point.PointType.Length; i++)
        {
            Point.PointType typeTemp = (Point.PointType)i;
            if (!playerScoreDic.ContainsKey(typeTemp)) continue;
            if (typeTemp == Point.PointType.Goal)    //골의 경우 단체로 계산
            {
                foreach (var playerNum in playerScoreDic[typeTemp])
                {
                    //점수 계산
                    PointArea area = Instantiate(pointAreaFactory, playerInfos[playerNum].pointParent).GetComponent<PointArea>();
                    area.Set(playerScore[playerNum], typeTemp, points[typeTemp]);
                    playerScore[playerNum] += points[typeTemp].point;
                }

                SoundManager.Instance.PlaySFX("PaperWrite");
                //for (int j = 0; j < playerScoreDic[typeTemp].Count; j++)
                //{
                //    //점수 계산
                //    PointArea area = Instantiate(pointAreaFactory, playerInfos[0].pointParent).GetComponent<PointArea>();
                //    area.Set(playerScore[playerScoreDic[typeTemp][j]], typeTemp, points[typeTemp]);
                //    playerScore[playerScoreDic[typeTemp][j]] += points[typeTemp].point;
                //}
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                foreach (var playerNum in playerScoreDic[typeTemp])
                {
                    //점수 계산
                    PointArea area = Instantiate(pointAreaFactory, playerInfos[playerNum].pointParent).GetComponent<PointArea>();
                    area.Set(playerScore[playerNum], typeTemp, points[typeTemp]);
                    playerScore[playerNum] += points[typeTemp].point;

                    SoundManager.Instance.PlaySFX("PaperWrite");
                    yield return new WaitForSeconds(0.5f);
                }

                //for (int j = 0; j < playerScoreDic[typeTemp].Count; j++)
                //{
                //    //점수 계산
                //    PointArea area = Instantiate(pointAreaFactory, playerInfos[0].pointParent).GetComponent<PointArea>();
                //    area.Set(playerScore[playerScoreDic[typeTemp][j]], typeTemp, points[typeTemp]);
                //    playerScore[playerScoreDic[typeTemp][j]] += points[typeTemp].point;
                //    yield return new WaitForSeconds(0.5f);
                //}
            }
        }

        yield return new WaitForSeconds(1f);
        //ui 치우기
        scorePaper.DOLocalMoveY(-1200, 0.5f);
        if (needCalc == false)
        {
            noPointPaper.DOLocalMove(new Vector3(1100, 300, 0), 0.5f);
            noPointPaper.DOLocalRotate(new Vector3(0, 0, -90), 0.5f);
        }

        //점수관련 변수 초기화
        playerScoreDic.Clear();
        goalNum = 0;

        winner = CheckWinner();
        //게임이 종료된 경우
        if (winner == -1)
        {
            yield return new WaitForSeconds(0.5f);
            MainGameManager.instance.ChangeState(MainGameManager.GameState.Select);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            winnerObj.SetActive(true);
            winnerCharacterText.text = playerInfos[winner].Character.characterName;
            MainGameManager.instance.ChangeState(MainGameManager.GameState.End);
        }
    }

    int CheckWinner()
    {
        //점수가 25점 이상인 플레이어가 있는지 체크
        foreach (var item in playerScore)
        {
            if (item.Value >= maxScore)
            {
                return item.Key;
            }
        }

        return -1;
    }
}
