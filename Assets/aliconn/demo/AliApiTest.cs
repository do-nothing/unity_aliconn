using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Iot.Model.V20170420;
using com.microwise.unity.aliconn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AliApiTest : MonoBehaviour {

    PubRequest request;
    DefaultAcsClient client;

    void Start()
    {
        client = AliApiClient.getAliApiClient("LTAIRW2cXPNzAWus", "WT2zYhVbIpbv7HQfk9QVYYrK28eHjg");
        request = AliApiClient.getRequest("r7gAvsuXY2Y", "862285030043537", "test");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(320, 40, 150, 20), "send message by API"))
        {
            AliApiClient.setPalyLoad4Request(request, "{\"volume\":25}");

            try
            {
                PubResponse response = client.GetAcsResponse(request);
                if (response.Success.HasValue && response.Success.Value)
                {
                    Debug.Log("message has sent by Aliyun API.");
                }
                else
                {
                    Debug.Log(response.ErrorMessage);
                }
            }
            catch (ServerException e)
            {
                Debug.Log(e.ErrorCode);
                Debug.Log(e.ErrorMessage);
            }
            catch (ClientException e)
            {
                Debug.Log(e.ErrorCode);
                Debug.Log(e.ErrorMessage);
            }
        }
    }
}
