﻿using DashBe.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Services.Message
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }   

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpClient = new SmtpClient(smtpSettings["Host"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["SenderEmail"], "DashBe Support"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
