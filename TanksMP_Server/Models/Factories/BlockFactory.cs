using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanksMP_Server.Models.BlockModels;

namespace TanksMP_Server.Models.Factories
{
    public class BlockFactory
    {
        public IBlock GetBlock(BlockType.BlockTypes type)
        {
            switch (type)
            {
                case BlockType.BlockTypes.Brick:
                    return new Brick();
                case BlockType.BlockTypes.Grass:
                    return new Grass();
                case BlockType.BlockTypes.Water:
                    return new Water();
                case BlockType.BlockTypes.Ground:
                    return new Ground();
                case BlockType.BlockTypes.Iron:
                    return new Iron();
                case BlockType.BlockTypes.Border:
                    return new Border();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
