using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UTK.Manager;
public class CharacterSceneManager : MonoBehaviour, IUtkEventListener<UtkEvent>
{
    public GameObject dayObjects;
    public GameObject nightObjects;
    
    protected void OnEnable()
    {
        this.UtkEventStartListening<UtkEvent>();
    }
    protected void OnDisable()
    {
        this.UtkEventStopListening<UtkEvent>();
    }
    public virtual void OnUtkEvent(UtkEvent utkEvent)
    {
        switch (utkEvent.EventType)
        {
            case UtkEventTypes.SetDayTime:
                ChangeDayNight(true);
                break;
            case UtkEventTypes.SetNightTime:
                ChangeDayNight(false);
                break;
            case UtkEventTypes.MoveToCharacter:
                SceneManager.LoadSceneAsync(SceneNames.MovementScene.ToString(), LoadSceneMode.Single);
                break;
        }
    }

    private void ChangeDayNight(bool isDay)
    {
        if(dayObjects) dayObjects.SetActive(isDay);
        if(nightObjects) nightObjects.SetActive(!isDay);
    }
}
