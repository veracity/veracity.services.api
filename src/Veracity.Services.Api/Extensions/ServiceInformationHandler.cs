﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Stardust.Interstellar.Rest.Annotations;
using Stardust.Interstellar.Rest.Extensions;
using Stardust.Particles;
using Veracity.Common.Authentication;

namespace Veracity.Services.Api.Extensions
{
    public class ServiceInformationHandler : IHeaderHandler
    {
        private readonly IServiceProvider _provider;

        public ServiceInformationHandler(IServiceProvider provider)
        {
            _provider = provider;
        }
        public void SetHeader(HttpRequestMessage req)
        {
            req.Headers.UserAgent.Add(new ProductInfoHeaderValue("veracity.services","sdk5.0"));// = "veracity.services;sdk3.1";
        }

        public void GetHeader(HttpResponseMessage response)
        {

        }

        public void GetServiceHeader(HttpRequestHeaders headers)
        {
            Logging.DebugMessage($"Message received from {headers.UserAgent}");
        }

        public void GetServiceHeader(IDictionary<string, StringValues> headers)
        {
            Logging.DebugMessage($"Message received from {headers["user-agent"]}");
        }

        public void SetServiceHeaders(HttpResponseHeaders headers)
        {
            try
            {
                headers.Add("x-serviceVersion", ServiceVersion);
                headers.Add("x-uptime", (DateTime.UtcNow - StartTime).ToString("g"));
                headers.Add("x-region", Environment.GetEnvironmentVariable("REGION_NAME"));
                var user = _provider.GetService<IUserNameResolver>()?.GetCurrentUserName();
                if (!string.IsNullOrWhiteSpace(user))
                    headers.Add("x-principal", user);
                var actor = _provider.GetService<IUserNameResolver>()?.GetActorId();
                if (!string.IsNullOrWhiteSpace(actor))
                    headers.Add("x-actor", actor);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        public void SetServiceHeaders(IDictionary<string, StringValues> headers)
        {
            headers.Add("x-serviceVersion", ServiceVersion);
            headers.Add("x-uptime", (DateTime.UtcNow - StartTime).ToString("g"));
            headers.Add("x-region", Environment.GetEnvironmentVariable("REGION_NAME"));
            var user = _provider.GetService<IUserNameResolver>()?.GetCurrentUserName();
            if (!string.IsNullOrWhiteSpace(user))
                headers.Add("x-principal", user);
            var actor = _provider.GetService<IUserNameResolver>()?.GetActorId();
            if (!string.IsNullOrWhiteSpace(actor))
                headers.Add("x-actor", actor);
        }

        public static DateTime StartTime { get; } = DateTime.UtcNow;

        public string ServiceVersion { get; } = ConfigurationManagerHelper.GetValueOnKey("Version");
        public int ProcessingOrder => 10;
    }
}