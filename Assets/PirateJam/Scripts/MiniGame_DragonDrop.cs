using Cinemachine;
using DG.Tweening;
using System;
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


    [Tooltip("The collection of time seperated sequences the dragon uses to shoot")]
    [SerializeField] private List<FiringSequence> dragonFiringSequences = new List<FiringSequence>();

    [Tooltip("The location the player character moves to when the activity begins")]
    [SerializeField] private Transform activityStartPositionMarker;

    [Tooltip("The rotation of the player character when they move to the start position")]
    [SerializeField] private float activityStartPlayerRotation;

    private const float PLAYER_ROTATE_TIME = 0.5F;
    private const float PLAYER_MOVE_TIME = 1F;

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
    }

    // Runs the visual/audio gameplay logic when a minigame reaches its end
    public override void MinigameEnd()
    {
        base.MinigameEnd();
    }

    // Runs the visual/audio gameplay logic for a skill check
    public override void TriggerSkillCheck()
    {

    }

    [ContextMenu("Test Fire")]
    // Makes the dragon shoot spit or fireballs
    public void MakeDragonProjectileSpit()
    {
        dragonFiringSequences[0].GetFiringPattern().Launch(dragon.transform.position);
    }

    [Serializable]
    public class FiringSequence
    {
        [Tooltip("The delay before the firing pattern occurs")]
        [SerializeField] private float delayBeforeFiring;

        [Tooltip("The firing pattern (think shotgun spread) used")]
        [SerializeField] private FiringPattern firingPattern;

        public FiringPattern GetFiringPattern() => firingPattern;
    }
}
