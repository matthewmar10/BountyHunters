/*
using System;
using UnityEngine;
using Aws.GameLift.Realtime.Types;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.CognitoIdentity;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Zenject;
*/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//using RTSGame;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime;
using Aws.GameLift.Realtime.Types;
using System.IO;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using Zenject;




// This data structure is returned by the client service when a game match is found
[System.Serializable]
public class PlayerSessionObject
{
   public string PlayerSessionId;
   public string PlayerId;
   public string GameSessionId;
   public string FleetId;
   public string CreationTime;
   public string Status;
   public string IpAddress;
   public string Port;
}

public class GameSessionFirst
{
   private RealTimeClient _realTimeClient;

   private byte[] connectionPayload = new Byte[64];

   private static readonly IPEndPoint DefaultLoopbackEndpoint = new IPEndPoint(IPAddress.Loopback, port: 0);

   public static string gameSessionId;

   //private GameObject enemy;

   [Inject]
   public GameSessionFirst(RealTimeClient realTimeClient)
   {
        Debug.Log("Establishing game session.");
        _realTimeClient = realTimeClient;
        setupMatch();
        //enemy = _enemy;
   }

   // make this more descriptive to the player's action, like ballThrown
   /*
   public void playerAction(int opcode, string data)
   {
      _realTimeClient.SendMessage(DeliveryIntent.Fast, opcode, data);
   }
    */

    // creates a Lambda client (client) using credentials
   async void setupMatch()
   {
      Debug.Log("Setting up match.");

      CognitoAWSCredentials credentials = new CognitoAWSCredentials(
         "us-east-2:6b2f8a17-7959-4616-a113-d30ae2f80fea", // Identity pool ID
         RegionEndpoint.USEast2 // Region
      );

      Debug.Log("Credentials established. Establishing client connection.");

      AmazonLambdaClient client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast2);
      InvokeRequest request = new InvokeRequest
      {
         FunctionName = "TestLambda1",
         //InvocationType = "Event"
         InvocationType = InvocationType.RequestResponse
      };

      var response = await client.InvokeAsync(request);

      Debug.Log("Asynchronous request recorded as response.");

    /*
      await client.InvokeAsync(request, (x) =>
        {
            var response = x.Response;
            if (response.FunctionError == null)
            {
                if (response.StatusCode == 200)
                {
                    var payload = Encoding.ASCII.GetString(response.Payload.ToArray()) + "\n";
                    var playerSessionObj = JsonUtility.FromJson<PlayerSessionObject>(payload);

                    if (playerSessionObj.FleetId == null)
                    {
                        Debug.Log($"Error in Lambda: {payload}");
                    }
                    else
                    {
                        joinMatch(playerSessionObj.IpAddress, playerSessionObj.Port, playerSessionObj.PlayerSessionId);
                    }
                }
                else
                {
                    Debug.Log("Status Code does not equal 200");
                }
            }
        });*/
    
      // if HTTP request is successful and fleet ID exists, calls joinMatch
      if (response.FunctionError == null)
      {
         Debug.Log("Status Code: " + response.StatusCode);

         var i = 0;

         while (response.StatusCode != 200)
            {
                System.Threading.Thread.Sleep(6000);
                i++;
                Debug.Log(i);
                if (i == 10)
                {
                    break;
                }
            }

         if (response.StatusCode == 200)
         {
            var payload = Encoding.ASCII.GetString(response.Payload.ToArray()) + "\n";
            var playerSessionObj = JsonUtility.FromJson<PlayerSessionObject>(payload);

                Debug.Log("Game Session ID in GameSessionFirst: " + playerSessionObj.GameSessionId);
            gameSessionId = playerSessionObj.GameSessionId;

            if (playerSessionObj.FleetId == null)
            {
               Debug.Log($"Error in Lambda: {payload}");
            }
            else
            {
               joinMatch(playerSessionObj.IpAddress, playerSessionObj.Port, playerSessionObj.PlayerSessionId, playerSessionObj.PlayerId);
            }
         }
            else
            {
                Debug.Log("Status Code does not equal 200");
                Debug.Log("Status Code: " + response.StatusCode);
            }
      }
      else
      {
         Debug.LogError(response.FunctionError);
      } 
   }
        

   // joins match using these parameters
   void joinMatch(string playerSessionDns, string playerSessionPort, string playerSessionId, string PlayerId)
   {
      Debug.Log($"[client] Attempting to connect to server dns: {playerSessionDns} TCP port: {playerSessionPort} Player Session ID: {playerSessionId}");

      int localPort = GetAvailablePort();

      _realTimeClient.init(playerSessionDns,
         Int32.Parse(playerSessionPort), localPort, ConnectionType.RT_OVER_WS_UDP_UNSECURED, playerSessionId, connectionPayload, PlayerId);
   }

   // gets an available port using System.Net (got this from the video)
   public static int GetAvailablePort()
   {
        Debug.Log("Getting available port.");
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Bind(DefaultLoopbackEndpoint);
            return ((IPEndPoint)socket.LocalEndPoint).Port;
        }
   }
}
