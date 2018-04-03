using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System;

namespace com.microwise.unity.aliconn
{
    public class AliIotClient
    {
        public static MqttClient getAliIotClient(string productKey, string deviceName, string deviceSecret)
        {
            MqttClient client = new MqttClient(productKey + ".iot-as-mqtt.cn-shanghai.aliyuncs.com", 1883, false, null);

            string clientId = Guid.NewGuid().ToString();
            string timestamp = GetTimeStamp();
            String signContent = "clientId" + clientId + "deviceName" + deviceName + "productKey" + productKey + "timestamp" + timestamp;
            clientId = clientId + "|securemode=3,signmethod=hmacsha1,timestamp=" + timestamp + "|";
            string mqttUsername = deviceName + "&" + productKey; //mqtt用户名格式
            string mqttPassword = HMACSHA1Encrypt(signContent, deviceSecret); //签名

            byte b = client.Connect(clientId, mqttUsername, mqttPassword);
            if (b == 0)
            {
                Debug.Log("/"+productKey+"/"+deviceName+" is connected!");
            }
            else
            {
                Debug.Log("/" + productKey + "/" + deviceName + " connects failed!");
            }

            return client;
        }

        private static string HMACSHA1Encrypt(string EncryptText, string EncryptKey)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(EncryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(EncryptText));
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in RstRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return EnText.ToString();
        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds * 1000).ToString();
        }
    }
}
