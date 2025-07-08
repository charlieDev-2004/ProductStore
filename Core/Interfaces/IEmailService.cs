using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Email;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
    }
}