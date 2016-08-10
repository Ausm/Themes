﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Ausm.EmptyTheme;
using Microsoft.AspNetCore.Builder;

namespace ObjectStore.Test.Identity.Fixtures
{
    public class TestServerFixture : IDisposable
    {
        TestServer _server;
        HttpClient _client;
        RequestDelegate _function;

        public TestServerFixture()
        {
            _server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => {
                    services.AddTheme();
                    services.AddMvc();
                })
                .Configure(app => {
                    app.UseTheme();
                    app.UseMvcWithDefaultRoute();
                }));
            _client = _server.CreateClient();
        }

        public HttpClient Client => _client;

        public void Dispose()
        {
            _server.Dispose();
            _client.Dispose();
            _server = null;
            _client = null;
        }
    }
}
