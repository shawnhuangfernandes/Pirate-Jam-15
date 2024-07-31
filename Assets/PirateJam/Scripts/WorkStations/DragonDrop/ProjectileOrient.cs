using UnityEngine;

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileOrient : MonoBehaviour
    {

        private Rigidbody _body;
        
        // Start is called before the first frame update
        void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //rotate in the direction of velocity
            transform.forward = _body.velocity.normalized;
        }
    }
}
