namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task Register(string username, string email, string password);
        Task<string> Authorize(string email, string password);
        Task RemoveUser(string username, string email);
    }
}
