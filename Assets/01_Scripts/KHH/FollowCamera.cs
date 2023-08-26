using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Camera[] cameras;

    public enum CameraState
    {
        FullScreen,
        Place,  //플레이어에게 초점을 맞춤
        Play,  //플레이어에게 초점을 맞춤
        Goal,   //골인 플레이어에게 초점을 맞춤
        End, //승자에게 초점을 맞춤
    }

    CameraState state;
    public CameraState State { get { return state; } set { state = value; } }

    Vector3 newCameraPos;
    Vector3 cameraCenterPos;

    float newCameraSize;
    float cameraPlaceMinSize = 12;
    float cameraPlayMinSize = 8;
    float cameraMaxSize;

    Vector2 mapSize;
    Dictionary<int, KHHPlayerMain> players = new Dictionary<int, KHHPlayerMain>();
    List<(int, string)> actors = new List<(int, string)>();
    List<UserCursor> playerCursors = new List<UserCursor>();

    public void Init(Vector2 mapSize)
    {
        this.mapSize = mapSize;
        cameraCenterPos = new Vector3(mapSize.x * 0.5f - 0.5f, mapSize.y * 0.5f - 0.5f, -20);

        //카메라 최대 사이즈 설정
        float x = mapSize.x * 0.1f * 3;
        float y = mapSize.y * 0.1f * 6;
        cameraMaxSize = x > y ? x : y;
        //if (cameraMaxSize < cameraMinSize)
        //    cameraMaxSize = cameraMinSize;
    }

    public void Set(Dictionary<int, KHHPlayerMain> players, List<(int, string)> actors, List<UserCursor> cursors)
    {
        this.players = players;
        this.actors = actors;
        playerCursors = cursors;
    }

    public void LateUpdate()
    {
        switch (state)
        {
            case CameraState.FullScreen:
                newCameraPos = cameraCenterPos;
                newCameraSize = cameraMaxSize;
                break;
            case CameraState.Place:
                Place();
                break;
            case CameraState.Play:
                Play();
                break;
        }

        Clamp();
        transform.position = Vector3.Lerp(transform.position, newCameraPos, Time.deltaTime);
        foreach (var cam in cameras)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newCameraSize, Time.deltaTime);
    }

    void Place()
    {
        int activePlayerCount = 0;
        float distance;
        float maxDistance = 0;
        float playerDistance = 0;
        Vector3 playersCenterPos = Vector3.zero;
        for (int i = 0; i < playerCursors.Count; i++)
        {
            if (playerCursors[i] == null)
                continue;
            if (playerCursors[i].IsPlace)
                continue;

            activePlayerCount++;
            playersCenterPos += playerCursors[i].transform.position;

            distance = Vector3.Distance(playerCursors[i].transform.position, cameraCenterPos);
            if (distance > maxDistance)
                maxDistance = distance;
            for (int j = i + 1; j < playerCursors.Count; j++)
            {
                distance = Vector3.Distance(playerCursors[i].transform.position, playerCursors[j].transform.position);
                if (distance > maxDistance)
                    maxDistance = distance;
                if (distance > playerDistance)
                    playerDistance = distance;
            }
        }

        if (activePlayerCount > 0)
        {
            playersCenterPos /= activePlayerCount;
            newCameraPos = new Vector3(playersCenterPos.x * 0.8f + cameraCenterPos.x * 0.2f, playersCenterPos.y * 0.8f + cameraCenterPos.y * 0.2f, -20);
        }
        else
            newCameraPos = cameraCenterPos;

        newCameraSize = cameraPlaceMinSize + playerDistance * 0.4f;
        if (newCameraSize > cameraMaxSize)
            newCameraSize = cameraMaxSize;
    }

    void Play()
    {
        int activePlayerCount = 0;
        float distance;
        float maxDistance = 0;
        float playerDistance = 0;
        Vector3 playersCenterPos = Vector3.zero;
        for (int i = 0; i < actors.Count; i++)
        {
            if (players[actors[i].Item1] == null)
                continue;
            if (!players[actors[i].Item1].IsActive)
                continue;

            activePlayerCount++;
            playersCenterPos += players[actors[i].Item1].transform.position;

            distance = Vector3.Distance(players[actors[i].Item1].transform.position, cameraCenterPos);
            if (distance > maxDistance)
                maxDistance = distance;
            for (int j = i + 1; j < actors.Count; j++)
            {
                if (players[actors[j].Item1] == null)
                    continue;
                if (!players[actors[j].Item1].IsActive)
                    continue;

                distance = Vector3.Distance(players[actors[i].Item1].transform.position, players[actors[j].Item1].transform.position);
                if (distance > maxDistance)
                    maxDistance = distance;
                if (distance > playerDistance)
                    playerDistance = distance;
            }
        }

        if (activePlayerCount > 0)
        {
            playersCenterPos /= activePlayerCount;
            newCameraPos = new Vector3(playersCenterPos.x * 0.8f + cameraCenterPos.x * 0.2f, playersCenterPos.y * 0.8f + cameraCenterPos.y * 0.2f, -20);
        }
        else
            newCameraPos = cameraCenterPos;

        //사이즈는 플레이어간의 거리에 의해 결정 (맵중앙X)
        newCameraSize = cameraPlayMinSize + playerDistance * 0.2f;
        if (newCameraSize > cameraMaxSize)
            newCameraSize = cameraMaxSize;
    }

    public void SetGoal(List<GameObject> goalPlayers)
    {
        state = CameraState.Goal;

        int activePlayerCount = 0;
        Vector3 playersCenterPos = Vector3.zero;
        for (int i = 0; i < goalPlayers.Count; i++)
        {
            activePlayerCount++;
            playersCenterPos += goalPlayers[i].transform.position;
        }

        playersCenterPos /= activePlayerCount;
        newCameraPos = new Vector3(playersCenterPos.x, playersCenterPos.y, -20);
        newCameraSize = cameraPlayMinSize;
    }

    public void SetNoGoal()
    {
        newCameraPos = cameraCenterPos;
        newCameraSize = cameraMaxSize;
    }

    public void SetEnd(Vector3 endpos)
    {
        state = CameraState.End;

        newCameraPos = new Vector3(endpos.x, endpos.y, -20);
        newCameraSize = cameraPlayMinSize;
    }

    //카메라가 영역바깥으로 못나가게
    void Clamp()
    {
        float x = cameras[0].orthographicSize * cameras[0].aspect;
        float y = cameras[0].orthographicSize;
        float minX = cameraCenterPos.x - mapSize.x * 0.5f - 10 + x;
        float maxX = cameraCenterPos.x + mapSize.x * 0.5f + 10 - x;
        float minY = cameraCenterPos.y - mapSize.y * 0.5f - 10 + y;
        float maxY = cameraCenterPos.y + mapSize.y * 0.5f + 10 - y;

        //Debug.Log("x" + x + "y" + y);
        //Debug.Log("min:" + minX + "," + minY + " max:" + maxX + "," + maxY);

        if (newCameraPos.x < minX)
            newCameraPos.x = minX;
        else if (newCameraPos.x > maxX)
            newCameraPos.x = maxX;

        if (newCameraPos.y < minY)
            newCameraPos.y = minY;
        else if (newCameraPos.y > maxY)
            newCameraPos.y = maxY;
    }
}