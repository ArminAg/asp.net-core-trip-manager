using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_trip_manager.Core.Services
{
    public interface IMailService
    {
        void SendMail(string to, string from, string subject, string body);
    }
}
