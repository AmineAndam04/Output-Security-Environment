using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using System;
using System.Collections.Generic;

public class SendRewards : SideChannel
{
    public SendRewards()
    {
        
        ChannelId = new Guid("Individual_Rewards"); // Change the Guid
    }

    protected override void OnMessageReceived(IncomingMessage msg)
    {
        
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
}
