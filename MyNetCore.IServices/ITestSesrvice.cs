namespace MyNetCore.IServices
{
    public interface ITestSesrvice : IBatchDIServicesTag
    {
        int Sum(int x, int y);
    }
}