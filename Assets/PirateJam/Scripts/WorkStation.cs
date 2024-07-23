using System.Collections.Generic;
using PirateJam.Scripts.App;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PirateJam.Scripts
{
    public abstract class WorkStation : MonoBehaviour
    {
        [SerializeField] protected GameObject screen;
        [SerializeField,Range(0,3)] protected int WorkStationNumber;
        
        [field: SerializeField] public int Level { get; protected set; }
        
        public struct Demerit
        {
            public string Cause;
            public int Value;
        }

        [SerializeField] private List<Demerit> _demerits = new();
        public int Score { get; protected set; }
        public virtual void Open()
        {
            //Swap State
            GameManager.Instance.SwapGameState(GameManager.GameState.WorkStation, WorkStationNumber);
            
            //Open scene
            screen.SetActive(true);
            
            _demerits.Clear();
        }
        
        public virtual int AddDemerit(Demerit demerit)
        {
            _demerits.Add(demerit);
            
            Score -= demerit.Value;
            return Score;
        }

        public virtual void Evaluate()
        {
            //TODO: list demerits
            
            //TODO: publish score
        }

        public virtual void Close()
        {
            InputManager.Instance.SwapInputMaps("BasicMove");
            
            screen.SetActive(false);

        }
    }
}
