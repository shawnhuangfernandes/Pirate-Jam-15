using Cinemachine;
using UnityEngine;

/*
 * This script generic mini game behavior such as triggering it, 
 *  stopping it, earning demerits and triggering skill checks.
 */

[RequireComponent(typeof(BoxCollider))]
public abstract class MiniGame : MonoBehaviour
{
    [Header("DEBUG")]
    [Tooltip("DEBUG - make this minigame active at the start")]
    [SerializeField] private bool debugMakeActiveOnStart = false;

    [Header("Properties")]

    [Tooltip("The camera that provides the player view while in the minigame")]
    [SerializeField] private CinemachineVirtualCamera miniGameCam;





    private int demeritsEarned = 0;
    private int skillChecks = 0;

    private bool isInteractable = false;

    private void Awake()
    {
        if (miniGameCam == null)
            Debug.LogError("Please attach a miniGameCam to " + gameObject);

        SetInteractable(debugMakeActiveOnStart);
        miniGameCam.Priority = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isInteractable)
        {
            MinigameStart();
            isInteractable = false;
        }
        
    }

    // Runs the visual/audio gameplay logic when a minigame is triggered by the player
    public virtual void MinigameStart()
    {
        miniGameCam.Priority = 100;
        
    }

    // Runs the visual/audio gameplay logic when a minigame reaches its end
    public virtual void MinigameEnd()
    {
        miniGameCam.Priority = 0;
    }

    // Runs the visual/audio gameplay logic for a skill check
    public abstract void TriggerSkillCheck();


    protected void EarnDemerit()
    {
        demeritsEarned++;
        GameManager.Instance.IncrementDemerit();
    }

    protected void SkillCheckCount()
    {
        skillChecks++;
        GameManager.Instance.IncrementSkillCheck();
    }

    private void SetInteractable(bool val) => isInteractable = val;

    [ContextMenu("DEBUG Set Mini Game View")]
    public void SetMiniGameViewManually()
    {
        miniGameCam.Priority = 100;
    }
}
