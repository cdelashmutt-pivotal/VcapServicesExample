using Microsoft.AspNetCore.Hosting;
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

            //EnvVar load defaults from JSON
            var envvars = new ConfigurationBuilder()
                .AddEnvironmentVariables().Build();
            var envvarDefaults = new ConfigurationBuilder()
                .AddJsonFile("envvars.json", optional: true, reloadOnChange: false)
                .Build();

            foreach( var envvarDefault in envvarDefaults.GetChildren())
            {
                if (envvars[envvarDefault.Key] == null)
                {
                    System.Environment.SetEnvironmentVariable(envvarDefault.Key, envvarDefault.Value);
                }
            }

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
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