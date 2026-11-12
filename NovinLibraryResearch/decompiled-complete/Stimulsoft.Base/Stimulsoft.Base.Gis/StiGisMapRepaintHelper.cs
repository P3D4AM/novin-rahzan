using System;
using System.Collections.Generic;
using System.Reflection;

namespace Stimulsoft.Base.Gis;

public static class StiGisMapRepaintHelper
{
	public delegate IStiGisMapSimpleView CreateViewDelegate(StiGeoMapProviderType provider, StiGisMapData data, InvokeTickDelegate invokeTick);

	public delegate IStiGisMapControl CreateWinFormsControlDelegate();

	public delegate IStiGisMapControl CreateWpfControlDelegate(object fontFamily);

	public delegate void InvokeTickDelegate();

	private static CreateViewDelegate createViewMethod;

	private static CreateWinFormsControlDelegate createWinFormsControlMethod;

	private static CreateWpfControlDelegate createWpfControlMethod;

	private static Dictionary<object, IStiGisMapSimpleView> cache = new Dictionary<object, IStiGisMapSimpleView>();

	private static bool isInit;

	public static event EventHandler Tick;

	public static void Init()
	{
		if (isInit)
		{
			return;
		}
		try
		{
			Type type = StiTypeFinder.GetType("Stimulsoft.Map.Gis.StiGisBridge, Stimulsoft.Map, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("CreateDesignViewDefault", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					throw new Exception("Method 'CreateDesignViewDefault' not found");
				}
				createViewMethod = method.Invoke(null, null) as CreateViewDelegate;
				method = type.GetMethod("CreateWinFormsControl", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					throw new Exception("Method 'CreateWinFormsControl' not found");
				}
				createWinFormsControlMethod = method.Invoke(null, null) as CreateWinFormsControlDelegate;
				method = type.GetMethod("CreateWpfControl", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					throw new Exception("Method 'CreateWpfControl' not found");
				}
				createWpfControlMethod = method.Invoke(null, null) as CreateWpfControlDelegate;
				isInit = true;
			}
		}
		catch
		{
		}
	}

	public static IStiGisMapControl CreateWinFormsControl()
	{
		if (createWinFormsControlMethod == null)
		{
			return null;
		}
		return createWinFormsControlMethod();
	}

	public static IStiGisMapControl CreateWpfControl(object fontFamily)
	{
		if (createWpfControlMethod == null)
		{
			return null;
		}
		return createWpfControlMethod(fontFamily);
	}

	private static void InvokeTick()
	{
		StiGisMapRepaintHelper.Tick?.Invoke(null, EventArgs.Empty);
	}

	public static List<object> FetchAllComponents()
	{
		lock (cache)
		{
			if (cache.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (KeyValuePair<object, IStiGisMapSimpleView> item in cache)
				{
					if (item.Value.IsComplete)
					{
						list.Add(item.Key);
					}
				}
				return list;
			}
		}
		return null;
	}

	public static IStiGisMapSimpleView Get(object element, StiGeoMapProviderType provider, StiGisMapData data)
	{
		lock (cache)
		{
			if (!cache.TryGetValue(element, out var value))
			{
				if (createViewMethod == null)
				{
					throw new NotSupportedException();
				}
				value = createViewMethod(provider, data, InvokeTick);
				cache.Add(element, value);
				value.Run();
				return null;
			}
			if (value.IsChanged(provider, data))
			{
				value.Restart(provider, data);
				return null;
			}
			if (value.IsComplete)
			{
				return value;
			}
		}
		return null;
	}

	public static IStiGisMapSimpleView GetForExport(StiGeoMapProviderType provider, StiGisMapData data)
	{
		return createViewMethod(provider, data, InvokeTick);
	}
}
