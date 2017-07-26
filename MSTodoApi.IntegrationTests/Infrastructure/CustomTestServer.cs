using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;

namespace MSTodoApi.IntegrationTests.Infrastructure
{
  public class CustomTestServer : IServer, IDisposable
  {
    private const string DefaultEnvironmentName = "Development";
    private const string ServerName = "TestServer";
    private readonly IWebHost _hostInstance;
    private bool _disposed;
    private IHttpApplication<HostingApplication.Context> _application;

    public Uri BaseAddress { get; set; } = new Uri("http://localhost/");

    public IWebHost Host => _hostInstance;

    IFeatureCollection IServer.Features { get; }

    public CustomTestServer(IWebHostBuilder builder)
    {
      IWebHost webHost = builder.UseServer(this).Build();
      webHost.Start();
      _hostInstance = webHost;
    }

    public HttpMessageHandler CreateHandler()
    {
      return new ClientHandler(BaseAddress == (Uri) null
        ? PathString.Empty
        : PathString.FromUriComponent(BaseAddress), _application);
    }

    public HttpClient CreateClient(ITokenStore tokenStore, AppCredentials appCredentials)
    {
      RefreshTokenHandler refreshTokenHandler = new RefreshTokenHandler
      {
        TokenStore = tokenStore,
        Credentials = appCredentials,
        InnerHandler = CreateHandler()
      };

      HttpClient httpClient = new HttpClient(refreshTokenHandler);
      Uri baseAddress = BaseAddress;
      httpClient.BaseAddress = baseAddress;
      return httpClient;
    }

    public void Dispose()
    {
      if (_disposed)
        return;
      _disposed = true;
      _hostInstance?.Dispose();
    }

    void IServer.Start<TContext>(IHttpApplication<TContext> application)
    {
      _application = new ApplicationWrapper<HostingApplication.Context>(
        (IHttpApplication<HostingApplication.Context>) application, () =>
        {
          if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
        });
    }

    private class ApplicationWrapper<TContext> : IHttpApplication<TContext>
    {
      private readonly IHttpApplication<TContext> _application;
      private readonly Action _preProcessRequestAsync;

      public ApplicationWrapper(IHttpApplication<TContext> application, Action preProcessRequestAsync)
      {
        _application = application;
        _preProcessRequestAsync = preProcessRequestAsync;
      }

      public TContext CreateContext(IFeatureCollection contextFeatures)
      {
        return _application.CreateContext(contextFeatures);
      }

      public void DisposeContext(TContext context, Exception exception)
      {
        _application.DisposeContext(context, exception);
      }

      public Task ProcessRequestAsync(TContext context)
      {
        _preProcessRequestAsync();
        return _application.ProcessRequestAsync(context);
      }
    }
  }
}