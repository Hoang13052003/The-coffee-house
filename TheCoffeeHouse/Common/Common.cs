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
        public static bool SendEmailAsync(string toEmail, string callbackUrl)
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
                MailAddress formAddress = new MailAddress(Email, "The Coffee House");
                message.From = formAddress;
                message.To.Add(toEmail);
                message.Subject = "Xác nhận đổi mật khẩu";
                message.IsBodyHtml = true;
                message.Body = $@"
                            <div style='font-family: Arial, sans-serif; line-height: 1.6; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                                <h2 style='color: #333; text-align: center;'>Yêu cầu đặt lại mật khẩu</h2>
                                <p>Xin chào,</p>
                                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu của bạn. Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                                <p>Để đặt lại mật khẩu của bạn, vui lòng nhấn vào nút bên dưới:</p>
                                <div style='text-align: center; margin: 20px 0;'>
                                    <a href='{callbackUrl}' style='background-color: #28a745; color: white; text-decoration: none; padding: 12px 20px; border-radius: 5px; display: inline-block; font-weight: bold;'>Đổi mật khẩu</a>
                                </div>
                                <hr>
                                <p style='font-size: 12px; color: #555;'>Nếu bạn gặp bất kỳ vấn đề nào, vui lòng liên hệ với đội ngũ hỗ trợ của chúng tôi.</p>
                            </div>";
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