using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPathFollow : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Transform targetPoint;

    void Start()
    {
        targetPoint = pointB;

        // leci w prawo, ale sprite domyœlnie patrzy w lewo, wiêc odwracamy
        LookRight();
    }

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            if (targetPoint == pointA)
            {
                targetPoint = pointB;
                LookRight();
            }
            else
            {
                targetPoint = pointA;
                LookLeft();
            }
        }
    }

    void LookRight()
    {
        transform.localScale = new Vector3(
            -Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
    }

    void LookLeft()
    {
        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}