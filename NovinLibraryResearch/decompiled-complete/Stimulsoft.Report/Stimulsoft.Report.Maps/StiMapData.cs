using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Maps;

public class StiMapData : INotifyPropertyChanged
{
	private double valueDouble;

	private bool isValueInit;

	private string value;

	private string group;

	private string color;

	public string Key { get; private set; }

	public string Gss { get; set; }

	public string Value
	{
		get
		{
			return value;
		}
		set
		{
			if (this.value != value)
			{
				this.value = value;
				InvokeValueChanged();
			}
		}
	}

	public string Group
	{
		get
		{
			return group;
		}
		set
		{
			if (group != value)
			{
				group = value;
				InvokeValueChanged();
			}
		}
	}

	public string Name { get; set; }

	public string Color
	{
		get
		{
			return color;
		}
		set
		{
			if (color != value)
			{
				color = value;
				InvokeValueChanged();
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	internal event EventHandler ValuesChanged;

	public double? GetValue()
	{
		if (!isValueInit)
		{
			isValueInit = true;
			if (double.TryParse(Value, out var result))
			{
				valueDouble = result;
			}
		}
		return valueDouble;
	}

	public override string ToString()
	{
		return $"{Key}, {Group}";
	}

	public StiMapData Clone()
	{
		return new StiMapData(Key)
		{
			Gss = Gss,
			value = value,
			group = group,
			Name = Name,
			color = color
		};
	}

	internal void InvokePropertyChanged(string propName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
	}

	private void InvokeValueChanged()
	{
		this.ValuesChanged?.Invoke(this, EventArgs.Empty);
	}

	public StiMapData(string key)
	{
		Key = key;
	}
}
