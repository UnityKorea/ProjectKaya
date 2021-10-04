using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTK.Manager;

namespace UTK.GUI
{
	public class SettingButton : MonoBehaviour
	{
		private void Start()
		{
			var button = GetComponent<Button>();
			if (null != button)
			{
				button.onClick.AddListener(() =>
				{
					StartCoroutine(OpenSettingCo());        
				});
			}
		}
        
		protected virtual IEnumerator OpenSettingCo()
		{
			yield return 0;
			UtkEvent.Trigger(UtkEventTypes.OpenSetting);
		}
	}
}

