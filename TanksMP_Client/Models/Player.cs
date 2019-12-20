using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksMP_Client.Models
{
    class Player
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Score { get; set; }
        public long PosX { get; set; }
        public long PosY { get; set; }

        public Player()
        {
        }
    }
}
