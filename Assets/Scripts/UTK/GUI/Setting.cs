using System;
using UnityEngine;

namespace UTK.GUI
{
    public class Setting : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log(Screen.currentResolution);
            
            foreach (var resolution in Screen.resolutions)
            {
                // Debug.Log(resolution);    
            }
            
            
        }
    }
}