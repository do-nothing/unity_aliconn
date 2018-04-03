using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Iot.Model.V20170420;
using System;
using System.Text;
using UnityEngine;
namespace com.microwise.unity.aliconn
{
    public class AliApiClient
    {
        public static DefaultAcsClient getAliApiClient(string accessKey, string accessSecret)
        {
            DefaultProfile.AddEndpoint("cn-shanghai", "cn-shanghai", "Iot", "iot.cn-shanghai.aliyuncs.com");
            IClientProfile clientProfile = DefaultProfile.GetProfile("cn-shanghai", accessKey, accessSecret);
            DefaultAcsClient client = new DefaultAcsClient(clientProfile);
            return client;
        }

        public static PubRequest getRequest(string productKey, string deviceName, string topic)
        {
            PubRequest request = new PubRequest();
            request.ProductKey = productKey;
            request.TopicFullName = "/" + request.ProductKey + "/" + deviceName + "/" + topic;
            request.Qos = 0;
            return request;
        }

        public static void setPalyLoad4Request(PubRequest request, string message)
        {
            byte[] payload = Encoding.Default.GetBytes(message);
            String payloadStr = Convert.ToBase64String(payload);
            request.MessageContent = payloadStr;
        }
    }
}
