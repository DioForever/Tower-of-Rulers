using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    public enum MonsterState
    {
        Patrol,
        Stalk
    }

    public float patrolSpeed;
    public float stalkSpeed;
    public float patrolWaitTime;
    public float startPatrolWaitTime;
    public float distanceToStalk;
    public float patrolAreaRadius; 
    public float minPatrolPointDistance; 
    public float maxPatrolPointDistance; 

    private float patrolWaitTimeRemaining;
    private Vector2 currentPatrolPoint;
    private MonsterState currentState;

    public GameObject player;

    void Start()
    {
        currentState = MonsterState.Patrol;
        patrolWaitTimeRemaining = startPatrolWaitTime;

        SetNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Stalk:
                Stalk();
                break;
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolPoint, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPatrolPoint) < 0.2f)
        {
            if (patrolWaitTimeRemaining <= 0)
            {
                SetNextPatrolPoint();
                patrolWaitTimeRemaining = patrolWaitTime;
            }
            else
            {
                patrolWaitTimeRemaining -= Time.deltaTime;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < distanceToStalk)
        {
            currentState = MonsterState.Stalk;
        }
    }

    void Stalk()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > distanceToStalk)
        {
            currentState = MonsterState.Patrol;
            SetNextPatrolPoint();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, stalkSpeed * Time.deltaTime);
        }
    }

    void SetNextPatrolPoint()
    {
        currentPatrolPoint = GetRandomPointInPatrolArea();
        currentState = MonsterState.Patrol;
    }

    Vector2 GetRandomPointInPatrolArea()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomDistance = Random.Range(minPatrolPointDistance, maxPatrolPointDistance);

        float x = transform.position.x + Mathf.Cos(randomAngle) * randomDistance;
        float y = transform.position.y + Mathf.Sin(randomAngle) * randomDistance;

        x = Mathf.Clamp(x, transform.position.x - patrolAreaRadius, transform.position.x + patrolAreaRadius);
        y = Mathf.Clamp(y, transform.position.y - patrolAreaRadius, transform.position.y + patrolAreaRadius);

        return new Vector2(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            currentState = MonsterState.Stalk;
        }
    }
}
