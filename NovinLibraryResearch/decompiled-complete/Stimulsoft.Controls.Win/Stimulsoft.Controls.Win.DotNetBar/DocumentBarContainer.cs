using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[TypeConverter(typeof(DocumentBarContainerConverter))]
public class DocumentBarContainer : DocumentBaseContainer
{
	private Bar m_Bar;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Bar Bar
	{
		get
		{
			return m_Bar;
		}
		set
		{
			m_Bar = value;
		}
	}

	public override bool Visible
	{
		get
		{
			if (m_Bar == null)
			{
				return false;
			}
			if (m_Bar.GetDesignMode())
			{
				return true;
			}
			return m_Bar.IsVisible;
		}
	}

	protected internal override Size MinimumSize => m_Bar.GetAdjustedFullSize(m_Bar.MinimumDockSize(eOrientation.Horizontal));

	public DocumentBarContainer()
	{
	}

	public DocumentBarContainer(Bar bar)
	{
		m_Bar = bar;
		if (m_Bar.SplitDockHeight > 0 || m_Bar.SplitDockWidth > 0)
		{
			SetLayoutBounds(new Rectangle(0, 0, m_Bar.SplitDockWidth, m_Bar.SplitDockHeight));
		}
	}

	public DocumentBarContainer(Bar bar, int proposedWidth, int proposedHeight)
	{
		m_Bar = bar;
		SetLayoutBounds(new Rectangle(0, 0, proposedWidth, proposedHeight));
	}

	public override void Layout(Rectangle bounds)
	{
		if (m_Bar != null)
		{
			m_Bar.SplitDockHeight = 0;
			m_Bar.SplitDockWidth = 0;
			m_Bar.MinHeight = 0;
			m_Bar.RecalcSize(bounds.Size);
			m_Bar.Location = bounds.Location;
		}
		SetDisplayBounds(bounds);
	}
}
