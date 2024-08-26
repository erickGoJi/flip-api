using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using flip.api.Models;
using flip.biz.Entities;
using flip.dal.DB_context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace flip.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly Db_FlipContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public MessageController(IConfiguration config, Db_FlipContext context, IHubContext<ChatHub> chatHubContext)
        {
            _config = config;
            _context = context;
            _chatHubContext = chatHubContext;

        }
        public class SeeMessageModel
        {
            public int userid { get; set; }
            public int buildingid { get; set; }
        }

        public class InviteModel
        {
            public int userid { get; set; }
            public List<int> inviteidList { get; set; }
            public int idActivity { get; set; }
            public string inviteMessage { get; set; }
        }

        public class NewMessageModel
        {
            public int userid { get; set; }
            public List<int> userList { get; set; }
            public string message { get; set; }
        }

        public class RealTimeMessageResponse
        {
            public int conversationId { get; set; }
            public int user1Id { get; set; }
            public int user2Id { get; set; }

            public RealTimeMessageResponse(int conversationId, int user1Id, int user2Id)
            {
                this.conversationId = conversationId;
                this.user1Id = user1Id;
                this.user2Id = user2Id;
            }
        }

        [Route("NewConversation")]
        [HttpPost("{NewConversation}")]
        public ActionResult<ApiResponse<string>> NewConversation([FromBody]  Conversation item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.UserId
                             select new { id = a.Id }).ToList();

            var response = new ApiResponse<string>();
            var result = new ApiResponse<string>();

            try
            {
                if (item.Id == 0) // Crea Registro 
                {
                    item.Date = DateTime.Now;
                    _context.Conversations.Add(item);
                    _context.SaveChanges();
                    //return response = Ok(new { result = "Success", detalle = "Registro Creado con éxito.", id = item.Id });
                    result = new ApiResponse<string>()
                    {
                        Result = "Success",
                        Success = true,
                        Message = "Post the comment",


                    };
                    return result;
                }
                else
                {
                    result = new ApiResponse<string>()
                    {
                        Result = "Error",
                        Success = false,
                        Message = "Exception "


                    };
                    return result;
                }

            }
            catch (Exception ex)
            {
                result = new ApiResponse<string>()
                {
                    Result = "Error",
                    Success = false,
                    Message = "Exception " + ex


                };
                return result;
            }
        }

        ////Lastmessages
        ////Notifications (Slide Gesture) in the CLIENT
        //[Route("GetLastMessages")]
        //[HttpPost("GetLastMessages")]
        //public IActionResult GetLastMessages([FromBody]  Conversation item)
        //{
        //    var U_session = (from a in _context.Users
        //                     where a.Id == item.UserId
        //                     select new { id = a.Id }).ToList();
        //    IActionResult response = Unauthorized();

        //    var result = new ApiResponse<string>();

        //    try
        //    {
        //        var users = (from c in _context.Users
        //                     join u in _context.Conversations.Include(r => r.Messages) on c.Id equals u.UserId
        //                     join c2 in _context.Users on u.UserIdReciver equals c2.Id
        //                     where u.UserIdReciver == item.UserId || u.UserId == item.UserId
        //                     select new
        //                     {
        //                         iduser = (item.UserId != c.Id ? c.Id : c2.Id),
        //                         name = (item.UserId != c.Id ? c.Name : c2.Name),
        //                         lastname = (item.UserId != c.Id ? c.LastName : c2.LastName),
        //                         photo = (item.UserId != c.Id ? c.Avatar : c2.Avatar),
        //                         conversationId = u.Id,
        //                         lastMessage = u.Messages.OrderByDescending(r => r.Id).FirstOrDefault(r => r.UserId != item.UserId).Message1,
        //                         lastMessageTime = u.Messages.OrderByDescending(r => r.Id).FirstOrDefault(r => r.UserId != item.UserId).Time,
        //                         unreadMessages = u.Messages.Count(r => r.Status == false && r.UserId != item.UserId),
        //                     }
        //                 );

        //        users = users.Where(r => r.unreadMessages > 0);
        //        users = users.OrderByDescending(r => r.lastMessageTime);
        //        var entidades = users.Take(3).ToList();

        //        if (entidades.Count == 0) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }

        //        return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new ApiResponse<string>() { Result = "Error", Success = false, Message = "Exception " + ex };
        //        return response = Ok(new { result = "Error", detalle = "Error", item = 0 });
        //    }
        //}

        //Lastmessages
        //Notifications (Slide Gesture) in the CLIENT
        [Route("GetLastMessages")]
        [HttpPost("GetLastMessages")]
        public IActionResult GetLastMessages([FromBody]  Conversation item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.UserId
                             select new { id = a.Id }).ToList();
            IActionResult response = Unauthorized();

            var result = new ApiResponse<string>();

            try
            {
                var users = (from c in _context.Users
                             join u in _context.Conversations.Include(r => r.Messages) on c.Id equals u.UserId
                             join c2 in _context.Users on u.UserIdReciver equals c2.Id
                             where u.UserIdReciver == item.UserId || u.UserId == item.UserId
                             select new
                             {
                                 iduser = (item.UserId != c.Id ? c.Id : c2.Id),
                                 name = (item.UserId != c.Id ? c.Name : c2.Name),
                                 lastname = (item.UserId != c.Id ? c.LastName : c2.LastName),
                                 photo = (item.UserId != c.Id ? c.Avatar : c2.Avatar),
                                 conversationId = u.Id,
                                 lastMessage = u.Messages.OrderByDescending(r => r.Id).FirstOrDefault(r => r.UserId != item.UserId).Message1,
                                 lastMessageTime = u.Messages.OrderByDescending(r => r.Id).FirstOrDefault(r => r.UserId != item.UserId).Time,
                                 unreadMessages = u.Messages.Count(r => r.Status == false && r.UserId != item.UserId),
                             }
                         );

                users = users.OrderByDescending(r => r.lastMessageTime);
                var entidades = users.Take(3).ToList();

                if (entidades.Count == 0) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }

                return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            }
            catch (Exception ex)
            {
                result = new ApiResponse<string>() { Result = "Error", Success = false, Message = "Exception " + ex };
                return response = Ok(new { result = "Error", detalle = "Error", item = 0 });
            }
        }

        // Chat View
        [Route("SeeChats")]
        [HttpPost("{SeeChats}")]
        public IActionResult SeeChats([FromBody]SeeMessageModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();

            var result = new ApiResponse<List<Post>>();

            var users = (from c in _context.Users
                         join u in _context.Conversations.Include(r => r.Messages) on c.Id equals u.UserId
                         join c2 in _context.Users on u.UserIdReciver equals c2.Id
                         where u.UserIdReciver == item.userid || u.UserId == item.userid
                         select new
                         {
                             iduser = (item.userid != c.Id ? c.Id : c2.Id),
                             name = (item.userid != c.Id ? c.Name : c2.Name),
                             lastname = (item.userid != c.Id ? c.LastName : c2.LastName),
                             photo = (item.userid != c.Id ? c.Avatar : c2.Avatar),
                             conversationId = u.Id,
                             lastMessage = u.Messages.OrderByDescending(r => r.Id).FirstOrDefault().Message1,
                             lastMessageTime = u.Messages.OrderByDescending(r => r.Id).FirstOrDefault().Time,
                             unreadMessages = u.Messages.Count(r => r.Status == false && r.UserId != item.userid)
                         }
                         ).OrderByDescending(r => r.lastMessageTime).ToList();

            var entidades = users;
            if (entidades.Count == 0) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }
            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
        }

        //Conversation View in the Client
        [Route("GetConversationUser")]
        [HttpGet("GetConversationUser")]
        public IActionResult GetConversationUser([FromQuery] int conversationId, [FromQuery] int userId)
        {
            var U_session = (from a in _context.Users
                             where a.Id == userId
                             select new { id = a.Id }
                            ).ToList();

            if (U_session.Count() <= 0) { return Unauthorized(); }

            var res = _context.Conversations.Include(r => r.User).Include(r => r.UserIdReciverNavigation).Where(r => r.Id == conversationId).FirstOrDefault();

            User nUserId;
            if (res.UserId == userId) { nUserId = res.UserIdReciverNavigation; }
            else { nUserId = res.User; }

            return Ok(new { result = "Success", detalle = "Consulta realizada con exito", item = new { nUserId.Id, nUserId.Name, nUserId.LastName, nUserId.Avatar, nUserId.Fcmtoken } });
        }

        //Conversation View in the Client
        [Route("GetMessages")]
        [HttpGet("GetMessages")]
        public IActionResult GetMessages([FromQuery] int conversationId, [FromQuery] int userId, [FromQuery]DateTime? startDate = null, [FromQuery]DateTime? endDate = null)
        {
            var U_session = (from a in _context.Users
                             where a.Id == userId
                             select new { id = a.Id }
                            ).ToList();

            if (U_session.Count() <= 0) { return Unauthorized(); }

            var res = _context.Messages.Where(r => r.ConversationId == conversationId);
            if (startDate.HasValue) { res = res.Where(r => r.Time > startDate); }
            if (startDate.HasValue) { res = res.Where(r => r.Time < endDate); }

            foreach (Message m in res.ToList())
            {
                if (m.UserId == userId) { continue; }

                m.Status = true;
                _context.Messages.Update(m);
            }
            _context.SaveChanges();

            return Ok(new { result = "Success", detalle = "Consulta realizada con exito", item = res });
        }

        [Route("SeeMessage")]
        [HttpPost("{SeeMessage}")]
        public IActionResult SeeMessage([FromBody]  Conversation item)
        {
            var U_session = (from a in _context.Users
                             where a.Id == item.UserId
                             select new { id = a.Id }).ToList();

            IActionResult response = Unauthorized();

            var result = new ApiResponse<string>();

            try
            {
                var posts = (from c in _context.Conversations
                             join a in _context.Messages on c.Id equals a.ConversationId
                             join u in _context.Users on item.UserId equals u.Id
                             where u.Id == c.UserId || u.Id == c.UserIdReciver

                             select new
                             {
                                 idpost = (Int32)c.Id,
                                 message = c.Messages,
                                 date = c.Date,
                                 userid = c.UserId,
                                 name = (from c1 in _context.Users where c1.Id == c.UserId select new { c1.Name, c1.LastName }).ToList()
                             }
                             ).ToList();

                var entidades = posts;

                if (entidades.Count == 0) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }

                return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "Error", Success = false, Message = "Exception " + ex });
            }
        }

        [Route("SeeUsers")]
        [HttpPost("{SeeUsers}")]
        public IActionResult SeeUsers([FromBody]SeeMessageModel item)
        {
            IActionResult response = Unauthorized();

            var U_session = (from a in _context.Users
                             where a.Id == item.userid
                             select new { id = a.Id }).ToList();
            var result = new ApiResponse<List<Post>>();


            var users = (from c in _context.Users
                         where c.BuildingId == item.buildingid
                         select new { iduser = (Int32)c.Id, name = c.Name, lastname = c.LastName, photo = c.Avatar }
                         ).ToList();


            var entidades = users;
            if (entidades.Count == 0) { return response = Ok(new { result = "Error", detalle = "Error", item = 0 }); }

            return response = Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = entidades });
        }

        //Conversation View in the Client
        [Route("SentMessage")]
        [HttpPost("SentMessage")]
        public IActionResult SentMessage([FromBody] Message message)
        {
            var U_session = (from a in _context.Users where a.Id == message.UserId select new { id = a.Id }).ToList();
            if (U_session.Count() <= 0) { return Unauthorized(); }

            try
            {
                message.Time = DateTime.Now;
                message.Status = false;
                _context.Messages.Add(message);
                _context.SaveChanges();
            }
            catch (Exception e) { Ok(new { result = "Error", detalle = "Error", item = 0 }); }

            var conversation = _context.Conversations.FirstOrDefault(r => r.Id == message.ConversationId);
            RealTimeMessageResponse rtmessage = new RealTimeMessageResponse(message.ConversationId, conversation.UserId, conversation.UserIdReciver);
            _chatHubContext.Clients.All.SendAsync("Send", rtmessage);   // Send is the name thaw we're going to use in the client to bind the event   

            return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = 0 });
        }

        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private static string ServerKey = "AAAAWYeDs9o:APA91bHCFgKW9TwCNgpLutW_rio5kzRYxsKRoMoEnOPYwzC-MZwHWAwUeAcwsB4RuTHnll1FphgyNw15sUWx1eP5v0hdN2gzq_XjfFzNaidsUn5_arW1GrHP8QjYqNyQQgDk7V3gh4ne";
        [Route("PushNotification")]
        [HttpPost("PushNotification")]
        public async Task<IActionResult> PushNotification([FromBody] PushNotificationRequest notificationRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
            request.Headers.TryAddWithoutValidation("Authorization", "key =" + ServerKey);
            string jsonMessage = JsonConvert.SerializeObject(notificationRequest);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.SendAsync(request);
            }
            return Ok();
        }

        //Message sent to one or multiple users
        //If theres no conversations first create a conversation between those users
        //NewMessage Modal in the CLIENT
        [Route("SentNewMessage")]
        [HttpPost("SentNewMessage")]
        public IActionResult SentNewMessage([FromBody] NewMessageModel item)
        {
            Conversation conversation = new Conversation();
            Message message = new Message();

            var U_session = (from a in _context.Users where a.Id == item.userid select new { id = a.Id }).ToList();
            if (U_session.Count() <= 0) { return Unauthorized(); }

            foreach (var invite in item.userList)
            {
                bool alreadyMessage = _context.Conversations.Count(
                    a => (a.UserId == invite && a.UserIdReciver == item.userid) ||
                         (a.UserId == item.userid && a.UserIdReciver == invite)) > 0;

                if (!alreadyMessage)
                {
                    try
                    {
                        // Creacion Conversacion
                        //Conversation conversation = new Conversation();
                        conversation = new Conversation();
                        conversation.UserId = item.userid;
                        conversation.UserIdReciver = invite;
                        conversation.Date = DateTime.Now;
                        conversation.Status = false;

                        // Insercion Mensaje
                        //Message message = new Message();
                        message = new Message();
                        message.UserId = item.userid;
                        message.Time = DateTime.Now;
                        message.Status = false;
                        message.Message1 = item.message;

                        conversation.Messages.Add(message);
                        _context.Conversations.Add(conversation);
                    }
                    catch (Exception ex) { return Ok(new ApiResponse<string>() { Result = "Error", Success = false, Message = "Exception " + ex }); }
                }
                else
                {
                    try
                    {
                        var conversationId = _context.Conversations.FirstOrDefault(
                            a => (a.UserId == invite && a.UserIdReciver == item.userid) ||
                            (a.UserId == item.userid && a.UserIdReciver == invite));

                        //Message message = new Message();
                        message = new Message();
                        message.UserId = item.userid;
                        message.Time = DateTime.Now;
                        message.Status = false;
                        message.ConversationId = conversationId.Id;
                        message.Message1 = item.message;

                        _context.Messages.Add(message);
                    }
                    catch (Exception ex) { return Ok(new ApiResponse<string>() { Result = "Error", Success = false, Message = "Exception " + ex }); }

                } // End else (alreadyMessage)

            } // End foreach     

            _context.SaveChanges();
            RealTimeMessageResponse rtmessage = new RealTimeMessageResponse(message.ConversationId, conversation.UserId, conversation.UserIdReciver);
            _chatHubContext.Clients.All.SendAsync("Send", rtmessage);   // Send is the name thaw we're going to use in the client to bind the event           
            return Ok(new ApiResponse<string>() { Result = "Success", Success = true });
        }

        [Route("SentInviteActivity")]
        [HttpPost("{SentInviteActivity}")]
        public IActionResult SentInviteActivity([FromBody] InviteModel item)
        {
            Conversation conversation = new Conversation();
            Message message = new Message();

            var activity = _context.Activities.FirstOrDefault(a => a.Id == item.idActivity);
            var user = _context.Users.FirstOrDefault(a => a.Id == item.userid);
            var activityURL = "<a>" + item.idActivity + "</a>";
            string MENSAJE = user.Name + " " + user.LastName + " te invita a " + activity.Name + "  " + activityURL;

            foreach (var invite in item.inviteidList)
            {
                bool alreadyMessage = _context.Conversations.Count(
                    a => (a.UserId == invite && a.UserIdReciver == item.userid) ||
                         (a.UserId == item.userid && a.UserIdReciver == invite)) > 0;

                if (!alreadyMessage)
                {
                    try
                    {
                        // Creacion Conversacion
                        //Conversation conversation = new Conversation();
                        conversation.UserId = item.userid;
                        conversation.UserIdReciver = invite;
                        conversation.Date = DateTime.Now;
                        conversation.Status = false;

                        // Insercion Mensaje
                        //Message message = new Message();
                        message.UserId = item.userid;
                        message.Time = DateTime.Now;
                        message.Status = false;
                        //item.inviteMessage = user.Name + " " + user.LastName + " te invita a " + activity.Name + "  " + activityURL;
                        item.inviteMessage = MENSAJE;
                        message.Message1 = item.inviteMessage;

                        conversation.Messages.Add(message);
                        _context.Conversations.Add(conversation);
                    }
                    catch (Exception ex) { return Ok(new ApiResponse<string>() { Result = "Error", Success = false, Message = "Exception " + ex }); }

                }
                else
                {
                    try
                    {
                        var conversationId = _context.Conversations.FirstOrDefault(
                            a => (a.UserId == invite && a.UserIdReciver == item.userid) ||
                            (a.UserId == item.userid && a.UserIdReciver == invite));

                        //Message message = new Message();                       
                        message.UserId = item.userid;
                        message.Time = DateTime.Now;
                        message.Status = false;
                        message.ConversationId = conversationId.Id;

                        //item.inviteMessage = user.Name + user.LastName + " te invita a " + activity.Name + "  " + activityURL;
                        item.inviteMessage = MENSAJE;
                        message.Message1 = item.inviteMessage;

                        _context.Messages.Add(message);
                    }
                    catch (Exception ex) { return Ok(new ApiResponse<string>() { Result = "Error", Success = false, Message = "Exception " + ex }); }

                } // End else (alreadyMessage)

            } // End foreach     

            _context.SaveChanges();
            RealTimeMessageResponse rtmessage = new RealTimeMessageResponse(message.ConversationId, conversation.UserId, conversation.UserIdReciver);
            _chatHubContext.Clients.All.SendAsync("Send", rtmessage);   // Send is the name thaw we're going to use in the client to bind the event           
            return Ok(new ApiResponse<string>() { Result = "Success", Success = true, Message = "Post the comment", });
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// SIGNAL R TESTING PURPOSES
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Inside the conversation View
        [Route("SignalRMEssage")]
        [HttpPost("SignalRMEssage")]
        public IActionResult SignalRMEssage([FromBody] Message message)
        {
            try
            {
                message.Time = DateTime.Now;
                message.Status = false;
            }
            catch (Exception e) { Ok(new { result = "Error", detalle = "Error", item = 0 }); }

            var conversation = _context.Conversations.FirstOrDefault(r => r.Id == message.ConversationId);
            RealTimeMessageResponse rtmessage = new RealTimeMessageResponse(message.ConversationId, conversation.UserId, conversation.UserIdReciver);
            _chatHubContext.Clients.All.SendAsync("Send", rtmessage);   // Send is the name thaw we're going to use in the client to bind the event           
            return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = message.Message1 });
        }

        [Route("SentMessageAll")]
        [HttpPost("SentMessageAll")]
        public IActionResult SentMessageAll([FromBody] List<Message> message)
        {
            //    var U_session = (from a in _context.Users where a.Id == message.UserId select new { id = a.Id }).ToList();
            //    if (U_session.Count() <= 0) { return Unauthorized(); }

            try
            {
                foreach (Message mess in message)
                {
                    mess.Time = DateTime.Now;
                    mess.Status = false;
                    if (mess.ConversationId == 0)
                    {
                        //mess.Conversation = new Conversation();
                        mess.Conversation.Status = mess.Status;
                        //mess.Conversation.UserId = mess.UserId;
                        mess.Conversation.Date = mess.Time;

                         //_context.Conversations.Add(mess.Conversation);
                        //mess.ConversationId = mess.Conversation.Id;
                        //_context.SaveChanges();
                        //_context.Add(mess.Conversation);

                    } 


                }
                _context.AddRange(message);
                _context.SaveChanges();
                foreach (Message sendMessage in message)
                {
                    var conversation = _context.Conversations.FirstOrDefault(r => r.Id == sendMessage.ConversationId);
                    RealTimeMessageResponse rtmessage = new RealTimeMessageResponse(sendMessage.ConversationId, conversation.UserId, conversation.UserIdReciver);
                    _chatHubContext.Clients.All.SendAsync("Send", rtmessage);   // Send is the name thaw we're going to use in the client to bind the event   
                }
            }

            catch (Exception e)
            {
                Ok(new { result = "Error", detalle = "Error", item = 0 });
            }


            return Ok(new { result = "Success", detalle = "Consulta realizada con éxito", item = 0 });
        }

    }
}
