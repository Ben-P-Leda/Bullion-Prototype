﻿using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Avatar;
using Assets.Scripts.Gameplay.Player.Interfaces;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerBullRush : MonoBehaviour, IConfigurable, IAnimated
    {
        private Transform _transform;
        private Animator _animator;
        private GameObject _rushCollider;
        private PlayerInput _input;

        private bool _roundInProgress;
        private float _rushChargeLevel;
        private float _rushDurationRemaining;
        private bool _isSwimming;
        private bool _isDead;
        private bool _hasBeenLaunched;
        private bool _rushInProgress;

        public CharacterConfiguration Configuration { private get; set; }

        private void Start()
        {
            _transform = transform;
            _rushCollider = _transform.FindChild("Rush Collider").gameObject;
            _input = GetComponent<PlayerInput>();

            _roundInProgress = false;
            _rushChargeLevel = 0.0f;
            _rushDurationRemaining = 0.0f;
            _isSwimming = false;
            _isDead = false;
            _rushInProgress = false;
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _animator = aliveModelAnimator;
            _animator.GetBehaviour<AvatarRestingAnimationStateChange>().AddStateEntryHandler(EnterRestState);
            _animator.GetBehaviour<AvatarRushingAnimationStateChange>().StateEntryCallback = EnterRushState;
        }

        private void EnterRestState()
        {
            _rushInProgress = false;

            EndRush();
        }

        private void EnterRushState()
        {
            _rushDurationRemaining = Configuration.RushDuration;
            EventDispatcher.FireEvent(_transform, _transform, EventMessage.Begin_Rush_Movement);
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
            EventDispatcher.BoolEventHandler += BoolEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
            EventDispatcher.BoolEventHandler -= BoolEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if ((_rushCollider != null) && (target == _rushCollider.transform) && (message == EventMessage.Hit_Trigger_Collider))
            {
                EventDispatcher.FireEvent(_transform, originator, EventMessage.Inflict_Damage, Configuration.RushDamage);
                EventDispatcher.FireEvent(_transform, originator, EventMessage.Rush_Knockback);
            }

            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Has_Died: _isDead = true; _rushChargeLevel = 0.0f; break;
                    case EventMessage.Respawn: _isDead = false; break;
                    case EventMessage.Respawn_Blast: _hasBeenLaunched = true; break;
                    case EventMessage.End_Launch_Effect: _hasBeenLaunched = false; EndRush(); break;
                    case EventMessage.End_Rush_Movement: EndRush(); break;
                    case EventMessage.Rush_Stun_Impact: SetForStun(); break;
                    case EventMessage.Rush_Head_On_Collision: SetForStun(); break;
                }
            }

            if (message == EventMessage.Start_Round) { _roundInProgress = true; }
            if (message == EventMessage.End_Round) { _roundInProgress = false; }
        }

        private void BoolEventHandler(Transform originator, Transform target, string message, bool value)
        {
            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Block_Attack_Swimming: _isSwimming = value; break;
                }
            }
        }

        private void EndRush()
        {
            _animator.SetBool("IsStunned", false);
            _animator.SetBool("IsRushing", false);

            _rushCollider.SetActive(false);
        }

        private void SetForStun()
        {
            _animator.SetBool("IsStunned", true);
            _rushCollider.SetActive(false);

            if (_rushDurationRemaining <= 0.0f)
            {
                _rushDurationRemaining = Configuration.RushDuration;
            }
        }

        private void FixedUpdate()
        {
            if ((CanRecharge()) && (_rushChargeLevel < Maximum_Rush_Charge))
            {
                _rushChargeLevel = Mathf.Min(_rushChargeLevel + Configuration.RushRechargeSpeed, Maximum_Rush_Charge);
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Rush_Charge, _rushChargeLevel);
            }
        }

        private bool CanRecharge()
        {
            return (!_isDead)
                && (!_rushInProgress);
        }

        private void Update()
        {
            if ((CanRush()) && (_input.Rush))
            {
                _rushInProgress = true;
                _rushChargeLevel = 0.0f;
                _animator.SetBool("IsRushing", true);
                _rushCollider.SetActive(true);
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Begin_Rush_Sequence);
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Rush_Charge, _rushChargeLevel);
            }

            if ((_rushInProgress) && (_rushDurationRemaining > 0.0f))
            {
                _rushDurationRemaining -= Time.deltaTime;
                if (_rushDurationRemaining <= 0.0f)
                {
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.End_Rush_Movement);
                }
            }
        }

        private bool CanRush()
        {
            return (_roundInProgress)
                && (_rushChargeLevel >= Maximum_Rush_Charge)
                && (!_isSwimming)
                && (!_isDead)
                && (!_hasBeenLaunched)
                && (!_rushInProgress);
        }

        private const float Maximum_Rush_Charge = 100.0f;
    }
}