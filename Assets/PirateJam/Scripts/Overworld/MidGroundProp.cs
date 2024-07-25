using UnityEngine;
using UnityEngine.Rendering;

namespace PirateJam.Scripts
{
    [RequireComponent(typeof(SortingGroup))]
    public class MidGroundProp : MonoBehaviour
    {
        private SortingGroup _spriteGroup;

       [SerializeField, ShowOnly] private bool isOver = false;

        [SerializeField] private Transform target;

        [SerializeField] private Transform pivot;
        // Start is called before the first frame update
        void Start()
        {
            _spriteGroup = GetComponent<SortingGroup>();
            if (target == null || pivot == null)
            {
                Debug.LogError("No target set for " + gameObject.name);
                target = transform;
                pivot = target;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isOver)
            {
                if (target.position.y <= pivot.position.y)
                {
                    isOver = false;

                    _spriteGroup.sortingLayerName = "Background";
                }
            }
            else
            {
                if (target.position.y >= pivot.position.y)
                {
                    isOver = true;

                    _spriteGroup.sortingLayerName = "Midground";
                }
            }
        }
    }
}
