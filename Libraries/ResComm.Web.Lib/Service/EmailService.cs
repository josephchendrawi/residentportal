using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace ResComm.Web.Lib.Service
{
    public class EmailService
    {
        private const string fromEmail = "mailer@residentportal.my";
        private const string fromName = "Resident Portal";
        private const string fromPassword = "mailerservice";
        private const string smtpAddress = "mail.residentportal.my";
        private const int portNumber = 2525;
        private const bool enableSSL = false;

        public static void SendEmail(string toEmail, string Subject, string BodyHTML)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(toEmail);
            mail.Subject = Subject;
            mail.Body = BodyHTML;
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword);
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }
        }

        public static void SendInvitationEmail(string toEmail, string token, string PropertyName, string Title, string Name)
        {
            var ActivationLink = "http://property.residentportal.my/Home/Authentication?token=" + token;
            string subject = "Important - Registration for " + PropertyName + " Website";
            string body = GenerateInvitationBody(ActivationLink, PropertyName, Title, Name);

            SendEmail(toEmail, subject, body);
        }

        public static void SendMemberCreatedEmail(string toEmail, string PropertyName, string Title, string Name)
        {
            var WebsiteLink = "http://property.residentportal.my/";
            string subject = "Thank you for registering with " + PropertyName + " website.";
            string body = GenerateMemberCreatedEmailBody(WebsiteLink, PropertyName, Title, Name);

            SendEmail(toEmail, subject, body);
        }

        public static void SendAffiliateCreatedEmail(string toEmail, string Title, string Name)
        {
            var WebsiteLink = "http://www.residentportal.my/affiliate";
            string subject = "Welcome to Resident Portal Affiliate Programme.";
            string body = GenerateAffiliateCreatedEmailBody(WebsiteLink, Title, Name);

            SendEmail(toEmail, subject, body);
        }

        public static void SendBetaThankYou(string toEmail)
        {
            string subject = "Resident Portal Beta Programme Registration - Thank You";
            string body = GenerateBetaThankYouBody();

            SendEmail(toEmail, subject, body);
        }

        private static string GenerateInvitationBody(string ActivationLink, string PropertyName, string Title, string Name)
        {
            return "<!DOCTYPE html>" +
                    "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">" +
                    "    <meta name=\"viewport\" content=\"width=device-width\">" +
                    "    " +
                    "    <title>Simple Transactional Email</title>" +
                    "    <style>" +
                    "      /* -------------------------------------" +
                    "          GLOBAL RESETS" +
                    "      ------------------------------------- */" +
                    "      img {" +
                    "        border: none;" +
                    "        -ms-interpolation-mode: bicubic;" +
                    "        max-width: 100%; }" +
                    "" +
                    "      body {" +
                    "        background-color: #f6f6f6;" +
                    "        font-family: sans-serif;" +
                    "        -webkit-font-smoothing: antialiased;" +
                    "        font-size: 14px;" +
                    "        line-height: 1.4;" +
                    "        margin: 0;" +
                    "        padding: 0; " +
                    "        -ms-text-size-adjust: 100%;" +
                    "        -webkit-text-size-adjust: 100%; }" +
                    "" +
                    "      table {" +
                    "        border-collapse: separate;" +
                    "        mso-table-lspace: 0pt;" +
                    "        mso-table-rspace: 0pt;" +
                    "        width: 100%; }" +
                    "        table td {" +
                    "          font-family: sans-serif;" +
                    "          font-size: 14px;" +
                    "          vertical-align: top; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          BODY & CONTAINER" +
                    "      ------------------------------------- */" +
                    "" +
                    "      .body {" +
                    "        background-color: #f6f6f6;" +
                    "        width: 100%; }" +
                    "" +
                    "      /* Set a max-width, and make it display as block so it will automatically stretch to that width, but will also shrink down on a phone or something */" +
                    "      .container {" +
                    "        display: block;" +
                    "        Margin: 0 auto !important;" +
                    "        /* makes it centered */" +
                    "        max-width: 580px;" +
                    "        padding: 10px;" +
                    "        width: auto !important;" +
                    "        width: 580px; }" +
                    "" +
                    "      /* This should also be a block element, so that it will fill 100% of the .container */" +
                    "      .content {" +
                    "        box-sizing: border-box;" +
                    "        display: block;" +
                    "        Margin: 0 auto;" +
                    "        max-width: 580px;" +
                    "        padding: 10px; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          HEADER, FOOTER, MAIN" +
                    "      ------------------------------------- */" +
                    "      .main {" +
                    "        background: #fff;" +
                    "        border-radius: 3px;" +
                    "        width: 100%; }" +
                    "" +
                    "      .wrapper {" +
                    "        box-sizing: border-box;" +
                    "        padding: 20px; }" +
                    "" +
                    "      .footer {" +
                    "        clear: both;" +
                    "        padding-top: 10px;" +
                    "        text-align: center;" +
                    "        width: 100%; }" +
                    "        .footer td," +
                    "        .footer p," +
                    "        .footer span," +
                    "        .footer a {" +
                    "          color: #999999;" +
                    "          font-size: 12px;" +
                    "          text-align: center; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          TYPOGRAPHY" +
                    "      ------------------------------------- */" +
                    "      h1," +
                    "      h2," +
                    "      h3," +
                    "      h4 {" +
                    "        color: #000000;" +
                    "        font-family: sans-serif;" +
                    "        font-weight: 400;" +
                    "        line-height: 1.4;" +
                    "        margin: 0;" +
                    "        Margin-bottom: 30px; }" +
                    "" +
                    "      h1 {" +
                    "        font-size: 35px;" +
                    "        font-weight: 300;" +
                    "        text-align: center;" +
                    "        text-transform: capitalize; }" +
                    "" +
                    "      p," +
                    "      ul," +
                    "      ol {" +
                    "        font-family: sans-serif;" +
                    "        font-size: 14px;" +
                    "        font-weight: normal;" +
                    "        margin: 0;" +
                    "        Margin-bottom: 15px; }" +
                    "        p li," +
                    "        ul li," +
                    "        ol li {" +
                    "          list-style-position: inside;" +
                    "          margin-left: 5px; }" +
                    "" +
                    "      a {" +
                    "        color: #3498db;" +
                    "        text-decoration: underline; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          BUTTONS" +
                    "      ------------------------------------- */" +
                    "      .btn {" +
                    "        box-sizing: border-box;" +
                    "        width: 100%; }" +
                    "        .btn > tbody > tr > td {" +
                    "          padding-bottom: 15px; }" +
                    "        .btn table {" +
                    "          width: auto; }" +
                    "        .btn table td {" +
                    "          background-color: #ffffff;" +
                    "          border-radius: 5px;" +
                    "          text-align: center; }" +
                    "        .btn a {" +
                    "          background-color: #ffffff;" +
                    "          border: solid 1px #3498db;" +
                    "          border-radius: 5px;" +
                    "          box-sizing: border-box;" +
                    "          color: #3498db;" +
                    "          cursor: pointer;" +
                    "          display: inline-block;" +
                    "          font-size: 14px;" +
                    "          font-weight: bold;" +
                    "          margin: 0;" +
                    "          padding: 12px 25px;" +
                    "          text-decoration: none;" +
                    "          text-transform: capitalize; }" +
                    "" +
                    "      .btn-primary table td {" +
                    "        background-color: #3498db; }" +
                    "" +
                    "      .btn-primary a {" +
                    "        background-color: #3498db;" +
                    "        border-color: #3498db;" +
                    "        color: #ffffff; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          OTHER STYLES THAT MIGHT BE USEFUL" +
                    "      ------------------------------------- */" +
                    "      .last {" +
                    "        margin-bottom: 0; }" +
                    "" +
                    "      .first {" +
                    "        margin-top: 0; }" +
                    "" +
                    "      .align-center {" +
                    "        text-align: center; }" +
                    "" +
                    "      .align-right {" +
                    "        text-align: right; }" +
                    "" +
                    "      .align-left {" +
                    "        text-align: left; }" +
                    "" +
                    "      .clear {" +
                    "        clear: both; }" +
                    "" +
                    "      .mt0 {" +
                    "        margin-top: 0; }" +
                    "" +
                    "      .mb0 {" +
                    "        margin-bottom: 0; }" +
                    "" +
                    "      .preheader {" +
                    "        color: transparent;" +
                    "        display: none;" +
                    "        height: 0;" +
                    "        max-height: 0;" +
                    "        max-width: 0;" +
                    "        opacity: 0;" +
                    "        overflow: hidden;" +
                    "        mso-hide: all;" +
                    "        visibility: hidden;" +
                    "        width: 0; }" +
                    "" +
                    "      .powered-by a {" +
                    "        text-decoration: none; }" +
                    "" +
                    "      hr {" +
                    "        border: 0;" +
                    "        border-bottom: 1px solid #f6f6f6;" +
                    "        Margin: 20px 0; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          RESPONSIVE AND MOBILE FRIENDLY STYLES" +
                    "      ------------------------------------- */" +
                    "      @media only screen and (max-width: 620px) {" +
                    "        table[class=body] h1 {" +
                    "          font-size: 28px !important;" +
                    "          margin-bottom: 10px !important; }" +
                    "        table[class=body] p," +
                    "        table[class=body] ul," +
                    "        table[class=body] ol," +
                    "        table[class=body] td," +
                    "        table[class=body] span," +
                    "        table[class=body] a {" +
                    "          font-size: 16px !important; }" +
                    "        table[class=body] .wrapper," +
                    "        table[class=body] .article {" +
                    "          padding: 10px !important; }" +
                    "        table[class=body] .content {" +
                    "          padding: 0 !important; }" +
                    "        table[class=body] .container {" +
                    "          padding: 0 !important;" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .main {" +
                    "          border-left-width: 0 !important;" +
                    "          border-radius: 0 !important;" +
                    "          border-right-width: 0 !important; }" +
                    "        table[class=body] .btn table {" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .btn a {" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .img-responsive {" +
                    "          height: auto !important;" +
                    "          max-width: 100% !important;" +
                    "          width: auto !important; }}" +
                    "" +
                    "      /* -------------------------------------" +
                    "          PRESERVE THESE STYLES IN THE HEAD" +
                    "      ------------------------------------- */" +
                    "      @media all {" +
                    "        .ExternalClass {" +
                    "          width: 100%; }" +
                    "        .ExternalClass," +
                    "        .ExternalClass p," +
                    "        .ExternalClass span," +
                    "        .ExternalClass font," +
                    "        .ExternalClass td," +
                    "        .ExternalClass div {" +
                    "          line-height: 100%; }" +
                    "        .apple-link a {" +
                    "          color: inherit !important;" +
                    "          font-family: inherit !important;" +
                    "          font-size: inherit !important;" +
                    "          font-weight: inherit !important;" +
                    "          line-height: inherit !important;" +
                    "          text-decoration: none !important; } " +
                    "        .btn-primary table td:hover {" +
                    "          background-color: #34495e !important; }" +
                    "        .btn-primary a:hover {" +
                    "          background-color: #34495e !important;" +
                    "          border-color: #34495e !important; } }" +
                    "" +
                    "    </style>" +
                    "  </head>" +
                    "  <body class=\"\">" +
                    "    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\">" +
                    "      <tbody><tr>" +
                    "        <td>&nbsp;</td>" +
                    "        <td class=\"container\">" +
                    "          <div class=\"content\">" +
                    "" +
                //"            <!-- START CENTERED WHITE CONTAINER -->" +
                //"            <span class=\"preheader\">Hello. Your Account has been successfully created. Please activate it now.</span>" +
                //"            <table class=\"main\">" +
                //"" +
                    "              <!-- START MAIN CONTENT AREA -->" +
                    "              <tbody><tr>" +
                    "                <td class=\"wrapper\">" +
                    "                  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "                    <tbody><tr>" +
                    "                      <td>" +
                    "                        <p>Dear " + Title + " " + Name + ",</p>" +
                    "                        <p>We are pleased to inform you that we have adopted a new computer system to better manage " + PropertyName + " and to facilitate communication with owners and residents alike.</p>" +
                    "                        <p>With this new system, you can:<p>" +
                    "                        <ul>" +
                    "                          <li>Get the latest announcements online.</li>" +
                    "                          <li>Make reservations for facilities.</li>" +
                    "                          <li>Make payments for your bills online.</li>" +
                    "                          <li>Submit feedbacks and inquiries.</li>" +
                    "                          <li>Retrieve the latest documents such as forms, residence rules and regulations, etc.</li>" +
                    "                          <li>Update your contact and personal information.</li>" +
                    "                          <li>And many more…</li>" +
                    "                        </ul>" +
                     "                       <p>To gain access to the website, you must first create a login and password. Please follow the link below to create your account:<p>" +
                    "                        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\">" +
                    "                          <tbody>" +
                    "                            <tr>" +
                    "                              <td align=\"left\">" +
                    "                                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "                                  <tbody>" +
                    "                                    <tr>" +
                    "                                      <td> <a href=\"" + ActivationLink + "\" target=\"_blank\">Click Here to Create My Account</a> </td>" +
                    "                                    </tr>" +
                    "                                  </tbody>" +
                    "                                </table>" +
                    "                              </td>" +
                    "                            </tr>" +
                    "                          </tbody>" +
                    "                        </table>" +
                    "                        <p>Alternatively, you can use this link. <a href=\"" + ActivationLink + "\">" + ActivationLink + "</a></p>" +
                    "                        <p>We look forward to serving you better with " + PropertyName + " new website</p>" +
                    "                        <p>Thank you.</p>" +
                    "                        <span>Best Regards,</span>" +
                    "                        <p>Staff</p>" +
                    "                        <span>Property Management Team</span>" +
                    "                        <p>" + PropertyName + "</p>" +
                    "                      </td>" +
                    "                    </tr>" +
                    "                  </tbody></table>" +
                    "                </td>" +
                    "              </tr>" +
                    "" +
                    "              <!-- END MAIN CONTENT AREA -->" +
                    "              </tbody></table>" +
                    "" +
                    "			<!-- END FOOTER -->" +
                    "            " +
                    "			<!-- END CENTERED WHITE CONTAINER --></div>" +
                    "        </td>" +
                    "        <td>&nbsp;</td>" +
                    "      </tr>" +
                    "    </tbody></table>  " +
                    "</body></html>";
        }

        private static string GenerateMemberCreatedEmailBody(string Link, string PropertyName, string Title, string Name)
        {
            return "<!DOCTYPE html>" +
                    "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">" +
                    "    <meta name=\"viewport\" content=\"width=device-width\">" +
                    "    " +
                    "    <title>Simple Transactional Email</title>" +
                    "    <style>" +
                    "      /* -------------------------------------" +
                    "          GLOBAL RESETS" +
                    "      ------------------------------------- */" +
                    "      img {" +
                    "        border: none;" +
                    "        -ms-interpolation-mode: bicubic;" +
                    "        max-width: 100%; }" +
                    "" +
                    "      body {" +
                    "        background-color: #f6f6f6;" +
                    "        font-family: sans-serif;" +
                    "        -webkit-font-smoothing: antialiased;" +
                    "        font-size: 14px;" +
                    "        line-height: 1.4;" +
                    "        margin: 0;" +
                    "        padding: 0; " +
                    "        -ms-text-size-adjust: 100%;" +
                    "        -webkit-text-size-adjust: 100%; }" +
                    "" +
                    "      table {" +
                    "        border-collapse: separate;" +
                    "        mso-table-lspace: 0pt;" +
                    "        mso-table-rspace: 0pt;" +
                    "        width: 100%; }" +
                    "        table td {" +
                    "          font-family: sans-serif;" +
                    "          font-size: 14px;" +
                    "          vertical-align: top; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          BODY & CONTAINER" +
                    "      ------------------------------------- */" +
                    "" +
                    "      .body {" +
                    "        background-color: #f6f6f6;" +
                    "        width: 100%; }" +
                    "" +
                    "      /* Set a max-width, and make it display as block so it will automatically stretch to that width, but will also shrink down on a phone or something */" +
                    "      .container {" +
                    "        display: block;" +
                    "        Margin: 0 auto !important;" +
                    "        /* makes it centered */" +
                    "        max-width: 580px;" +
                    "        padding: 10px;" +
                    "        width: auto !important;" +
                    "        width: 580px; }" +
                    "" +
                    "      /* This should also be a block element, so that it will fill 100% of the .container */" +
                    "      .content {" +
                    "        box-sizing: border-box;" +
                    "        display: block;" +
                    "        Margin: 0 auto;" +
                    "        max-width: 580px;" +
                    "        padding: 10px; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          HEADER, FOOTER, MAIN" +
                    "      ------------------------------------- */" +
                    "      .main {" +
                    "        background: #fff;" +
                    "        border-radius: 3px;" +
                    "        width: 100%; }" +
                    "" +
                    "      .wrapper {" +
                    "        box-sizing: border-box;" +
                    "        padding: 20px; }" +
                    "" +
                    "      .footer {" +
                    "        clear: both;" +
                    "        padding-top: 10px;" +
                    "        text-align: center;" +
                    "        width: 100%; }" +
                    "        .footer td," +
                    "        .footer p," +
                    "        .footer span," +
                    "        .footer a {" +
                    "          color: #999999;" +
                    "          font-size: 12px;" +
                    "          text-align: center; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          TYPOGRAPHY" +
                    "      ------------------------------------- */" +
                    "      h1," +
                    "      h2," +
                    "      h3," +
                    "      h4 {" +
                    "        color: #000000;" +
                    "        font-family: sans-serif;" +
                    "        font-weight: 400;" +
                    "        line-height: 1.4;" +
                    "        margin: 0;" +
                    "        Margin-bottom: 30px; }" +
                    "" +
                    "      h1 {" +
                    "        font-size: 35px;" +
                    "        font-weight: 300;" +
                    "        text-align: center;" +
                    "        text-transform: capitalize; }" +
                    "" +
                    "      p," +
                    "      ul," +
                    "      ol {" +
                    "        font-family: sans-serif;" +
                    "        font-size: 14px;" +
                    "        font-weight: normal;" +
                    "        margin: 0;" +
                    "        Margin-bottom: 15px; }" +
                    "        p li," +
                    "        ul li," +
                    "        ol li {" +
                    "          list-style-position: inside;" +
                    "          margin-left: 5px; }" +
                    "" +
                    "      a {" +
                    "        color: #3498db;" +
                    "        text-decoration: underline; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          BUTTONS" +
                    "      ------------------------------------- */" +
                    "      .btn {" +
                    "        box-sizing: border-box;" +
                    "        width: 100%; }" +
                    "        .btn > tbody > tr > td {" +
                    "          padding-bottom: 15px; }" +
                    "        .btn table {" +
                    "          width: auto; }" +
                    "        .btn table td {" +
                    "          background-color: #ffffff;" +
                    "          border-radius: 5px;" +
                    "          text-align: center; }" +
                    "        .btn a {" +
                    "          background-color: #ffffff;" +
                    "          border: solid 1px #3498db;" +
                    "          border-radius: 5px;" +
                    "          box-sizing: border-box;" +
                    "          color: #3498db;" +
                    "          cursor: pointer;" +
                    "          display: inline-block;" +
                    "          font-size: 14px;" +
                    "          font-weight: bold;" +
                    "          margin: 0;" +
                    "          padding: 12px 25px;" +
                    "          text-decoration: none;" +
                    "          text-transform: capitalize; }" +
                    "" +
                    "      .btn-primary table td {" +
                    "        background-color: #3498db; }" +
                    "" +
                    "      .btn-primary a {" +
                    "        background-color: #3498db;" +
                    "        border-color: #3498db;" +
                    "        color: #ffffff; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          OTHER STYLES THAT MIGHT BE USEFUL" +
                    "      ------------------------------------- */" +
                    "      .last {" +
                    "        margin-bottom: 0; }" +
                    "" +
                    "      .first {" +
                    "        margin-top: 0; }" +
                    "" +
                    "      .align-center {" +
                    "        text-align: center; }" +
                    "" +
                    "      .align-right {" +
                    "        text-align: right; }" +
                    "" +
                    "      .align-left {" +
                    "        text-align: left; }" +
                    "" +
                    "      .clear {" +
                    "        clear: both; }" +
                    "" +
                    "      .mt0 {" +
                    "        margin-top: 0; }" +
                    "" +
                    "      .mb0 {" +
                    "        margin-bottom: 0; }" +
                    "" +
                    "      .preheader {" +
                    "        color: transparent;" +
                    "        display: none;" +
                    "        height: 0;" +
                    "        max-height: 0;" +
                    "        max-width: 0;" +
                    "        opacity: 0;" +
                    "        overflow: hidden;" +
                    "        mso-hide: all;" +
                    "        visibility: hidden;" +
                    "        width: 0; }" +
                    "" +
                    "      .powered-by a {" +
                    "        text-decoration: none; }" +
                    "" +
                    "      hr {" +
                    "        border: 0;" +
                    "        border-bottom: 1px solid #f6f6f6;" +
                    "        Margin: 20px 0; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          RESPONSIVE AND MOBILE FRIENDLY STYLES" +
                    "      ------------------------------------- */" +
                    "      @media only screen and (max-width: 620px) {" +
                    "        table[class=body] h1 {" +
                    "          font-size: 28px !important;" +
                    "          margin-bottom: 10px !important; }" +
                    "        table[class=body] p," +
                    "        table[class=body] ul," +
                    "        table[class=body] ol," +
                    "        table[class=body] td," +
                    "        table[class=body] span," +
                    "        table[class=body] a {" +
                    "          font-size: 16px !important; }" +
                    "        table[class=body] .wrapper," +
                    "        table[class=body] .article {" +
                    "          padding: 10px !important; }" +
                    "        table[class=body] .content {" +
                    "          padding: 0 !important; }" +
                    "        table[class=body] .container {" +
                    "          padding: 0 !important;" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .main {" +
                    "          border-left-width: 0 !important;" +
                    "          border-radius: 0 !important;" +
                    "          border-right-width: 0 !important; }" +
                    "        table[class=body] .btn table {" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .btn a {" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .img-responsive {" +
                    "          height: auto !important;" +
                    "          max-width: 100% !important;" +
                    "          width: auto !important; }}" +
                    "" +
                    "      /* -------------------------------------" +
                    "          PRESERVE THESE STYLES IN THE HEAD" +
                    "      ------------------------------------- */" +
                    "      @media all {" +
                    "        .ExternalClass {" +
                    "          width: 100%; }" +
                    "        .ExternalClass," +
                    "        .ExternalClass p," +
                    "        .ExternalClass span," +
                    "        .ExternalClass font," +
                    "        .ExternalClass td," +
                    "        .ExternalClass div {" +
                    "          line-height: 100%; }" +
                    "        .apple-link a {" +
                    "          color: inherit !important;" +
                    "          font-family: inherit !important;" +
                    "          font-size: inherit !important;" +
                    "          font-weight: inherit !important;" +
                    "          line-height: inherit !important;" +
                    "          text-decoration: none !important; } " +
                    "        .btn-primary table td:hover {" +
                    "          background-color: #34495e !important; }" +
                    "        .btn-primary a:hover {" +
                    "          background-color: #34495e !important;" +
                    "          border-color: #34495e !important; } }" +
                    "" +
                    "    </style>" +
                    "  </head>" +
                    "  <body class=\"\">" +
                    "    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\">" +
                    "      <tbody><tr>" +
                    "        <td>&nbsp;</td>" +
                    "        <td class=\"container\">" +
                    "          <div class=\"content\">" +
                    "              <!-- START MAIN CONTENT AREA -->" +
                    "              <tbody><tr>" +
                    "                <td class=\"wrapper\">" +
                    "                  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "                    <tbody><tr>" +
                    "                      <td>" +
                    "                        <p>Dear " + Title + " " + Name + ",</p>" +
                    "                        <p>Please use the link below to access the " + PropertyName + " website:" +
                    "                        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\">" +
                    "                          <tbody>" +
                    "                            <tr>" +
                    "                              <td align=\"left\">" +
                    "                                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "                                  <tbody>" +
                    "                                    <tr>" +
                    "                                      <td> <a href=\"" + Link + "\" target=\"_blank\">Access Property Website </a> </td>" +
                    "                                    </tr>" +
                    "                                  </tbody>" +
                    "                                </table>" +
                    "                              </td>" +
                    "                            </tr>" +
                    "                          </tbody>" +
                    "                        </table>" +
                    "                        <p>or clicking this link. <a href=\"" + Link + "\">" + Link + "</a></p>" +
                    "                        <p>We look forward to serving you better with " + PropertyName + " new website.</p>" +
                    "                        <p>Thank you.</p>" +
                    "                        <span>Best Regards,</span>" +
                    "                        <p>Staff</p>" +
                    "                        <span>Property Management Team</span>" +
                    "                        <p>" + PropertyName + "</p>" +
                    "                      </td>" +
                    "                    </tr>" +
                    "                  </tbody></table>" +
                    "                </td>" +
                    "              </tr>" +
                    "" +
                    "              <!-- END MAIN CONTENT AREA -->" +
                    "              </tbody></table>" +
                    "" +
                    "			<!-- END FOOTER -->" +
                    "            " +
                    "			<!-- END CENTERED WHITE CONTAINER --></div>" +
                    "        </td>" +
                    "        <td>&nbsp;</td>" +
                    "      </tr>" +
                    "    </tbody></table>  " +
                    "</body></html>";
        }

        private static string GenerateAffiliateCreatedEmailBody(string Link, string Title, string Name)
        {
            return "<!DOCTYPE html>" +
                    "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">" +
                    "    <meta name=\"viewport\" content=\"width=device-width\">" +
                    "    " +
                    "    <title>Simple Transactional Email</title>" +
                    "    <style>" +
                    "      /* -------------------------------------" +
                    "          GLOBAL RESETS" +
                    "      ------------------------------------- */" +
                    "      img {" +
                    "        border: none;" +
                    "        -ms-interpolation-mode: bicubic;" +
                    "        max-width: 100%; }" +
                    "" +
                    "      body {" +
                    "        background-color: #f6f6f6;" +
                    "        font-family: sans-serif;" +
                    "        -webkit-font-smoothing: antialiased;" +
                    "        font-size: 14px;" +
                    "        line-height: 1.4;" +
                    "        margin: 0;" +
                    "        padding: 0; " +
                    "        -ms-text-size-adjust: 100%;" +
                    "        -webkit-text-size-adjust: 100%; }" +
                    "" +
                    "      table {" +
                    "        border-collapse: separate;" +
                    "        mso-table-lspace: 0pt;" +
                    "        mso-table-rspace: 0pt;" +
                    "        width: 100%; }" +
                    "        table td {" +
                    "          font-family: sans-serif;" +
                    "          font-size: 14px;" +
                    "          vertical-align: top; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          BODY & CONTAINER" +
                    "      ------------------------------------- */" +
                    "" +
                    "      .body {" +
                    "        background-color: #f6f6f6;" +
                    "        width: 100%; }" +
                    "" +
                    "      /* Set a max-width, and make it display as block so it will automatically stretch to that width, but will also shrink down on a phone or something */" +
                    "      .container {" +
                    "        display: block;" +
                    "        Margin: 0 auto !important;" +
                    "        /* makes it centered */" +
                    "        max-width: 580px;" +
                    "        padding: 10px;" +
                    "        width: auto !important;" +
                    "        width: 580px; }" +
                    "" +
                    "      /* This should also be a block element, so that it will fill 100% of the .container */" +
                    "      .content {" +
                    "        box-sizing: border-box;" +
                    "        display: block;" +
                    "        Margin: 0 auto;" +
                    "        max-width: 580px;" +
                    "        padding: 10px; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          HEADER, FOOTER, MAIN" +
                    "      ------------------------------------- */" +
                    "      .main {" +
                    "        background: #fff;" +
                    "        border-radius: 3px;" +
                    "        width: 100%; }" +
                    "" +
                    "      .wrapper {" +
                    "        box-sizing: border-box;" +
                    "        padding: 20px; }" +
                    "" +
                    "      .footer {" +
                    "        clear: both;" +
                    "        padding-top: 10px;" +
                    "        text-align: center;" +
                    "        width: 100%; }" +
                    "        .footer td," +
                    "        .footer p," +
                    "        .footer span," +
                    "        .footer a {" +
                    "          color: #999999;" +
                    "          font-size: 12px;" +
                    "          text-align: center; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          TYPOGRAPHY" +
                    "      ------------------------------------- */" +
                    "      h1," +
                    "      h2," +
                    "      h3," +
                    "      h4 {" +
                    "        color: #000000;" +
                    "        font-family: sans-serif;" +
                    "        font-weight: 400;" +
                    "        line-height: 1.4;" +
                    "        margin: 0;" +
                    "        Margin-bottom: 30px; }" +
                    "" +
                    "      h1 {" +
                    "        font-size: 35px;" +
                    "        font-weight: 300;" +
                    "        text-align: center;" +
                    "        text-transform: capitalize; }" +
                    "" +
                    "      p," +
                    "      ul," +
                    "      ol {" +
                    "        font-family: sans-serif;" +
                    "        font-size: 14px;" +
                    "        font-weight: normal;" +
                    "        margin: 0;" +
                    "        Margin-bottom: 15px; }" +
                    "        p li," +
                    "        ul li," +
                    "        ol li {" +
                    "          list-style-position: inside;" +
                    "          margin-left: 5px; }" +
                    "" +
                    "      a {" +
                    "        color: #3498db;" +
                    "        text-decoration: underline; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          BUTTONS" +
                    "      ------------------------------------- */" +
                    "      .btn {" +
                    "        box-sizing: border-box;" +
                    "        width: 100%; }" +
                    "        .btn > tbody > tr > td {" +
                    "          padding-bottom: 15px; }" +
                    "        .btn table {" +
                    "          width: auto; }" +
                    "        .btn table td {" +
                    "          background-color: #ffffff;" +
                    "          border-radius: 5px;" +
                    "          text-align: center; }" +
                    "        .btn a {" +
                    "          background-color: #ffffff;" +
                    "          border: solid 1px #3498db;" +
                    "          border-radius: 5px;" +
                    "          box-sizing: border-box;" +
                    "          color: #3498db;" +
                    "          cursor: pointer;" +
                    "          display: inline-block;" +
                    "          font-size: 14px;" +
                    "          font-weight: bold;" +
                    "          margin: 0;" +
                    "          padding: 12px 25px;" +
                    "          text-decoration: none;" +
                    "          text-transform: capitalize; }" +
                    "" +
                    "      .btn-primary table td {" +
                    "        background-color: #3498db; }" +
                    "" +
                    "      .btn-primary a {" +
                    "        background-color: #3498db;" +
                    "        border-color: #3498db;" +
                    "        color: #ffffff; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          OTHER STYLES THAT MIGHT BE USEFUL" +
                    "      ------------------------------------- */" +
                    "      .last {" +
                    "        margin-bottom: 0; }" +
                    "" +
                    "      .first {" +
                    "        margin-top: 0; }" +
                    "" +
                    "      .align-center {" +
                    "        text-align: center; }" +
                    "" +
                    "      .align-right {" +
                    "        text-align: right; }" +
                    "" +
                    "      .align-left {" +
                    "        text-align: left; }" +
                    "" +
                    "      .clear {" +
                    "        clear: both; }" +
                    "" +
                    "      .mt0 {" +
                    "        margin-top: 0; }" +
                    "" +
                    "      .mb0 {" +
                    "        margin-bottom: 0; }" +
                    "" +
                    "      .preheader {" +
                    "        color: transparent;" +
                    "        display: none;" +
                    "        height: 0;" +
                    "        max-height: 0;" +
                    "        max-width: 0;" +
                    "        opacity: 0;" +
                    "        overflow: hidden;" +
                    "        mso-hide: all;" +
                    "        visibility: hidden;" +
                    "        width: 0; }" +
                    "" +
                    "      .powered-by a {" +
                    "        text-decoration: none; }" +
                    "" +
                    "      hr {" +
                    "        border: 0;" +
                    "        border-bottom: 1px solid #f6f6f6;" +
                    "        Margin: 20px 0; }" +
                    "" +
                    "      /* -------------------------------------" +
                    "          RESPONSIVE AND MOBILE FRIENDLY STYLES" +
                    "      ------------------------------------- */" +
                    "      @media only screen and (max-width: 620px) {" +
                    "        table[class=body] h1 {" +
                    "          font-size: 28px !important;" +
                    "          margin-bottom: 10px !important; }" +
                    "        table[class=body] p," +
                    "        table[class=body] ul," +
                    "        table[class=body] ol," +
                    "        table[class=body] td," +
                    "        table[class=body] span," +
                    "        table[class=body] a {" +
                    "          font-size: 16px !important; }" +
                    "        table[class=body] .wrapper," +
                    "        table[class=body] .article {" +
                    "          padding: 10px !important; }" +
                    "        table[class=body] .content {" +
                    "          padding: 0 !important; }" +
                    "        table[class=body] .container {" +
                    "          padding: 0 !important;" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .main {" +
                    "          border-left-width: 0 !important;" +
                    "          border-radius: 0 !important;" +
                    "          border-right-width: 0 !important; }" +
                    "        table[class=body] .btn table {" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .btn a {" +
                    "          width: 100% !important; }" +
                    "        table[class=body] .img-responsive {" +
                    "          height: auto !important;" +
                    "          max-width: 100% !important;" +
                    "          width: auto !important; }}" +
                    "" +
                    "      /* -------------------------------------" +
                    "          PRESERVE THESE STYLES IN THE HEAD" +
                    "      ------------------------------------- */" +
                    "      @media all {" +
                    "        .ExternalClass {" +
                    "          width: 100%; }" +
                    "        .ExternalClass," +
                    "        .ExternalClass p," +
                    "        .ExternalClass span," +
                    "        .ExternalClass font," +
                    "        .ExternalClass td," +
                    "        .ExternalClass div {" +
                    "          line-height: 100%; }" +
                    "        .apple-link a {" +
                    "          color: inherit !important;" +
                    "          font-family: inherit !important;" +
                    "          font-size: inherit !important;" +
                    "          font-weight: inherit !important;" +
                    "          line-height: inherit !important;" +
                    "          text-decoration: none !important; } " +
                    "        .btn-primary table td:hover {" +
                    "          background-color: #34495e !important; }" +
                    "        .btn-primary a:hover {" +
                    "          background-color: #34495e !important;" +
                    "          border-color: #34495e !important; } }" +
                    "" +
                    "    </style>" +
                    "  </head>" +
                    "  <body class=\"\">" +
                    "    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\">" +
                    "      <tbody><tr>" +
                    "        <td>&nbsp;</td>" +
                    "        <td class=\"container\">" +
                    "          <div class=\"content\">" +
                    "              <!-- START MAIN CONTENT AREA -->" +
                    "              <tbody><tr>" +
                    "                <td class=\"wrapper\">" +
                    "                  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "                    <tbody><tr>" +
                    "                      <td>" +
                    "                        <p>Dear " + Title + " " + Name + ",</p>" +
                    "                        <p>Welcome to Resident Portal Affiliate Programme. " +
                    "                        <p>We are delighted to have you on-board. We can’t wait to help you register your first property and start earning your commissions. To get you started, please login to the Resident Portal Affiliate portal to access your Quick Start Guide." +
                    "                        <p>You can follow the link below to login to Resident Portal Affiliate portal:" +
                    "                        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\">" +
                    "                          <tbody>" +
                    "                            <tr>" +
                    "                              <td align=\"left\">" +
                    "                                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "                                  <tbody>" +
                    "                                    <tr>" +
                    "                                      <td> <a href=\"" + Link + "\" target=\"_blank\">Access Affiliate Website </a> </td>" +
                    "                                    </tr>" +
                    "                                  </tbody>" +
                    "                                </table>" +
                    "                              </td>" +
                    "                            </tr>" +
                    "                          </tbody>" +
                    "                        </table>" +
                    "                        <p>or clicking this link. <a href=\"" + Link + "\">" + Link + "</a></p>" +
                    "                        <p>Should you require any assistance or have any inquiries on the affiliate programme, please don’t hesitate to email us at affiliate@residentportal.my.</p>" +
                    "                        <p>Thank you.</p>" +
                    "                        <div>Best Regards,</div>" +
                    "                        <div>Jim Liong</div>" +
                    "                        <div>Resident Portal Customer Service Team</div>" +
                    "                      </td>" +
                    "                    </tr>" +
                    "                  </tbody></table>" +
                    "                </td>" +
                    "              </tr>" +
                    "" +
                    "              <!-- END MAIN CONTENT AREA -->" +
                    "              </tbody></table>" +
                    "" +
                    "			<!-- END FOOTER -->" +
                    "            " +
                    "			<!-- END CENTERED WHITE CONTAINER --></div>" +
                    "        </td>" +
                    "        <td>&nbsp;</td>" +
                    "      </tr>" +
                    "    </tbody></table>  " +
                    "</body></html>";
        }

        public static string GenerateBetaThankYouBody()
        {
            return "<!DOCTYPE html>" +
                    "<html>" +
                    "<head>" +
                    "    <title>Resident Portal Beta Programme Registration - Thank You</title>" +
                    "    <meta charset=\"utf-8\" />" +
                    "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
                    "    <meta name=\"robots\" content=\"noindex,nofollow\">" +
                    "" +
                    "    <!-- FONTS -->" +
                    "    <link href=\"https://fonts.googleapis.com/css?family=Archivo+Narrow:400,700\" rel=\"stylesheet\"> " +
                    "    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>" +
                    "" +
                    "    <!-- CSS -->" +
                    "    <link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css\" integrity=\"sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u\" crossorigin=\"anonymous\">" +
                    "    <style type=\"text/css\"> " +
                    "        #mainsection { text-align:center; }" +
                    "        #mainsection h1 { font-family:'Archivo Narrow',sans-serif; font-size:32px; font-weight:700; color:black; text-align:center; padding:0px; margin-top:0px; }" +
                    "        #mainsection p { font-family:'Open Sans',sans-serif; font-size:18px; font-weight:400; color:black; text-align:center; }" +
                    "        #mainsection .importantnotice { padding:20px; background-color:#efefef; }" +
                    "        #mainsection .importantnotice .alert { color:#af2121; font-size:20px; padding:0px 5px 0px 0px; font-weight:700; margin-bottom:5px; }" +
                    "        #mainsection .importantnotice p { font-family:'Open Sans',sans-serif; font-size:16px;  }" +
                    "        #mainsection .paymentdetailbox { font-family:'Open Sans',sans-serif; text-align:left; font-size:16px; }" +
                    "        #mainsection .paymentdetailbox .title { font-weight:bold; text-transform:uppercase; padding:5px 0px; }" +
                    "        #mainsection .bankinfotable { font-size:15px; text-align:left; margin:auto; }" +
                    "    </style>" +
                    "</head>" +
                    "<body style=\"padding-top:20px;\">" +
                    "    <div id=\"mainsection\" class=\"container\">" +
                    "        <div class=\"row\">" +
                    "            <div class=\"col-xs-12 col-sm-10 col-sm-offset-1 col-md-8 col-md-offset-2\">" +
                    "                <img src=\"https://www.residentportal.my/images/logo/Logo%20-%20Vertical%20(Small).png\"  width=\"220\" />" +
                    "                <h1>Beta Programme Registration Successful</h1>" +
                    "                <p>" +
                    "                    <br />" +
                    "                    Thank you for registering and supporting our Resident Portal Beta Programme. " +
                    "                    <br /><br />" +
                    "                </p>" +
                    "                <div class=\"importantnotice\"> " +
                    "                    <div class=\"alert\">IMPORTANT NOTICE</div> " +
                    "                    <p>" +
                    "                        Please make the payment <b>soonest</b> to guarantee your spot. <b>Only the first 30 registrants</b> that successfully made a payment to us will secure a spot in our beta programme. We will send an email update to inform you once these limited 30 spots are filled up." +
                    "                    </p>" +
                    "                </div>" +
                    "                <br /><br />" +
                    "                <div class=\"row\">" +
                    "                    <div class=\"col-sm-5\">" +
                    "                        <img src=\"https://www.residentportal.my//images/packages/beta-lifetime.png\" class=\"img-responsive\" />" +
                    "                    </div>" +
                    "                    <div class=\"col-sm-7 paymentdetailbox\">" +
                    "                        <br class=\"visible-xs\"/>" +
                    "                        <div class=\"title\">HOW TO MAKE PAYMENT:</div> " +
                    "                        Payment can be made either via cheque made payable to Business Origin Sdn. Bhd. or a direct deposit into the following bank account:" +
                    "                        <div style=\"padding:10px 10px 15px 10px;\">" +
                    "                            <table class=\"bankinfotable\">" +
                    "                                <tr>" +
                    "                                    <td>Bank</td>" +
                    "                                    <td>:&nbsp;&nbsp;Hong Leong Bank</td>" +
                    "                                </tr>" +
                    "                                <tr>" +
                    "                                    <td>Name</td>" +
                    "                                    <td>:&nbsp;&nbsp;Business Origin Sdn. Bhd.</td>" +
                    "                                </tr>" +
                    "                                <tr>" +
                    "                                    <td>Account No.</td>" +
                    "                                    <td>:&nbsp;&nbsp;20900033926</td>" +
                    "                                </tr>" +
                    "                            </table>" +
                    "                        </div>" +
                    "                        Once you have made the payment, please send us the payment confirmation slip along with your sign-up email and property name to <a href=\"mailto:payment@residentportal.my\">payment@residentportal.my</a> and we will proceed to activate your account." +
                    "                    </div>" +
                    "                </div>" +
                    "                <br />" +
                    "                <hr />" +
                    "                <i>Do note that a copy of this payment instruction has also been sent to your email. Should you have any enquiries on payment, please email us at payment@residentportal.my or if you wish to speak to someone, call us at +603-64110828. We will be more than glad to help.</i>" +
                    "                <p style=\"display:none;\">" +
                    "                    We will be launching our beta system on the 15th March 2017." +
                    "                    <br /><br />" +
                    "                    An email will be sent to you when the beta system is launched along with instructions to access the system. We look forward to helping you manage your property more effectively and efficiently soon.<span style=\"display:none;\">In the meantime, here is a checklist so that you can start gathering the information required to setup the system.</span>" +
                    "                </p>     " +
                    "                <br /><br /><br />" +
                    "            </div>" +
                    "        </div>" +
                    "    </div>" +
                    "" +
                    "    <script src=\"https://code.jquery.com/jquery-3.2.1.min.js\" integrity=\"sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=\" crossorigin=\"anonymous\"></script>" +
                    "    <script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js\" integrity=\"sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa\" crossorigin=\"anonymous\"></script>" +
                    "</body>" +
                    "</html>";
        }
    
    }
}
