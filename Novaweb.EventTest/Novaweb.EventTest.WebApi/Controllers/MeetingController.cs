using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Novaweb.EventTest.WebApi.Controllers
{
    public class MeetingController : ApiController
    {

        public MeetingController()
        {
            
        }

        public string GetAll()
        {
            EventsStoreConfig.Initialize();
            return "Initialized";
        }
    }
}
