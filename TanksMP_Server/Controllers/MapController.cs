using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TanksMP_Server.Models;
using TanksMP_Server.Models.Factories;
using System.Text.Json;
using TanksMP_Server.Models.BlockModels;

namespace TanksMP_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly PlayerContext _context;

        public MapController(PlayerContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<string>> BuildMap()
        {
            if (_context.Maps.Count() > 0)
            {
                var a = _context.Maps.First();
                return a.jsonBLocks;
            }

            MapBuilder bb = new SmalMapBuilder();
            bb.CreateMap(20, 20);
            bb.AddItems();
            bb.Map.CreateBlocksArray();
            

            MapDirector d = new MapDirector();
            d.BuildMap(bb);

            List<Brick> lstb = new List<Brick>();
            foreach (var item in bb.Map.Blocks)
            {
                if (item.getType() =="Brick")
                {
                    lstb.Add((Brick)item);
                }
            }
            List<Water> lstw = new List<Water>();
            foreach (var item in bb.Map.Blocks)
            {
                if (item.getType() == "Water")
                {
                    lstw.Add((Water)item);
                }
            }
            List<Ground> lstg = new List<Ground>();
            foreach (var item in bb.Map.Blocks)
            {
                if (item.getType() == "Ground")
                {
                    lstg.Add((Ground)item);
                }
            }

            string huj = lstb.ToString() + lstw.ToString();
            var bbb = Newtonsoft.Json.JsonConvert.SerializeObject(lstb);
            var bbb2 = Newtonsoft.Json.JsonConvert.SerializeObject(lstw);
            var bbb3 = Newtonsoft.Json.JsonConvert.SerializeObject(lstg);
            var cc = bbb + bbb2 + bbb3;

            bb.Map.jsonBLocks = cc;
            _context.Maps.Add(bb.Map);
           
            _context.SaveChanges();
            return cc;
        }
        [HttpGet("{update}")]
        public async Task<ActionResult<string>> UpdateMap()
        {
            var a = _context.Maps.First();

            return a.jsonBLocks;
        }

    }

}