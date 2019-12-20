using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TanksMP_Server.Models;
using TanksMP_Server.Models.BlockModels;

namespace TanksMP_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayersController(PlayerContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return _context.Players.ToList();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public async Task<IActionResult> PutPlayer(Player player)
        {

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(player.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public Player generatePosP(Player p)
        {
            p.PosX = 0;
            p.PosY = 0;

            return p;
        }
        // POST: api/Players
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<Player> PostPlayer()
        {
            List<IBlock> blocks = new List<IBlock>();
            var a = _context.Maps.First();
            Player p = new Player();
            p = generatePosP(p);


            char[] ss = { '[', ']' };
            List<String> splited = a.jsonBLocks.Split(ss).ToList();
            splited = splited.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            string jsonCorrect = "";
            string jsonCorrect2 = "";
            string jsonCorrect3 = "";
            int cnt = splited.Count();


            jsonCorrect += "[" + splited[0] + "]";
            var model = JsonConvert.DeserializeObject<List<Brick>>(jsonCorrect);
            foreach (var item in model)
            {
                blocks.Add(item);
            }
            jsonCorrect2 += "[" + splited[1] + "]";
            var modelw = JsonConvert.DeserializeObject<List<Water>>(jsonCorrect2);
            foreach (var item in modelw)
            {
                blocks.Add((Water)item);
            }
            jsonCorrect3 += "[" + splited[2] + "]";
            var model3 = JsonConvert.DeserializeObject<List<Ground>>(jsonCorrect3);
            foreach (var item in model3)
            {
                blocks.Add((Ground)item);
            }

            foreach (var item in blocks)
            {
                if (item.getPosX() == p.PosX || item.getPosY() == p.PosY)
                {
                    p = generatePosP(p);
                }
            }

            p.Id = _context.Players.Count();
            _context.Players.Add(p);
            await _context.SaveChangesAsync();

            return p;
        }



        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Player>>> DeletePlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return _context.Players.ToList();
        }

        private bool PlayerExists(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
