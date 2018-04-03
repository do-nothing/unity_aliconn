using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using com.microwise.unity.aliconn;

using System;
using System.Security.Cryptography;
using System.Text;

public class MqttTest : MonoBehaviour
{
    private MqttClient client;
    // Use this for initialization
    void Start()
    {
        // create client instance 
        client = AliIotClient.getAliIotClient("r7gAvsuXY2Y", "MonitorTools", "4CvHAL4vv11q86aUNNIIOw2LkCjAwkoB");
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        client.Subscribe(new string[] { "/r7gAvsuXY2Y/MonitorTools/get" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
    }

    private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message) + "\n");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 40, 150, 20), "publish mqtt message"))
        {
            Debug.Log("publish message from mqtt:");
            client.Publish("/r7gAvsuXY2Y/MonitorTools/update", System.Text.Encoding.UTF8.GetBytes("{	\"volume\":30}"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
        }
    }
}
