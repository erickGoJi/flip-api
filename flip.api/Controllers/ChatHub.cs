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
    public class ChatHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;

        public ChatHub(IConfiguration config, Db_FlipContext context)
        {
            _config = config;
            _context = context;
        }       

        public async Task SendToAll(string name, string message, int ConnectionId, int userID)
        {
            await Clients.All.SendAsync("sendToAll", name, message);                         

            Message item = new Message();            
            item.Message1 = message;
            item.UserId = userID;
            item.ConversationId = ConnectionId;
            
             try
             {
                 if (item.Id == 0) // Crea Registro 
                 {
                     item.Status = false; 
                     item.Time = DateTime.Now; 
                     _context.Messages.Add(item);
                     _context.SaveChanges();
                     //return response = Ok(new { result = "Success", detalle = "Registro Creado con éxito.", id = item.Id });                 
                 }
             }
             catch (Exception ex) { }            
        }                
    }
}
