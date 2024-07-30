using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PirateJam.Scripts.WorkStations;
using UnityEngine;

public class NurseryGameManager : WorkStation
{
    public int TotalCreatures = 15;
    public float SpawnMargin = 0.1f;
    public float FoodHeight = 1f;
    public float MaxRaycastDistance = 5f;

    public GameObject[] CreaturePrefabs;
    public GameObject FoodPrefab;
    public GameObject IndicatorPrefab;
    public Transform PlayArea;
    public GameObject FoodBag;
    public Camera MainCamera;
    public GameObject ParticleSystemPrefab;

    private List<CreatureController> CreatureList = new List<CreatureController>();
    private Queue<CreatureController> FeedingQueue = new Queue<CreatureController>();
    private List<GameObject> ActiveFoods = new List<GameObject>();
    private GameObject ActiveIndicator;
    private bool IsDraggingFood = false;
    private Plane FoodPlane;
    private Bounds PlayAreaBounds;
    private GameObject HungryParticles;
    private int RemainingFood;

    private bool isOpen = false;

    protected override void Start()
    {
        base.Start();
        
        PlayAreaBounds = PlayArea.GetComponent<Collider>().bounds;
        FoodPlane = new Plane(Vector3.up, new Vector3(0, PlayArea.position.y + FoodHeight, 0));
       
    }

    public override void Open()
    {
        base.Open();
        isOpen = true;
        StartCoroutine(SpawnCreaturesSequentially());
    }

    private void Update()
    {
        if (!isOpen) return;
        
        HandleFoodInteraction();
        UpdateParticleSystemPosition();
    }

    private IEnumerator SpawnCreaturesSequentially()
    {
        for (int i = 0; i < TotalCreatures; i++)
        {
            GameObject creaturePrefab = CreaturePrefabs[Random.Range(0, CreaturePrefabs.Length)];
            Vector3 spawnPosition = GetRandomPositionInPlayArea();

            int attempts = 0;
            while (Physics.OverlapSphere(spawnPosition, 0.5f).Length > 0 && attempts < 30)
            {
                spawnPosition = GetRandomPositionInPlayArea();
                attempts++;
                yield return null;
            }

            GameObject creature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity, PlayArea);
            CreatureList.Add(creature.GetComponent<CreatureController>());

            yield return new WaitForSeconds(0.1f);
        }

        RemainingFood = TotalCreatures;
        GenerateFeedingQueue();
    }

    private void GenerateFeedingQueue()
    {
        List<CreatureController> shuffledCreatures = new List<CreatureController>(CreatureList);
        for (int i = 0; i < shuffledCreatures.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledCreatures.Count);
            CreatureController temp = shuffledCreatures[i];
            shuffledCreatures[i] = shuffledCreatures[randomIndex];
            shuffledCreatures[randomIndex] = temp;
        }

        foreach (CreatureController creature in shuffledCreatures)
        {
            FeedingQueue.Enqueue(creature);
        }

        SpawnParticleSystem();
    }

    private void SpawnParticleSystem()
    {
        if (FeedingQueue.Count > 0)
        {
            CreatureController currentCreature = FeedingQueue.Peek();
            HungryParticles = Instantiate(ParticleSystemPrefab, currentCreature.transform.position, Quaternion.identity, currentCreature.transform);
        }
        else
        {
            Debug.Log("All creatures have been fed!");
            // Add any game completion logic here
        }
    }

    private void UpdateParticleSystemPosition()
    {
        if (HungryParticles != null && FeedingQueue.Count > 0)
        {
            HungryParticles.transform.position = FeedingQueue.Peek().transform.position;
        }
    }

    private void HandleFoodInteraction()
    {
        if (Input.GetMouseButtonDown(0) && !IsDraggingFood && RemainingFood > 0)
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == FoodBag)
                {
                    StartDraggingFood();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && IsDraggingFood)
        {
            StopDraggingFood();
        }

        if (IsDraggingFood && ActiveFoods.Count > 0)
        {
            UpdateDraggingFood();
        }
    }

    private void StartDraggingFood()
    {
        Vector3 foodPosition = GetFoodPositionFromMouse();
        GameObject newFood = Instantiate(FoodPrefab, foodPosition, Quaternion.identity);
        ActiveFoods.Add(newFood);
        IsDraggingFood = true;
        RemainingFood--;

        Rigidbody foodRb = newFood.GetComponent<Rigidbody>();
        if (foodRb != null)
        {
            foodRb.useGravity = false;
            foodRb.isKinematic = true;
        }

        ActiveIndicator = Instantiate(IndicatorPrefab, foodPosition, Quaternion.identity);
        UpdateIndicatorPosition();
    }

    private void StopDraggingFood()
    {
        IsDraggingFood = false;
        if (ActiveIndicator != null)
        {
            Destroy(ActiveIndicator);
            ActiveIndicator = null;
        }

        if (ActiveFoods.Count > 0)
        {
            GameObject lastFood = ActiveFoods[ActiveFoods.Count - 1];
            Rigidbody foodRb = lastFood.GetComponent<Rigidbody>();
            if (foodRb != null)
            {
                foodRb.useGravity = true;
                foodRb.isKinematic = false;
            }
        }
    }

    private void UpdateDraggingFood()
    {
        Vector3 foodPosition = GetFoodPositionFromMouse();
        ActiveFoods[ActiveFoods.Count - 1].transform.position = foodPosition;
        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        if (ActiveIndicator != null && ActiveFoods.Count > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(ActiveFoods[ActiveFoods.Count - 1].transform.position, Vector3.down, out hit, MaxRaycastDistance))
            {
                if (hit.collider.GetComponent<CreatureController>() == null)
                    ActiveIndicator.transform.position = hit.point;
                else
                {
                    RaycastHit[] hits = Physics.RaycastAll(ActiveFoods[ActiveFoods.Count - 1].transform.position, Vector3.down, MaxRaycastDistance);
                    foreach (RaycastHit h in hits)
                    {
                        if (h.collider.GetComponent<CreatureController>() == null)
                        {
                            ActiveIndicator.transform.position = h.point;
                            break;
                        }
                    }
                }
            }
            else
            {
                ActiveIndicator.transform.position = ActiveFoods[ActiveFoods.Count - 1].transform.position + Vector3.down * MaxRaycastDistance;
            }
        }
    }

    private Vector3 GetFoodPositionFromMouse()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (FoodPlane.Raycast(ray, out distance))
            return ray.GetPoint(distance);

        return Vector3.zero;
    }

    private Vector3 GetRandomPositionInPlayArea()
    {
        float marginX = PlayAreaBounds.size.x * SpawnMargin;
        float marginZ = PlayAreaBounds.size.z * SpawnMargin;

        return new Vector3(
            Random.Range(PlayAreaBounds.min.x + marginX, PlayAreaBounds.max.x - marginX),
            PlayAreaBounds.center.y,
            Random.Range(PlayAreaBounds.min.z + marginZ, PlayAreaBounds.max.z - marginZ)
        );
    }

    public bool IsCurrentQueuedCreature(CreatureController creature)
    {
        return FeedingQueue.Count > 0 && FeedingQueue.Peek() == creature;
    }

    public void OnFoodDestroyed(GameObject food)
    {
        ActiveFoods.Remove(food);
        Destroy(food);
    }

    public void OnCreatureAteFood(CreatureController creature)
    {
        if (IsCurrentQueuedCreature(creature))
        {
            OnCorrectCreatureFed(creature);
        }
        else
        {
            OnIncorrectCreatureFed(creature);
        }
    }

    public void OnCorrectCreatureFed(CreatureController creature)
    {
        if (FeedingQueue.Count > 0 && FeedingQueue.Peek() == creature)
        {
            FeedingQueue.Dequeue();
            CreatureList.Remove(creature);
            Destroy(HungryParticles);
            AddAchievement(new Grade("Fed correct pet", 10));
            SpawnParticleSystem();
        }
    }

    public void OnIncorrectCreatureFed(CreatureController creature)
    {
        AddDemerit(new Grade("Incorrect food for pet", 10));

        if (FeedingQueue.Count > 0 && HungryParticles != null && HungryParticles.transform.parent == creature.transform)
        {
            Destroy(HungryParticles);
            SpawnParticleSystem();
        }
    }
}