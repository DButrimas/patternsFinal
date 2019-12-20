using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TanksMP_Server.Models;

namespace TanksMP_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChatController : ControllerBase
    {
        private readonly PlayerContext _context;

        public ChatController(PlayerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var asd = _context.Messages.ToList();
            return asd;

        }
        [HttpPost]
        public async Task<ActionResult<int>> PostMessage(Message msg)
        {
            _context.Messages.Add(msg);
            _context.SaveChanges();
            return 0;
        }

    }
}