using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTK.Manager;

public class LightToggleButton : MonoBehaviour
{
    public GameObject dayIcon;
    public GameObject nightIcon;
    public Text label;

    private bool _day;
    
    // Start is called before the first frame update
    private void Start()
    {
        var button = GetComponent<Button>();
        if (null != button)
        {
            button.onClick.AddListener(() =>
            {
                StartCoroutine(LightToggleCo());        
            });
        }

        _day = true;
        SetDayNight(_day);
    }

    // Update is called once per frame
    protected virtual IEnumerator LightToggleCo()
    {
        yield return null;
        _day = !_day;
        SetDayNight(_day);
        UtkEvent.Trigger(_day ? UtkEventTypes.SetDayTime : UtkEventTypes.SetNightTime);
    }

    private void SetDayNight(bool day)
    {
        if(dayIcon) dayIcon.SetActive(day);
        if(nightIcon) nightIcon.SetActive(!day);
        if (label) label.text = day ? "Lighting: Day" : "Lighting: Night";
    }
}
