using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DemoController : MonoBehaviour 
{
	[SerializeField] 
	private Text label;
	// Use this for initialization
    private IEnumerator Start ()
    {
	    var frameChecker = GetComponent<FrameChecker>();
	    
	    var systemInformation = "";
	    while (true)
	    {
		    systemInformation = "";
		    systemInformation += $"Device : {SystemInfo.graphicsDeviceType}\n";
		    systemInformation += $"GPU : {SystemInfo.graphicsDeviceName}\n";
		    systemInformation += $"API : {SystemInfo.graphicsDeviceVersion}\n";
		    systemInformation += $"Screen Resolution : {Screen.width} x {Screen.height}\n";
		    if (frameChecker)
			    systemInformation += frameChecker.fpsText;
		    label.text = systemInformation;
		    yield return new WaitForSeconds(0.5f);
	    }
		
    }
}
