﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteSpace.GameLoop;
using WhiteSpace.Components;
using WhiteSpace.Temp;
using Microsoft.Xna.Framework;
using WhiteSpace.Network;

namespace WhiteSpace.GameClasses
{
    public class NetworkGame
    {

        ComponentsSector<gamestate> gameSector = new ComponentsSector<gamestate>(gamestate.game);

        public NetworkGame()
        {
            buildMotherShips();
            StateMachine<gamestate>.getInstance().changeState(gamestate.game);
        }

        void buildMotherShips()
        {
            Transform ship1Transform = Transform.createTransformWithSizeOnPosition(new Vector2(), new Vector2(150,350));
            GameObjectFactory.createMotherShip(gameSector, ship1Transform, "Ship", Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 100);
            Transform ship2Transform = Transform.createTransformWithSizeOnPosition(new Vector2(650, 0), new Vector2(150, 350));
            GameObjectFactory.createMotherShip(gameSector, ship2Transform, "Ship", Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally, 100);
            Transform test = Transform.createTransformWithSizeOnPosition(new Vector2(0, 150), new Vector2(50,50));
            GameObjectFactory.createBasicShip(gameSector, test , "Ship", Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 10, ship2Transform);
        }
    }
}
