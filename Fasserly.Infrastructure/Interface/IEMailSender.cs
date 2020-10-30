using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Interface
{
    public interface IEMailSender
    {
        Task SendEmailAsync(string userMail, string subject, string message);
    }
}
