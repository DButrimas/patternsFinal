using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanksMP_Server.Models.ItemModels;

namespace TanksMP_Server.Models.Factories
{
    public abstract class ItemFactory
    {
        public abstract IStatusPowerUp createStatusPowerUp();
        public abstract IConsumablePowerUp createConsumablePowerUp();
    }
}
