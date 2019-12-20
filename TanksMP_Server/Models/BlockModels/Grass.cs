using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.BlockModels
{
    public class Grass : IBlock
    {
        public int PosX { get; set; }
        public int PosY { get; set; }

        public string Type { get; } = "Grass";

        public Grass(int PosX, int PosY, string Type)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.Type = Type;
        }
        public Grass()
        {

        }
        public void setPosXY(int x, int y)
        {
            PosX = x;
            PosY = y;
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
        public string getType()
        {
            return Type;
        }
    }
}
