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

    void Start()
    {
        inputField.text = "{\n\t\"Position\":[46.34,-89.88, 0.86],\n\t\"EulerAngle\":[74.32,83.72,3.3],\n\t\"KeypadIds\":[0],\n\t\"Remaining\":0.94\n}";
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
            +">>></color>" 
            + text 
            + "\n";

        if (height > 1500)
        {
            int index = textField.text.IndexOf("\n") + 1;
            textField.text = textField.text.Remove(0, index);
            vScrollbar.value = 0;
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