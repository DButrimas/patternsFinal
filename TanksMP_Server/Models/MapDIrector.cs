using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models { 
    public class MapDirector
    {
        public Map BuildMap(MapBuilder mapBuilder)
        {
            mapBuilder.AddBlocks();
            mapBuilder.AddItems();

            return mapBuilder.Map;
        }
    }
}
