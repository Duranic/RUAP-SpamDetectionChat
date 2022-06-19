using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Spam_detection_chat.Startup)) ]
namespace Spam_detection_chat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {
            app.MapSignalR();
        }
    }
}