﻿using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class TreasureLifeCycle : ChestItemLifeCycle
    {
        public float Value;

        public override void InitializeComponents()
        {
            base.InitializeComponents();

            LaunchTriggerEvent = EventMessage.Collect_Treasure;
            EventValue = Value;
        }
    }
}