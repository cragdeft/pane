using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility
{
    public class EmailSender
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        public static string SendEmail(string to, string from, string subject, string body)
        {
            try
            {
                string smtpAddress = "wasa-noreply@sinepulse.net ";
                string smtpPassword = "hX3uJtj9";

                string uri = "http://emailservice.azurewebsites.net/EmailService.svc/SendMail?to=" + to + "&from=" +
                             from + "&subject=" + subject + "&body=" + body + "&smtpAddress=" + smtpAddress +
                             "&smtpPassword=" + smtpPassword;

                //var httpReq = (HttpWebRequest)WebRequest.Create(uri);
                //httpReq.Method = "POST";
                //httpReq.ContentType = "application/x-www-form-urlencoded";
                //httpReq.ContentLength = 0;
                //var response = (HttpWebResponse)httpReq.GetResponse();
                //string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var result = HttpClient.GetAsync(uri).Result;
                result.EnsureSuccessStatusCode();
                string responseString = result.Content.ReadAsStringAsync().Result;

                if (responseString.Contains("Success"))
                {
                    return "Email Sent";
                }
                else
                {
                    return "Email Failed";
                }
            }
            catch (Exception ex)
            {
                return "Email Failed";
            }
        }
    }
}
