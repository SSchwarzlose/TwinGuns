﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteSpace.Temp;

namespace WhiteSpace.GameClasses
{
    public class RessourceContributor : UpdateableComponent
    {
        int amount;
        int interval;
        int counter = 0;
        GameRessources ressourcesToContributeTo;

        public RessourceContributor(GameRessources ressourcesToContributeTo, int amount, int interval)
        {
            this.amount = amount;
            this.interval = interval;
            this.ressourcesToContributeTo = ressourcesToContributeTo;
        }

        protected override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            int elpased = gameTime.ElapsedGameTime.Milliseconds;  
            counter += elpased;
                
            if (counter >= interval) 
            { 
                counter = 0;  
                this.ressourcesToContributeTo.adjustRessources(amount);
            } 
        }
    }
}
