using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTK.Manager;

namespace UTK.Manager
{
    public class EffectManager : MonoBehaviour, IUtkEventListener<UtkEvent>
    {
        public GameObject fallingLeaves;
        public GameObject dof;
    
        protected void OnEnable()
        {
            this.UtkEventStartListening<UtkEvent>();
        }

        protected void OnDisable()
        {
            this.UtkEventStopListening<UtkEvent>();
        }

        public virtual void OnUtkEvent(UtkEvent utkEvent)
        {
            switch (utkEvent.EventType)
            {
                case UtkEventTypes.InventoryOpen:
                case UtkEventTypes.OpenSetting:
                    OnPause(true);
                    break;
                case UtkEventTypes.Resume:
                    OnPause(false);
                    break;
                case UtkEventTypes.MoveToLobby:
                case UtkEventTypes.QualityChange:
                default:
                    break;
            }
        }

        private void OnPause(bool status)
        {
            if(fallingLeaves)
                fallingLeaves.SetActive(!status);
            if(dof)
                dof.SetActive(status);
        }
    }
}

