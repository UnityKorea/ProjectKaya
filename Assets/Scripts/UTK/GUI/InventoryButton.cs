using System.Collections;
using UnityEngine;
using UTK.Manager;
using UnityEngine.UI;

namespace UTK.GUI
{
    public class InventoryButton:MonoBehaviour
    {
        
        private void Start()
        {
            var button = GetComponent<Button>();
            if (null != button)
            {
                button.onClick.AddListener(() =>
                {
                    StartCoroutine(InventoryOpenCo());        
                });
            }
        }
        
        protected virtual IEnumerator InventoryOpenCo()
        {
            yield return 0;
            UtkEvent.Trigger(UtkEventTypes.InventoryOpen);
            Debug.Log("UTKEvent Triggered: Inventory Open");
        }
    }
}