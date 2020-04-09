﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MySDK.Redis
{

    public class RedisConfiguration
    {

        /* 
         * configuration file eg: 
            {
                RedisConfiguration: [
                    {
                        "Name": "",
                        "ConnectTimeout": 10000,
                        "SyncTimeout": 50000,
                        "AllowAdmin": true,
                        "ConnectRetry": 4,
                        "AbortConnect": false,
                        "KeepAlive": 180,
                        "Hosts": [
                            "192.168.56.101:6001",
                            "192.168.56.102:6001",
                            "192.168.56.102:6001"
 	                    ]
                    },
                    ...
                ]
            }
        */
        public RedisConfiguration()
        {
            ConnectTimeout = 10000;
            SyncTimeout = 50000;
            AllowAdmin = true;
            ConnectRetry = 3;
            AbortConnect = false;
            KeepAlive = 180;
        }

        public string Name { get; set; }
        public int ConnectTimeout { get; set; }
        public int SyncTimeout { get; set; }
        public bool AllowAdmin { get; set; }
        public int ConnectRetry { get; set; }
        public bool AbortConnect { get; set; }
        public int KeepAlive { get; set; }

        /// <summary>
        /// "Hosts": [ "IP:Port", ...]
        /// </summary>
        public List<string> Hosts { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            if (Hosts == null || !Hosts.Any())
                throw new ArgumentNullException("Hosts");

            return $"ConnectTimeout={ConnectTimeout},SyncTimeout={SyncTimeout},AllowAdmin={AllowAdmin},ConnectRetry={ConnectRetry},AbortConnect={AbortConnect},KeepAlive={KeepAlive},{string.Join(",", Hosts)}";
        }

    }

    public static class RedisConifgurationExtend
    {

        public static List<RedisConfiguration> GetRedisConfigurations(this IConfigurationRoot root)
        {
            var configs = new List<RedisConfiguration>();
            root.GetSection("RedisConfiguration").Bind(configs);
            return configs;
        }

        public static RedisConfiguration GetRedisConfiguration(this IConfigurationRoot root, string redisServerName)
        {
            var configs = root.GetRedisConfigurations();
            if (null != configs)
                configs.Where(i => i.Name.ToLower() == redisServerName.ToLower()).FirstOrDefault();
            return null;
        }
    }
}