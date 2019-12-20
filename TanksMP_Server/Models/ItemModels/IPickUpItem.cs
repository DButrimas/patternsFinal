using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.ItemModels
{
    public interface IPickUpItem
    {
        void setPosX(int x);
        void setPosY(int y);
        int getPosX();
        int getPosY();
    }
}
