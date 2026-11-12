using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiImageDataColumnEditor : StiImageEditor
{
	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value != null)
		{
			Rectangle bounds = e.Bounds;
			Graphics graphics = e.Graphics;
			graphics.FillRectangle(Brushes.WhiteSmoke, bounds);
			if (StiScale.Id == SystemScaleID.x1)
			{
				int width = StiDataBuilder.Images.ImageSize.Width;
				int height = StiDataBuilder.Images.ImageSize.Height;
				int x = bounds.X + (bounds.Width - width) / 2;
				int y = bounds.Y + (bounds.Height - height) / 2;
				Rectangle rectangle = new Rectangle(x, y, width, height);
				graphics.DrawImage(StiDataBuilder.Images.Images["DataColumn"], rectangle);
			}
			else
			{
				bounds = new Rectangle(bounds.X + 2, bounds.Y + (bounds.Height - StiScale.I5) / 2, bounds.Width - 4, StiScale.I5);
				graphics.FillRectangle(Brushes.White, bounds);
				bounds.Width--;
				bounds.Height--;
				graphics.DrawRectangle(Pens.Gray, bounds);
				graphics.DrawLine(Pens.Gray, bounds.X + StiScale.I2, bounds.Y + bounds.Height / 2, bounds.Right - StiScale.I2, bounds.Y + bounds.Height / 2);
			}
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
