using UnityEngine;

public enum CreatureType { TypeA, TypeB, TypeC }

public class CreatureController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float MoveRadius = 5f;
    public float MinMoveTime = 3f;
    public float MaxMoveTime = 10f;
    public float MinIdleTime = 2f;
    public float MaxIdleTime = 5f;
    public CreatureType Type;

    private Rigidbody RB;
    private Vector3 MoveDestination;
    private float MoveTimer;
    private float IdleTimer;
    private bool IsMoving;
    private Collider GameBounds;
    private float YPos;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        GameBounds = FindObjectOfType<NurseryGameManager>().PlayArea.GetComponent<Collider>();
        YPos = transform.position.y;
        GetNewDestination();
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            MoveCreature();
        }
        else
        {
            IdleCreature();
        }

        RB.position = ClampPositionToBounds(RB.position);
        Vector3 position = RB.position;
        position.y = YPos;
        RB.position = position;
    }

    private void MoveCreature()
    {
        if (MoveTimer > 0)
        {
            Vector3 direction = (MoveDestination - RB.position).normalized;
            RB.velocity = direction * MoveSpeed;

            if (Vector3.Distance(new Vector3(RB.position.x, 0, RB.position.z), 
                                 new Vector3(MoveDestination.x, 0, MoveDestination.z)) < 0.1f)
            {
                IdleTimer = 0;
                IsMoving = false;
                RB.velocity = Vector3.zero;
            }

            MoveTimer -= Time.fixedDeltaTime;
        }
        else
        {
            IdleTimer = 0;
            IsMoving = false;
            RB.velocity = Vector3.zero;
        }
    }

    private void IdleCreature()
    {
        RB.velocity = Vector3.zero;
        float timeToIdle = Random.Range(MinIdleTime, MaxIdleTime);
        if (IdleTimer >= timeToIdle)
        {
            GetNewDestination();
        }
        else
        {
            IdleTimer += Time.fixedDeltaTime;
        }
    }

    public void GetNewDestination()
    {
        int attempts = 0;
        const int maxAttempts = 30;

        while (attempts < maxAttempts)
        {
            Vector3 randomCircle = Random.insideUnitCircle * MoveRadius;
            Vector3 randomPoint = new Vector3(
                RB.position.x + randomCircle.x,
                YPos,
                RB.position.z + randomCircle.y
            );

            if (GameBounds.bounds.Contains(randomPoint))
            {
                MoveDestination = randomPoint;
                MoveTimer = Random.Range(MinMoveTime, MaxMoveTime);
                IsMoving = true;
                return;
            }

            attempts++;
        }

        // If we couldn't find a valid destination, just stay put
        MoveDestination = RB.position;
        MoveTimer = Random.Range(MinMoveTime, MaxMoveTime);
        IsMoving = true;
    }

    private Vector3 ClampPositionToBounds(Vector3 position)
    {
        Bounds bounds = GameBounds.bounds;
        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.z = Mathf.Clamp(position.z, bounds.min.z, bounds.max.z);
        return position;
    }
}