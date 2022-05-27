using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MenuController;

public class FanSpeedController : MonoBehaviour
{
	public Image circularBar;
	public Text fanSpeedText;
	private int fanSpeed;
    // Start is called before the first frame update
    void Start()
    {
    	fanSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Command currentCommand = StateMachine.ParseCommand();
        if (HasMouseMoved())
        {            
            Vector3 mousePos = Input.mousePosition;
            fanSpeed = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
        }
        else if (currentCommand == Command.Right)
        {
            SwitchFan();
        }
        else if (currentCommand == Command.Up) {
            IncreaseFanSpeed();
        }
        else if (currentCommand == Command.Down) {
            DecreaseFanSpeed();
        }
        // else if (HasMouseMoved())
        // {            
        //     Vector3 mousePos = Input.mousePosition;
        //     fanSpeed = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
        // }

        circularBar.fillAmount = (float)fanSpeed/100;
        fanSpeedText.text = fanSpeed + "%";
    }

    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    void SwitchFan(){
    	if (fanSpeed == 0)
    	{
    		fanSpeed = 100;
    	} 
    	else if (fanSpeed > 0)
    	{
    		fanSpeed = 0;
    		circularBar.fillAmount = 0;
    	}
    }

    void IncreaseFanSpeed(){
        if (fanSpeed < 90)
        {
            fanSpeed += 10;
        } 
        else
        {
            fanSpeed = 100;
        }
    }

    void DecreaseFanSpeed(){
        if (fanSpeed >10)
        {
            fanSpeed -= 10;
        } 
        else
        {
            fanSpeed = 0;
        }
    }

    int GetAngle(float x, float y)
    {
    	double angle = 0;
	    if (x > 0)
	    {
	        angle = 360 * Math.Atan((y) / (x)) / (2 * Math.PI) + 90;
	    }
	    else if (x < 0 && y < 0)
	    {
	        angle = 360 * Math.Atan((y) / (x)) / (2 * Math.PI) + 270;
	    }
	    else if (x < 0 && y >= 0)
	    {
	        angle = 360 * Math.Atan((y) / (x)) / (2 * Math.PI) + 270;
	    }
	    else if (x == 0 && y >= 0)
	    {
	        angle = 180;
	    }
	    else if (x == 0 && y < 0)
	    {
	        angle = 0;
	    }
	    else if (x == 0)
	    {
	        angle = 0;
	    }
	    return (int)angle;
    }
}
