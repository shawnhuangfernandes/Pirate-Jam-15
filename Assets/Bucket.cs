using UnityEngine;

/*
 * This script is used to determine if the player is catching spit.
 * This also tracks if the player is touching a fireball (earning a demerit)
 */

public class Bucket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Fireball fireball))
        {
            FindObjectOfType<MiniGame_DragonDrop>().OnDemeritEarned();

            Destroy(other.gameObject);
        }

        if (other.TryGetComponent(out DragonSpit dragonSpit))
        {
            Destroy(other.gameObject);
        }
    }
}
