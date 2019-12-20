using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.ItemModels
{
    public class Mine : IConsumablePowerUp
    {

        private int PosX { get; set; }
        private int PosY { get; set; }


        public Mine(int PosX, int PosY)
        {
            this.PosX = PosX;
            this.PosY = PosY;
        }
        public Mine()
        {

        }

        public int getPosX()
        {
            return PosX;
        }

        public int getPosY()
        {
            return PosY;
        }

        public void setPosX(int x)
        {
            PosX = x;
        }

        public void setPosY(int y)
        {
            PosY = y;
        }

        public void activateConsumable()
        {
            throw new NotImplementedException();
        }
    }

}
