using UnityEngine;

public class Move : MonoBehaviour
{
    // 오브젝트 및 속도 변수 선언
    public GameObject Sphere;
    public GameObject Cube;

    // Sphere Material color 변수
    private Renderer SphereColor;

    public float SphereSpeed = 7f;      // 구체 기본 속도
    public float CubeSpeed = 10f;       // 큐브 기본 속도
    public float targetDistance = 5f;   // 구체와 큐브 간 유지 거리
    public float maxSpeed = 50f;        // 최대 속도 배율


    private void Start()
    {
        // Sphere의 Renderer 가져오기 및 초기 색상 설정
        SphereColor = Sphere.GetComponent<Renderer>();
        SphereColor.material.color = Color.green;
    }

    private void Update()
    {
        Sphere_Tracking(); // Sphere 추적
        CubeMove();        // Cube 이동
    }

    // Sphere 추적 로직
    private void Sphere_Tracking()
    {
        if (Cube != null)
        {
            // Cube와 Sphere 사이의 방향 계산
            Vector3 direction = Cube.transform.position - Sphere.transform.position;
            float currentDistance = direction.magnitude; // 현재 거리 계산

            // 소수점 둘째 자리까지 반올림(너무 작은 미세한 값 무시)
            currentDistance = Mathf.Round(currentDistance * 100f) / 100f;

            // 속도 배율 계산: 거리 차이에 비례해서 속도 조절
            float speedMultiplier = Mathf.Clamp((currentDistance - targetDistance) / targetDistance, -maxSpeed, maxSpeed);
            float speed = SphereSpeed * (1 + Mathf.Abs(speedMultiplier)); // 기본 속도에 속도 배율 적용

            if (currentDistance > targetDistance) // 너무 멀면 더 빨리 다가가기
            {             
                Vector3 targetPosition = Cube.transform.position - direction.normalized * targetDistance;
                Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, targetPosition, speed * Time.deltaTime);
            }
            else if (currentDistance < targetDistance) // 너무 가까우면 더 빨리 멀어지기
            {                
                Vector3 awayPosition = Sphere.transform.position - direction.normalized * (targetDistance - currentDistance);
                Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, awayPosition, speed * Time.deltaTime);
            }



            Debug.Log("Sphere 거리: " + currentDistance + ", Sphere 속도: " + speedMultiplier);





            // 거리별 색상 변경
            if (currentDistance > targetDistance) // 멀리 떨어져 있을 때
            {
                SphereColor.material.color = Color.blue; 
            }
            else if (currentDistance < targetDistance)  // 너무 가까울 때
            {
                SphereColor.material.color = Color.red;
            }
            else // 적정 거리일 때 색상 유지
            {
                SphereColor.material.color = Color.green;
            }
        }
        else
        {
            Debug.LogWarning("Cube Object가 설정되지 않았습니다!");
        }
    }

    // Cube 이동 로직
    private void CubeMove()
    {
        if (Cube != null)
        {
            // 입력값 받기
            float horizontal = Input.GetAxis("Horizontal"); // 좌우 입력
            float vertical = Input.GetAxis("Vertical");     // 전후 입력

            // 이동 방향 계산
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            // Cube 이동 (로컬 좌표 기준)
            Cube.transform.Translate(direction * CubeSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            Debug.LogWarning("Cube Object가 설정되지 않았습니다!");
        }
    }
}
