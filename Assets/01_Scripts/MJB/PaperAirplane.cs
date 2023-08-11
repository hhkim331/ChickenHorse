using UnityEngine;

public class PaperAirplane : MonoBehaviour
{
    //플레이어가 충돌하면 충돌하는 애니메이션을 실행시키고 이벤트를 실행하여 삭제시킨다.
    //태어나면 앞 방향으로 날라가고 싶다.
    //속력값
    public float speed = 5f;

    //애니메이터
    public Animator anim;

    private void Update()
    {
        // 앞 방향으로 날라간다.
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어 이외의 물체에 충돌하면
        if (!collision.gameObject.CompareTag("Player"))
        {
            //부모까지 오브젝트를 삭제시키는 애니메이션 이벤트를 실행시킨다.
            anim.SetTrigger("Crash");
        }
    }

    //플레이어가 나한테 다으면 플레이어의 위치 값을 나의 부모 위치 값으로 한다.
    private void OnTriggerEnter(Collider other)
    {
        //플레이어 태그를 만나면
        if (other.CompareTag("Player"))
        {
            //플레이어 transform은 종이 비행기의 tranform으로 한다.
            other.gameObject.transform.SetParent(transform);
        }
    }

    //플레이어가 나한테 떨어지면 플레이어의 위치 값을 자신의 위치 값으로 변경한다.
    private void OnTriggerExit(Collider other)
    {
        //플레이어 태그가 나간다면
        if (other.CompareTag("Player"))
        {
            //플레이어의 transform을 null로 만든다.
            other.gameObject.transform.SetParent(null);
        }
    }
}