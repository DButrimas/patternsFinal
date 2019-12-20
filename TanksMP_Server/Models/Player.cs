using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models
{
    public class Player
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Score { get; set; }
        public long PosX { get; set; }
        public long PosY { get; set; }
        public int Rotation { get; set; }

        public Player()
        {
        }
    }
}
