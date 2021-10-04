using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UTK.Manager;

namespace UTK.GUI
{
    public class QualityButton : MonoBehaviour
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            if (null != button)
            {
                button.onClick.AddListener(() =>
                {
                    StartCoroutine(QualityChangeCo());        
                });
            }
        }
        
        protected virtual IEnumerator QualityChangeCo()
        {
            yield return 0;
            UtkEvent.Trigger(UtkEventTypes.QualityChange);
        }
    }
}