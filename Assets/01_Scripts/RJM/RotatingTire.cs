using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingTire : MonoBehaviour
{
    public float rotateTime = 2.0f;
    public float timer = 0.0f;

    //회전횟수
    public int zCount = 0;

    //z의 각도
    public float zAngle;

    public bool rotateStart = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //돌기 시작해라가 false 일때만 시간 측정
        if (rotateStart == false)
        {
            timer += Time.deltaTime;

            // 일정 시간이 지났을 때
            if (timer >= rotateTime)
            {
                //돌기 시작해라 (true)
                rotateStart = true;

                //회전 횟수 하나 증가
                // Z 축 회전을 90도씩 추가
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zCount);

                //zCount += 18;
                //if (zCount <= 90)
                //{
                // 타이머 초기화
                //timer = 0.0f;

                //}
            }


            //만약에 돌기 시작해라가 true
            if (rotateStart == true)
            {
                //Z축 회전해라
                transform.DOLocalRotate(new Vector3(0, 0, transform.rotation.eulerAngles.z+90), 0.5f).SetEase(Ease.Linear);

               // zAngle += 90;

                timer = 0.0f;
                rotateStart = false;
                //zCount 증가
                //zCount++;
                //만약에 zCount 90 * 회전횟수 보다 커지면
                //if (zAngle >= 90 * zCount)
                //{
                //    //회전 횟수 하나 증가
                //    zCount++;
                //    //타이머 초기화
                //    //돌기 시작해라 false
                //    rotateStart = false;
                //}
            }


        }
    }
}
