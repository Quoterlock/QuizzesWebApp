namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IChachingService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);

        void Delete(string key);
    }
}
