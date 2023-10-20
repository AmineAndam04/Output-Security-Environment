using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using System;
using System.Collections.Generic;

public class SendHyperParameters : SideChannel
{
    List<float> hyperParameters ;
    public SendHyperParameters()
    {
        
        ChannelId = new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"); // Change the Guid
    }

    protected override void OnMessageReceived(IncomingMessage msg)
    {
        hyperParameters = new List<float>();
        int numHyperParameters = msg.ReadInt32(); // Read the count of float values

        for (int i = 0; i < numHyperParameters; i++)
        {
            float hyperParameter = msg.ReadFloat32();
            hyperParameters.Add(hyperParameter);
        }
        
    }
    public void SendIndividualRewards(List<float> floatList)
    {
        using (var msgOut = new OutgoingMessage())
        {
            
            foreach (var floatValue in floatList)
            {
                msgOut.WriteFloat32(floatValue);
            }
            QueueMessageToSend(msgOut);
        }
    }

    public List<float> GetReceivedHyperParameters()
    {
        return hyperParameters;
    }
}
