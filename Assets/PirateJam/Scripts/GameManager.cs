using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles the recording and updating player's score.
 */
public class GameManager : MonoBehaviour
{
    [Tooltip("The number of mistakes the player has made during this game")]
    [SerializeField] private int demerits;

    [Tooltip("The number of skill checks the player has gone through")]
    [SerializeField] private int skillChecks;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void IncrementDemerit() => demerits += 1;

    public int GetDemerits => demerits;

    public void IncrementSkillCheck() => skillChecks += 1;

    public int GetSkillChecks => skillChecks;
}
