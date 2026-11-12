using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Ribbon;

namespace Stimulsoft.Report.Design.Controls;

public sealed class StiStyleCollectionNameItem : StiCustomItem
{
	private static Image bitmap;

	public StiStyleCollectionNameItem()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		if (bitmap == null)
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
			bitmap = (Image)componentResourceManager.GetObject("pageOpen1.Image");
			if (StiScale.Id != 0)
			{
				bitmap = (Image)(object)StiImageUtils.ResizeImage(bitmap, StiScale.XXI(bitmap.Width), StiScale.YYI(bitmap.Height));
			}
		}
	}

	protected override void RenderButton(ItemPaintArgs p)
	{
		if (!base.IsOnGallery)
		{
			base.RenderButton(p);
		}
	}

	public override void Paint(ItemPaintArgs e)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		Rectangle bounds = Bounds;
		if (!e.IsOnMenu)
		{
			if (Enabled)
			{
				e.Graphics.FillRectangle(Brushes.White, bounds);
				e.Graphics.DrawRectangle(Pens.Silver, bounds);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.WhiteSmoke, bounds);
				e.Graphics.DrawRectangle(Pens.LightGray, bounds);
			}
		}
		base.Paint(e);
		e.Graphics.DrawImage(bitmap, new Rectangle(bounds.X + StiScale.XXI(7.0), bounds.Y + StiScale.YYI(3.0), bitmap.Width, bitmap.Height));
		bounds.X += bitmap.Width + StiScale.XXI(11.0);
		bounds.Width -= bitmap.Height + StiScale.YYI(6.0);
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.LineAlignment = (StringAlignment)1;
				e.Graphics.DrawString(Text, val, Brushes.Black, (RectangleF)bounds, val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
