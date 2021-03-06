﻿using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Configuration
{
    public class CharacterConfigurationManager
    {
        private static CharacterConfigurationManager _instance = null;

        public static CharacterConfiguration GetCharacterConfiguration(string characterName)
        {
            _instance = _instance == null ? new CharacterConfigurationManager() : _instance;
            return _instance._characters[characterName];
        }

        private Dictionary<string, CharacterConfiguration> _characters = null;

        public CharacterConfigurationManager()
        {
            CreateCharacterConfigurations();
        }

        private void CreateCharacterConfigurations()
        {
            _characters = new Dictionary<string, CharacterConfiguration>();

            _characters.Add("Red", new CharacterConfiguration()
            {
                Name = "Red Player",
                HeightOffset = 1.0f,
                HipShoulderDistance = 1.0f,
                AliveMovementSpeed = 5.0f,
                DeadMovementSpeed = 5.0f,
                RushMovementSpeed = 7.0f,
                RushDuration = 3.0f,
                RushDamage = 20.0f,
                ComboStepCount = 3,
                ComboStepDamage = new float[] { 10, 10, 10 },
                MaximumHealth = 100.0f,
                RushRechargeSpeed = 0.25f,
                RespawnPointColour = new Color(1.0f, 0.0f, 0.0f, 1.0f)
            });

            _characters.Add("Green", new CharacterConfiguration()
            {
                Name = "Green Player",
                HeightOffset = 1.0f,
                HipShoulderDistance = 1.0f,
                AliveMovementSpeed = 5.5f,
                DeadMovementSpeed = 4.0f,
                RushMovementSpeed = 7.0f,
                RushDuration = 3.0f,
                RushDamage = 20.0f,
                ComboStepCount = 2,
                ComboStepDamage = new float[] { 20, 20 },
                MaximumHealth = 85.0f,
                RushRechargeSpeed = 0.25f,
                RespawnPointColour = new Color(0.0f, 0.596f, 0.145f, 1.0f)
            });

            _characters.Add("Blue", new CharacterConfiguration()
            {
                Name = "Blue Player",
                HeightOffset = 1.0f,
                HipShoulderDistance = 1.0f,
                AliveMovementSpeed = 5.25f,
                DeadMovementSpeed = 4.75f,
                RushMovementSpeed = 7.0f,
                RushDuration = 3.0f,
                RushDamage = 20.0f,
                ComboStepCount = 3,
                ComboStepDamage = new float[] { 10, 15, 20 },
                MaximumHealth = 75.0f,
                RushRechargeSpeed = 0.25f,
                RespawnPointColour = new Color(0.0f, 0.255f, 1.0f, 1.0f)
            });

            _characters.Add("Purple", new CharacterConfiguration()
            {
                Name = "Purple Player",
                HeightOffset = 1.0f,
                HipShoulderDistance = 1.0f,
                AliveMovementSpeed = 4.5f,
                DeadMovementSpeed = 6.0f,
                RushMovementSpeed = 7.0f,
                RushDuration = 3.0f,
                RushDamage = 20.0f,
                ComboStepCount = 2,
                ComboStepDamage = new float[] { 12.5f, 12.5f },
                MaximumHealth = 115.0f,
                RushRechargeSpeed = 0.25f,
                RespawnPointColour = new Color(0.663f, 0.059f, 0.875f, 1.0f)
            });
        }
    }
}