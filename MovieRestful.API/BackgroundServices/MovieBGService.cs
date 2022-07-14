using MovieRestful.Core.Services;

namespace MovieRestful.API.BackgroundServices
{
    public class MovieBGService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IMovieService _movieService;

        public MovieBGService(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public void Dispose()
        {
            timer = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(GetMovieList)} Service stared....");


            timer = new Timer(GetMovieList, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void GetMovieList(object state)
        {
            _movieService.GetAllAsync();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            Console.WriteLine($"{nameof(GetMovieList)} Service stopped....");

            return Task.CompletedTask;
        }
    }
}
