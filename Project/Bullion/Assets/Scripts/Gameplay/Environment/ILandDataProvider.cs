﻿using UnityEngine;

namespace Assets.Scripts.Gameplay.Environment
{
    public interface ILandDataProvider
    {
        float Width { get; }
        float Depth { get; }
        float HeightAtPosition(Vector3 position);
    }
}
