namespace MyNetCore.Services
{
    [ServiceLifetime()]
    public class TestService : ITestSesrvice
    {
        public int Sum(int x, int y)
        {
            return x + y;
        }
    }
}