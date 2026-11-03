using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(ProgressBarX), "Controls.ProgressBarX.ico")]
[ComVisible(false)]
[DefaultEvent("Click")]
[ToolboxItem(true)]
public class ProgressBarX : BaseItemControl
{
	private ProgressBarItem _2B58;

	[Description("Gets or sets the maximum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	[Browsable(true)]
	public int Maximum
	{
		get
		{
			return _2B58.Maximum;
		}
		set
		{
			_2B58.Maximum = value;
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	[DevCoBrowsable(true)]
	public int Minimum
	{
		get
		{
			return _2B58.Minimum;
		}
		set
		{
			_2B58.Minimum = value;
		}
	}

	[Description("Gets or sets the current position of the progress bar.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	public int Value
	{
		get
		{
			return _2B58.Value;
		}
		set
		{
			_2B58.Value = value;
		}
	}

	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.")]
	[DefaultValue(1)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public int Step
	{
		get
		{
			return _2B58.Step;
		}
		set
		{
			_2B58.Step = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the color of the progress chunk.")]
	[Browsable(true)]
	public Color ChunkColor
	{
		get
		{
			return _2B58.ChunkColor;
		}
		set
		{
			_2B58.ChunkColor = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the target gradient color of the progress chunk.")]
	public Color ChunkColor2
	{
		get
		{
			return _2B58.ChunkColor2;
		}
		set
		{
			_2B58.ChunkColor2 = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the gradient angle of the progress chunk.")]
	public int ChunkGradientAngle
	{
		get
		{
			return (int)_2B58.ChunkGradientAngle;
		}
		set
		{
			_2B58.ChunkGradientAngle = value;
			((Control)this).Invalidate();
		}
	}

	[Description("Gets or sets whether the text inside the progress bar is displayed.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool TextVisible
	{
		get
		{
			return _2B58.TextVisible;
		}
		set
		{
			_2B58.TextVisible = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(eProgressItemType.Standard)]
	[Description("Indicates type of progress bar used to indicate progress.")]
	[Category("Behavior")]
	public eProgressItemType ProgressType
	{
		get
		{
			return _2B58.ProgressType;
		}
		set
		{
			_2B58.ProgressType = value;
		}
	}

	[Description("Indicates marquee animation speed in milliseconds.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(100)]
	public int MarqueeAnimationSpeed
	{
		get
		{
			return _2B58.MarqueeAnimationSpeed;
		}
		set
		{
			_2B58.MarqueeAnimationSpeed = value;
		}
	}

	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	[DevCoBrowsable(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eProgressBarItemColor.Normal)]
	public eProgressBarItemColor ColorTable
	{
		get
		{
			return _2B58.ColorTable;
		}
		set
		{
			_2B58.ColorTable = value;
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	public eOrientation Orientation
	{
		get
		{
			return _2B58.Orientation;
		}
		set
		{
			if (_2B58.Orientation != value)
			{
				_2B58.Orientation = value;
				((Control)this).Invalidate();
			}
		}
	}

	internal ProgressBarItem _2B57 => _2B58;

	public ProgressBarX()
	{
		_2B58 = new ProgressBarItem();
		_2B58.Style = eDotNetBarStyle.Office2007;
		_2B58.Minimum = 0;
		_2B58.Maximum = 100;
		_2B58.Value = 0;
		_2B58.BackStyle = base.BackgroundStyle;
		HostItem = _2B58;
	}

	protected override void PaintBackground(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if ((!backgroundStyle.Custom || backgroundStyle.BackColor.IsEmpty) && !((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, ((Control)this).BackColor);
		}
	}

	public void PerformStep()
	{
		Value += _2B58.Step;
	}

	public void Increment(int value)
	{
		Value += value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor()
	{
		return !_2B58.ChunkColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor()
	{
		_2B58.ChunkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor2()
	{
		return !_2B58.ChunkColor2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor2()
	{
		_2B58.ChunkColor2 = Color.Empty;
	}
}
