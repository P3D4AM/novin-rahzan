using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Presentation;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroScrollBarPainter : ScrollBarPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	private bool m_AppStyleScrollBar;

	private ShapeBorder m_ThumbOuterBorder = new ShapeBorder(1);

	private ShapeBorder m_ThumbInnerBorder = new ShapeBorder(1);

	private ShapeFill m_ThumbInnerFill = new ShapeFill();

	private ShapeFill m_ThumbSignFill = new ShapeFill();

	private Shape m_ThumbShape;

	private ShapePath m_ThumbSignShape;

	private Size m_ThumbSignSize = new Size(9, 5);

	private Shape m_TrackShape;

	private ShapeBorder m_TrackOuterBorder = new ShapeBorder(1);

	private ShapeBorder m_TrackInnerBorder = new ShapeBorder(1);

	private ShapeFill m_TrackInnerFill = new ShapeFill();

	private Shape m_BackgroundShape;

	private ShapeBorder m_BackgroundBorder = new ShapeBorder(1);

	private ShapeFill m_BackgroundFill = new ShapeFill();

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public bool AppStyleScrollBar
	{
		get
		{
			return m_AppStyleScrollBar;
		}
		set
		{
			m_AppStyleScrollBar = value;
		}
	}

	public MetroScrollBarPainter()
	{
		m_ThumbShape = GetThumbShape();
		m_TrackShape = GetTrackShape();
		m_BackgroundShape = GetBackgroundShape();
	}

	public override void PaintThumb(Graphics g, System.Drawing.Rectangle bounds, eScrollThumbPosition position, eScrollBarState state)
	{
		Office2007ScrollBarStateColorTable colorTable = GetColorTable(state);
		if (colorTable != null)
		{
			m_ThumbOuterBorder.Apply(colorTable.ThumbOuterBorder);
			m_ThumbSignFill.Apply(colorTable.ThumbSignBackground);
			m_ThumbSignShape.Path = GetThumbSignPath(position);
			m_ThumbShape.Paint(new ShapePaintInfo(g, bounds));
			m_ThumbSignShape.Path.Dispose();
			m_ThumbSignShape.Path = null;
		}
	}

	private Office2007ScrollBarStateColorTable GetColorTable(eScrollBarState state)
	{
		Office2007ScrollBarColorTable office2007ScrollBarColorTable = (m_AppStyleScrollBar ? m_ColorTable.AppScrollBar : m_ColorTable.ScrollBar);
		return state switch
		{
			eScrollBarState.Normal => office2007ScrollBarColorTable.Default, 
			eScrollBarState.Disabled => office2007ScrollBarColorTable.Disabled, 
			eScrollBarState.ControlMouseOver => office2007ScrollBarColorTable.MouseOverControl, 
			eScrollBarState.PartMouseOver => office2007ScrollBarColorTable.MouseOver, 
			eScrollBarState.Pressed => office2007ScrollBarColorTable.Pressed, 
			_ => null, 
		};
	}

	public override void PaintTrackHorizontal(Graphics g, System.Drawing.Rectangle bounds, eScrollBarState state)
	{
		Office2007ScrollBarStateColorTable colorTable = GetColorTable(state);
		if (colorTable != null)
		{
			m_TrackInnerFill.Apply(colorTable.TrackSignBackground);
			m_TrackShape.Paint(new ShapePaintInfo(g, bounds));
		}
	}

	public override void PaintTrackVertical(Graphics g, System.Drawing.Rectangle bounds, eScrollBarState state)
	{
		Office2007ScrollBarStateColorTable colorTable = GetColorTable(state);
		if (colorTable != null)
		{
			m_TrackInnerFill.Apply(colorTable.TrackSignBackground);
			m_TrackShape.Paint(new ShapePaintInfo(g, bounds));
		}
	}

	public override void PaintBackground(Graphics g, System.Drawing.Rectangle bounds, eScrollBarState state, bool horizontal, bool sideBorderOnly, bool rtl)
	{
		Office2007ScrollBarStateColorTable colorTable = GetColorTable(state);
		if (colorTable == null)
		{
			return;
		}
		if (sideBorderOnly)
		{
			m_BackgroundBorder.Apply(null);
		}
		else
		{
			m_BackgroundBorder.Apply(colorTable.Border);
		}
		m_BackgroundFill.Apply(colorTable.Background);
		m_BackgroundFill.GradientAngle = (horizontal ? 90 : 0);
		m_BackgroundShape.Paint(new ShapePaintInfo(g, bounds));
		if (sideBorderOnly && !colorTable.Border.Start.IsEmpty)
		{
			if (horizontal)
			{
				DisplayHelp.DrawLine(g, bounds.X, bounds.Y, bounds.Right, bounds.Y, colorTable.Border.Start, 1);
			}
			else if (rtl)
			{
				DisplayHelp.DrawLine(g, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom, colorTable.Border.Start, 1);
			}
			else
			{
				DisplayHelp.DrawLine(g, 0, bounds.Y, 0, bounds.Bottom, colorTable.Border.Start, 1);
			}
		}
	}

	private Shape GetBackgroundShape()
	{
		Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle rectangle = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle();
		rectangle.Border = m_BackgroundBorder;
		rectangle.Fill = m_BackgroundFill;
		return rectangle;
	}

	private Shape GetThumbShape()
	{
		Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle rectangle = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle();
		rectangle.Padding = new PaddingInfo(1, 1, 1, 1);
		m_ThumbSignShape = new ShapePath();
		m_ThumbSignShape.Fill = m_ThumbSignFill;
		rectangle.Children.Add(m_ThumbSignShape);
		return rectangle;
	}

	private Shape GetTrackShape()
	{
		Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle rectangle = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle();
		rectangle.Padding = new PaddingInfo(1, 1, 1, 1);
		Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle rectangle2 = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle();
		rectangle2.Padding = new PaddingInfo(2, 2, 2, 2);
		rectangle2.Fill = m_TrackInnerFill;
		rectangle.Children.Add(rectangle2);
		return rectangle;
	}

	private GraphicsPath GetThumbSignPath(eScrollThumbPosition pos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Size thumbSignSize = m_ThumbSignSize;
		if (pos == eScrollThumbPosition.Left || pos == eScrollThumbPosition.Right)
		{
			int width = thumbSignSize.Width;
			thumbSignSize.Width = thumbSignSize.Height;
			thumbSignSize.Height = width;
		}
		switch (pos)
		{
		case eScrollThumbPosition.Top:
			val.AddPolygon(new PointF[6]
			{
				new PointF(-1f, thumbSignSize.Height),
				new PointF(thumbSignSize.Width / 2, -1f),
				new PointF(thumbSignSize.Width / 2, -1f),
				new PointF(thumbSignSize.Width, thumbSignSize.Height),
				new PointF(thumbSignSize.Width, thumbSignSize.Height),
				new PointF(-1f, thumbSignSize.Height)
			});
			val.CloseAllFigures();
			break;
		case eScrollThumbPosition.Bottom:
			val.AddLine(thumbSignSize.Width / 2, thumbSignSize.Height + 1, thumbSignSize.Width, 1);
			val.AddLine(thumbSignSize.Width, 1, 0, 1);
			val.AddLine(0, 1, thumbSignSize.Width / 2, thumbSignSize.Height + 1);
			val.CloseAllFigures();
			break;
		case eScrollThumbPosition.Left:
		{
			thumbSignSize.Height++;
			int num = thumbSignSize.Height / 2;
			val.AddLine(0, num, thumbSignSize.Width, 0);
			val.AddLine(thumbSignSize.Width, 0, thumbSignSize.Width, thumbSignSize.Height);
			val.AddLine(thumbSignSize.Width, thumbSignSize.Height, 0, num);
			val.CloseAllFigures();
			break;
		}
		case eScrollThumbPosition.Right:
			thumbSignSize.Height++;
			val.AddLine(thumbSignSize.Width, thumbSignSize.Height / 2, 0, 0);
			val.AddLine(0, 0, 0, thumbSignSize.Height);
			val.AddLine(0, thumbSignSize.Height, thumbSignSize.Width, thumbSignSize.Height / 2);
			val.CloseAllFigures();
			break;
		}
		return val;
	}
}
