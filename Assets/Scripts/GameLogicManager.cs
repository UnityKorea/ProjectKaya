using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameLogicManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayableDirector cinematic;
    private PlayerInput _input;
    private PlayerController _controller;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject cinematicUI;
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player)
        {
            _input = player.GetComponent<PlayerInput>();
            _controller = player.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopGamePlay()
    {
        Debug.Log("StopGamePlay");
       
        if (_input)
            _input.enabled = false;
        if (_controller)
            _controller.enabled = false;
        if(hud)
            hud.SetActive(false);
    }

    public void ResumeGamePlay(bool controllCinematic = true)
    {
        Debug.Log("ResumeGamePlay");
        
        if (_input)
            _input.enabled = true;
        if (_controller)
            _controller.enabled = true;
        if(hud)
            hud.SetActive(true);
        if (controllCinematic == true && cinematic != null)
        {
            cinematic.Stop();
            if( cinematicUI)
                cinematicUI.SetActive(false);
        }
    }
}
