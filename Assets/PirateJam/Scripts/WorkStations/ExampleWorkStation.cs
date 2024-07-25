using System;
using PirateJam.Scripts.App;
using PirateJam.Scripts.WorkStations;
using UnityEngine.PlayerLoop;

namespace PirateJam.Scripts
{
    public class ExampleWorkStation : WorkStation
    {
        private BoolAction escapeInput;
        public override void Open()
        {
            base.Open();
            escapeInput = InputManager.Instance.GetInput("ExampleWorkStation","Exit") as BoolAction;

        }

        public void Update()
        {
            
            if (enabled && escapeInput.IsTriggered)
            {
                InputManager.Instance.SwapInputMaps("BasicMove");
                screen.SetActive(false);
            }
        }
    }
}
