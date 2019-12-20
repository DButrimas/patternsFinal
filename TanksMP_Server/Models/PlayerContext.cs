using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanksMP_Server.Models;

namespace TanksMP_Server.Models
{
    public class PlayerContext : DbContext
    {

        public PlayerContext(DbContextOptions<PlayerContext> options)
    : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Map> Maps { get; set; }

        public DbSet<Message> Messages { get; set; }

    }
}
