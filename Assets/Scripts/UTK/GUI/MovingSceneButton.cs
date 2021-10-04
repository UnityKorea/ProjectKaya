using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTK.Manager;

public class MovingSceneButton : MonoBehaviour
{
    private void Start()
    {
          var button = GetComponent<Button>();
          if (null != button)
          {
              button.onClick.AddListener(() =>
              {
                  StartCoroutine(MoveSceneCo());        
              });
          }
      }
      
      protected virtual IEnumerator MoveSceneCo()
      {
          yield return 0;
          UtkEvent.Trigger(UtkEventTypes.MoveToCharacter);
      }
}
