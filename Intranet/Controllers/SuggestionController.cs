using Intranet.Classes;
using Intranet.Models;
using Intranet.Utilities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using MimeKit;
using NToastNotify;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    public class SuggestionController : Controller
    {
        private readonly SuggestionContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly EmailContext _email;
        private readonly IToastNotification _toastNotification;
        private string SenderName, SenderEmail, SenderSubject, SenderMessage, ReceiverName, ReceiverEmail; // variables for

        public SuggestionController(SuggestionContext context, EmailContext email, IOptions<EmailOptions> EmailOptions, IToastNotification toastNotification)
        {
            _context = context;
            _emailOptions = EmailOptions.Value;
            _email = email;
            _toastNotification = toastNotification;
        }

        // GET: Suggestion
        [Authorize(Roles = "Office of the Chief Information Officer")]
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.Suggestions.ToListAsync());
        }

        // GET: Suggestion/Create
        [Authorize]
        public IActionResult Create()
        {
            UserDetails();
            return View();
        }

        // POST: Suggestion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Obsolete]
        public async Task<IActionResult> Create([Bind("SuggId,SuggName,SuggEmail,SuggSubject,SuggMessage,UserName,UserIP,UserDate")] Suggestion suggestion)
        {
            UserDetails();

            //  assining values from inputs
            SenderName = suggestion.SuggName;
            SenderSubject = suggestion.SuggSubject;
            SenderEmail = ViewBag.EmailAddress;
            SenderMessage = suggestion.SuggMessage;
            //ReceiverName = _emailOptions.SenderName;
            ReceiverEmail = _emailOptions.AuthEmailMain;

            //  calling SendEmail email function
            SendEmail(SenderName, SenderEmail, ReceiverName, SenderMessage, SenderSubject);

            var message = new MimeMessage();
            var builder = new BodyBuilder();

            // reference value from EmailOptions.json
            string host = _emailOptions.SMTPHostClient;
            int port = _emailOptions.SMTPHostPort;
            bool boole = _emailOptions.SMTPHostBool;
            string authEmail = _emailOptions.AuthEmailMain;
            string authPass = _emailOptions.AuthPasswordMain;

            message.From.Add(new MailboxAddress(SenderName, SenderEmail));

            if (ModelState.IsValid)
            {
                _context.Add(suggestion);
                addToast();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }

            return View(suggestion);
        }

        #region UserDetails function

        /**
         *  START OF FUNCTION
         *  user.GetDepartment = getting user department from AD
         *  user.GetDisplayname = getting user full name from AD
         *  user.GetUserPrincipalName = getting email of user from AD
         */

        public void UserDetails()
        {
            var username = User.Identity.Name;
            var domain = _emailOptions.AuthDomain;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
                ViewBag.EmailAddress = user.GetUserPrincipalName();
            }
        }

        /**
         *  END OF FUNCTION
         */

        #endregion UserDetails function

        /**
        * ********************************************************************************
        * *                        START OF SENDING EMAIL                                *
        * ********************************************************************************
        *  START OF FUNCTION
        *  sending email function
        */

        #region SendEmail function

        [System.Obsolete]
        public void SendEmail(string FromName, string FromEmail, string ToName, string FromMessage, string FromTitle)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();

            // reference value from EmailOptions.json
            string host = _emailOptions.SMTPHostClient;
            int port = _emailOptions.SMTPHostPort;
            bool boole = _emailOptions.SMTPHostBool;
            string authEmail = _emailOptions.AuthEmailMain;
            string authPass = _emailOptions.AuthPasswordMain;

            message.From.Add(new MailboxAddress(FromName, FromEmail));

            var emails = _email.Emails;
            foreach (Email e in emails)
            {
                var emailadd = e.EmailAddress;

                message.To.Add(new MailboxAddress(emailadd));
                message.Subject = SenderSubject;
                builder.HtmlBody =
                    string.Format(@"
                    To <b>Management</b>,
                    <p>" + SenderMessage + "</p>" +
                        "Regards,<br  />" +
                         SenderName +
                        @"<br  /><br  />");

                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.Connect(host, port, boole);
                    client.Authenticate(authEmail, authPass);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }

        #endregion SendEmail function

        /**
        * ********************************************************************************
        * *                        END OF SENDING EMAIL                                  *
        * ********************************************************************************
        */

        private bool SuggSubjEmpty(string subj)
        {
            return _context.Suggestions.Any(e => e.SuggSubject == subj);
        }

        private bool SuggMsgEmpty(string Msg)
        {
            return _context.Suggestions.Any(e => e.SuggMessage == Msg);
        }

        #region Toastr function

        /**
       * ********************************************************************************
       * *                          START OF TOASTR                                     *
       * ********************************************************************************
       *  START OF Toastr
       */

        public void addToast()
        {
            _toastNotification.AddSuccessToastMessage("suggestion sent successfully.", new ToastrOptions()
            {
                Title = ""
            });
        }

        /**
        * ********************************************************************************
        * *                         END OF TOASTR                                        *
        * ********************************************************************************
        */

        #endregion Toastr function
    }
}