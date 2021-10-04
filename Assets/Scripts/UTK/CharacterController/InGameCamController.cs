using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InGameCamController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camTarget;
    [SerializeField] private CinemachineVirtualCamera [] lookAtPlayer;
    [SerializeField] private CinemachineVirtualCamera[] followPlayer;
    [SerializeField] private CinemachineVirtualCamera[] lookAtCamTaget;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if( camTarget == null)
            camTarget = GameObject.FindGameObjectWithTag("CamTarget");

        foreach ( var cam in lookAtPlayer)
        {
            cam.LookAt = player.transform;
        }

        foreach (var cam in followPlayer)
        {
            cam.Follow = player.transform;
        }

        foreach (var cam in lookAtCamTaget)
        {
            cam.LookAt = camTarget.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
