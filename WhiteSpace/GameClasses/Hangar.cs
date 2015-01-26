﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteSpace.Components;
using WhiteSpace.GameLoop;
using Microsoft.Xna.Framework;
using WhiteSpace.Network;
using WhiteSpace.Temp;
using WhiteSpace.Components.Drawables;
using Microsoft.Xna.Framework.Graphics;


namespace WhiteSpace.GameClasses
{
    public class Hangar : UpdateableComponent
    {
        Transform targetTransform;
        Transform transform;

        GameObject[,] droneStocks = new GameObject[3, 3];

        bool player;

        ComponentsSector<gamestate> sector;

        GameObject ressourceCounter;
        GameRessources ressources;

        public Hangar()
        {
        }

        public Hangar(Transform target, bool player, GameRessources ressources)
        {
            this.targetTransform = target;
            this.player = player;
            this.ressources = ressources;
        }

        public override void start()
        {
            ressourceCounter = GameObjectFactory.createLabel(this.parent.sector, Transform.createTransformWithSizeOnPosition(new Vector2(400, 30), new Vector2(100, 20)), "0");   

            base.start();
            this.transform = this.parent.getComponent<Transform>();
            if (this.player == Client.host)
            {
                this.parent.getComponent<Clickable>().releaseMethods += buildHangarButtons;
            }
            Client.registerNetworkListenerMethod("BuildDrone", OnBuildDroneMessageEnter);
            Client.registerNetworkListenerMethod("OpenHangar", OnOpenHangarMessageEnter);
        }

        public void buildHangarButtons(Clickable sender)
        {
            sector = new ComponentsSector<gamestate>(gamestate.game);
            Transform t1 = Transform.createTransformWithSizeOnPosition(new Vector2(160, 200), new Vector2(50, 30));
            GameObject b1 = GameObjectFactory.createButton(sector, t1, "+", 0, sendBuildDroneMessage);

            Transform t2 = Transform.createTransformWithSizeOnPosition(new Vector2(160, 240), new Vector2(50, 30));
            GameObject b2 = GameObjectFactory.createButton(sector, t2, "+", 1, sendBuildDroneMessage);

            Transform t3 = Transform.createTransformWithSizeOnPosition(new Vector2(160, 280), new Vector2(50, 30));
            GameObject b3 = GameObjectFactory.createButton(sector, t3, "+", 2, sendBuildDroneMessage);

            Transform t4 = Transform.createTransformWithSizeOnPosition(new Vector2(220, 200), new Vector2(50, 30));
            GameObject b4 = GameObjectFactory.createButton(sector, t4, "open", 0, sendOpenHangarMessage);

            Transform t5 = Transform.createTransformWithSizeOnPosition(new Vector2(220, 240), new Vector2(50, 30));
            GameObject b5 = GameObjectFactory.createButton(sector, t5, "open", 1, sendOpenHangarMessage);

            Transform t6 = Transform.createTransformWithSizeOnPosition(new Vector2(220, 280), new Vector2(50, 30));
            GameObject b6 = GameObjectFactory.createButton(sector, t6, "open", 2, sendOpenHangarMessage);

            StateMachine<gamestate>.getInstance().changeState(gamestate.game);
            sender.releaseMethods -= buildHangarButtons;
            sender.releaseMethods += deactivateHangarButtons;
        }

        public void reactivateHangarButtons(Clickable sender)
        {
            sector.reload();
            sender.releaseMethods += deactivateHangarButtons;
            sender.releaseMethods -= reactivateHangarButtons;
        }

        public void deactivateHangarButtons(Clickable sender)
        {
            sector.deactivate();
            sender.releaseMethods += reactivateHangarButtons;
            sender.releaseMethods -= deactivateHangarButtons;
        }

        protected override void update(GameTime gameTime)
        {
            if(KeyboardInput.wasKeyJustPressed(Microsoft.Xna.Framework.Input.Keys.B) && this.player == Client.host)
            {
                sendBuildDroneMessage(new Clickable());
            }

            if(KeyboardInput.wasKeyJustPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                this.player = !this.player;
                Client.host = true;
                this.parent.addComponent(new LifeSender(this.player));
            }

            if(KeyboardInput.wasKeyJustPressed(Microsoft.Xna.Framework.Input.Keys.U))
            {
                sendOpenHangarMessage(new Clickable());
            }

            ressourceCounter.getComponent<TextDrawer>().text = "Ressources: " + ressources.ressources.ToString();
        }

        public void sendBuildDroneMessage(Clickable sender)
        {
            if (ressources.haveEnoughRessources(15))
            {
                SendableNetworkMessage msg = new SendableNetworkMessage("BuildDrone");
                msg.addInformation("Player", player);
                msg.addInformation("Index", sender.id);
                Client.sendMessage(msg);
            }
        }

        public void sendOpenHangarMessage(Clickable sender)
        {
            SendableNetworkMessage msg = new SendableNetworkMessage("OpenHangar");
            msg.addInformation("Index", sender.id);
            msg.addInformation("Player", player);
            Client.sendMessage(msg);
        }

        void OnBuildDroneMessageEnter(ReceiveableNetworkMessage msg)
        {
            Transform transform = Transform.createTransformWithSize(new Vector2(55, 55));
            Transform target = new Transform();

            if (Boolean.Parse(msg.getInformation("Player")) == this.player)
            {
                if (this.player == Client.host)
                {
                    ressources.ressources = int.Parse(msg.getInformation("Ressources"));
                }
                addDrone(int.Parse(msg.getInformation("Index")));
            }
        }

        void OnOpenHangarMessageEnter(ReceiveableNetworkMessage msg)
        {
            if(Boolean.Parse(msg.getInformation("Player")) == this.player)
            {
                openStock(int.Parse(msg.getInformation("Index")));
            }
        }

        public void addDrone(int stock)
        {
            for(int i = 0; i <= 2; i++)
            {
                if(droneStocks[i, stock] == null)
                {
                    SpriteEffects effect = SpriteEffects.FlipHorizontally; 

                    if(this.player == Client.host)
                    {
                        effect = SpriteEffects.None;
                    }

                    Transform transform;
                    if (player == Client.host)
                    {
                        transform = Transform.createTransformOnPosition(new Vector2(this.transform.Position.X + 40 * i, this.transform.Center.Y + 30 + stock * 40));
                    }
                    else
                    {
                        transform = Transform.createTransformOnPosition(new Vector2(this.transform.Position.X + (this.transform.Size.X -25) - 40 * i, this.transform.Center.Y + 30 + 25 + stock * 40));
                    }

                    droneStocks[i, stock] = GameObjectFactory.createDrone(this.parent.sector, transform, "Ship", effect, 8, targetTransform);
                    break;
                }
            }
        }

        public void openStock(int stock)
        {
            for (int i = 0; i <= 2; i++)
            {
                if (droneStocks[i, stock] != null)
                {
                    if (this.player == Client.host)
                    {
                        Tower.thisDronesTransforms.Add(droneStocks[i, stock].getComponent<Transform>());
                    }
                    else
                    {
                        Tower.enemyDronesTransforms.Add(droneStocks[i, stock].getComponent<Transform>());
                    }
                    droneStocks[i, stock].addComponent(new Ship(targetTransform));
                    droneStocks[i, stock] = null;
                }
            }
        }

    }
}
