using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing;

public class RouteData
{
	public struct RouteDataSnapshot
	{
		private readonly RouteData _routeData;

		private readonly RouteValueDictionary _dataTokens;

		private readonly IList<IRouter> _routers;

		private readonly RouteValueDictionary _values;

		public RouteDataSnapshot(RouteData routeData, RouteValueDictionary dataTokens, IList<IRouter> routers, RouteValueDictionary values)
		{
			if (routeData == null)
			{
				throw new ArgumentNullException("routeData");
			}
			_routeData = routeData;
			_dataTokens = dataTokens;
			_routers = routers;
			_values = values;
		}

		public void Restore()
		{
			if (_routeData._dataTokens != null || _dataTokens != null)
			{
				if (_dataTokens == null)
				{
					_routeData._dataTokens.Clear();
				}
				else
				{
					_routeData._dataTokens.Clear();
					foreach (KeyValuePair<string, object> dataToken in _dataTokens)
					{
						_routeData._dataTokens.Add(dataToken.Key, dataToken.Value);
					}
				}
			}
			if (_routeData._routers != null || _routers != null)
			{
				if (_routers == null)
				{
					List<IRouter> routers = _routeData._routers;
					for (int num = routers.Count - 1; num >= 0; num--)
					{
						routers.RemoveAt(num);
					}
				}
				else
				{
					List<IRouter> routers2 = _routeData._routers;
					IList<IRouter> routers3 = _routers;
					int i;
					for (i = 0; i < routers3.Count && i < routers2.Count; i++)
					{
						routers2[i] = routers3[i];
					}
					for (; i < routers3.Count; i++)
					{
						routers2.Add(routers3[i]);
					}
					for (i = routers2.Count - 1; i >= routers3.Count; i--)
					{
						routers2.RemoveAt(i);
					}
				}
			}
			if (_routeData._values == null && _values == null)
			{
				return;
			}
			if (_values == null)
			{
				_routeData._values.Clear();
				return;
			}
			_routeData._values.Clear();
			foreach (KeyValuePair<string, object> value in _values)
			{
				_routeData._values.Add(value.Key, value.Value);
			}
		}
	}

	private RouteValueDictionary _dataTokens;

	private List<IRouter> _routers;

	private RouteValueDictionary _values;

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

	public IList<IRouter> Routers
	{
		get
		{
			if (_routers == null)
			{
				_routers = new List<IRouter>();
			}
			return _routers;
		}
	}

	public RouteValueDictionary Values
	{
		get
		{
			if (_values == null)
			{
				_values = new RouteValueDictionary();
			}
			return _values;
		}
	}

	public RouteData()
	{
	}

	public RouteData(RouteData other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (other._routers != null)
		{
			_routers = new List<IRouter>(other.Routers);
		}
		if (other._dataTokens != null)
		{
			_dataTokens = new RouteValueDictionary(other._dataTokens);
		}
		if (other._values != null)
		{
			_values = new RouteValueDictionary(other._values);
		}
	}

	public RouteDataSnapshot PushState(IRouter router, RouteValueDictionary values, RouteValueDictionary dataTokens)
	{
		List<IRouter> list = null;
		int? num = _routers?.Count;
		if (num > 0)
		{
			list = new List<IRouter>(num.Value);
			for (int i = 0; i < num.Value; i++)
			{
				list.Add(_routers[i]);
			}
		}
		RouteValueDictionary dataTokens2 = _dataTokens;
		RouteValueDictionary dataTokens3 = ((dataTokens2 != null && dataTokens2.Count > 0) ? new RouteValueDictionary(_dataTokens) : null);
		List<IRouter> routers = list;
		RouteValueDictionary values2 = _values;
		RouteDataSnapshot result = new RouteDataSnapshot(this, dataTokens3, routers, (values2 != null && values2.Count > 0) ? new RouteValueDictionary(_values) : null);
		if (router != null)
		{
			Routers.Add(router);
		}
		if (values != null)
		{
			foreach (KeyValuePair<string, object> value in values)
			{
				if (value.Value != null)
				{
					Values[value.Key] = value.Value;
				}
			}
		}
		if (dataTokens != null)
		{
			foreach (KeyValuePair<string, object> dataToken in dataTokens)
			{
				DataTokens[dataToken.Key] = dataToken.Value;
			}
		}
		return result;
	}
}
