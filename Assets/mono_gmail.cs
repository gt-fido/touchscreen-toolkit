using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class mono_gmail : MonoBehaviour {
	
	public static void send ()
	{
		MailMessage mail = new MailMessage();
		
		mail.From = new MailAddress("fidoalertsender@gmail.com");
		mail.To.Add("fidodogtouch@gmail.com");
		//mail.To.Add("7274208574@txt.att.net");
		//mail.To.Add("6789929532@txt.att.net");
		mail.Subject = "Touchscreen Study Alert";
		mail.Body = "An Alert has been triggered on the Touchscreen 1 device.";
		
		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential("fidoalertsender@gmail.com", "skyblitz") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback = 
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
		{ return true; };
		smtpServer.Send(mail);
        Debug.Log("Alert sent.");
		
	}
}