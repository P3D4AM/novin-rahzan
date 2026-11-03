using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Painters;

public class StiImageGdiPainter : StiViewGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiImage stiImage = component as StiImage;
		Graphics graphics = e.Graphics;
		RectangleD paintRectangle = stiImage.GetPaintRectangle();
		stiImage.UpdateImageToDrawInDesigner();
		base.Paint(component, e);
		if (!(((RectangleD)(ref paintRectangle)).Width > 0.0) || !(((RectangleD)(ref paintRectangle)).Height > 0.0))
		{
			return;
		}
		RectangleD clipRectangle = e.ClipRectangle;
		if ((!((RectangleD)(ref clipRectangle)).IsEmpty && !((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle)) || stiImage.ExistImageToDraw() || !stiImage.IsDesigning)
		{
			return;
		}
		string text = null;
		if (!string.IsNullOrEmpty(stiImage.DataColumn))
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "DataColumn"), stiImage.DataColumn);
		}
		else if (stiImage.ImageURL != null && !string.IsNullOrEmpty(stiImage.ImageURL.Value))
		{
			text = StiHyperlinkProcessor.HyperlinkToString(stiImage.ImageURL.Value);
		}
		else if (!string.IsNullOrEmpty(stiImage.File))
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "File"), stiImage.File);
		}
		else if (stiImage.ImageData != null && !string.IsNullOrEmpty(stiImage.ImageData.Value))
		{
			text = string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "ImageData"), StiExpressionPacker.PackExpression(stiImage.ImageData.Value, stiImage.Report.Designer, useBraces: true));
		}
		if (text != null)
		{
			Font val = new Font("Arial", 10f * (float)stiImage.Report.Info.Zoom);
			try
			{
				graphics.DrawString(text, val, Brushes.Black, ((RectangleD)(ref paintRectangle)).ToRectangleF());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		StiImage stiImage = (StiImage)component;
		stiImage.UpdateImageToDrawInDesigner();
		return base.GetImage(component, ref zoom, format);
	}
}
