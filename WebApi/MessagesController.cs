using _1109.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _1109.Controllers
{
    public class MessagesController : ApiController
    {
        private static List<Message> messages = new List<Message>();
        private static int counter = 0;
        static MessagesController()
        {
            messages.Add(new Message { Id = 1, Sender = "Danny", Text = "Hello from Danny" });
            messages.Add(new Message { Id = 2, Sender = "Galit", Text = "How are you Danny?" });
            messages.Add(new Message { Id = 3, Sender = "Danny", Text = "I'm good."});
            messages.Add(new Message { Id = 4, Sender = "Steve", Text = "What's up?" });
            messages.Add(new Message { Id = 5, Sender = "Danny", Text = "I'm good. again" });
            counter = messages.Count;
        }

        // GET api/values
        public List<Message> Get()
        {
            return messages;
        }

        // GET api/values/5
        [HttpGet]
        public Message Get([FromUri] int id)
        {
            Message result = messages.FirstOrDefault(m => m.Id == id);
            return result;
        }


        // GET api/values/5
        [Route("api/messages/bysender/{sender}")]
        [HttpGet]
        public IEnumerable<Message> GetBySenderName([FromUri] string sender)
        {
            IEnumerable<Message> result = messages.Where(m => m.Sender.ToUpper() == sender.ToUpper());
            return result;
        }

        [HttpPost]
        // POST api/values
        public void Post([FromBody]Message message)
        {
            message.Id = ++counter;
            messages.Add(message);
        }

        [HttpPut]
        // PUT api/values/5
        public void Put(int id, [FromBody]Message message)
        {
            Message result = messages.FirstOrDefault(m => m.Id == id);
            if (result != null)
            {
                result.Sender = message.Sender;
                result.Text = message.Text;
            }
        }

        [HttpDelete]
        // DELETE api/values/5
        public void Delete(int id)
        {
            Message result = messages.FirstOrDefault(m => m.Id == id);
            if (result != null)
                messages.Remove(result);

        }

        [Route("api/messages/search")]
        // GET api/values
        public List<Message> GetByFilter(string sender = null)
        {
            List<Message> result = messages.Where(m => sender == null || m.Sender.ToUpper() == sender.ToUpper()).ToList();
            return result;
        }
    }
}
