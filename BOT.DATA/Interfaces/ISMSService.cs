using BOT.DATA.Models;

namespace BOT.DATA.Interfaces
{
    public interface ISMSService
    {
        bool Send(SmsModel model);
    }
}
