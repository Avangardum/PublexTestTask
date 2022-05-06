namespace Avangardum.PublexTestTask
{
    public interface INPCConfig
    {
        float EnemySpeed { get; }
        float AllySpeed { get; }
        float EnemyUnfollowingDelay { get; }
        float EnemyFOV { get; }
        float AllyFOV { get; }
    }
}