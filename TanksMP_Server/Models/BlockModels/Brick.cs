using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.BlockModels
{
    public class Brick : IBlock
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string Type { get; } = "Brick";

        public Brick(int PosX, int PosY, string Type)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.Type = Type;
        }
        public Brick()
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
        public void setPosXY(int x, int y)
        {
            PosY = y;
            PosX = x;
        }


        public string getType()
        {
            return Type;
        }
    }
}
