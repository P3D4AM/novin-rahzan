using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2010NavPaneButtonPainter : Office2007ButtonItemPainter
{
	protected override void PaintStateBackground(ButtonItem button, Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault, bool paintBorder)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		if (stateColors != null && stateColors.Background != null && !stateColors.Background.IsEmpty)
		{
			Rectangle rectangle = r;
			ShadowPaintInfo shadowPaintInfo = new ShadowPaintInfo();
			shadowPaintInfo.Graphics = g;
			shadowPaintInfo.Rectangle = rectangle;
			ShadowPainter.Paint3(shadowPaintInfo);
			r.Width -= 4;
			r.Height -= 4;
			r.Offset(1, 1);
			DisplayHelp.FillRectangle(g, r, stateColors.Background);
			if (stateColors.BottomBackgroundHighlight != null && !stateColors.BottomBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle2 = new Rectangle(r.X, r.Y + r.Height / 2 - 2, r.Width, r.Height + 4);
				GraphicsPath val = new GraphicsPath();
				val.AddEllipse(rectangle2);
				PathGradientBrush val2 = new PathGradientBrush(val);
				val2.CenterColor = stateColors.BottomBackgroundHighlight.Start;
				val2.SurroundColors = new Color[1] { stateColors.BottomBackgroundHighlight.End };
				val2.CenterPoint = new PointF(rectangle2.X + rectangle2.Width / 2, r.Bottom);
				Blend val3 = new Blend();
				val3.Factors = new float[3] { 0f, 0.5f, 0.6f };
				val3.Positions = new float[3] { 0f, 0.4f, 1f };
				val2.Blend = val3;
				g.FillRectangle((Brush)(object)val2, r);
				((Brush)val2).Dispose();
				val.Dispose();
			}
			DisplayHelp.DrawGradientRectangle(g, r, stateColors.OuterBorder, 1);
			r.Inflate(-1, -1);
			DisplayHelp.DrawGradientRectangle(g, r, stateColors.InnerBorder, 1);
		}
	}
}
