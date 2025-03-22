using UnityEngine;

public class Move : MonoBehaviour
{
    // ������Ʈ �� �ӵ� ���� ����
    public GameObject Sphere;
    public GameObject Cube;

    // Sphere Material color ����
    private Renderer SphereColor;

    public float SphereSpeed = 7f;      // ��ü �⺻ �ӵ�
    public float CubeSpeed = 10f;       // ť�� �⺻ �ӵ�
    public float targetDistance = 5f;   // ��ü�� ť�� �� ���� �Ÿ�
    public float maxSpeed = 50f;        // �ִ� �ӵ� ����


    private void Start()
    {
        // Sphere�� Renderer �������� �� �ʱ� ���� ����
        SphereColor = Sphere.GetComponent<Renderer>();
        SphereColor.material.color = Color.green;
    }

    private void Update()
    {
        Sphere_Tracking(); // Sphere ����
        CubeMove();        // Cube �̵�
    }

    // Sphere ���� ����
    private void Sphere_Tracking()
    {
        if (Cube != null)
        {
            // Cube�� Sphere ������ ���� ���
            Vector3 direction = Cube.transform.position - Sphere.transform.position;
            float currentDistance = direction.magnitude; // ���� �Ÿ� ���

            // �Ҽ��� ��° �ڸ����� �ݿø�(�ʹ� ���� �̼��� �� ����)
            currentDistance = Mathf.Round(currentDistance * 100f) / 100f;

            // �ӵ� ���� ���: �Ÿ� ���̿� ����ؼ� �ӵ� ����
            float speedMultiplier = Mathf.Clamp((currentDistance - targetDistance) / targetDistance, -maxSpeed, maxSpeed);
            float speed = SphereSpeed * (1 + Mathf.Abs(speedMultiplier)); // �⺻ �ӵ��� �ӵ� ���� ����

            if (currentDistance > targetDistance) // �ʹ� �ָ� �� ���� �ٰ�����
            {             
                Vector3 targetPosition = Cube.transform.position - direction.normalized * targetDistance;
                Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, targetPosition, speed * Time.deltaTime);
            }
            else if (currentDistance < targetDistance) // �ʹ� ������ �� ���� �־�����
            {                
                Vector3 awayPosition = Sphere.transform.position - direction.normalized * (targetDistance - currentDistance);
                Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, awayPosition, speed * Time.deltaTime);
            }



            Debug.Log("Sphere �Ÿ�: " + currentDistance + ", Sphere �ӵ�: " + speedMultiplier);





            // �Ÿ��� ���� ����
            if (currentDistance > targetDistance) // �ָ� ������ ���� ��
            {
                SphereColor.material.color = Color.blue; 
            }
            else if (currentDistance < targetDistance)  // �ʹ� ����� ��
            {
                SphereColor.material.color = Color.red;
            }
            else // ���� �Ÿ��� �� ���� ����
            {
                SphereColor.material.color = Color.green;
            }
        }
        else
        {
            Debug.LogWarning("Cube Object�� �������� �ʾҽ��ϴ�!");
        }
    }

    // Cube �̵� ����
    private void CubeMove()
    {
        if (Cube != null)
        {
            // �Է°� �ޱ�
            float horizontal = Input.GetAxis("Horizontal"); // �¿� �Է�
            float vertical = Input.GetAxis("Vertical");     // ���� �Է�

            // �̵� ���� ���
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            // Cube �̵� (���� ��ǥ ����)
            Cube.transform.Translate(direction * CubeSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            Debug.LogWarning("Cube Object�� �������� �ʾҽ��ϴ�!");
        }
    }
}
