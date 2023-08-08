using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayerMove : MonoBehaviour
{
    Rigidbody rb;

    Vector3 dir;

    public float speed = 5;
    public float jumpPower = 5;

    //최대 점프 시간
    public float maxJumpTime = 1.0f;

    //최대 점프 높이
    public float maxJumpHeight = 3.0f;

    //점프를 한 현재 시간
    public float currentTime;

    //점프의 높이 체크
    public bool jumpTime = true;

    //RPlayerWallJump의 스크립트를 알고 싶다
    public RPlayerWallJump rPWJ;

    // jump의 높이를 제한하기 위한 addForce에 들어가는 power의 횟수 제한
     int fCount = 3;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 만약에 벽을 만나면
        // 작동을 멈추고 싶다.
        if (rPWJ.IsWall() == true) return;

        float h = Input.GetAxis("Horizontal");

        dir = Vector3.right * h;
        dir.Normalize();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //rb.AddForce(Vector3.forward * 1);            
            //rb.velocity = Vector3.forward * 2;
            rb.AddForce(new Vector3(1, 0, 0), ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rb.AddForce(new Vector3(-2, 0, 0), ForceMode.VelocityChange);
        }
        // 점프 입력을 받으면 점프 시작

        if (Input.GetKey(KeyCode.Space)&& IsGround())
        {
            jumpTime = true;
            currentTime = 0;
            jumpPower = 2;
        }

        // 점프 입력을 떼면 점프 중지
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTime = false;
            fCount = 3;
        }
    }

    private void FixedUpdate()
    {
        // 만약에 벽을 만나면
        // 작동을 멈추고 싶다.
        if (rPWJ.IsWall() == true) return;
        

        //만약에 땅에 닿아있다면
        if (IsGround() == true)
        {
            //현재 속도
            Vector3 currVelocity = rb.velocity;
            //움직여야 하는 속도
            Vector3 targetVelocity = dir * speed;

            rb.AddForce(targetVelocity - currVelocity, ForceMode.VelocityChange);

            //// 점프 입력이 유지되고 있으면 점프 진행
            //if (jumpTime)
            //{
            //    if (currentTime < maxJumpTime)
            //    {
            //        rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.VelocityChange);
            //        currentTime += Time.fixedDeltaTime;

            //        // 최대 점프 높이에 도달하면 점프 중지
            //        if (rb.position.y >= maxJumpHeight)
            //        {
            //            jumpTime = false;
            //        }
            //    }
            //    else
            //    {
            //        jumpTime = false;
            //    }
            //}

        }
        else
        {
            //현재 속도
            Vector3 currVelocity = rb.velocity;
            //움직여야 하는 속도
            Vector3 targetVelocity = dir * speed * 1.0f;

            // 평행으로 움직이는 y의 값과 점프하고 내려올 때의 y 값을 같게 하자.
            targetVelocity.y = currVelocity.y;

            rb.AddForce(targetVelocity - currVelocity, ForceMode.VelocityChange);
        }

        


        if (jumpTime)
        {

            if (fCount > 0)
            {
                rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.VelocityChange);
                fCount--;
            }



            //jumpTime = false;
        }

        // 중력
        rb.AddForce(new Vector3(0, -9.81f, 0));



    }

    bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        //if(Physics.Raycast(ray, 1.01f))
        //{
        //    //땅에 닿았다.
        //    return true;
        //}
        //else
        //{
        //    //공중에 있다.
        //    return false;
        //}

        return Physics.Raycast(ray, 1.02f);
    }
}

