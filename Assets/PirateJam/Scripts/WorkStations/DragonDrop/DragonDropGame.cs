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

        [Tooltip("The available firing sequences that this mini game will sample from to shoot stuff at the player's bucket")]
        [SerializeField] private List<FiringSequence> firingSequences = new List<FiringSequence>();

        [Tooltip("DEBUG - The firing pattern used to test (triggered through context menu)")]
        [SerializeField] private FiringPattern debugFiringPattern;

        [Header("SFX"),SerializeField] private FMODUnity.EventReference popInNoise;

        

        private const int LV_0_DRAGON_SPIT_CHALLENGES = 5;

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
            for (int i = 0; i < LV_0_DRAGON_SPIT_CHALLENGES; i++)
            {
                FiringSequence chosenFiringSequence = firingSequences[UnityEngine.Random.Range(0, firingSequences.Count)];

                foreach(FiringInstance firingInstance in chosenFiringSequence.firingInstances)
                {
                    yield return new WaitForSeconds(firingInstance.GetPreFireDelay());
                    MakeDragonProjectileSpit(firingInstance);
                }
            }

            //When done with patterns end game
            //TODO:
            Evaluate();
        
        }

        // Makes the dragon shoot spit or fireballs
        public void MakeDragonProjectileSpit(FiringInstance firingInstance)
        {
            // foreach prefab in the firing pattern, if the object to launch is a spit, increment the skill check
            FiringPattern firingPattern = firingInstance.GetFiringPattern();

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
}
