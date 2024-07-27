using System;
using UnityEngine;

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    public class ProjectileCatcher : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Projectile")) return;
            {
                Destroy(other.gameObject);
            }
        }
    }
}
