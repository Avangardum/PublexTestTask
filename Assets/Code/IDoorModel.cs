using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface IDoorModel
    {
        void Open();
        void Close();
        void Switch();
        void Initialize(GameObject doorGO);
    }
}