using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(ProgressBarX), "Controls.ProgressBarX.ico")]
[ToolboxItem(false)]
[DefaultEvent("Click")]
[ComVisible(false)]
public class ProgressBarX : BaseItemControl
{
	private ProgressBarItem m_ProgressBar;

	[Browsable(true)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return m_ProgressBar.Maximum;
		}
		set
		{
			m_ProgressBar.Maximum = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return m_ProgressBar.Minimum;
		}
		set
		{
			m_ProgressBar.Minimum = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the current position of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return m_ProgressBar.Value;
		}
		set
		{
			m_ProgressBar.Value = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(1)]
	public int Step
	{
		get
		{
			return m_ProgressBar.Step;
		}
		set
		{
			m_ProgressBar.Step = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the color of the progress chunk.")]
	public Color ChunkColor
	{
		get
		{
			return m_ProgressBar.ChunkColor;
		}
		set
		{
			m_ProgressBar.ChunkColor = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the target gradient color of the progress chunk.")]
	public Color ChunkColor2
	{
		get
		{
			return m_ProgressBar.ChunkColor2;
		}
		set
		{
			m_ProgressBar.ChunkColor2 = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the gradient angle of the progress chunk.")]
	[DefaultValue(0)]
	public int ChunkGradientAngle
	{
		get
		{
			return (int)m_ProgressBar.ChunkGradientAngle;
		}
		set
		{
			m_ProgressBar.ChunkGradientAngle = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Description("Gets or sets whether the text inside the progress bar is displayed.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool TextVisible
	{
		get
		{
			return m_ProgressBar.TextVisible;
		}
		set
		{
			m_ProgressBar.TextVisible = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(eProgressItemType.Standard)]
	[Description("Indicates type of progress bar used to indicate progress.")]
	public eProgressItemType ProgressType
	{
		get
		{
			return m_ProgressBar.ProgressType;
		}
		set
		{
			m_ProgressBar.ProgressType = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(100)]
	[Category("Behavior")]
	[Description("Indicates marquee animation speed in milliseconds.")]
	public int MarqueeAnimationSpeed
	{
		get
		{
			return m_ProgressBar.MarqueeAnimationSpeed;
		}
		set
		{
			m_ProgressBar.MarqueeAnimationSpeed = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(eProgressBarItemColor.Normal)]
	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public eProgressBarItemColor ColorTable
	{
		get
		{
			return m_ProgressBar.ColorTable;
		}
		set
		{
			m_ProgressBar.ColorTable = value;
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	public eOrientation Orientation
	{
		get
		{
			return m_ProgressBar.Orientation;
		}
		set
		{
			if (m_ProgressBar.Orientation != value)
			{
				m_ProgressBar.Orientation = value;
				((Control)this).Invalidate();
			}
		}
	}

	internal ProgressBarItem ProgressBarItem => m_ProgressBar;

	public ProgressBarX()
	{
		m_ProgressBar = new ProgressBarItem();
		m_ProgressBar.Style = eDotNetBarStyle.Office2007;
		m_ProgressBar.Minimum = 0;
		m_ProgressBar.Maximum = 100;
		m_ProgressBar.Value = 0;
		m_ProgressBar.BackStyle = base.BackgroundStyle;
		HostItem = m_ProgressBar;
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
		Value += m_ProgressBar.Step;
	}

	public void Increment(int value)
	{
		Value += value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor()
	{
		return !m_ProgressBar.ChunkColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor()
	{
		m_ProgressBar.ChunkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor2()
	{
		return !m_ProgressBar.ChunkColor2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor2()
	{
		m_ProgressBar.ChunkColor2 = Color.Empty;
	}
}
