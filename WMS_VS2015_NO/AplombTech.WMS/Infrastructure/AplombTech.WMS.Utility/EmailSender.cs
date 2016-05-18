using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility
{
    public class EmailSender
    {
        public static string SendEmail(string to, string from, string subject, string body)
        {
            try
            {
                var httpReq = (HttpWebRequest)WebRequest.Create("http://emailservice.azurewebsites.net/EmailService.svc/SendEmailMessage?to=" + to + "&from=" + from + "&subject=" + subject + "&body=" + body);
                httpReq.Method = "POST";
                httpReq.ContentType = "application/x-www-form-urlencoded";
                httpReq.ContentLength = 0;

                var response = (HttpWebResponse)httpReq.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

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
