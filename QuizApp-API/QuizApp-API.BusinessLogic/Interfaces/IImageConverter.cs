namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IImageConverter
    {
        byte[] ResizeImage(byte[] bytes, int width, int height);
    }
}
