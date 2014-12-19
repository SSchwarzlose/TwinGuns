﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteSpace.Network;

namespace MasterServer
{
    public class MasterServer
    {
        List<string> games = new System.Collections.Generic.List<string>();

        public void OnNetworkmessageEnter(ReceiveableNetworkMessage msg)
        {
            games.Add(msg.getInformation("Name"));
        }

        public void OnFindGamesRequest(ReceiveableNetworkMessage msg)
        {
            sendGames();
        }

        public void OnJoinGameRequestEnter(ReceiveableNetworkMessage msg)
        {
            SendableNetworkMessage message = new SendableNetworkMessage("Join");
            Server.sendMessage(message);
        }

        public void sendGames()
        {
            foreach (string s in games)
            {
                SendableNetworkMessage sendMessage = new SendableNetworkMessage("FoundGame");
                sendMessage.addInformation("GameName", s);
                Server.sendMessage(sendMessage);
            }
        }


    }
}
