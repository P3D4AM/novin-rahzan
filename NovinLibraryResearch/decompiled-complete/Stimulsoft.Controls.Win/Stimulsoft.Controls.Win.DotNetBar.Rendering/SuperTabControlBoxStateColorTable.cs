using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabControlBoxStateColorTableConvertor))]
public class SuperTabControlBoxStateColorTable : ICloneable
{
	private Color _Background;

	private Color _Border = Color.Empty;

	private Color _Image = Color.Empty;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color Background
	{
		get
		{
			return _Background;
		}
		set
		{
			if (_Background != value)
			{
				_Background = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color Border
	{
		get
		{
			return _Border;
		}
		set
		{
			if (_Border != value)
			{
				_Border = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color Image
	{
		get
		{
			return _Image;
		}
		set
		{
			if (_Image != value)
			{
				_Image = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (!_Background.IsEmpty)
			{
				return false;
			}
			if (_Border != Color.Empty)
			{
				return false;
			}
			if (_Image != Color.Empty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabControlBoxStateColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackground()
	{
		return _Background != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackground()
	{
		_Background = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorder()
	{
		return _Border != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorder()
	{
		_Border = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImage()
	{
		return _Image != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImage()
	{
		_Image = Color.Empty;
	}

	private void OnColorTableChanged()
	{
		if (this.ColorTableChanged != null)
		{
			this.ColorTableChanged(this, EventArgs.Empty);
		}
	}

	public object Clone()
	{
		SuperTabControlBoxStateColorTable superTabControlBoxStateColorTable = new SuperTabControlBoxStateColorTable();
		superTabControlBoxStateColorTable.Background = Background;
		superTabControlBoxStateColorTable.Border = Border;
		superTabControlBoxStateColorTable.Image = Image;
		return superTabControlBoxStateColorTable;
	}
}
