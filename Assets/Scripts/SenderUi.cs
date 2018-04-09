using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Aliyun.Acs.Core;
using com.microwise.unity.aliconn;
using Aliyun.Acs.Iot.Model.V20170420;
using Aliyun.Acs.Core.Exceptions;

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

    private DefaultAcsClient client;

    private string defaultUpdate = "{\n\t\"position\":[46.34,-89.88, 0.86],\n\t\"eulerAngle\":[74.32,83.72,3.3],\n\t\"keypadIds\":[0],\n\t\"remaining\":0.94\n}";
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
        string text = sendMessage(deviceName.text, topic.options[topic.value].text, inputField.text);

        text = text.Replace(" ", "");
        text = text.Replace("\n", "");
        text = text.Replace("\t", "");
        textField.text += "<color=blue>(" + count++ + ")"
            + "/" + lastDeviceName + "/" + lastTopic
            + ">>></color>"
            + text
            + "\n";

        if (height > 1500)
        {
            int index = textField.text.IndexOf("\n") + 1;
            textField.text = textField.text.Remove(0, index);
            vScrollbar.value = 0;
        }
    }

    public void OnDropdownChanged()
    {
        switch (topic.value)
        {
            case 0:
                inputField.text = defaultUpdate;
                break;
            case 1:
                inputField.text = defaultGet;
                break;
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