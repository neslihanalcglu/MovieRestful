using MovieRestful.Core.Services;
using MovieRestful.Repository;

namespace MovieRestful.API.BackgroundServices
{
    public class MovieBGService : IHostedService, IDisposable
    {
        private readonly ILogger<MovieBGService> _logger;
        private readonly DatabaseContext _context=null;
        private Timer timer;

        public MovieBGService(ILogger<MovieBGService> logger)
        {
            _logger = logger;
        }
        public void Dispose()
        {
            timer = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(GetMovieList)} Service started....");


            timer = new Timer(writeDateTimeOnLog, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void writeDateTimeOnLog(object state)
        {
            Console.WriteLine($"DateTime is {DateTime.Now.ToLongTimeString()}");
        }


        private void GetMovieList(object state)
        {
            //_movieService.GetAllAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            Console.WriteLine($"{nameof(GetMovieList)} Service stopped....");

            return Task.CompletedTask;
        }
    }
}
