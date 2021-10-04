using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacterController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float angle;
    private bool m_IsSwiping = false;
    private Vector3 m_PreviousTouch;
    
    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            target = gameObject;

        Application.targetFrameRate = 80;
    }

    // Update is called once per frame
    void Update()
    {

        if(m_IsSwiping)
        {
            Vector2 diff = Input.mousePosition - m_PreviousTouch;
            
            // Put difference in Screen ratio, but using only width, so the ratio is the same on both
            // axes (otherwise we would have to swipe more vertically...)
            target.transform.Rotate( Vector3.up * -diff.x * angle / (float)Screen.width);
            m_PreviousTouch = Input.mousePosition;
        }

        // Mouse Input also works on mobile devices.
        // a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            m_PreviousTouch = Input.mousePosition;
            m_IsSwiping = true;
        } 
        else if(Input.GetMouseButtonUp(0))
        {
            m_IsSwiping = false;
        }

    }
}
