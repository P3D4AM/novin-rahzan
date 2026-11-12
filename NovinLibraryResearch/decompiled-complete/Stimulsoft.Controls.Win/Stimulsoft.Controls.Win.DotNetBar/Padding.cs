using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

[TypeConverter(typeof(PaddingConvertor))]
public class Padding : INotifyPropertyChanged
{
	private int _Left;

	private int _Right;

	private int _Top;

	private int _Bottom;

	public static Padding Empty = new Padding(0);

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of Top padding.")]
	[NotifyParentProperty(true)]
	public int Top
	{
		get
		{
			return _Top;
		}
		set
		{
			_Top = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Top"));
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of Left padding.")]
	[NotifyParentProperty(true)]
	public int Left
	{
		get
		{
			return _Left;
		}
		set
		{
			_Left = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Left"));
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of Bottom padding.")]
	[NotifyParentProperty(true)]
	public int Bottom
	{
		get
		{
			return _Bottom;
		}
		set
		{
			_Bottom = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Bottom"));
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of Right padding.")]
	[NotifyParentProperty(true)]
	public int Right
	{
		get
		{
			return _Right;
		}
		set
		{
			_Right = value;
			OnPropertyChanged(new PropertyChangedEventArgs("Right"));
		}
	}

	[Browsable(false)]
	public int Horizontal => _Left + _Right;

	[Browsable(false)]
	public int Vertical => _Top + _Bottom;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (_Left == 0 && _Right == 0 && _Top == 0)
			{
				return _Bottom == 0;
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
			return _Top;
		}
		set
		{
			Top = value;
			Bottom = value;
			Left = value;
			Right = value;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public Padding(int all)
	{
		_Left = all;
		_Right = all;
		_Top = all;
		_Bottom = all;
	}

	public Padding(int left, int right, int top, int bottom)
	{
		_Left = left;
		_Right = right;
		_Top = top;
		_Bottom = bottom;
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		this.PropertyChanged?.Invoke(this, e);
	}
}
