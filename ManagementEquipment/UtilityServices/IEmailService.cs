using ManagementEquipment.Models;

namespace ManagementEquipment.UtilityServices
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
