namespace ServiceName.API.BackgroundTasks
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0
    /// https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Hosting/tests/UnitTests/BackgroundServiceTests.cs
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice
    /// </summary>
    public class RepeatingTask : BackgroundService
    {
        private ILogger _logger;
        private PeriodicTimer _timer;

        //public RepeatingTask(ILogger logger)
        public RepeatingTask()
        {
            //_logger = logger;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken)
                  && !stoppingToken.IsCancellationRequested)
            {
                await DoWork();
            }
        }

        private async Task DoWork()
        {
            //SQS
            //https://www.youtube.com/watch?v=7OfUi3h-wmM
            Console.WriteLine(DateTime.Now.ToString("O"));
        }
    }
}