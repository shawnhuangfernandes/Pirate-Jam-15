using FMODUnity;
using PirateJam.Scripts.ActionList;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace PirateJam.Scripts
{
    public class MentorReaction : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;
        [SerializeField] private ParticleSystem poofParticles;
        private InMemoryVariableStorage _storage;
        [SerializeField] private FMODUnity.EventReference teleportNoise;
        
        
        // Start is called before the first frame update
        void Start()
        {
            //GameManager.Instance.runner.AddCommandHandler("disappear",Disappear);
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
            poofParticles.Play();
            RuntimeManager.PlayOneShot(teleportNoise);
        }

        /// <summary>
        /// Called at the end of evaluating
        /// </summary>
        [YarnCommand("disappear")]
        public void Disappear()
        {
            visuals.SetActive(false);
            poofParticles.Play();
            RuntimeManager.PlayOneShot(teleportNoise);

        }

        /// <summary>
        /// Feedback given at the end of an activity
        /// </summary>
        /// <param name="grade"></param>
        public void GiveGrade(float grade)
        {
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
            

            GameManager.Instance.VariableStorage.SetValue("$grade", feedback);


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