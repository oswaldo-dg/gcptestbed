using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace GoogleSecretManagerConfigurationProvider
{
    public class SecretManagerConfigurationProvider : ConfigurationProvider
    {
        private readonly SecretManagerServiceClient _client;
        private readonly string _projectId;
        private GoogleConfiguration gc = new GoogleConfiguration();
        public SecretManagerConfigurationProvider()
        {
            var remoteConfigurationOptions = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            remoteConfigurationOptions.Bind("GoogleConfiguration", gc);

            System.Console.WriteLine($"-->Starting {gc.JsonCredentialsPath}");

            System.Console.WriteLine($"-->{(System.IO.File.ReadAllText(gc.JsonCredentialsPath))}");

            try {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", gc.JsonCredentialsPath);
                _client = SecretManagerServiceClient.Create();
                _projectId = GoogleProject.GetProjectId();
                if(string.IsNullOrEmpty(_projectId)) _projectId = gc.ProjectId;
            } catch (Exception ex) {
                Console.WriteLine($"{ex}");
            }
        }

        public SecretManagerConfigurationProvider(SecretManagerServiceClient client, string projectId)
        {
            _client = client;
            _projectId = projectId;
        }

        /// <summary>
        /// Load Secrets from Google Secret Manager
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "N/A")]
        public override void Load()
        {
            try
            {
                System.Console.WriteLine($"-->Loading");


            System.Console.WriteLine($"-->{_projectId}");
            if(string.IsNullOrEmpty(_projectId))
            {
                System.Console.WriteLine($"-->Outa");
                return; // skip for local debug
            }
                System.Console.WriteLine($"-->B");
                var secrets = _client.ListSecrets(new ProjectName(_projectId));
                System.Console.WriteLine($"-->{(secrets == null)}");
                System.Console.WriteLine($"-->{(secrets.ToList().Count())}");

                foreach (var secret in secrets)
            {
                try
                {
                    System.Console.WriteLine($"-->{secret.SecretName}");

                    var secretVersionName = new SecretVersionName(secret.SecretName.ProjectId, secret.SecretName.SecretId, "latest");
                    var secretVersion = _client.AccessSecretVersion(secretVersionName);
                    Set(NormalizeDelimiter(secret.SecretName.SecretId), secretVersion.Payload.Data.ToStringUtf8());
                }
                catch (Grpc.Core.RpcException gex)
                {
                    Console.WriteLine($">>> {gex}");
                    // Ignore. This might happen if secret is created but it has no versions available
                }
            }

            }
            catch (Exception ex)
            {

                Console.WriteLine($">>> {ex}");
            }
        }

        private static string NormalizeDelimiter(string key)
        {
            return key.Replace("__", ConfigurationPath.KeyDelimiter);
        }
    }
}
