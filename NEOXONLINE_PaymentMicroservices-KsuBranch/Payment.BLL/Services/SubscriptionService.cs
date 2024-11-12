using Payment.BLL.Contracts.ECommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.BLL.Services
{
    internal class SubscriptionService:ISubscriptionService
    {
        public void GetInfoAboutUsersSubscription(string username)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string username, bool isEmailNotificationRequired, bool isWebNotificationRequired)
        {
            if(isEmailNotificationRequired)
            {
                NotifyByEmail(username);
            }
            if (isWebNotificationRequired)
            {
                NotifyByWeb(username);
            }
        }

        public void NotifyByEmail(string username) 
        { 
            //todo
            //get user by username, send notification to user's email
            throw new NotImplementedException(); 
        }

        public void NotifyByWeb(string username)
        {
            //todo
            //get user by username, send notification to user's account
            throw new NotImplementedException();
        }
    }
}
