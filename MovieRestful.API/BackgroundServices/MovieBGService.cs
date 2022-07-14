using MovieRestful.Core.Services;
using MovieRestful.Repository;

namespace MovieRestful.API.BackgroundServices
{
    public class MovieBGService : IHostedService, IDisposable
    {
        private Timer timer;

        public void Dispose()
        {
            timer = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(GetMovieList)} Service started....");


            //timer = new Timer(GetMovieList, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void GetMovieList(object state)
        {
            //_movieService.GetAllAsync();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //timer?.Change(Timeout.Infinite, 0);

            Console.WriteLine($"{nameof(GetMovieList)} Service stopped....");

            return Task.CompletedTask;
        }
    }
}
