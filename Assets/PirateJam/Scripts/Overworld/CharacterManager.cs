using UnityEngine;

namespace PirateJam.Scripts
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        //Use a gameobejct reference for now, will change to do proper visuals
        [SerializeField] private GameObject PlayerVisual;

        public void DisappearPlayer()
        {
            //TODO: fancy transition
            PlayerVisual.SetActive(false);
        }

        public void AppearPlayer()
        {
            //TODO: fancy transition
            PlayerVisual.SetActive(true);
        }
    }
}
