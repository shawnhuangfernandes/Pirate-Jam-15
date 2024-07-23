using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Firing Pattern")]
public class FiringPattern : ScriptableObject
{
    [Tooltip("The list of objects to launch when this firing pattern activates")]
    [SerializeField] private List<LaunchableObject> launchables = new List<LaunchableObject>();

    public void Launch(Vector3 spawnPosition)
    {
        foreach(LaunchableObject launchable in launchables)
        {
            Rigidbody launchableRb2d = Instantiate(launchable.prefab, spawnPosition, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 directionToMove = Quaternion.AngleAxis(launchable.launchAngle, Vector3.right) * Vector3.up;

            launchableRb2d.velocity = directionToMove * launchable.speed;
        }
    }
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
