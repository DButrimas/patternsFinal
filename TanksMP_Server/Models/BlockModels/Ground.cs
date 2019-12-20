using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.BlockModels
{
    public class Ground : IBlock
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string Type { get; } = "Ground";

        public Ground(int PosX, int PosY, string Type)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.Type = Type;
        }
        public Ground()
        {
            this.Type = "Ground";
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

        }

        public string getType()
        {
            return Type;
        }

        public void setPosXY(int x, int y)
        {
            PosX = x;
            PosY = y;
        }
    }
}
