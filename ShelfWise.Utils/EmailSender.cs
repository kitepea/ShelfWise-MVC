using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using PostmarkDotNet;

namespace ShelfWise.Utils
{
    public class EmailSender : IEmailSender
    {
        public string PostMarkSecret { get; set; }

        public EmailSender(IConfiguration _config)
        {
            PostMarkSecret = _config.GetValue<string>("PostMark:ServerApiToken");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new PostmarkMessage()
            {
                To = email,
                From = "quan.dauduc@hcmut.edu.vn",
                TrackOpens = true,
                Subject = subject + " -- From Shelf Wise",
                HtmlBody = htmlMessage,
            };

            var client = new PostmarkClient(PostMarkSecret);
            return client.SendMessageAsync(message);
        }
    }
}
