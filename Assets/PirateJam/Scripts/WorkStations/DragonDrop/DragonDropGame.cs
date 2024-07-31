using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/*
 * This script generic mini game behavior such as triggering it, 
 *  stopping it, earning demerits and triggering skill checks.
 */

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    public class DragonDropGame : WorkStation
    {
        [Tooltip("The dragon visual that will spawn spit and fireballs")]
        [SerializeField] private Dragon dragon;

        [Tooltip("The bucket that follows the mouse to catch the spit")]
        [SerializeField] private MouseFollower bucketFollower;

        [Tooltip("DEBUG - The firing pattern used to test (triggered through context menu)")]
        [SerializeField] private FiringPattern debugFiringPattern;

        [Tooltip("The starting maximum firing delay between dragon shots")]
        [SerializeField] private float maximumFiringDelay = 2F;

        [Tooltip("The ending minimum firing delay between dragon shots")]
        [SerializeField] private float minimalFiringDelay = 1F;

        [Tooltip("The percentage of shots made until the delay is the ending firing delay")]
        [Range(0F, 1F)]
        [SerializeField] private float percentageOfShotsUntilMinimalDelay = 0.5F;

        [Tooltip("The firing patterns used by the dragon")]
        [SerializeField] private List<FiringPattern> dragonFiringPatterns = new List<FiringPattern>();

        [Header("SFX"),SerializeField] private FMODUnity.EventReference popInNoise;

        [Tooltip("The number of times the dragon shoots fire or spit")]
        [SerializeField] private int numberOfDragonShots = 35;

        // Runs the visual/audio gameplay logic when a minigame is triggered by the player
        public override void Open()
        {
            base.Open();

            bucketFollower.SetFollow(true);

            // IF DEBUGGING IS ENABLED, DO NOT START THE ACTIVITY
            if (debugFiringPattern == null)
                TriggerSkillCheck();
        }

        // Runs the visual/audio gameplay logic when a minigame reaches its end
        public override void Close()
        {
            base.Close();

            bucketFollower.SetFollow(false);
        }

        // Runs the visual/audio gameplay logic for a skill check
        public void TriggerSkillCheck()
        {
            StartCoroutine(ActivateDragonSpitting());
        }

        private IEnumerator ActivateDragonSpitting()
        {
            yield return new WaitForSeconds(2F);

            for (int i = 0; i < numberOfDragonShots; i++)
            {
                float interpolant = (i / (float) numberOfDragonShots) / percentageOfShotsUntilMinimalDelay;
                float firingDelay = Mathf.Lerp(maximumFiringDelay, minimalFiringDelay, interpolant);
                yield return new WaitForSeconds(firingDelay);
                MakeDragonProjectileSpit();
            }

            //When done with patterns end game
            //TODO:
            Evaluate();
        
        }

        // Makes the dragon shoot spit or fireballs
        public void MakeDragonProjectileSpit()
        {
            // foreach prefab in the firing pattern, if the object to launch is a spit, increment the skill check
            FiringPattern firingPattern = dragonFiringPatterns[UnityEngine.Random.Range(0, dragonFiringPatterns.Count)];

            firingPattern.Launch(dragon.transform);
        }

        [ContextMenu("DEBUG Fire Projectile")]
        public void DebugFireProjectile()
        {
            debugFiringPattern.Launch(dragon.transform);
        }

        /*
     * This class holds a pattern of fire that is time delayed
     */
        [Serializable]
        public class FiringInstance
        {
            [Tooltip("The firing pattern (think shotgun spread) used")]
            [SerializeField] private FiringPattern firingPattern;

            public FiringPattern GetFiringPattern() => firingPattern;
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
}
