using System.Reflection;

namespace DevComponents.DotNetBar.Animation;

internal class ခ
{
	private PropertyInfo _ဆ;

	private object _ဇ;

	private object _ဈ;

	private object _ဉ;

	private double _ည = double.NaN;

	private double _ဋ = double.NaN;

	public object ဂ
	{
		get
		{
			return _ဇ;
		}
		set
		{
			_ဇ = value;
		}
	}

	public object ဃ
	{
		get
		{
			return _ဈ;
		}
		set
		{
			_ဈ = value;
			_ဋ = ဌ(value);
		}
	}

	public object င
	{
		get
		{
			return _ဉ;
		}
		set
		{
			_ဉ = value;
			_ည = ဌ(value);
		}
	}

	internal PropertyInfo စ => _ဆ;

	public ခ(object target, string targetPropertyName, object to)
	{
		_ဆ = target.GetType().GetProperty(targetPropertyName);
		_ဇ = target;
		ဃ = _ဆ.GetValue(target, null);
		င = to;
	}

	public ခ(object target, string targetPropertyName, object from, object to)
	{
		_ဆ = target.GetType().GetProperty(targetPropertyName);
		_ဇ = target;
		ဃ = from;
		င = to;
	}

	private static double ဌ(object ԝ)
	{
		if (ԝ is int)
		{
			return (int)ԝ;
		}
		if (ԝ is double)
		{
			return (double)ԝ;
		}
		if (ԝ is long)
		{
			return (long)ԝ;
		}
		if (ԝ is float)
		{
			return (float)ԝ;
		}
		return double.NaN;
	}

	internal double ဍ()
	{
		return _ည;
	}

	internal double ဎ()
	{
		return _ဋ;
	}
}
