using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHGameManager : MonoBehaviour
{
    public static KHHGameManager instance;
    public PartyBox partyBox;
    public UserCursor cursor;


    public enum GameState
    {
        None,
        Select,
        Place,
        Play,
        Score,
        End,
    }
    public GameState state;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        partyBox.Init();
        partyBox.SetBox();
        partyBox.Open();

        //자신의 커서 생성
        //커서 비활성화
        cursor.Deactive();


        ChangeState(GameState.Select);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Select:
                if (cursor.IsSelect)
                    ChangeState(GameState.Place);
                break;
            case GameState.Place:
                if (cursor.IsPlace)
                    ChangeState(GameState.Play);
                break;
            case GameState.Play:
                break;
            case GameState.Score:
                break;
            case GameState.End:
                break;
        }
    }

    void ChangeState(GameState state)
    {
        this.state = state;
        switch (state)
        {
            case GameState.Select:
                partyBox.Open();
                break;
            case GameState.Place:
                partyBox.Close();
                cursor.Active();
                break;
            case GameState.Play:
                break;
            case GameState.Score:
                break;
            case GameState.End:
                break;
        }
    }
}
