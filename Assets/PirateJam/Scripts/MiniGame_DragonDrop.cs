using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script generic mini game behavior such as triggering it, 
 *  stopping it, earning demerits and triggering skill checks.
 */

[RequireComponent(typeof(BoxCollider))]
public class MiniGame_DragonDrop : MiniGame
{
    [Tooltip("The dragon visual that will spawn spit and fireballs")]
    [SerializeField] private Dragon dragon;

    [Tooltip("The location the player character moves to when the activity begins")]
    [SerializeField] private Transform activityStartPositionMarker;

    [Tooltip("The rotation of the player character when they move to the start position")]
    [SerializeField] private float activityStartPlayerRotation;

    [Tooltip("The bucket that follows the mouse to catch the spit")]
    [SerializeField] private MouseFollower bucketFollower;

    [Tooltip("The available firing sequences that this mini game will sample from to shoot stuff at the player's bucket")]
    [SerializeField] private List<FiringSequence> firingSequences = new List<FiringSequence>();

    private const float PLAYER_ROTATE_TIME = 0.5F;
    private const float PLAYER_MOVE_TIME = 1F;

    private const int LV_0_DRAGON_SPIT_CHALLENGES = 5;

    // Runs the visual/audio gameplay logic when a minigame is triggered by the player
    public override void MinigameStart()
    {
        base.MinigameStart();

        Transform playerXFRM = FindObjectOfType<PlayerController>().transform;
        Vector3 destinationPosition = new Vector3(activityStartPositionMarker.position.x, playerXFRM.position.y, activityStartPositionMarker.position.z);
        Vector3 destinationRotation = new Vector3(0F, activityStartPlayerRotation, 0F);
        playerXFRM.DOMove(destinationPosition, PLAYER_MOVE_TIME).OnComplete(() => 
        { 
            playerXFRM.DORotate(destinationRotation, PLAYER_ROTATE_TIME);
        });

        bucketFollower.SetFollow(true);

        TriggerSkillCheck();
    }

    // Runs the visual/audio gameplay logic when a minigame reaches its end
    public override void MinigameEnd()
    {
        base.MinigameEnd();

        bucketFollower.SetFollow(false);
    }

    // Runs the visual/audio gameplay logic for a skill check
    public override void TriggerSkillCheck()
    {
        StartCoroutine(ActivateDragonSpitting());
    }

    private IEnumerator ActivateDragonSpitting()
    {
        for (int i = 0; i < LV_0_DRAGON_SPIT_CHALLENGES; i++)
        {
            FiringSequence chosenFiringSequence = firingSequences[UnityEngine.Random.Range(0, firingSequences.Count)];

            foreach(FiringInstance firingInstance in chosenFiringSequence.firingInstances)
            {
                yield return new WaitForSeconds(firingInstance.GetPreFireDelay());
                MakeDragonProjectileSpit(firingInstance);
            }
        }

        yield return new WaitForSeconds(MINI_GAME_END_PADDING_TIME);
        MinigameEnd();
    }

    // Makes the dragon shoot spit or fireballs
    public void MakeDragonProjectileSpit(FiringInstance firingInstance)
    {
        // foreach prefab in the firing pattern, if the object to launch is a spit, increment the skill check
        FiringPattern firingPattern = firingInstance.GetFiringPattern();

        foreach(LaunchableObject launchable in firingPattern.GetLaunchables())
        {
            if (launchable.prefab.TryGetComponent(out DragonSpit skillChallenge))
            {
                OnSkillCheckTriggered();
            }
        }

        firingPattern.Launch(dragon.transform.position);
    }

    /*
     * This class holds a pattern of fire that is time delayed
     */
    [Serializable]
    public class FiringInstance
    {
        [Tooltip("The delay before the firing pattern occurs")]
        [SerializeField] private float delayBeforeFiring;

        [Tooltip("The firing pattern (think shotgun spread) used")]
        [SerializeField] private FiringPattern firingPattern;

        public FiringPattern GetFiringPattern() => firingPattern;

        public float GetPreFireDelay() => delayBeforeFiring;
    }

    /*
     * This class holds a list of firing instances (time delayed firing patterns)
     */
    [Serializable]
    public class FiringSequence
    {
        public List<FiringInstance> firingInstances = new List<FiringInstance>();
    }
}
