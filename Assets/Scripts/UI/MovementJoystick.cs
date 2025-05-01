using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementJoystick : MonoBehaviour
{

    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOGPos;
    private Vector2 joystickBGOGPos;
    private float joystickRad;



    void Start()
    {
        joystickOGPos = joystick.transform.localPosition;
        joystickBGOGPos = joystickBG.transform.localPosition;  // Store BG's original position
        joystickRad = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBG.transform.position = Input.mousePosition;
        joystickTouchPos = Input.mousePosition;
    }

    public void JoyStickDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        joystickVec = (dragPos - joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        if (joystickDist < joystickRad)
        {
            joystick.transform.position = joystickTouchPos + joystickVec * joystickDist;
        }
        else
        {
            joystick.transform.position = joystickTouchPos + joystickVec * joystickRad;
        }
    }

    public void PointerUp()
    {
        joystickVec = Vector2.zero;
        joystick.transform.localPosition = joystickOGPos;
        joystickBG.transform.localPosition = joystickBGOGPos;  // Reset BG to its original position
    }

    public Vector2 GetJoyStickVec()
    {
        return joystickVec;
    }


}
