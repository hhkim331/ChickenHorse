using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayerMove : MonoBehaviour
{

    // 속력
    public float speed = 5;

    public int maxJumps = 1;
    private int jumpCount = 0;
    private bool isJumping = false;

    // character controller 담을 변수
    CharacterController cc;

    // 점프파워
    float jumpPower = 5;
    // 중력
    float gravity = -9.81f;
    // y 속력
    float yVelocity;

    // 벽 점프
    public bool isWall;

    Vector3 dir;
    public float currentTime;
    public float MoveTime;


    // Start is called before the first frame update
    void Start()
    {
        //character controller 가져오자
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // W,A,S,D 키를 누르면 앞뒤좌우로 움직이고 싶다.

        // 1. 사용자의 입력을 받자.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 방향을 만든다.
        //좌우
        Vector3 dirH = transform.right * h;
        //위아래
        Vector3 dirV = transform.up * v;
        //최종
        Vector3 dir = dirH + dirV;

        //Vector3 dir = h * Vector3.right + v * Vector3.up;

        dir.Normalize();

        Vector3 velocity = dir * speed;

        //만약에 땅에 닿아있다면
        if (cc.isGrounded == true)
        {
            //yVelocity를 0으로 하자
            yVelocity = 0;

            jumpCount = 0;
        }

        // 스페이스바를 누르면 점프한다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < maxJumps)
            {
                //yVelocity에 jumpPower 를 셋팅
                yVelocity = jumpPower;
                jumpCount++;
            }
        }

        //yVelocity 를 중력만큼 감소시키자
        yVelocity += gravity * Time.deltaTime;

        //yVelocity 값을 dir의 y 값에 셋팅
        dir.y = yVelocity;

        //그 방향으로 움직이자
        //transform.position += velocity * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);
    }


    private void UpdateWallJump()
    {
        // 이동 방향이 없으면 함수를 종료합니다.
        if (dir.magnitude == 0)
        {
            return;
        }

        // 점프 시간과 속도를 초기화 합니다.
        MoveTime = 0.4f;
        speed = 3f;

        // 점프할 방향을 계산합니다. 기본 이동 방향의 반대 방향으로 2배만큼 이동합니다.
        Vector3 newdir = -dir * 2;

        // 점프할 방향에 위 방향으로 점프를 더합니다.
        Vector3 updir = Vector3.up * jumpPower;

        // characterController를 이용해 점프 방향으로 이동합니다
        cc.Move((newdir + updir) * Time.deltaTime * speed);

        // 새로운 방향으로 플레이어를 회전합니다.
        if (newdir.magnitude == 0) return;
        else
        {
            transform.forward = newdir;
        }

        // 이동 시간을 기록합니다.
        currentTime += Time.deltaTime;

        // 이동 시간이 MoveTime 보다 크거나 같으면 현재 시간을 초기화 합니다.
        if (currentTime >= MoveTime)
        {
            currentTime = 0;
        }
    }
}

