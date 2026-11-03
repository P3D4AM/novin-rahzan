using System;
using System.ComponentModel;

namespace DevComponents.AdvTree;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ColumnWidth
{
	private int ᜡ;

	private int ᜢ;

	private bool _ᜣ;

	private bool _ᜤ;

	private int _ᜥ;

	[DefaultValue(0)]
	[Browsable(true)]
	[Description("Gets or sets relative width in percent. Valid values are between 1-100 with 0 indicating that absolute width will be used.")]
	public int Relative
	{
		get
		{
			return ᜡ;
		}
		set
		{
			if (ᜡ != value)
			{
				ᜡ = value;
				ᜩ();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Gets or sets the absolute width of the column in pixels.")]
	public int Absolute
	{
		get
		{
			return ᜢ;
		}
		set
		{
			if (ᜢ >= 0 && ᜢ != value)
			{
				ᜢ = value;
				if (ᜢ != 0)
				{
					ᜡ = 0;
				}
				ᜩ();
			}
		}
	}

	[Description("Indicates whether column is sized based on the content.")]
	[DefaultValue(false)]
	public bool AutoSize
	{
		get
		{
			return _ᜣ;
		}
		set
		{
			if (_ᜣ != value)
			{
				_ᜣ = value;
				ᜩ();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether column auto-width is set to minimum of the column header text width. Applies to AutoSize=true only.")]
	public bool AutoSizeMinHeader
	{
		get
		{
			return _ᜤ;
		}
		set
		{
			_ᜤ = value;
			ᜩ();
		}
	}

	[Browsable(false)]
	public int AutoSizeWidth => _ᜥ;

	internal event EventHandler ᜠ;

	internal void ᜦ(int ԝ)
	{
		ᜢ = ԝ;
	}

	internal int ᜧ(int ᜨ)
	{
		if (ᜢ > 0)
		{
			return ᜢ;
		}
		if (ᜡ > 0)
		{
			return 100 / ᜡ * ᜨ;
		}
		return 0;
	}

	private void ᜩ()
	{
		if (this.ᜠ != null)
		{
			this.ᜠ(this, new EventArgs());
		}
	}

	internal void ᜪ(bool ᜫ)
	{
		_ᜣ = ᜫ;
	}

	internal void ᜬ(int ԝ)
	{
		_ᜥ = ԝ;
	}
}
