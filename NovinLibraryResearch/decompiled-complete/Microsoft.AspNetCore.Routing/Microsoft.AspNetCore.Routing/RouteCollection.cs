using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Routing;

public class RouteCollection : IRouteCollection, IRouter
{
	private static readonly char[] UrlQueryDelimiters = new char[2] { '?', '#' };

	private readonly List<IRouter> _routes = new List<IRouter>();

	private readonly List<IRouter> _unnamedRoutes = new List<IRouter>();

	private readonly Dictionary<string, INamedRouter> _namedRoutes = new Dictionary<string, INamedRouter>(StringComparer.OrdinalIgnoreCase);

	private RouteOptions _options;

	public IRouter this[int index] => _routes[index];

	public int Count => _routes.Count;

	public void Add(IRouter router)
	{
		if (router == null)
		{
			throw new ArgumentNullException("router");
		}
		if (router is INamedRouter namedRouter)
		{
			if (!string.IsNullOrEmpty(namedRouter.Name))
			{
				_namedRoutes.Add(namedRouter.Name, namedRouter);
			}
		}
		else
		{
			_unnamedRoutes.Add(router);
		}
		_routes.Add(router);
	}

	public virtual async Task RouteAsync(RouteContext context)
	{
		RouteData.RouteDataSnapshot snapshot = context.RouteData.PushState(null, null, null);
		for (int i = 0; i < Count; i++)
		{
			IRouter router = this[i];
			context.RouteData.Routers.Add(router);
			try
			{
				await router.RouteAsync(context);
				if (context.Handler != null)
				{
					break;
				}
			}
			finally
			{
				if (context.Handler == null)
				{
					snapshot.Restore();
				}
			}
		}
	}

	public virtual VirtualPathData GetVirtualPath(VirtualPathContext context)
	{
		EnsureOptions(context.HttpContext);
		if (!string.IsNullOrEmpty(context.RouteName))
		{
			VirtualPathData virtualPathData = null;
			if (_namedRoutes.TryGetValue(context.RouteName, out var value))
			{
				virtualPathData = value.GetVirtualPath(context);
			}
			VirtualPathData virtualPath = GetVirtualPath(context, _unnamedRoutes);
			if (virtualPathData != null && virtualPath != null)
			{
				throw new InvalidOperationException(Resources.FormatNamedRoutes_AmbiguousRoutesFound(context.RouteName));
			}
			return NormalizeVirtualPath(virtualPathData ?? virtualPath);
		}
		return NormalizeVirtualPath(GetVirtualPath(context, _routes));
	}

	private VirtualPathData GetVirtualPath(VirtualPathContext context, List<IRouter> routes)
	{
		for (int i = 0; i < routes.Count; i++)
		{
			VirtualPathData virtualPath = routes[i].GetVirtualPath(context);
			if (virtualPath != null)
			{
				return virtualPath;
			}
		}
		return null;
	}

	private VirtualPathData NormalizeVirtualPath(VirtualPathData pathData)
	{
		if (pathData == null)
		{
			return pathData;
		}
		string virtualPath = pathData.VirtualPath;
		if (!string.IsNullOrEmpty(virtualPath) && (_options.LowercaseUrls || _options.AppendTrailingSlash))
		{
			int num = virtualPath.IndexOfAny(UrlQueryDelimiters);
			string text = virtualPath;
			string text2 = string.Empty;
			if (num != -1)
			{
				text = virtualPath.Substring(0, num);
				text2 = virtualPath.Substring(num);
			}
			if (_options.LowercaseUrls)
			{
				text = text.ToLowerInvariant();
			}
			if (_options.AppendTrailingSlash && !text.EndsWith("/"))
			{
				text += "/";
			}
			virtualPath = text + text2;
			return new VirtualPathData(pathData.Router, virtualPath, pathData.DataTokens);
		}
		return pathData;
	}

	private void EnsureOptions(HttpContext context)
	{
		if (_options == null)
		{
			_options = context.RequestServices.GetRequiredService<IOptions<RouteOptions>>().Value;
		}
	}
}
