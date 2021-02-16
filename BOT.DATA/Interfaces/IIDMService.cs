namespace BOT.DATA.Interfaces
{
    public interface IIDMService
    {
        bool ResetPasswordAD(string username, string newPassword);
    }
}
