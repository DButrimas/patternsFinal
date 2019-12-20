using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models.ItemModels
{
    public interface IConsumablePowerUp : IPickUpItem
    {
        void activateConsumable();
    }
}
