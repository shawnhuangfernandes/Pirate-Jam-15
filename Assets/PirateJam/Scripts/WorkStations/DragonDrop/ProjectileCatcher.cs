using System;
using FMODUnity;
using UnityEngine;

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    public class ProjectileCatcher : MonoBehaviour
    {
        [SerializeField] private FMODUnity.EventReference fireDrop;
        [SerializeField] private FMODUnity.EventReference spitDrop;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Projectile")) return;
            {
                RuntimeManager.PlayOneShot(other.gameObject.TryGetComponent(out DragonSpit ball) ? spitDrop : fireDrop);

                Destroy(other.gameObject);
            }
        }
    }
}
