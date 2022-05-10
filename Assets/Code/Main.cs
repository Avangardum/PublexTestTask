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
            INpcManager npcManager = new NpcManager();
            npcManager.Initialize(_updateProvider, _config);
            IUserInterface userInterface = new UserInterface();
            userInterface.Initialize(_updateProvider);
            IGameManager gameManager = new GameManager();

            Presenter presenter = new Presenter(new Presenter.Dependencies
            {
                LevelLoader = levelLoader,
                PlayerModel = playerModel,
                InputManager = inputManager,
                CameraManager = cameraManager,
                FloorButtonsAndDoorsManager = floorButtonsAndDoorsManager,
                NpcManager = npcManager,
                UserInterface = userInterface,
                GameManager = gameManager,
            });
            
            levelLoader.LoadLevel("Level 1");
        }
    }
}