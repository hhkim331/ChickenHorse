using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 풀링을 구현하고 싶다.

public class ObjectPool : MonoBehaviour
{
    //오브젝트 풀링을 전역으로 관리하자
    public static ObjectPool Instance;

    //종이 비행기 Prefab을 가져오자
    [SerializeField]
    private GameObject poolingAirPlane;

    //종이 비행기를 넣어서 관리하자
    Queue<GameObject> poolingAirplaneQueue = new Queue<GameObject>();


    private void Awake()
    {
        Instance = this;
        Initialize(5);
    }


    //종이 비행기를 생성하자
    private GameObject CreateAirplane()
    {
        // 종이 비행기를 생성하고
        GameObject newAirplane = Instantiate(poolingAirPlane, transform);
        // 오브젝트를 끈다.
        newAirplane.gameObject.SetActive(false);
        // 오브젝트를 반환한다.
        return newAirplane;
    }


    //원하는 갯수 만큼 처음 생성
    private void Initialize(int number)
    {
        //원하는 갯수만큼 초기 생성
        for (int i = 0; i < number; i++)
        {
            poolingAirplaneQueue.Enqueue(CreateAirplane());
        }
    }

    //종이 비행기를 빌려온다.
    public static GameObject GetAirplane()
    {
        //풀링 큐에 0개 이상이 있으면
        if (Instance.poolingAirplaneQueue.Count > 0)
        {
            //부모를 끊어버리고
            GameObject airplaneObject = Instance.poolingAirplaneQueue.Dequeue();
            airplaneObject.gameObject.SetActive(true);
            //나를 키고 반환한다.
            return airplaneObject;

        }
        else
        {
            //없으면 나를 생성한다.
            GameObject newAirplaneObject = Instance.CreateAirplane();
            //부모는 없고
            newAirplaneObject.gameObject.SetActive(true);
            //나를 키고 반환한다.
            return newAirplaneObject;
        }
    }


    //돌려 받은 오브젝트를 비활성화 하자
    public static void DisabledAirplane(GameObject paperAirplane)
    {
        //이동 오브젝트의 위치를 처음 위치로 바꾼다.
        //종이 비행기를 끄자
        paperAirplane.gameObject.SetActive(false);
        paperAirplane.GetComponentInChildren<PaperAirplane>().transform.localPosition = new Vector3(0, 0, 0);
        //인스턴스 안에다가 넣자
        paperAirplane.transform.SetParent(Instance.transform);
        //불렛에 넣자
        Instance.poolingAirplaneQueue.Enqueue(paperAirplane);
    }

}
