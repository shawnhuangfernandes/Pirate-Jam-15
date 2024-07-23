
using System;
using PirateJam.Scripts.App;


namespace PirateJam.Scripts
{
    public class ExampleWorkStation : WorkStation
    {
        private BoolAction exit;

        public void Start()
        {
            exit = InputManager.Instance.GetInput("WorkStation", "Pause") as BoolAction;

        }

        public override void Open()
        {
            base.Open();

        }

        public void Update()
        {
            if (exit.IsPressed)
            {
                Close();
            }
        }
    }
}
