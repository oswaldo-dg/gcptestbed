using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSecretManagerConfigurationProvider
{
    public class GoogleConfiguration
    {
        public const string ConfigSectionName = "GoogleConfiguration";
        public string JsonCredentialsPath { get; set; }
        public string ProjectId { get; set; }
    }
}
