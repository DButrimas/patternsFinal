using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanksMP_Server.Models
{
    public class Coordinates
    {
        public long Id { get; set; }
        public long PosX { get; set; }
        public long PosY { get; set; }

        public Coordinates()
        {
        }
    }
}
