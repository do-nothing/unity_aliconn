using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeopleController : MonoBehaviour
{
    [SerializeField]
    private InputField areaName;
    private float angle;
    private Vector2 position;
    private Vector2 rectXY;
    private RectTransform rect;
    private float time;
    private float speedLevel;
    private float pressTimer;

    void Start()
    {
        angle = 90;
        position = Vector2.zero;
        rectXY = Vector2.zero;
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        calcSpeedLevel();
        sendPressMessage();
        calcAngle();
        calcPosition();
        autoSend();
    }

    private void sendPressMessage()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            pressTimer = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            int cost = (int)((Time.time - pressTimer) * 1000);
            string message = "{\"keypad\":{\"id\":0,\"cost\":" + cost + "}}";
            transform.parent.parent.parent.SendMessage("sendAuto", message);
        }
    }

    private void calcSpeedLevel()
    {
        speedLevel = 1;
        if (Input.GetKey(KeyCode.LeftControl))
            speedLevel *= 2;
        if (Input.GetKey(KeyCode.RightControl))
            speedLevel *= 2;
        if (Input.GetKey(KeyCode.LeftShift))
            speedLevel *= 0.5f;
        if (Input.GetKey(KeyCode.RightShift))
            speedLevel *= 0.5f;
    }

    private void autoSend()
    {
        if (Time.time - time < 0.3f)
            return;

        string message = "";
        if (Input.GetAxis("Vertical") != 0)
        {
            message = "\"areaName\":\"" + areaName.text + "\",\"position\":[" + position.x + "," + position.y + ", 0]";
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            if (message == "")
                message = "\"eulerAngle\":[" + angle * 100 + ",0,0]";
            else
                message += ",\"eulerAngle\":[" + angle * 100 + ",0,0]";
        }

        if (message != "")
        {
            message = "{" + message + "}";
            time = Time.time;
            print(message);
            transform.parent.parent.parent.SendMessage("sendAuto", message);
        }
    }

    private void calcAngle()
    {
        angle -= Input.GetAxis("Horizontal") * speedLevel;
        if (angle > 360)
            angle -= 360;
        if (angle < 0)
            angle += 360;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void calcPosition()
    {
        float v = Input.GetAxis("Vertical");
        rectXY.x = (Mathf.Cos(angle * Mathf.Deg2Rad));
        rectXY.y = (Mathf.Sin(angle * Mathf.Deg2Rad));
        rectXY *= v;
        rect.anchoredPosition = rectXY * 40;

        position += rectXY * Time.deltaTime * 2 * speedLevel;
    }
}
