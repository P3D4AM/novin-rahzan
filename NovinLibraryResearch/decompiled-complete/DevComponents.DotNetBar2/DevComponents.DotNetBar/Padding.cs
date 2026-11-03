using System;
using System.ComponentModel;
using System.Threading;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(PaddingConvertor))]
public class Padding : INotifyPropertyChanged
{
	private int _খ;

	private int _গ;

	private int _ঘ;

	private int _ঙ;

	public static Padding Empty = new Padding(0);

	private PropertyChangedEventHandler চ;

	[DefaultValue(0)]
	[NotifyParentProperty(true)]
	[Description("Indicates the amount of Top padding.")]
	[Browsable(true)]
	public int Top
	{
		get
		{
			return _ঘ;
		}
		set
		{
			_ঘ = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Top"));
		}
	}

	[NotifyParentProperty(true)]
	[Description("Indicates the amount of Left padding.")]
	[DefaultValue(0)]
	[Browsable(true)]
	public int Left
	{
		get
		{
			return _খ;
		}
		set
		{
			_খ = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Left"));
		}
	}

	[DefaultValue(0)]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Indicates the amount of Bottom padding.")]
	public int Bottom
	{
		get
		{
			return _ঙ;
		}
		set
		{
			_ঙ = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Bottom"));
		}
	}

	[Browsable(true)]
	[Description("Indicates the amount of Right padding.")]
	[DefaultValue(0)]
	[NotifyParentProperty(true)]
	public int Right
	{
		get
		{
			return _গ;
		}
		set
		{
			_গ = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Right"));
		}
	}

	[Browsable(false)]
	public int Horizontal => _খ + _গ;

	[Browsable(false)]
	public int Vertical => _ঘ + _ঙ;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (_খ == 0 && _গ == 0 && _ঘ == 0)
			{
				return _ঙ == 0;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int All
	{
		get
		{
			return _ঘ;
		}
		set
		{
			Top = value;
			Bottom = value;
			Left = value;
			Right = value;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	public Padding(int all)
	{
		_খ = all;
		_গ = all;
		_ঘ = all;
		_ঙ = all;
	}

	public Padding(int left, int right, int top, int bottom)
	{
		_খ = left;
		_গ = right;
		_ঘ = top;
		_ঙ = bottom;
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}
}
