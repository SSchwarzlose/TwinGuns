﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteSpace.Network;
using System.Threading;

namespace TestServer
{
    public class Player
    {
        float ressources;
        bool player1;
        Server gameServer;
        int towerCosts = 25;
        ThreadStart start;
        Thread ressourceThread;
        public float ressourceGain;
        public float ressourceGainPerTower = 0.3f;

        public Player(int ressources, bool player1, Server gameServer)
        {
            this.ressources = ressources;
            this.player1 = player1;
            this.gameServer = gameServer;
            gameServer.registerNetworkListenerMethod("BuildTower", OnBuildTowerRequest);
            gameServer.registerNetworkListenerMethod("DestroyTower", OnDestroyTowerRequest);
            gameServer.registerNetworkListenerMethod("BuildDrone", OnBuildDroneRequest);

            start = new ThreadStart(this.higherRessources);
            ressourceThread = new Thread(start);
            ressourceThread.Start();
        }

        public void OnBuildTowerRequest(ReceiveableNetworkMessage msg)
        {
            if (Boolean.Parse(msg.getInformation("Player")) == this.player1 && ressources >= towerCosts)
            {
                ressources -= towerCosts;
                SendableNetworkMessage smsg = new SendableNetworkMessage("BuildTower");
                smsg.addInformation("x", msg.getInformation("x"));
                smsg.addInformation("y", msg.getInformation("y"));
                smsg.addInformation("Player", msg.getInformation("Player"));
                smsg.addInformation("Type", msg.getInformation("Type"));
                smsg.addInformation("Ressources", ressources);
                gameServer.sendMessage(smsg);

                if(Boolean.Parse(msg.getInformation("Type")) == false)
                {
                    ressourceGain += ressourceGainPerTower;
                }
            }
        }

        public void OnDestroyTowerRequest(ReceiveableNetworkMessage msg)
        {
            if (Boolean.Parse(msg.getInformation("Player")) != this.player1)
            {
                ressources -= towerCosts;
                SendableNetworkMessage smsg = new SendableNetworkMessage("DestroyTower");
                smsg.addInformation("x", msg.getInformation("x"));
                smsg.addInformation("y", msg.getInformation("y"));
                smsg.addInformation("Player", msg.getInformation("Player"));
                smsg.addInformation("Ressources", ressources);
                gameServer.sendMessage(smsg);
            }
        }

        public void OnBuildDroneRequest(ReceiveableNetworkMessage msg)
        {
            if (Boolean.Parse(msg.getInformation("Player")) == player1)
            {
                SendableNetworkMessage smsg = new SendableNetworkMessage("BuildDrone");
                smsg.addInformation("Player", msg.getInformation("Player"));
                smsg.addInformation("Index", msg.getInformation("Index"));
                ressources -= 15;
                smsg.addInformation("Ressources", ressources);
                gameServer.sendMessage(smsg);
            }
        }

        public void higherRessources()
        {

            Thread.Sleep(1000);
            Console.WriteLine(this.ressources += ressourceGain);
            sendRessourceUpdate();
            higherRessources();
        }


        public void sendRessourceUpdate()
        {
            SendableNetworkMessage msg = new SendableNetworkMessage("RessourceUpdate");

            msg.addInformation("Ressources", this.ressources);
            msg.addInformation("Player", this.player1);

            gameServer.sendMessage(msg);
        }
    }
}
