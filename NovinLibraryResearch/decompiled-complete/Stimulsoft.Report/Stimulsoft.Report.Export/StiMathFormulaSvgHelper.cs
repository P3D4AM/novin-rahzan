using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Export.Services.Helpers;

namespace Stimulsoft.Report.Export;

internal static class StiMathFormulaSvgHelper
{
	internal static void WriteEmptyDataMessage(XmlTextWriter writer, StiSvgData svgData)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		if (svgData.Width > 50.0)
		{
			writer.WriteStartElement("image");
			writer.WriteAttributeString("x", StiContextSvgHelper.DoubleToString(svgData.Width / 2.0 - 16.0));
			writer.WriteAttributeString("y", StiContextSvgHelper.DoubleToString(svgData.Height / 2.0 - 16.0));
			writer.WriteAttributeString("width", "32");
			writer.WriteAttributeString("height", "32");
			writer.WriteStartAttribute("href");
			writer.WriteString("data:image/png;base64," + Convert.ToBase64String(GetEmptyDataImage()));
			writer.WriteRaw("\r\n");
			writer.WriteEndAttribute();
			writer.WriteEndElement();
		}
		Font baseFont = new Font("Arial", 8f);
		string text = Loc.Get("PropertyMain", "NoElements");
		double num = MeasureTextWidth(text, baseFont);
		if (svgData.Width > num + 30.0)
		{
			writer.WriteStartElement("text");
			writer.WriteAttributeString("x", StiContextSvgHelper.DoubleToString(svgData.Width / 2.0 - num / 2.0));
			writer.WriteAttributeString("y", StiContextSvgHelper.DoubleToString(svgData.Height / 2.0 + 30.0));
			writer.WriteAttributeString("style", "font-size:8pt; font-family:'Arial'; fill: #a9a9a9;");
			writer.WriteRaw(text);
			writer.WriteEndElement();
		}
	}

	private static byte[] GetEmptyDataImage()
	{
		Assembly assembly = typeof(StiChartSvgHelper).Assembly;
		using Stream stream = assembly.GetManifestResourceStream("Stimulsoft.Report.Images.NotDefined.StiMathFormula_x2.png");
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		return array;
	}

	private static double MeasureTextWidth(string text, Font baseFont)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				return val2.MeasureString(text, baseFont).Width;
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
