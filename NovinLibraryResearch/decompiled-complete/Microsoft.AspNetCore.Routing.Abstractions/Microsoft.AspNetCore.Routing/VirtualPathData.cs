using System;

namespace Microsoft.AspNetCore.Routing;

public class VirtualPathData
{
	private RouteValueDictionary _dataTokens;

	private string _virtualPath;

	public RouteValueDictionary DataTokens
	{
		get
		{
			if (_dataTokens == null)
			{
				_dataTokens = new RouteValueDictionary();
			}
			return _dataTokens;
		}
	}

	public IRouter Router { get; set; }

	public string VirtualPath
	{
		get
		{
			return _virtualPath;
		}
		set
		{
			_virtualPath = NormalizePath(value);
		}
	}

	public VirtualPathData(IRouter router, string virtualPath)
		: this(router, virtualPath, null)
	{
	}

	public VirtualPathData(IRouter router, string virtualPath, RouteValueDictionary dataTokens)
	{
		if (router == null)
		{
			throw new ArgumentNullException("router");
		}
		Router = router;
		VirtualPath = virtualPath;
		_dataTokens = ((dataTokens == null) ? null : new RouteValueDictionary(dataTokens));
	}

	private static string NormalizePath(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return string.Empty;
		}
		if (!path.StartsWith("/", StringComparison.Ordinal))
		{
			return "/" + path;
		}
		return path;
	}
}
