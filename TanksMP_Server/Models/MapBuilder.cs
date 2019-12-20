using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanksMP_Server.Models.Factories;

namespace TanksMP_Server.Models
{
    public abstract class MapBuilder
    {
        public BlockFactory BlockFactory = new BlockFactory();

        public ItemFactory Itemfactory;


        public Map Map { get; private set; }
        public void CreateMap(int SizeX, int SizeY)
        {
            Map = new Map(SizeX, SizeY);
            Map.CreateBlocksArray();
        }

        public abstract void AddBlocks();
        public abstract void AddItems();
    }
}
