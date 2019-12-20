using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.BlockModels
{
    public interface IBlock
    {
        void setPosX(int x);
        void setPosY(int y);
        void setPosXY(int x, int y);
        int getPosX();
        int getPosY();
        string getType();

    }
}
