using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanksMP_Server.Models.BlockModels;
using TanksMP_Server.Models.ItemModels;
using TanksMP_Server.Models.Factories;


namespace TanksMP_Server.Models
{
    public class SmalMapBuilder : MapBuilder
    {
        public override void AddBlocks()
        {// Tik kaip pvz. Reikia sugalvoti algoritma kuris sugeneruos visus blokus zemelapije.
            for (int i = 0; i < Map.SizeX; i++)
            {
                this.Map.Blocks[i, 0] = BlockFactory.GetBlock(BlockType.BlockTypes.Border);
                this.Map.Blocks[i, 0].setPosXY(i, 0);

                this.Map.Blocks[0, i] = BlockFactory.GetBlock(BlockType.BlockTypes.Border);
                this.Map.Blocks[0, i].setPosXY(0, i);

                this.Map.Blocks[Map.SizeX - 1, i] = BlockFactory.GetBlock(BlockType.BlockTypes.Border);
                this.Map.Blocks[Map.SizeX - 1, i].setPosXY(Map.SizeX - 1, i);

                this.Map.Blocks[i, Map.SizeY - 1] = BlockFactory.GetBlock(BlockType.BlockTypes.Border);
                this.Map.Blocks[i, Map.SizeY - 1].setPosXY(i, Map.SizeY - 1);

            }
            
            Random rnd = new Random();
            int waterCnt = rnd.Next(1, 3);

            for (int x = 0; x < waterCnt; x++)
            {
                int x1 = rnd.Next(1, 19);
                int x2 = rnd.Next(1, 19);
                int y1 = rnd.Next(1, 19);
                int y2 = rnd.Next(1, 19);

                this.Map.Blocks[x1, y1] = BlockFactory.GetBlock(BlockType.BlockTypes.Water);
                this.Map.Blocks[x1, y1].setPosXY(x1, y1);

                this.Map.Blocks[x2, y2] = BlockFactory.GetBlock(BlockType.BlockTypes.Water);
                this.Map.Blocks[x2, y2].setPosXY(x2, y2);

                int numOfBlocksTodrawX = 0;
                int numOfBlocksTodrawY = 0;
                int tempX = 0;

                if (x1 < x2)
                {
                    numOfBlocksTodrawX = x2 - x1;
                    for (int i = 1; i < numOfBlocksTodrawX + 1; i++)
                    {
                        if (this.Map.Blocks[x1 + i, y1] == null)
                        {
                            this.Map.Blocks[x1 + i, y1] = BlockFactory.GetBlock(BlockType.BlockTypes.Water);
                            this.Map.Blocks[x1 + i, y1].setPosXY(x1 + i, y1);
                        }
                    }
                    tempX = x2;
                }
                else
                {
                    numOfBlocksTodrawX = x1 - x2;
                    for (int i = 1; i < numOfBlocksTodrawX + 1; i++)
                    {
                        if (this.Map.Blocks[x2 + i, y1] == null)
                        {
                            this.Map.Blocks[x2 + i, y2] = BlockFactory.GetBlock(BlockType.BlockTypes.Water);
                            this.Map.Blocks[x2 + i, y2].setPosXY(x2 + i, y2);
                        }
                    }
                    tempX = x1;
                }

                if (y1 < y2)
                {
                    numOfBlocksTodrawY = y2 - y1;
                    for (int i = 1; i < numOfBlocksTodrawY; i++)
                    {
                        if (this.Map.Blocks[tempX, y1 + i] == null)
                        {
                            this.Map.Blocks[tempX, y1 + i] = BlockFactory.GetBlock(BlockType.BlockTypes.Water);
                            this.Map.Blocks[tempX, y1 + i].setPosXY(tempX, y1 + i);
                        }
                    }
                }
                else
                {
                    numOfBlocksTodrawY = y1 - y2;
                    for (int i = 1; i < numOfBlocksTodrawY; i++)
                    {
                        if (this.Map.Blocks[tempX, y2 + i] == null)
                        {
                            this.Map.Blocks[tempX, y2 + i] = BlockFactory.GetBlock(BlockType.BlockTypes.Water);
                            this.Map.Blocks[tempX, y2 + i].setPosXY(tempX, y2 + i);
                        }
                    }
                }
            }
            int bricksPatter = rnd.Next(0, 2);

            if (bricksPatter == 0)
            {
                for (int i = 5; i < Map.SizeX - 5; i++)
                {
                    this.Map.Blocks[i, i] = BlockFactory.GetBlock(BlockType.BlockTypes.Brick);
                    this.Map.Blocks[i, i].setPosXY(i, i);

                    this.Map.Blocks[Map.SizeX - i, i] = BlockFactory.GetBlock(BlockType.BlockTypes.Brick);
                    this.Map.Blocks[Map.SizeX - i, i].setPosXY(Map.SizeX - i, i);
                }

            }
            if (bricksPatter == 1)
            {
                for (int i = Map.SizeX / 5; i < (Map.SizeX - Map.SizeX / 5) + 1; i+=2)
                {
                    this.Map.Blocks[i, Map.SizeX / 5] = BlockFactory.GetBlock(BlockType.BlockTypes.Brick);
                    this.Map.Blocks[i, Map.SizeX / 5].setPosXY(i, Map.SizeX / 5);

                    this.Map.Blocks[Map.SizeX / 5, i] = BlockFactory.GetBlock(BlockType.BlockTypes.Brick);
                    this.Map.Blocks[Map.SizeX / 5, i].setPosXY(Map.SizeX / 5, i);

                    this.Map.Blocks[i, Map.SizeX - Map.SizeX / 5] = BlockFactory.GetBlock(BlockType.BlockTypes.Brick);
                    this.Map.Blocks[i, Map.SizeX - Map.SizeX / 5].setPosXY(i, Map.SizeX - Map.SizeX / 5);


                    this.Map.Blocks[Map.SizeX - Map.SizeX / 5, i] = BlockFactory.GetBlock(BlockType.BlockTypes.Brick);
                    this.Map.Blocks[Map.SizeX - Map.SizeX / 5, i].setPosXY(Map.SizeX - Map.SizeX / 5, i);
                }
            }

            for (int i = 0; i < Map.SizeX; i++)
            {
                for (int j = 0; j < Map.SizeY; j++)
                {
                    if (Map.Blocks[i, j] == null)
                    {
                        Map.Blocks[i, j] = BlockFactory.GetBlock(BlockType.BlockTypes.Ground);
                        Map.Blocks[i, j].setPosXY(i, j);
                    }
                }
            }
        }

        public override void AddItems()
        {
            Itemfactory = new DefensiveItemFactory();
            this.Map.Items.Add(Itemfactory.createStatusPowerUp());
        }

    }
}
