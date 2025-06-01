using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anserem.Models;

namespace Anserem.Services
{
    public interface IEmailProcessor
    {
        void Process(Email email);
    }
}
