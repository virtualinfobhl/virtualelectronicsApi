using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace CityStoreAPIWeb.Models
{
    public class Msg
    {
        #region
        /*--------- start SMS -----------------------------------------------------------------------------------*/
        public static class Gateway
        {
            public static string High = "high";
            public static string Regular = "regular";
        }
        public static string CountryCode(string Mobile)
        {
            if (Mobile != "")
            {
                string MobileNo = "";
                char[] delimeter = new char[] { ',', ' ' };
                string[] mobileList = Mobile.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mobile in mobileList)
                {
                    MobileNo = MobileNo + mobile.Substring(mobile.Length - Math.Min(10, mobile.Length)) + ",";
                }
                MobileNo = MobileNo.Remove(MobileNo.LastIndexOf(','));
                return (MobileNo);
            }
            else
            {
                return ("");
            }
        }
        public static string SendSMS(string Mobile, string Message)
        {
            try
            {
                string Gateway = Msg.Gateway.Regular;
                string UserName = "Friends";
                string Password = "5114";
                string gsmSenderId = "FndMob";
                Mobile = Msg.CountryCode(Mobile);
                Message = Message.Replace("&", "%26");
                Message = Message.Replace("+", "%2B");
                //string baseurl = "";
                string baseurl = "http://kit19.com/ComposeSMS.aspx?username=" + UserName + "&password=" + Password + "&sender=" + gsmSenderId + "&to=" + Mobile + "&message=" + Message + "&priority=1&dnd=1&unicode=0";
                WebClient client = new WebClient();
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return (s);
            }
            catch (Exception Ex)
            {
                return ("");
            }
        }

        public static string SendSMS1(string Mobile, string Message)
        {
            try
            {
                string Gateway = Msg.Gateway.Regular;
                string UserName = "virtual349836";
                string Password = "5288";
                //string gsmSenderId = "CTstor";
                string gsmSenderId = "TXTSMS";
                Mobile = Msg.CountryCode(Mobile);
                Message = Message.Replace("&", "%26");
                Message = Message.Replace("+", "%2B");
                //string baseurl = "";
                string baseurl = "http://kit19.com/ComposeSMS.aspx?username=" + UserName + "&password=" + Password + "&sender=" + gsmSenderId + "&to=" + Mobile + "&message=" + Message + "&priority=1&dnd=1&unicode=0";
                WebClient client = new WebClient();
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return (s);
            }
            catch (Exception Ex)
            {
                return ("");
            }
        }
        /*--------- end SMS--------------------------------------------------------------------------------------*/
        #endregion
        #region
        /*--------- start Email -----------------------------------------------------------------------------------*/
        public static void SendEmail(string Receiver_Email, string Subject, string Message)
        {
            try
            {
                //************************************* E-mail system *********************************************************//
                System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                System.Net.Configuration.MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;
                if (mailSettings != null)
                {
                    int port = mailSettings.Smtp.Network.Port;
                    string host = mailSettings.Smtp.Network.Host;
                    string password = mailSettings.Smtp.Network.Password;
                    string username = mailSettings.Smtp.Network.UserName;
                    System.Net.Mail.MailMessage Msg = new System.Net.Mail.MailMessage();
                    Msg.From = new System.Net.Mail.MailAddress(username);
                    Msg.To.Add(Receiver_Email);
                    Msg.Subject = "Friends Mobile";
                    Msg.Subject = Subject;
                    Msg.Body = Message;
                    Msg.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = host;
                    smtp.Port = port;
                    smtp.Credentials = new System.Net.NetworkCredential(username, password);
                    smtp.Send(Msg);
                    Msg = null;
                }
                //*************************************************************************************************************//
            }
            catch (Exception Ex)
            {

            }
        }
        public static void SendEmailtoAll(string Receiver_Email, string Subject, string Message)
        {
            if (Receiver_Email != "")
            {
                char[] delimeter = new char[] { ',', ' ' };
                string[] emailList = Receiver_Email.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string email in emailList)
                {
                    SendEmail(email, Subject, Message);
                }
            }
        }
        public static void ABC()
        {

        }
        /*--------- end Email--------------------------------------------------------------------------------------*/
        #endregion
    }
}