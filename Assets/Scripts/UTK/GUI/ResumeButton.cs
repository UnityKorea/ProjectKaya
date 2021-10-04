using System.Collections;
using UnityEngine;
using UTK.Manager;
using UnityEngine.UI;

namespace UTK.GUI
{
    public class ResumeButton : MonoBehaviour
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            if (null != button)
            {
                button.onClick.AddListener(() =>
                {
                    StartCoroutine(ResumeCo());        
                });
            }
        }
        
        private IEnumerator ResumeCo()
        {
            yield return 0;
            UtkEvent.Trigger(UtkEventTypes.Resume);
        }
    }
}