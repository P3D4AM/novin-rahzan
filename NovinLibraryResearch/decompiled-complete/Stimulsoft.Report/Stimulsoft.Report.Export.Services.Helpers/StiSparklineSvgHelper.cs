using System;
using System.Drawing;
using System.Xml;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export.Services.Helpers;

public static class StiSparklineSvgHelper
{
	public static void WriteSparkline(XmlTextWriter writer, StiSvgData svgData)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		StiSparkline stiSparkline = svgData.Component as StiSparkline;
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, stiSparkline.IsPrinting, 1f);
				StiSparklineGdiPainter.RenderSparkline(context, new RectangleD(svgData.X, svgData.Y, svgData.Width, svgData.Height), stiSparkline, 1f);
				StiContextSvgHelper.WriteGeoms(writer, context, needAnimation: false);
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
