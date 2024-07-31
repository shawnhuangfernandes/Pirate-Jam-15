using System.Collections.Generic;
using FMODUnity;
using PirateJam.Scripts.ActionList;
using UnityEngine;

namespace PirateJam.Scripts.WorkStations
{
    public abstract class WorkStation : MonoBehaviour
    {
        [SerializeField] protected GameObject screen;
        [SerializeField, Range(0, 3)] protected int WorkStationNumber;

        [SerializeField, ShowOnly] protected int currentLevel;
        [field: SerializeField] public int Level { get; protected set; }

        [SerializeField] protected MentorReaction mentor;
        [SerializeField] protected FMODUnity.EventReference goodGrade, passGrade, failGrade;

        public class Grade
        {
            public string Cause;
            public int Value;

            public Grade(string cause, int value)
            {
                Cause = cause;
                Value = value;
            }
        }

        [SerializeField] private List<Grade> _demerits = new();
        [SerializeField] private List<Grade> _achievements = new();

        protected virtual void Start()
        {
            GameManager.Instance.workStations.Add(this);
            Score = 70;
        }

        public int Score { get; protected set; } = 69;

        public virtual void Open()
        {
            //Swap State
            GameManager.Instance.SwapGameState(GameManager.GameState.WorkStation, WorkStationNumber);

            //Open scene
            screen.SetActive(true);

            _demerits.Clear();
            _achievements.Clear();
            mentor.Appear();

            Score = 100;
        }

        public virtual int AddDemerit(Grade demerit)
        {
            _demerits.Add(demerit);

            mentor.Scold();

            Score -= demerit.Value;
            return Score;
        }

        public virtual int AddAchievement(Grade achievement)
        {
            _achievements.Add(achievement);
            Score += achievement.Value;

            mentor.Approve();

            return Score;
        }

        public virtual void Evaluate()
        {
            var grade = Score / 100f;
            var status = grade >= 0.7f ? (grade >= 0.9f ? "Good" : "Pass") : "Fail";
            var clip = grade >= 0.7f ? (grade >= 0.9f ? goodGrade : passGrade) : failGrade;

            Debug.Log("Score: " + grade);

            ActionList.ActionList.Instance.AddAction(new DelegateAction<float>(true, mentor.GiveGrade, grade, 1,
                0.5f));

            ActionList.ActionList.Instance.AddAction(new DelegateAction<string>(true, GameManager.Instance.RunQuip,
                "Feedback" + WorkStationNumber + status));
            ActionList.ActionList.Instance.AddAction(
                new DelegateAction<EventReference>(true, PlayClip, clip, 1, 1f));

            ActionList.ActionList.Instance.AddAction(new DelegateAction(false, Close, 1, 3f));
        }

        private void PlayClip(EventReference er)
        {
            RuntimeManager.PlayOneShot(er);
        }

        public virtual bool IsDone()
        {
            return currentLevel >= Level;
        }

        public virtual void Close()
        {
            screen.SetActive(false);

            if (Score / 100f > .70 && currentLevel <= Level)
                ++currentLevel;

            GameManager.Instance.GameComplete();
        }
    }
}