using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Configuration;

/// <summary>
/// Summary description for mailMgr
/// </summary>
public class mailMgr : MailMessage
{
    // create properties
    public string myFrom { get; set; }
    public string myTo { get; set; }
    public string mySubject { get; set; }
    public bool myIsBodyHtml { get; set; }
    public string myBody { get; set; }
    public int myPortNumber { get; set; }
    public bool myEnableSSL { get; set; }
    public string myUserName { get; set; }
    public string myPassword { get; set; }
    public string myHostsmtpAddress { get; set; }
    public int myPort { get; set; }
    public NetworkCredential myCredentials { get; set; }
    public string myEnableSsl { get; set; }

    public mailMgr()
    {
        //............ Gmail SMTP
        //// constructor to initialize properties 
        myFrom = ConfigurationManager.AppSettings["emailFrom"];
        myTo = ConfigurationManager.AppSettings["emailTo"];
        myHostsmtpAddress = ConfigurationManager.AppSettings["HostsmtpAddress"];
        myPortNumber = int.Parse(ConfigurationManager.AppSettings["PortNumber"]);  //25;//587;
        myEnableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"]);
        myUserName = ConfigurationManager.AppSettings["emailUserName"];
        myPassword = ConfigurationManager.AppSettings["emailPassword"]; // my Email password 

        mySubject = " Notify Admin of Site Activity via gmail smtp ";
        myIsBodyHtml = true;
        myBody = @"you will pass the body from the sender. as part of the constructor value";

    }

    // use this one 
    public  string sendEmailViaGmail() // worked 100%, this is a nice one use it with  properties
    {
        //string visitorUserName = Page.User.Identity.Name;
        using (MailMessage m = new MailMessage(myFrom, myTo, mySubject, myBody)) // .............................1
        {
            SmtpClient sc = new SmtpClient(myHostsmtpAddress, myPortNumber); //..................................2
            try
            {
                 sc.Credentials = new System.Net.NetworkCredential(myUserName, myPassword);  //.................3
                sc.EnableSsl = true; 
                sc.Send(m);
                return "Email Sent successfully";
                
            }
            catch (SmtpFailedRecipientException ex)
            {
                SmtpStatusCode statusCode = ex.StatusCode;
                if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                {   // wait 5 seconds, try a second time
                    Thread.Sleep(5000);
                    sc.Send(m);
                    return ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                m.Dispose();
            }
        }// end using 
    }
    public string sendEmailViaGmail(FileUpload fuAttachment) // worked 100%, this is a nice one use it with  properties
    {
        //string visitorUserName = Page.User.Identity.Name;
        using (MailMessage m = new MailMessage(myFrom, myTo, mySubject, myBody)) // .............................1
        {
            SmtpClient sc = new SmtpClient(myHostsmtpAddress, myPortNumber); //..................................2
            try
            {
                if (fuAttachment.HasFile)
                {
                    foreach (HttpPostedFile file in fuAttachment.PostedFiles)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        //file.SaveAs(Server.MapPath("~/docEmailed/") + fileName); // causing error
                        m.Attachments.Add(new Attachment(file.InputStream, fileName));
                    }
                }
                sc.Credentials = new System.Net.NetworkCredential(myUserName, myPassword);  //.................3
                sc.EnableSsl = true;
                sc.Send(m);
                return "Email Sent successfully";
            }
            catch (SmtpFailedRecipientException ex)
            {
                SmtpStatusCode statusCode = ex.StatusCode;
                if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                {   // wait 5 seconds, try a second time
                    Thread.Sleep(5000);
                    sc.Send(m);
                    return ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                m.Dispose();
            }
        }// end using 
    }
    public string sendEmailViaGmail2(string myTo, string myFrom, string myBody) // worked 100%, this is a nice one use it with  properties
    {
        //string visitorUserName = Page.User.Identity.Name;
        using (MailMessage m = new MailMessage(myFrom, myTo, mySubject, myBody)) // .............................1
        {
            SmtpClient sc = new SmtpClient(myHostsmtpAddress, myPortNumber); //..................................2
            try
            {
                //if (myFileUpload.HasFile)
                //{
                //    foreach (HttpPostedFile file in myFileUpload.PostedFiles)
                //    {
                //        string fileName = Path.GetFileName(file.FileName);
                //        //file.SaveAs(Server.MapPath("~/docEmailed/") + fileName); 
                //        m.Attachments.Add(new Attachment(file.InputStream, fileName));
                //    }
                //}
                sc.Credentials = new System.Net.NetworkCredential(myUserName, myPassword);  //.................3
                sc.EnableSsl = true;
                sc.Send(m);
                return "Email Send successfully";
                //lblMsg.Text = ("Email Send successfully");
                //lblMsg.ForeColor = Color.Green; // using System.Drawing above 2/2018
            }
            catch (SmtpFailedRecipientException ex)
            {
                SmtpStatusCode statusCode = ex.StatusCode;
                if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                {   // wait 5 seconds, try a second time
                    Thread.Sleep(5000);
                    sc.Send(m);
                    return ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
                
            }
            finally
            {
                m.Dispose();
            }
        }// end using 
    }

    public string sendEmailWithAttachment(string filePath)
    {
        using (MailMessage m = new MailMessage(myFrom, myTo, mySubject, myBody))
        {
            SmtpClient sc = new SmtpClient(myHostsmtpAddress, myPortNumber);
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    Attachment attachment = new Attachment(filePath);
                    m.Attachments.Add(attachment);
                }
                sc.Credentials = new System.Net.NetworkCredential(myUserName, myPassword);
                sc.EnableSsl = true;
                sc.Send(m);
                return "Email Sent successfully";
            }
            catch (SmtpFailedRecipientException ex)
            {
                SmtpStatusCode statusCode = ex.StatusCode;
                if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                {
                    Thread.Sleep(5000);
                    sc.Send(m);
                    return ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                m.Dispose();
            }
        }
    }

}

