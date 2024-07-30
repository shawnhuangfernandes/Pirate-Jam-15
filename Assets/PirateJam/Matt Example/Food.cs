using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject EatingEffectPrefab;
    private NurseryGameManager NGM;

    private void Start()
    {
        NGM = FindObjectOfType<NurseryGameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillPlane"))
        {
            NGM.OnFoodDestroyed(gameObject);
        }
        else
        {
            CreatureController creature = collision.gameObject.GetComponent<CreatureController>();
            if (creature != null)
            {
                // Instantiate eating effect
                if (EatingEffectPrefab != null)
                {
                    Instantiate(EatingEffectPrefab, transform.position, Quaternion.identity);
                }

                NGM.OnCreatureAteFood(creature);
                NGM.OnFoodDestroyed(gameObject);
            }
        }
    }
}