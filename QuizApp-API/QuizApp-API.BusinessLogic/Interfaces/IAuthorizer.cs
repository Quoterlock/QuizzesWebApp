namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IAuthorizer
    {
        Task<string> Authorize(string email, string password);
    }
}
