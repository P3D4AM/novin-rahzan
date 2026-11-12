using System.Reflection;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

internal class AnimationRequest
{
	private PropertyInfo _TargetProperty;

	private object _Target;

	private object _From;

	private object _To;

	private double _ToValue = double.NaN;

	private double _FromValue = double.NaN;

	public object Target
	{
		get
		{
			return _Target;
		}
		set
		{
			_Target = value;
		}
	}

	public object From
	{
		get
		{
			return _From;
		}
		set
		{
			_From = value;
			_FromValue = GetDoubleValue(value);
		}
	}

	public object To
	{
		get
		{
			return _To;
		}
		set
		{
			_To = value;
			_ToValue = GetDoubleValue(value);
		}
	}

	internal PropertyInfo Property => _TargetProperty;

	public AnimationRequest(object target, string targetPropertyName, object to)
	{
		_TargetProperty = target.GetType().GetProperty(targetPropertyName);
		_Target = target;
		From = _TargetProperty.GetValue(target, null);
		To = to;
	}

	public AnimationRequest(object target, string targetPropertyName, object from, object to)
	{
		_TargetProperty = target.GetType().GetProperty(targetPropertyName);
		_Target = target;
		From = from;
		To = to;
	}

	private static double GetDoubleValue(object value)
	{
		if (value is int)
		{
			return (int)value;
		}
		if (value is double)
		{
			return (double)value;
		}
		if (value is long)
		{
			return (long)value;
		}
		if (value is float)
		{
			return (float)value;
		}
		return double.NaN;
	}

	internal double GetToValue()
	{
		return _ToValue;
	}

	internal double GetFromValue()
	{
		return _FromValue;
	}
}
