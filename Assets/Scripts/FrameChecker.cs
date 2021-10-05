using UnityEngine;
using System.Collections;



public class FrameChecker : MonoBehaviour
{
    private float _deltaTime = 0.0f;
    private GUIStyle _style;
    private Rect _rect;
    private float _msec;
    private float _fps;
    private float _worstFps = 100f;
    [HideInInspector]
    public string fpsText;
    public bool showFPS = true;
    
    private void Awake()
    {
        int w = Screen.width, h = Screen.height;

        _rect = new Rect(0, 0, w, h * 4 / 100f);

        _style = new GUIStyle();
        _style.alignment = TextAnchor.UpperLeft;
        _style.fontSize = h * 4 / 130;
        _style.normal.textColor = Color.cyan;

        StartCoroutine(WorstReset_Coroutine());
    }
   
    private IEnumerator WorstReset_Coroutine() 
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            _worstFps = 100f;
        }
    }
    
    private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        _msec = _deltaTime * 1000.0f;
        _fps = 1.0f / _deltaTime;
        if (_fps < _worstFps)  
            _worstFps = _fps;
        fpsText = $"{_msec.ToString("F1")}ms ({_fps.ToString("F1")}) | Worst: {_worstFps.ToString("F1")}";
    }

    private void OnGUI()
    {
        if(showFPS)
            GUI.Label(_rect, fpsText, _style);
    }
}


