using System;
using System.Text;
#if NET_STANDARD_21
using Microsoft.Extensions.Hosting.Internal;
#endif
using NLog.Config;
using NLog.LayoutRenderers;
#if ASP_NET_CORE
using Microsoft.AspNetCore.Hosting;
#if ASP_NET_CORE1 || ASP_NET_CORE2
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#endif
using Microsoft.Extensions.DependencyInjection;
using NLog.Web.DependencyInjection;
#else
using System.Web.Hosting;
#endif

namespace NLog.Web.LayoutRenderers
{
#if ASP_NET_CORE
    /// <summary>
    /// Rendering WebRootPath. <see cref="IHostingEnvironment" />
    /// </summary>
#else
    /// <summary>
    /// Rendering WebRootPath. <see cref="HostingEnvironment.MapPath"/>("/")
    /// </summary>
#endif
    [LayoutRenderer("aspnet-webrootpath")]
    [ThreadAgnostic]
    [ThreadSafe]
    public class AspNetWebRootPathLayoutRenderer : LayoutRenderer
    {
#if ASP_NET_CORE && !NET_STANDARD_21
        private static IWebHostEnvironment _webHostEnvironment;

        private static IWebHostEnvironment WebHostEnvironment => _webHostEnvironment ?? (_webHostEnvironment = ServiceLocator.ServiceProvider?.GetService<IWebHostEnvironment>());

        private string WebRootPath => WebHostEnvironment?.WebRootPath;
#elif NET_STANDARD_21
        private string WebRootPath => _webRootPath ?? (_webRootPath = new HostingEnvironment().ContentRootPath);
        private static string _webRootPath;
#else
        private string WebRootPath => _webRootPath ?? (_webRootPath = HostingEnvironment.MapPath("/"));
        private static string _webRootPath;
#endif

        /// <summary>
        /// Append to target
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder" /> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(WebRootPath);
        }

        /// <inheritdoc />
        protected override void CloseLayoutRenderer()
        {
#if ASP_NET_CORE && !NET_STANDARD_21
            _webHostEnvironment = null;
#elif NET_STANDARD_21
            _webRootPath = null;
#else
            _webRootPath = null;
#endif
            base.CloseLayoutRenderer();
        }
    }
}