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

        // GET api/messages
        public HttpResponseMessage Get()
        {
            if (messages.Count == 0)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.OK, messages);
            return msg;
        }

        // GET api/messages/5
        [HttpGet]
        public HttpResponseMessage Get([FromUri] int id)
        {
            Message result = messages.FirstOrDefault(m => m.Id == id);
            if (result == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);
            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.OK, result);
            return msg;
        }


        [Route("api/messages/bysender/{sender}")]
        [HttpGet]
        public IEnumerable<Message> GetBySenderName([FromUri] string sender)
        {
            IEnumerable<Message> result = messages.Where(m => m.Sender.ToUpper() == sender.ToUpper());
            return result;
        }

        [Route("api/messages/bytext/{text}")]
        [HttpGet]
        public IEnumerable<Message> GetBySenderText([FromUri] string text)
        {
            IEnumerable<Message> result = messages.Where(m => m.Text.ToUpper().Contains(text.ToUpper()));
            return result;
        }

        // POST could be alternative for GET in some cases
        [Route("api/messages/bytext")]
        [HttpPost]
        public IEnumerable<Message> BySenderTextPost([FromBody] SearchModel searchModel)
        {
            IEnumerable<Message> result = messages.Where(m => m.Text.ToUpper().Contains(searchModel.Text.ToUpper()));
            return result;
        }

        [Route("api/messages/bytextandsender/{text}/{sender}")]
        [HttpGet]
        public IEnumerable<Message> GetByTextAndSender([FromUri] string text, [FromUri] string sender)
        {
            IEnumerable<Message> result_text = messages.Where(m => m.Text.ToUpper().Contains(text.ToUpper()));
            IEnumerable<Message> result_sender = messages.Where(m => m.Sender.ToUpper().Contains(sender.ToUpper()));
            return result_text.Concat(result_sender);
        }


        [HttpPost]
        // POST api/messages
        public void Post([FromBody]Message message)
        {
            message.Id = ++counter;
            messages.Add(message);
        }

        [HttpPut]
        // PUT api/messages/5
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
        // DELETE api/messages/5
        public void Delete(int id)
        {
            Message result = messages.FirstOrDefault(m => m.Id == id);
            if (result != null)
                messages.Remove(result);

        }

        // query string
        // ...api/messages/search ? sender = d & text = o
        // ...api/messages/search ? text = o & sender = d 
        [Route("api/messages/search")]
        [HttpGet]
        public IEnumerable<Message> GetByFilter(string sender = "", string text = "")
        {
            if (sender == "" && text != "")
                return messages.Where(m => m.Text.ToUpper().Contains(text.ToUpper()));
            if (sender != "" && text == "")
                return messages.Where(m => m.Sender.ToUpper().Contains(sender.ToUpper())); ;
            return messages.Where(m => m.Sender.ToUpper().Contains(sender.ToUpper()) && m.Text.ToUpper().Contains(text.ToUpper()));
        }

        [Route("api/messages/biggerthanid")]
        [HttpGet]
        public IEnumerable<Message> BiggerThanId(int id = 0)
        {
            // if nothing sent return all
            // if id was sent return all messages with ID bigger than the id sent...
            return messages.Where(m => m.Id > id);
        }
    }
}
