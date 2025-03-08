using System;
using UnityEngine;

public static class NoiseEventManager
{
    public static event Action<Transform> OnNoiseMade;

    public static void MakeNoise(Transform position)
    {
        OnNoiseMade?.Invoke(position);
    }
}
