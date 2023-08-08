using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayer : MonoBehaviour
{

    public Rigidbody rB;
    Vector3 dir;
    public float speed = 5;
    public float jumpPower = 5;
    public float maxVelocity = 25;


    //점프의 높이 체크
    public bool jumpTime = true;

    // jump의 높이를 제한하기 위한 addForce에 들어가는 power의 횟수 제한
    public int fCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        dir = Vector3.right * h;
        dir.Normalize();

        if (jumpTime)
        {
            if (IsGround())
            {
                rB.AddForce(dir * speed);
            }
            else
            {
                rB.AddForce(dir * speed * 0.5f);
            }

            if (fCount > 0)
            {

                //스페이스바를 누르면
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //윗방향으로 힘을 가한다
                    rB.AddForce(Vector3.up * jumpPower);
                }



                //rB 의 속도를 지역 변수 담아놓자.
                Vector3 hVelocity = rB.velocity;
                hVelocity.y = 0;

                //만약에 hVelocity 의 크기가 일정값보다 커지면
                if (hVelocity.magnitude > maxVelocity)
                {
                    //maxVelocity 의 크기 만큼 수평방향 속도를 만든다.
                    hVelocity = hVelocity.normalized * maxVelocity;

                    //중력에 의한 위아래 속력을 다시 대입한다.
                    hVelocity.y = rB.velocity.y;

                    //보정된 값으로 다시 움직여라
                    rB.velocity = hVelocity;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    rB.AddForce(Vector3.left * jumpPower);
                }
            }


        }


        bool IsGround()
        {
            Ray ray = new Ray(transform.position, Vector3.down);

            return Physics.Raycast(ray, 1.02f);
        }
    }
}



