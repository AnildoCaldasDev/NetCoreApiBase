using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreApiBase.Api.Hubs;
using NetCoreApiBase.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreApiBase.Api.Services
{
    public class UpdateStockPriceHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        const double ruleVariation = -50.0;

        public IServiceProvider Services { get; }
        private readonly List<string> _stocks;
        public UpdateStockPriceHostedService(IServiceProvider Services)
        {
            this.Services = Services;
            _stocks = new List<string>
              {
                  "ITSA4","TAEE11","PETR4"
              };
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancelattionToken)
        {
            _timer = new Timer(UpdatePrices, null, 0, 3000);

            return Task.CompletedTask;
        }


        private void UpdatePrices(object state)
        {
            using (var scope = Services.CreateScope())
            {
                //obtenho a instancia do IhubContext para permitir interagir com os hubs e as conexões dos grupos.
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<RealtimeBrokerHub>>();


                // Para cada sigla/ação da lista eu gero um numero aleatório entre 5 e 30, e então notifico um grupo do hub
                // dessa ação sobre o novo objeto que contem o valor.
                foreach (var stock in _stocks)
                {
                    var stockPrice = GetRandomNumber(5, 30);

                    var resultStock = ((stockPrice - 20.0) / 20.0) * 100;

                    if (resultStock < ruleVariation)
                    {
                        hubContext.Clients.Group(stock).SendAsync("AlertNotification", new { stock = stock, message = "Perda em ação ultrapassou limite da regra. Valor:" + resultStock.ToString() });

                    }
                    else
                        hubContext.Clients.Group(stock).SendAsync("ClearNotification", "");


                    hubContext.Clients.Group(stock).SendAsync("UpdatePrice", new StockPrice(stock, stockPrice));
                }
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }



        private double GetRandomNumber(double minimum, double maximum)
        {
            var random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}
