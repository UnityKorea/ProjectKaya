using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UTK.Common;
using UTK.GUI;
using Debug = UnityEngine.Debug;


namespace UTK.Manager
{
    public enum UtkEventTypes
    {
        InventoryOpen,
        MoveToCharacter,
        MoveToLobby,
        QualityChange,
        OpenSetting,
        Resume,
        SetDayTime,
        SetNightTime
    }

    public enum SceneNames
    {
        UTKTemplate_Lobby = 0,
        UTKTemplate_Action = 1,
    }

    public struct UtkEvent
    {
        public UtkEventTypes EventType;

        public UtkEvent(UtkEventTypes eventType)
        {
            EventType = eventType;
        }

        private static UtkEvent _e;

        public static void Trigger(UtkEventTypes eventType)
        {
            _e.EventType = eventType;
            UTKEventManager.TriggerEvent(_e);
        }
    }
    
    public class GameManager : UTKSingleton<GameManager>, IUtkEventListener<UtkEvent>
    {
        [SerializeField]
        private GameObject inventory;
        [SerializeField]
        private GameObject setting;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetActiveInventory(false);
            SetActiveSetting(false);
        }
        

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
                    PauseGame(true);
                    break;
                case UtkEventTypes.MoveToLobby:
                case UtkEventTypes.Resume:
                    PauseGame(false);
                    break;
            }

            switch (utkEvent.EventType)
            {
                case UtkEventTypes.InventoryOpen:
                    SetActiveInventory(true);
                    break;
                case UtkEventTypes.MoveToLobby:
                    SceneManager.LoadSceneAsync(SceneNames.UTKTemplate_Lobby.ToString(), LoadSceneMode.Single);
                    break;
                case UtkEventTypes.OpenSetting:
                    SetActiveSetting(true);
                    break;
                case UtkEventTypes.Resume:
                    SetActiveSetting(false);
                    SetActiveInventory(false);
                    break;
            }
        }

        private static void PauseGame(bool status)
        {
            Time.timeScale = status ? 0 : 1;
        }

        protected virtual void SetActiveSetting(bool status)
        {
            if (setting != null)
                setting.gameObject.SetActive(status);
        }

        protected virtual void SetActiveInventory(bool status)
        {
            if (inventory != null)
                inventory.gameObject.SetActive(status);
        }
    }
    
}
