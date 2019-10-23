﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PowerMage
{
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour
    {
        public IResource health = null;
        public IInput input = null;
        public IVision vision = null;
        public IPhysicsCharacter character = null;

        private InputContainer container = new InputContainer();

        














        #region TEAM

        public Teams.Team team = Teams.Team.NONE;

        private Color teamColor = Color.gray;

        private void UpdateTeamColor()
        {
            switch (team)
            {
                case Teams.Team.BADBOYS: teamColor = Color.red; break;
                case Teams.Team.GOODGUYS: teamColor = Color.green; break;
                default: teamColor = Color.gray; break;
            }
        }

        #endregion

        #region STATUS_EFFECTS

        [System.Flags]
        public enum StatusEffects
        {
            None = 0,
            SetOnFire = 1,
            Confused = 2,
            Frozen = 4,
            Ragdolled = 8
        }
        
        private StatusEffects effects = StatusEffects.None;
        
        public bool GetEffect(StatusEffects se)
        {
            return (effects & se) == se;
        }

        public void SetEffect(StatusEffects se, bool b)
        {
            if (GetEffect(se) != b)
            {
                effects = effects ^ se;

                switch (se)
                {
                    case StatusEffects.SetOnFire: OnSetOnFire(); break;
                    case StatusEffects.Confused: OnConfused(); break;
                    case StatusEffects.Frozen: OnFrozen(); break;
                    case StatusEffects.Ragdolled: OnRagdolled(); break;
                    default: break;
                }
            }
        }

        /* ----- CALLBACKS ----- */

        protected virtual void OnSetOnFire() { }

        protected virtual void OnConfused()
        {
            Teams.Team newTeam;
            switch (team)
            {
                case Teams.Team.GOODGUYS: newTeam = Teams.Team.BADBOYS; break;
                case Teams.Team.BADBOYS: newTeam = Teams.Team.GOODGUYS; break;
                default: newTeam = Teams.Team.NONE; break;
            }
            team = newTeam;
        }

        protected virtual void OnFrozen() { }
        protected virtual void OnRagdolled() { }

        #endregion
        
        #region MONOBEHAVIOUR

        protected virtual void Awake()
        {
            health = GetComponent<IResource>();
            health.Initialize(this);
            input = GetComponent<IInput>();
            character = GetComponent<IPhysicsCharacter>();
        }

        protected virtual void Update()
        {
            InputContainer ic = input.GetInput();
            container = new InputContainer(
                ic.moveVector,
                ic.lookVector,
                container.jump ? true : ic.jump,
                container.dash ? true : ic.dash,
                container.attack ? true : ic.attack
                );
        }

        protected virtual void FixedUpdate()
        {

        }

        public virtual void OnValidate()
        {
            UpdateTeamColor();
        }

        public virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR

            //if (controller == null)
            //{
            //    return;
            //}

            //if (Selection.activeGameObject != gameObject)
            //{
            //    //Draw when NOT selected

            //    //Draw a faded circle at the pivot point
            //    Handles.color = Color.Lerp(teamColor, Color.clear, 0.9f);
            //    Handles.DrawSolidDisc(transform.position, transform.up, controller.radius);

            //    Handles.color = Color.Lerp(teamColor, Color.clear, 0.3f);
            //    Handles.DrawWireDisc(transform.position, transform.up, controller.radius);

            //    Gizmos.color = Handles.color;
            //    Gizmos.DrawCube(transform.position, Vector3.up * 0.01f + (Vector3.one - Vector3.up) * 0.15f);
            //}
            //else
            //{
            //    //Draw when selected

            //    //Draw a circle at the pivot point

            //    Handles.color = Color.Lerp(teamColor, Color.clear, 0.8f);
            //    Handles.DrawSolidDisc(transform.position, transform.up, controller.radius);

            //    Handles.color = Color.Lerp(teamColor, Color.clear, 0.0f);
            //    Handles.DrawWireDisc(transform.position, transform.up, controller.radius);

            //    Gizmos.color = Handles.color;
            //    Gizmos.DrawCube(transform.position, Vector3.up * 0.01f + (Vector3.one - Vector3.up) * 0.15f);
            //}
            
#endif
        }

        #endregion
    }
}