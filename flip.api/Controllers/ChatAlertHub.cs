using flip.api.Models;
using flip.dal.DB_context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flip.biz.Entities;

namespace flip.api.Controllers
{
    public class ChatAlertHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;

        public ChatAlertHub(IConfiguration config, Db_FlipContext context)
        {
            _config = config;
            _context = context;
        }
    }
}
