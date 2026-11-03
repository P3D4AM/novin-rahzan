using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Components.MathFormula;

public static class StiMathFormulaHelper
{
	public static string GetSvgText(string latextContent, float fontSize, string colorHex)
	{
		if (!latextContent.StartsWith("\\begin{document} $\\["))
		{
			latextContent = "\\begin{document} $\\[ " + latextContent + " \\]$ \\end{document}";
		}
		string text = StiMathFormulaCache.GetLatextMatml(latextContent);
		if (string.IsNullOrEmpty(text))
		{
			text = StiMathHelper.GetMatmML(latextContent);
			StiMathFormulaCache.SetLatextMatml(latextContent, text);
		}
		string svg = StiMathFormulaCache.GetSvg(text, fontSize, colorHex);
		if (svg == null)
		{
			svg = StiMathHelper.GetSvg(text, fontSize, colorHex);
			StiMathFormulaCache.SetSvg(svg, latextContent, fontSize, colorHex);
		}
		return svg;
	}

	public static RectangleF GetSvgRect(StiMathFormula mathFormula, string svgMathFormula, RectangleF rect)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		SizeF sizeSVG = GetSizeSVG(svgMathFormula, (float)StiScale.Factor);
		float x = 0f;
		float y = 0f;
		StiHorAlignment horAlignment = mathFormula.HorAlignment;
		if ((int)horAlignment != 2)
		{
			if ((int)horAlignment == 3)
			{
				x = rect.Width - sizeSVG.Width;
			}
		}
		else
		{
			x = (rect.Width - sizeSVG.Width) / 2f;
		}
		StiVertAlignment vertAlignment = mathFormula.VertAlignment;
		if ((int)vertAlignment != 1)
		{
			if ((int)vertAlignment == 2)
			{
				y = rect.Height - sizeSVG.Height;
			}
		}
		else
		{
			y = (rect.Height - sizeSVG.Height) / 2f;
		}
		return new RectangleF(x, y, sizeSVG.Width, sizeSVG.Height);
	}

	private static SizeF GetSizeSVG(string svgText, float zoom)
	{
		float width = 0f;
		float height = 0f;
		string[] array = svgText.Split(new string[1] { "viewBox" }, StringSplitOptions.None);
		if (array.Length > 1)
		{
			width = GetWidth(array[0], zoom);
			height = GetHeight(array[0], zoom);
		}
		return new SizeF(width, height);
	}

	private static float GetWidth(string pathSVG, float zoom)
	{
		try
		{
			Regex regex = new Regex("(?<widthAtr>\\bwidth=\")(?<valueWidth>([0-9]*[.,][0-9]*|[0-9]*))");
			string widthText = "";
			regex.Replace(pathSVG, delegate(Match m)
			{
				widthText = m.Groups["valueWidth"].Value;
				return string.Empty;
			});
			if (float.TryParse(widthText, out var result))
			{
				float num = 1.3513514f;
				return result * zoom * num;
			}
			return 0f;
		}
		catch
		{
			return 0f;
		}
	}

	private static float GetHeight(string pathSVG, float zoom)
	{
		try
		{
			Regex regex = new Regex("(?<heightAtr>\\bheight=\")(?<valueHeight>([0-9]*[.,][0-9]*|[0-9]*))");
			string heightText = "";
			regex.Replace(pathSVG, delegate(Match m)
			{
				heightText = m.Groups["valueHeight"].Value;
				return string.Empty;
			});
			if (float.TryParse(heightText, out var result))
			{
				float num = 1.3513514f;
				return result * zoom * num;
			}
			return 0f;
		}
		catch
		{
			return 0f;
		}
	}
}
