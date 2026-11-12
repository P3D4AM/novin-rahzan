using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Check;

public abstract class StiCheck : ICloneable
{
	private List<StiAction> actions;

	public object Element { get; set; }

	public virtual bool PreviewVisible => false;

	public abstract string ElementName { get; }

	public abstract string ShortMessage { get; }

	public abstract string LongMessage { get; }

	public abstract StiCheckStatus Status { get; }

	public abstract StiCheckObjectType ObjectType { get; }

	public virtual bool DefaultStateEnabled => true;

	public bool Enabled
	{
		get
		{
			StiSettings.Load();
			string subkey = GetType().ToString();
			return StiSettings.GetBool("ReportChecks", subkey, DefaultStateEnabled);
		}
		set
		{
			if (value != Enabled)
			{
				StiSettings.Load();
				string subkey = GetType().ToString();
				if (value)
				{
					StiSettings.ClearKey("ReportChecks", subkey);
				}
				else
				{
					StiSettings.Set("ReportChecks", subkey, false);
				}
				StiSettings.Save();
			}
		}
	}

	public List<StiAction> Actions => actions ?? (actions = new List<StiAction>());

	public object Clone()
	{
		return MemberwiseClone() as StiCheck;
	}

	public abstract object ProcessCheck(StiReport report, object obj);

	public virtual void CreatePreviewImage(out Image elementImage, out Image highlightedElementImage, bool useScale = false)
	{
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		elementImage = null;
		highlightedElementImage = null;
		if (!(Element is StiComponent stiComponent))
		{
			return;
		}
		int num = 500;
		int num2 = 500;
		if (useScale)
		{
			num *= 2;
			num2 *= 2;
		}
		if (Element is StiPage)
		{
			StiGdiPainter stiGdiPainter = StiPainter.GetPainter(stiComponent.GetType(), StiGuiMode.Gdi) as StiGdiPainter;
			elementImage = (Image)(object)stiGdiPainter.GetThumbnail(stiComponent, num, num2, isDesignTime: true);
		}
		else
		{
			StiGdiPainter stiGdiPainter2 = StiPainter.GetPainter(stiComponent.Page.GetType(), StiGuiMode.Gdi) as StiGdiPainter;
			elementImage = (Image)(object)stiGdiPainter2.GetThumbnail(stiComponent.Page, num, num2, isDesignTime: true);
		}
		if (Element is StiPage)
		{
			return;
		}
		StiGdiPainter stiGdiPainter3 = StiPainter.GetPainter(stiComponent.Page.GetType(), StiGuiMode.Gdi) as StiGdiPainter;
		Bitmap thumbnail = stiGdiPainter3.GetThumbnail(stiComponent.Page, num, num2, isDesignTime: true);
		StiPage page = stiComponent.Page;
		RectangleD paintRectangle = stiComponent.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
		paintRectangle.Y += page.Margins.Top;
		paintRectangle.X += page.Margins.Left;
		RectangleD displayRectangle = page.DisplayRectangle;
		double num3 = (double)((Image)thumbnail).Width / displayRectangle.Width;
		double num4 = (double)((Image)thumbnail).Height / displayRectangle.Height;
		paintRectangle.X *= num3;
		paintRectangle.Y *= num4;
		paintRectangle.Width *= num3;
		paintRectangle.Height *= num4;
		Graphics val = Graphics.FromImage((Image)(object)thumbnail);
		try
		{
			Pen val2 = new Pen(Color.FromArgb(2013200384));
			try
			{
				val2.Width = 3f;
				val.DrawRectangle(val2, (float)paintRectangle.X - 2f, (float)paintRectangle.Y - 2f, (float)paintRectangle.Width + 3f, (float)paintRectangle.Height + 3f);
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
		highlightedElementImage = (Image)(object)thumbnail;
	}
}
