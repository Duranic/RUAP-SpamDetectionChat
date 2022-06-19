using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Spam_detection_chat
{
    public class MyHub1 : Hub
    {
        private SpamChecker spamChecker = new SpamChecker();
        public async Task Send(String name, String message)
        {

            if (await spamChecker.checkSpam(message))
            {
                Clients.All.addNewMessage(name, message);
                Clients.All.markSpam();
            }
            else
            {
                Clients.All.addNewMessage(name, message);
            }

        }
    }
}