using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPlayer : MonoBehaviourPun
{
    public Rigidbody rB;
    Vector3 dir;
    public float speed = 5;
    public float jumpPower = 500;
    public float maxVelocity = 5;

    //힘을 가하는 횟수 제한
    float fCount = 3;

    // 점프 시간
    private float jumpStart;
    public float shortJump = 0.1f;
    public float longJump = 0.5f;

    //점프의 높이 체크
    bool jumpTime = false;

    //벽점프
    public float walljumpPower = 5;

    //애니메이션
    public Animator anim;
   
    // Start is called before the first frame update
    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //소유권이 나한테 없다면 함수를 나가자
        if (photonView.IsMine == false) return;

        float h = Input.GetAxis("Horizontal");

        dir = Vector3.right * h;
        dir.Normalize();

        if(h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        if (h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }



        //스페이스바를 누르면
        if (Input.GetKeyDown(KeyCode.Space) && IsGround())
        {                                     
            //jumpStart = Time.time;
            jumpTime = true;

            anim.SetTrigger("Jump");
            

            // 점프 중일 때 Idle 애니메이션 전환 방지
            anim.SetBool("IsWalkingRight", false);

            //if (h > 0)
            //{
            // 오른쪽으로 점프를 나타내는 애니메이션 파라미터 설정
            //  anim.SetTrigger("RightJump");

            //}
            //else if (h < 0)
            //{
            // 왼쪽으로 점프를 나타내는 애니메이션 파라미터 설정
            //  anim.SetTrigger("LeftJump");

            //}
        }

        if(IsGround())
        {
            if (rB.velocity.y <= 0)
            {
                anim.SetBool("IsJump", false);
            }
        }


            if (h == 0)
        {
            // 움직임 입력이 없을 때 Idle 애니메이션 실행
            anim.SetBool("IsWalkingRight", false);
            anim.SetTrigger("Idle");

        }

        if (h > 0)
        {
            // 오른쪽으로 움직임을 나타내는 애니메이션 파라미터 설정
            anim.SetBool("IsWalkingRight", true);
            
        }
        else if (h < 0)
        {
            // 왼쪽으로 움직임을 나타내는 애니메이션 파라미터 설정
            
            anim.SetBool("IsWalkingRight", true);
        }
        else
        {
            // 가만히 있을 때 모든 파라미터 비활성화
            anim.SetBool("IsWalkingRight", false);
            
            anim.SetTrigger("Idle");
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 속도를 빨리 내며 걷도록 설정
            speed = 50;
        }

        //스페이스바를 누르면
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTime = false;
            fCount = 3;

            //float jumpHold = Time.time - jumpStart;

            //// 시간에 따라 jumpPower결정

            //float dJump = Mathf.Lerp(300f, 500f, jumpHold / longJump);


            //if (transform.position.y <= 1.1f)
            //{
            //    //윗방향으로 힘을 가한다
            //    rB.AddForce(Vector3.up * dJump);
            //    jumpTime = false;
            //}

            //// 벽점프

            ////벽에 닿으면
            //if(IsWall())
            //{
            //    // jumpPower을 동일한 값으로 막는다.
            //    // 점프를 할 때 부딪힌 방향의 반대 방향으로 점프한다
            //    // 
            //}

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

        if(IsWall() && !IsGround())
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                // 벽에 붙어 있는 경우
                anim.SetTrigger("Slide");
                rB.velocity = new Vector3(-dir.x * walljumpPower, walljumpPower, 0);
                // walljumpPower++;
                //rB.AddForce(Vector3.left * jumpPower);
                //if (h > 0)
                //{
                    // 오른쪽으로 점프를 나타내는 애니메이션 파라미터 설정
                  //  anim.SetTrigger("RightJump");

                //}
                //else if (h < 0)
                //{
                    // 왼쪽으로 점프를 나타내는 애니메이션 파라미터 설정
                  //  anim.SetTrigger("LeftJump");

                //}
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsGround())
        {
            rB.AddForce(dir * speed);
        }
        else
        {
            rB.AddForce(dir * speed * 0.5f);
        }


        //만약에 jumpTime 이 true 라면
        if(jumpTime == true)
        {
            if (fCount>0)
            {
            
                //위로 힘을 준다.
                rB.AddForce(Vector3.up * jumpPower);
                fCount--;
                anim.SetBool("IsJump", true);
            }
            else
            {
                // 점프 종료 시 jumpTime을 false로 설정
                jumpTime = false;
            

            }
        }

    }



    bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        return Physics.Raycast(ray, 1.02f, (-1) - (1 << LayerMask.NameToLayer("Player")));

    }

    bool IsWall()
    {
        Ray rayRight = new Ray(transform.position, Vector3.right);
        Ray rayLeft = new Ray(transform.position, Vector3.left);

        return Physics.Raycast(rayRight, 1.02f) || Physics.Raycast(rayLeft, 1.02f);
    }

    
}




