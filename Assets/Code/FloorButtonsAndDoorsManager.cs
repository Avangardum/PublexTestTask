using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Avangardum.PublexTestTask
{
    class FloorButtonsAndDoorsManager : IFloorButtonsAndDoorsManager
    {
        private Dictionary<IFloorButtonModel, IDoorModel> ButtonsAndDoors = new Dictionary<IFloorButtonModel, IDoorModel>();
        
        public void OnLevelLoaded()
        {
            Cleanup();

            var buttonMBs = GameObject.FindObjectsOfType<FloorButtonMB>();
            foreach (var buttonMB in buttonMBs)
            {
                var button = new FloorButtonModel();
                button.Initialise(buttonMB.gameObject);
                button.Pressed += OnButtonPressed;

                var doorGO = buttonMB.ConnectedDoor;
                Assert.IsNotNull(doorGO);
                var door = new DoorModel();
                door.Initialize(doorGO);
                
                ButtonsAndDoors.Add(button, door);
            }
        }

        private void OnButtonPressed(object sender, EventArgs e)
        {
            var button = (FloorButtonModel) sender;
            var door = ButtonsAndDoors[button];
            door.Switch();
        }

        private void Cleanup()
        {
            foreach (var pair in ButtonsAndDoors)
            {
                var button = pair.Key;
                var door = pair.Value;
                
                button.Cleanup();
                button.Pressed -= OnButtonPressed;
            }
            ButtonsAndDoors.Clear();
        }
    }
}