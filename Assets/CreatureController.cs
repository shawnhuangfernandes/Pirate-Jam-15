using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementPattern { Straight, ZigZag, Looped }
public class CreatureController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public MovementPattern Type;
    public float MoveRadius = 5f;
    public float MinMoveTime = 3f;
    public float MaxMoveTime = 10f;
    public float MinIdleTime = 2f;
    public float MaxIdleTime = 5f;
    public bool IsMoving = false;
    public bool FoodDetected = false;

    public Collider GameBounds;
    private float boundaryMinX, boundaryMaxX, boundaryMinZ, boundaryMaxZ;
    public Vector3 MoveDestination;

    private Rigidbody RB;
    private float MoveTimer;
    private float IdleTimer;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float foodPlaneHeight = 1f;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();

        RB = GetComponent<Rigidbody>();
        InitializeBoundary();
        GetDestination();
        transform.position = MoveDestination;
    }

    private void InitializeBoundary()
    {
        if (GameBounds != null)
        {
            Bounds bounds = GameBounds.bounds;
            boundaryMinX = bounds.min.x;
            boundaryMaxX = bounds.max.x;
            boundaryMinZ = bounds.min.z;
            boundaryMaxZ = bounds.max.z;
        }
        else
        {
            Debug.LogError("Boundary object does not have a Collider component!");
        }
    }

    private void GetDestination()
    {
        if (GameBounds == null) return;

        for (int i = 0; i < 30; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * MoveRadius;
            Vector3 randomPoint = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y,
                transform.position.z + randomCircle.y
            );

            if (IsPointWithinBoundary(randomPoint))
            {
                MoveDestination = randomPoint;
                MoveTimer = UnityEngine.Random.Range(MinMoveTime, MaxMoveTime);
                IsMoving = true;
                return;
            }
        }

        Debug.LogWarning("Could not find a valid destination within 30 attempts.");

    }

    private bool IsPointWithinBoundary(Vector3 point)
    {
        return point.x >= boundaryMinX && point.x <= boundaryMaxX &&
               point.z >= boundaryMinZ && point.z <= boundaryMaxZ;
    }

    // Update is called once per frame
    void Update()
    {
        FoodDetected = Input.GetMouseButton(0);
        if (!FoodDetected)
        {
            if (IsMoving)
                MoveCreature(MoveDestination);
            else
                IdleCreature();
        }
        else
        {
            MoveCreature(GrabFoodLocation());
        }
    }

    private Vector3 GrabFoodLocation()
    {
        float planeY = GameBounds.bounds.max.y + foodPlaneHeight;
        Plane foodPlane = new Plane(Vector3.up, new Vector3(0, planeY, 0));

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (foodPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            Vector3 foodLocation = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);

            foodLocation.x = Mathf.Clamp(foodLocation.x, boundaryMinX, boundaryMaxX);
            foodLocation.z = Mathf.Clamp(foodLocation.z, boundaryMinZ, boundaryMaxZ);

            return foodLocation;
        }

        return transform.position;
    }

    private void IdleCreature()
    {
        var timeToIdle = UnityEngine.Random.Range(MinIdleTime, MaxIdleTime);
        if (IdleTimer >= timeToIdle)
        {
            GetDestination();
        }
        else
        {
            IdleTimer += Time.deltaTime;
        }

    }

    private void MoveCreature(Vector3 destination)
    {
        if (FoodDetected)
        {
            Vector3 direction = (destination - transform.position).normalized;
            RB.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);
        }
        else
        {
            if ((MoveTimer > 0))
            {
                Vector3 direction = (destination - transform.position).normalized;
                RB.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, destination) < 0.1f)
                {
                    IdleTimer = 0;
                    IsMoving = false;
                }

                MoveTimer -= Time.deltaTime;
            }
            else
            {
                IdleTimer = 0;
                IsMoving = false;
            }
        }

    }
}
