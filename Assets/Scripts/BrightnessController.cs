using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MenuController;

public class BrightnessController : MonoBehaviour
{
	public Image circularBar;
	public Text brightnessText;
	private int brightness;
    // Start is called before the first frame update
    void Start()
    {
    	brightness = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Command currentCommand = StateMachine.ParseCommand();
        if (HasMouseMoved())
        {            
            Vector3 mousePos = Input.mousePosition;
            brightness = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
        }
        else if (currentCommand == Command.Right)
        {
            SwitchLight();
        }
        else if (currentCommand == Command.Up) {
            IncreaseBrightness();
        }
        else if (currentCommand == Command.Down) {
            DecreaseBrightness();
        }
        // else if (HasMouseMoved())
        // {            
        //     Vector3 mousePos = Input.mousePosition;
        //     brightness = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
        // }

        circularBar.fillAmount = (float)brightness/100;
        brightnessText.text = brightness + "%";
    }

    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    void SwitchLight(){
    	if (brightness == 0)
    	{
    		brightness = 100;
    	} 
    	else if (brightness > 0)
    	{
    		brightness = 0;
    		circularBar.fillAmount = 0;
    	}
    }

    void IncreaseBrightness(){
        if (brightness < 90)
        {
            brightness += 10;
        } 
        else
        {
            brightness = 100;
        }
    }

    void DecreaseBrightness(){
        if (brightness >10)
        {
            brightness -= 10;
        } 
        else
        {
            brightness = 0;
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
