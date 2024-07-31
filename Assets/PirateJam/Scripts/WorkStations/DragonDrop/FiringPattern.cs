using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    [CreateAssetMenu(fileName = "Firing Pattern")]
    public class FiringPattern : ScriptableObject
    {
        [Tooltip("The list of objects to launch when this firing pattern activates")]
        [SerializeField] private List<LaunchableObject> launchables = new List<LaunchableObject>();

        [SerializeField] private FMODUnity.EventReference fireCast;
        [SerializeField] private FMODUnity.EventReference spitCast;
        public void Launch(Transform origin)
        {
            foreach(LaunchableObject launchable in launchables)
            {
               var launch = Instantiate(launchable.prefab, origin.position, origin.rotation, origin);
               var launchableRb = launch.GetComponent<Rigidbody>();
               RuntimeManager.PlayOneShot(launch.TryGetComponent(out DragonSpit ball) ? spitCast : fireCast);

                launch.transform.Rotate(origin.up,90f);
                launch.transform.Rotate(origin.forward,launchable.launchAngle -90f);
               
               launchableRb.velocity = launch.transform.forward * launchable.speed;
            }
        }

        public List<LaunchableObject> GetLaunchables() => launchables;
    }

    [Serializable]
    public class LaunchableObject
    {
        [Tooltip("The rigidbody object to spawn")]
        public Rigidbody prefab;

        [Tooltip("The speed of the object when launched")]
        public float speed;

        [Tooltip("The angle of launch, where 0 is left, and 90 is up")]
        public float launchAngle;
    }
}