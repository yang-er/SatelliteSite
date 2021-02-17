﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plag.Backend.Services
{
    public abstract class ContextNotifyService<T> : BackgroundService
    {
        private IServiceProvider ServiceProvider { get; }

        public ILogger<T> Logger { get; }

        public IResettableSignal<T> CurrentSignal { get; }

        public ContextNotifyService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Logger = serviceProvider.GetRequiredService<ILogger<T>>();
            CurrentSignal = serviceProvider.GetRequiredService<IResettableSignal<T>>();
        }

        protected abstract Task ProcessAsync(IStoreExtService context, CancellationToken stoppingToken);

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                await CurrentSignal.WaitAsync(stoppingToken);
                if (stoppingToken.IsCancellationRequested) break;

                try
                {
                    using var scope = ServiceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<IStoreExtService>();
                    await ProcessAsync(dbContext, stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error happened unexpected.");
                }
            }
        }
    }
}
