using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private UpdateProvider _updateProvider;
        [SerializeField] private Config _config;
        
        private void Start()
        {
            ILevelLoader levelLoader = new LevelLoader();
            IPlayerModel playerModel = new PlayerModel();
            playerModel.Initialise(_config, _updateProvider);
            IInputManager inputManager = new InputManager();
            inputManager.Initialize(_updateProvider);
            ICameraManager cameraManager = new CameraManager();
            cameraManager.Initialize(_updateProvider, _config);
            IFloorButtonsAndDoorsManager floorButtonsAndDoorsManager = new FloorButtonsAndDoorsManager();
            INPCManager npcManager = new NpcManager();
            npcManager.Initialize(_updateProvider, _config);

            Presenter presenter = new Presenter(new Presenter.Dependencies
            {
                LevelLoader = levelLoader,
                PlayerModel = playerModel,
                InputManager = inputManager,
                CameraManager = cameraManager,
                FloorButtonsAndDoorsManager = floorButtonsAndDoorsManager,
                NpcManager = npcManager,
            });
            
            levelLoader.LoadLevel("Level 1");
        }
    }
}