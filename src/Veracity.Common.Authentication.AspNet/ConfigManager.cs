﻿using System.Collections.Specialized;
using System.Configuration;
using Stardust.Particles;

namespace Veracity.Common.Authentication.AspNet
{
    public class ConfigManager : IConfigurationReader
    {
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;
    }
}