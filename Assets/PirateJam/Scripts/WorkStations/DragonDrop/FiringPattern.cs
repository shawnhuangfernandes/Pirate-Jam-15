using System;
using System.Collections.Generic;
using UnityEngine;

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    [CreateAssetMenu(fileName = "Firing Pattern")]
    public class FiringPattern : ScriptableObject
    {
        [Tooltip("The list of objects to launch when this firing pattern activates")]
        [SerializeField] private List<LaunchableObject> launchables = new List<LaunchableObject>();

        public void Launch(Transform origin)
        {
            foreach(LaunchableObject launchable in launchables)
            {
               var launch = Instantiate(launchable.prefab, origin.position, Quaternion.AngleAxis(launchable.launchAngle, origin.right));
               var launchableRb = launch.GetComponent<Rigidbody>(); 
               
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

        [Tooltip("The angle of launch, where 0 is up, and 90 is right")]
        public float launchAngle;
    }
}