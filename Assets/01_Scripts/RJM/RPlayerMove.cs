using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayerMove : MonoBehaviour
{

    // �ӷ�
    public float speed = 5;

    public int maxJumps = 1;
    private int jumpCount = 0;
    private bool isJumping = false;

    // character controller ���� ����
    CharacterController cc;

    // �����Ŀ�
    float jumpPower = 5;
    // �߷�
    float gravity = -9.81f;
    // y �ӷ�
    float yVelocity;

    // �� ����
    public bool isWall;

    Vector3 dir;
    public float currentTime;
    public float MoveTime;


    // Start is called before the first frame update
    void Start()
    {
        //character controller ��������
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // W,A,S,D Ű�� ������ �յ��¿�� �����̰� �ʹ�.

        // 1. ������� �Է��� ����.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. ������ �����.
        //�¿�
        Vector3 dirH = transform.right * h;
        //���Ʒ�
        Vector3 dirV = transform.up * v;
        //����
        Vector3 dir = dirH + dirV;

        //Vector3 dir = h * Vector3.right + v * Vector3.up;

        dir.Normalize();

        Vector3 velocity = dir * speed;

        //���࿡ ���� ����ִٸ�
        if (cc.isGrounded == true)
        {
            //yVelocity�� 0���� ����
            yVelocity = 0;

            jumpCount = 0;
        }

        // �����̽��ٸ� ������ �����Ѵ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < maxJumps)
            {
                //yVelocity�� jumpPower �� ����
                yVelocity = jumpPower;
                jumpCount++;
            }
        }

        //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
        yVelocity += gravity * Time.deltaTime;

        //yVelocity ���� dir�� y ���� ����
        dir.y = yVelocity;

        //�� �������� ��������
        //transform.position += velocity * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);
    }


    private void UpdateWallJump()
    {
        // �̵� ������ ������ �Լ��� �����մϴ�.
        if (dir.magnitude == 0)
        {
            return;
        }

        // ���� �ð��� �ӵ��� �ʱ�ȭ �մϴ�.
        MoveTime = 0.4f;
        speed = 3f;

        // ������ ������ ����մϴ�. �⺻ �̵� ������ �ݴ� �������� 2�踸ŭ �̵��մϴ�.
        Vector3 newdir = -dir * 2;

        // ������ ���⿡ �� �������� ������ ���մϴ�.
        Vector3 updir = Vector3.up * jumpPower;

        // characterController�� �̿��� ���� �������� �̵��մϴ�
        cc.Move((newdir + updir) * Time.deltaTime * speed);

        // ���ο� �������� �÷��̾ ȸ���մϴ�.
        if (newdir.magnitude == 0) return;
        else
        {
            transform.forward = newdir;
        }

        // �̵� �ð��� ����մϴ�.
        currentTime += Time.deltaTime;

        // �̵� �ð��� MoveTime ���� ũ�ų� ������ ���� �ð��� �ʱ�ȭ �մϴ�.
        if (currentTime >= MoveTime)
        {
            currentTime = 0;
        }
    }
}

