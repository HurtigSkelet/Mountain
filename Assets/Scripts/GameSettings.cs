using System;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public Boolean CapFPS = false; // Whether to cap the FPS to 60 FPS
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(CapFPS)
        {
            Application.targetFrameRate = 60; // Set the target frame rate to 60 FPS
        }
        else
        {
            Application.targetFrameRate = -1; // No cap on the frame rate
        }
    }
}
