using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TanksMP_Server.Models.BlockModels;
using TanksMP_Server.Models.ItemModels;

namespace TanksMP_Server.Models
{
    public class Map
    {
         public int Id { get; set; }

        [NotMapped]
        public List<IPickUpItem> Items { get; set; } = new List<IPickUpItem>();
        [NotMapped]
        public IBlock[,] Blocks { get; set; }

        public int SizeX { get; set; }
        public string jsonBLocks { get; set; }
        public int SizeY { get; set; }

        public Map(int SizeX, int SizeY)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }
        public Map()
        {

        }

        public void CreateBlocksArray()
        {
            Blocks = new IBlock[SizeX, SizeY];
        }
    }
}
