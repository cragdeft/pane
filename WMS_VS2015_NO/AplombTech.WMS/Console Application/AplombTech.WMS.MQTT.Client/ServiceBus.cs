﻿using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client
{
    public static class ServiceBus
    {
        public static IBus Bus { get; private set; }

        private static readonly object PadLock = new object();

        public static void Init()
        {
            if (Bus != null)
                return;

            lock (PadLock)
            {
                if (Bus != null)
                    return;

                var cfg = new BusConfiguration();

                cfg.UseTransport<MsmqTransport>();
                cfg.UsePersistence<InMemoryPersistence>();
                cfg.EndpointName("WMSSensorDataSender");
                cfg.PurgeOnStartup(true);
                cfg.EnableInstallers();

                Bus = NServiceBus.Bus.Create(cfg).Start();
            }
        }
    }
}
