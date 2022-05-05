namespace Avangardum.PublexTestTask
{
    public interface ICameraManager
    {
        void OnLevelLoaded();

        void Initialize(IFixedUpdateProvider fixedUpdateProvider, ICameraConfig config);
    }
}