using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TheCoffeeHouse.Common
{
    public class Common
    {
        private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
        private static string Email = ConfigurationManager.AppSettings["Email"];

        public static bool SendMail(string name, string subject, string content, string toMail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = Email, Password = password
                    };
                }
                MailAddress formAddress = new MailAddress(Email, name);
                message.From = formAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
            }
            catch (Exception)
            {
                rs = false;
            }
            return rs;
        }
        public static bool SendEmailAsync(string toEmail, string subject, string body)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = Email,
                        Password = password
                    };
                }
                //MailAddress formAddress = new MailAddress(Email, name);
                //message.From = formAddress;
                message.To.Add(toEmail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Send(message);
                rs = true;
            }
            catch (Exception)
            {
                rs = false;
            }
            return rs;
        }

        public static string GenerateSlug(string str)
        {
            str = str.ToLower().Trim();
            str = Regex.Replace(str, @"\s+", "-"); // Thay khoảng trắng bằng dấu "-"
            str = Regex.Replace(str, @"[^\w\-]", ""); // Loại bỏ ký tự đặc biệt
            return str;
        }
    }
}