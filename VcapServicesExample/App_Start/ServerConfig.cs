﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using PA = Microsoft.Extensions.PlatformAbstractions;

namespace VcapServicesExample
{
    public class ServerConfig
    {
        public static CloudFoundryApplicationOptions CloudFoundryApplication
        {

            get
            {
                var opts = new CloudFoundryApplicationOptions();
                ConfigurationBinder.Bind(Configuration, opts);
                return opts;
            }
        }
        public static CloudFoundryServicesOptions CloudFoundryServices
        {
            get
            {
                var opts = new CloudFoundryServicesOptions();
                ConfigurationBinder.Bind(Configuration, opts);
                return opts;
            }
        }

        public static IConfigurationRoot Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {
            var env = new HostingEnvironment(environment);

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                //This is a very important step!
                .AddCloudFoundry();

            Configuration = builder.Build();

        }
    }
    public class HostingEnvironment : IHostingEnvironment
    {
        public HostingEnvironment(string env)
        {
            EnvironmentName = env;
            ApplicationName = PA.PlatformServices.Default.Application.ApplicationName;
            ContentRootPath = PA.PlatformServices.Default.Application.ApplicationBasePath;
        }

        public string ApplicationName { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }

        public string ContentRootPath { get; set; }

        public string EnvironmentName { get; set; }

        public IFileProvider WebRootFileProvider { get; set; }

        public string WebRootPath { get; set; }

        IFileProvider IHostingEnvironment.WebRootFileProvider { get; set; }
    }

}