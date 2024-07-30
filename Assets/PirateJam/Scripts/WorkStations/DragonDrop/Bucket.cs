using FMODUnity;
using UnityEngine;

/*
 * This script is used to determine if the player is catching spit.
 * This also tracks if the player is touching a fireball (earning a demerit)
 */

namespace PirateJam.Scripts.WorkStations.DragonDrop
{


    public class Bucket : MonoBehaviour
    {
        [SerializeField] private FMODUnity.EventReference fireCaught;
        [SerializeField] private FMODUnity.EventReference spitCaught;
        [SerializeField] private WorkStation game;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Fireball fireball))
            {
                game.AddDemerit(new WorkStation.Grade("caught fireball", 10));
                RuntimeManager.PlayOneShot(fireCaught);
                Destroy(other.gameObject);
            }
            
            else if (other.TryGetComponent(out DragonSpit spitball))
            {
                game.AddAchievement(new WorkStation.Grade("spit caught", 1));
                RuntimeManager.PlayOneShot(spitCaught);
                Destroy(other.gameObject);
            }
        }
    }
}
