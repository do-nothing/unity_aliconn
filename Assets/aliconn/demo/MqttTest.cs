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
        client = AliIotClient.getAliIotClient("vvepFlt7L0W", "MonitorTools", "jiBQM0QGBtCFGBM3jziD73AfND4y6SW0");
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        client.Subscribe(new string[] { "/vvepFlt7L0W/MonitorTools/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
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
            client.Publish("/vvepFlt7L0W/MonitorTools/test", System.Text.Encoding.UTF8.GetBytes("{\"volume\":30}"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
        }
    }
}
