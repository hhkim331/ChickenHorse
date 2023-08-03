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

    // 벽 점프 시간 변수 추가
    private float wallJumpTime = 0.4f;

    // 벽 점프
    public bool isWall = false;

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

        if (cc.isGrounded)
        {
            isWall = false;
        }
    }



    // 벽점프

    private float wallJumpPower = 5f;
    private float wallJumpSpeed = 3f;
    private void UpdateWallJump()
    {
        {
            // 벽에 붙어 있는지 확인할 Raycast 설정
            // 시작 위치 
            Vector3 raycast = transform.position + Vector3.up * 0.1f;

            // 충돌 검사 거리 
            float castDistance = 0.5f;

            // 바닥 레이어 마스크 설정
            LayerMask groundLayerMask = LayerMask.GetMask("Ground");

            // Raycast 또는 SphereCast를 사용하여 충돌 검사를 수행합니다.
            bool isTouchingWall = Physics.Raycast(raycast, transform.forward, castDistance, groundLayerMask);

            // 벽에 붙어 있으면 벽 점프 로직을 수행합니다.
            if (isTouchingWall)
            {
                // 이동 방향이 없으면 함수를 종료합니다.
                if (dir.magnitude == 0)
                {
                    return;
                }

                // 벽점프 시간과 속도를 초기화 합니다.
                float wallJumpTimer = 0f;
                float currentWallJumpSpeed = wallJumpSpeed;

                // 점프할 방향을 계산합니다. 기본 이동 방향의 반대 방향으로 2배만큼 이동합니다.
                Vector3 newdir = -dir.normalized * 2;

                // charactercontroller를 이용해서 점프방향으로 이동합니다.
                // 새로운 방향으로 플레이어를 회전합니다.
                if (newdir.magnitude == 0) return;
                else
                {
                    transform.forward = newdir;
                }
            }
        }
    }
}



