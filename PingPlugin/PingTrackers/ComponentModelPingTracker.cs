﻿using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace PingPlugin.PingTrackers
{
    public class ComponentModelPingTracker : PingTracker
    {
        private readonly Ping ping;

        public ComponentModelPingTracker(PingConfiguration config) : base(config)
        {
            this.ping = new Ping();
        }

        protected override async Task PingLoop(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                var pingReply = await this.ping.SendPingAsync(SeAddress);
                if (pingReply.Status == IPStatus.Success)
                    NextRTTCalculation(pingReply.RoundtripTime);
                await Task.Delay(3000, token);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ping.Dispose();
                base.Dispose(true);
            }
        }
    }
}
