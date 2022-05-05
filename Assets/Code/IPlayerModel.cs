using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface IPlayerModel
    {
        void SetMovingDirection(Vector3 direction);

        void OnLevelLoaded();

        void Initialise(IPlayerConfig config, IFixedUpdateProvider fixedUpdateProvider);
    }
}