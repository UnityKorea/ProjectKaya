using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UTK.Manager;

namespace UTK.GUI
{
    public class LobbyButton : MonoBehaviour
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            if (null != button)
            {
                button.onClick.AddListener(() =>
                {
                    StartCoroutine(MoveLobbyCo());        
                });
            }
        }
        
        protected virtual IEnumerator MoveLobbyCo()
        {
            yield return 0;
            UtkEvent.Trigger(UtkEventTypes.MoveToLobby);
        }
    }
}