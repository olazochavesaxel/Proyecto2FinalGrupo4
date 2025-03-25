using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace CoreApp
{
    public class NotificationManager

    {
       

        public static void EnviarNotificacion(Usuario user, string subject, string message)
        {
            try
            {/*
                string smtpSender = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "miEmail@gmail.com";
                string senderPassword = "miContrasenna";

                var mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(user.Correo, user.Nombre));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = false;

                using (var smtpClient = new SmtpClient(smtpSender, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                }*/
            }
            
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo: " + ex.Message);
            }
        }
    



     
        //enviar correo smtp gmail
        public static void SendEmail(Usuario user, string subject, string message)
        {
            try
            {
                string smtpSender = "smtp@gamil.com";
                int smptpPort = 597;
                string senderEmail = "miEmail@gmail.com";
                string senderPasswrod = ",iContrasenna";

                var mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(user.Correo, user.Nombre));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = false;
                using (var smtpClient = new SmtpClient(smtpSender, smptpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPasswrod);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
