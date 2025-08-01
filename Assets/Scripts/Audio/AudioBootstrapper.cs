using UnityEngine;

/// <summary>
/// Bootstrapper to ensure AudioManager is created at runtime.
/// </summary>
public static class AudioBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitIfNeeded()
    {
        var _ = AudioManager.Instance;
    }
}
