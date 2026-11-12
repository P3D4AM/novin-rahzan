using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Presentation;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007DialogLauncherPainter : DialogLauncherPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

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

	public override void PaintDialogLauncher(RibbonBarRendererEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		System.Drawing.Rectangle bounds = e.Bounds;
		Graphics graphics = e.Graphics;
		bool flag = (int)((Control)e.RibbonBar).RightToLeft == 1;
		Office2007DialogLauncherStateColorTable colorTable = GetColorTable(e);
		if (!colorTable.TopBackground.IsEmpty && !colorTable.BottomBackground.IsEmpty)
		{
			Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle rectangle = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle(new ShapeBorder(colorTable.OuterBorder), new ShapeFill(colorTable.TopBackground));
			rectangle.Padding = new PaddingInfo(StiScale2.XXI(1.0), StiScale2.XXI(1.0), StiScale2.XXI(1.0), StiScale2.XXI(1.0));
			Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle rectangle2 = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle(new ShapeFill(colorTable.BottomBackground));
			rectangle2.Size.Height = bounds.Height / 2;
			rectangle.Children.Add(rectangle2);
			rectangle2 = new Stimulsoft.Controls.Win.DotNetBar.Presentation.Rectangle(new ShapeBorder(colorTable.InnerBorder));
			rectangle.Children.Add(rectangle2);
			ShapePaintInfo p = new ShapePaintInfo(graphics, bounds);
			rectangle.Paint(p);
		}
		Size size = new Size(StiScale2.XXI(8.0), StiScale2.YYI(8.0));
		bounds = ((!flag) ? new System.Drawing.Rectangle(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y + (bounds.Height - size.Height) / 2, size.Width, size.Height) : new System.Drawing.Rectangle(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y + (bounds.Height - size.Height) / 2, size.Width, size.Height));
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		ShapeBorder shapeBorder = new ShapeBorder(colorTable.DialogLauncherShade, 1);
		ShapeFill shapeFill = new ShapeFill(colorTable.DialogLauncherShade);
		Shape shape = new Shape();
		Line shape2 = new Line(new Location(), new Location(StiScale2.XXI(7.0), 0), shapeBorder);
		shape.Children.Add(shape2);
		shape2 = new Line(new Location(), new Location(0, StiScale2.YYI(7.0)), shapeBorder);
		shape.Children.Add(shape2);
		shape2 = new Line(new Location(StiScale2.XXI(3.0), StiScale2.YYI(3.0)), new Location(StiScale2.XXI(6.0), StiScale2.YYI(6.0)), shapeBorder);
		shape.Children.Add(shape2);
		bounds.Offset(1, 1);
		ShapePaintInfo shapePaintInfo = new ShapePaintInfo(graphics, bounds);
		shape.Paint(shapePaintInfo);
		shapeBorder.Color1 = Color.Gray;
		shapeFill.Color1 = Color.Gray;
		bounds.Offset(-1, -1);
		shapePaintInfo.Bounds = bounds;
		shape.Paint(shapePaintInfo);
		shapePaintInfo.Graphics.FillPolygon(Brushes.Gray, new Point[3]
		{
			new Point(bounds.Right, bounds.Bottom),
			new Point(bounds.Right - StiScale2.XXI(4.0), bounds.Bottom),
			new Point(bounds.Right, bounds.Bottom - StiScale2.XXI(4.0))
		});
		graphics.SmoothingMode = smoothingMode;
	}

	private Office2007DialogLauncherStateColorTable GetColorTable(RibbonBarRendererEventArgs e)
	{
		if (e.Pressed)
		{
			return m_ColorTable.DialogLauncher.Pressed;
		}
		if (e.MouseOver)
		{
			return m_ColorTable.DialogLauncher.MouseOver;
		}
		return m_ColorTable.DialogLauncher.Default;
	}
}
