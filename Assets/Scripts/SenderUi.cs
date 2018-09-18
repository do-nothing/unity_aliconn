using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Aliyun.Acs.Core;
using com.microwise.unity.aliconn;
using Aliyun.Acs.Iot.Model.V20170420;
using Aliyun.Acs.Core.Exceptions;
using System;

public class SenderUi : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private InputField deviceName;
    [SerializeField]
    private Dropdown topic;
    [SerializeField]
    private Text textField;
    [SerializeField]
    private Scrollbar vScrollbar;
    [SerializeField]
    private GameObject positionPanel;
    [SerializeField]
    private GameObject sender;
    [SerializeField]
    private GameObject areaName;

    private DefaultAcsClient client;

    private string defaultUpdate = "{\n\t\"areaName\":\"microwise_501\",\n\t\"position\":[46.34,-89.88, 0.86],\n\t\"eulerAngle\":[7432,8372,330],\n\t\"keypad\":{\"id\":0,\"cost\":2560},\n\t\"remaining\":94\n}";
    private string defaultGet = "{\n\t\"stop\":0,\n\t\"insert\":\"0101\",\n\t\"play\":[\"0102\",\"0103\"]\n}";

    void Start()
    {
        inputField.text = defaultUpdate;
        client = AliApiClient.getAliApiClient("LTAIRW2cXPNzAWus", "WT2zYhVbIpbv7HQfk9QVYYrK28eHjg");
    }

    float height = 0;
    bool scroll = false;
    void OnGUI()
    {
        if (textField.preferredHeight != height)
        {
            height = textField.preferredHeight;
            textField.transform.parent.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, textField.preferredHeight);
            scroll = true;
            vScrollbar.value = 0.1f;
        }
        if (scroll)
        {
            vScrollbar.value = 0;
            scroll = false;
        }
    }

    int count = 0;
    public void OnClickSender()
    {
        sendManual(deviceName.text, topic.options[topic.value].text, inputField.text);
    }

    private void sendAuto(string message)
    {
        sendManual(deviceName.text, "update", message);
    }

    private void sendManual(string deviceName, string topic, string message)
    {
        string text = sendMessage(deviceName, topic, message);

        text = text.Replace(" ", "");
        text = text.Replace("\n", "");
        text = text.Replace("\t", "");
        textField.text += "<color=blue>(" + count++ + ")"
            + "/" + lastDeviceName + "/" + lastTopic
            + ">>></color>";
        if (isJson(text))
        {
            textField.text += text + "\n";
        }
        else
        {
            textField.text += "<color=red>" + text + "</color>\n";
        }


        if (height > 1500)
        {
            int index = textField.text.IndexOf("\n") + 1;
            textField.text = textField.text.Remove(0, index);
            vScrollbar.value = 0;
        }
    }

    private bool isJson(string text)
    {
        try
        {
            JsonData json = JsonMapper.ToObject(text);
            return true;
        }catch(Exception e){
            print(e.StackTrace);
            return false;
        }
    }

    public void OnDropdownChanged()
    {
        switch (topic.value)
        {
            case 0:
                showPositonPanel(false);
                inputField.text = defaultUpdate;
                break;
            case 1:
                showPositonPanel(false);
                inputField.text = defaultGet;
                break;
            case 2:
                showPositonPanel(true);
                break;
        }
    }

    private void showPositonPanel(bool flag)
    {
        if (flag)
        {
            inputField.gameObject.SetActive(false);
            positionPanel.SetActive(true);
            sender.SetActive(false);
            areaName.SetActive(true);
        }
        else
        {
            inputField.gameObject.SetActive(true);
            positionPanel.SetActive(false);
            sender.SetActive(true);
            areaName.SetActive(false);
        }
    }

    private string lastDeviceName, lastTopic;
    private PubRequest request;
    private string sendMessage(string deviceName, string topic, string message)
    {
        string text;
        if (lastDeviceName != deviceName || lastTopic != topic)
        {
            request = AliApiClient.getRequest("r7gAvsuXY2Y", deviceName, topic);
            lastDeviceName = deviceName;
            lastTopic = topic;
        }

        AliApiClient.setPalyLoad4Request(request, message);

        try
        {
            PubResponse response = client.GetAcsResponse(request);
            if (response.Success.HasValue && response.Success.Value)
            {
                text = message;
            }
            else
            {
                text = response.ErrorMessage;
            }
        }
        catch (ServerException e)
        {
            Debug.Log(e.ErrorCode);
            Debug.Log(e.ErrorMessage);
            text = e.ErrorMessage;
        }
        catch (ClientException e)
        {
            Debug.Log(e.ErrorCode);
            Debug.Log(e.ErrorMessage);
            text = e.ErrorMessage;
        }

        return text;
    }
}