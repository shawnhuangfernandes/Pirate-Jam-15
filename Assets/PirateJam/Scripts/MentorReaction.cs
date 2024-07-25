using PirateJam.Scripts.ActionList;
using TMPro;
using UnityEngine;

namespace PirateJam.Scripts
{
    public class MentorReaction : Singleton<MentorReaction>
    {
        [SerializeField] private TMP_Text _bubbleText;
        [SerializeField] private GameObject _talkBubble;

        [SerializeField] private GameObject visuals;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Called at the begining of an activity
        /// </summary>
        public void Appear()
        {
            visuals.SetActive(true);
        }

        /// <summary>
        /// Called at the end of evaluating
        /// </summary>
        public void Disappear()
        {
            visuals.SetActive(false);
            _talkBubble.SetActive(false);
        }

        /// <summary>
        /// Feedback given at the end of an activity
        /// </summary>
        /// <param name="grade"></param>
        public void GiveGrade(float grade)
        {
            _talkBubble.SetActive(true);
            var feedback = grade switch
            {
                >= .99f => "A+",
                >= .95f => "A",
                >= .9f => "A-",
                >= .87f => "B+",
                >= .84f => "B",
                >= .8f => "B-",
                >= .77f => "C+",
                >= .74f => "C",
                >= .7f => "C-",
                >= .67f => "D+",
                >= .64f => "D",
                >= .6f => "D-",
                _ => "F"
            };

            _bubbleText.text = feedback;

            ActionList.ActionList.Instance.AddAction(new DelegateAction(false, Disappear, 1, 2.5f));
        }

        /// <summary>
        /// Feedback for doing well
        /// </summary>
        public void Approve()
        {
            //TODO: Random Chance so it doesn't spam
        }

        /// <summary>
        /// Feedback for earning demerits
        /// </summary>
        public void Scold()
        {
        }
    }
}