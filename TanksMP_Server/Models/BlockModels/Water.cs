using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.BlockModels
{
    public class Water : IBlock
    {
        public int PosX { get; set; }
        public int PosY { get; set; }

        public string Type { get; } = "Water";


        public Water(int PosX, int PosY, string Type)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.Type = Type;
        }
        public Water()
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
        public void setPosXY(int x, int y)
        {
            PosX = x;
            PosY = y;
        }
        public void setPosY(int y)
        {
            PosY = y;
        }
        public string getType()
        {
            return Type;
        }
    }
}
