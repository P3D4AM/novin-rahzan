using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Components.MathFormula;

internal static class StiMathFormulaCache
{
	private struct StiCacheSvgKey
	{
		public string Formula { get; set; }

		public float FontSize { get; set; }

		public string ColorHex { get; set; }

		public void Create(string latextContent, float fontSize, string colorHex)
		{
			FontSize = fontSize;
			Formula = latextContent;
			ColorHex = colorHex;
		}
	}

	private struct StiCacheKey
	{
		public string Formula { get; set; }

		public SizeD Size { get; set; }

		public float FontSize { get; set; }

		public int Argb { get; set; }

		public StiVertAlignment VertAlignment { get; set; }

		public StiHorAlignment HorAlignment { get; set; }

		public double Zoom { get; set; }

		public void Create(StiMathFormula mathFormula)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			VertAlignment = mathFormula.VertAlignment;
			HorAlignment = mathFormula.HorAlignment;
			Argb = StiBrush.ToColor(mathFormula.TextBrush).ToArgb();
			FontSize = mathFormula.Font.Size;
			RectangleD clientRectangle = mathFormula.ClientRectangle;
			Size = ((RectangleD)(ref clientRectangle)).Size;
			Formula = ((mathFormula.Report != null && mathFormula.Report.IsDesigning) ? mathFormula.LaTexExpression : mathFormula.Value);
			Zoom = mathFormula.Report.Info.Zoom;
		}
	}

	private static Dictionary<StiCacheSvgKey, string> svgCache = new Dictionary<StiCacheSvgKey, string>();

	private static Dictionary<StiCacheKey, Bitmap> imageCache = new Dictionary<StiCacheKey, Bitmap>();

	private static Hashtable latextMathMLCache = new Hashtable();

	internal static void Clear()
	{
		svgCache.Clear();
		imageCache.Values.ToList().ForEach(delegate(Bitmap v)
		{
			((Image)v).Dispose();
		});
		imageCache.Clear();
	}

	internal static string GetSvg(string latextContent, float fontSize, string colorHex)
	{
		lock (svgCache)
		{
			StiCacheSvgKey key = default(StiCacheSvgKey);
			key.Create(latextContent, fontSize, colorHex);
			if (svgCache.ContainsKey(key))
			{
				return svgCache[key];
			}
			return null;
		}
	}

	internal static void SetSvg(string svg, string latextContent, float fontSize, string colorHex)
	{
		lock (svgCache)
		{
			StiCacheSvgKey key = default(StiCacheSvgKey);
			key.Create(latextContent, fontSize, colorHex);
			svgCache[key] = svg;
		}
	}

	internal static Bitmap GetImage(StiMathFormula mathFormula)
	{
		lock (imageCache)
		{
			StiCacheKey key = default(StiCacheKey);
			key.Create(mathFormula);
			if (imageCache.ContainsKey(key))
			{
				return imageCache[key];
			}
			return null;
		}
	}

	internal static void SetImage(StiMathFormula mathFormula, string svgMathFormula, RectangleF rect)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		lock (imageCache)
		{
			Bitmap val = new Bitmap((int)rect.Width, (int)rect.Height);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			RectangleF svgRect = StiMathFormulaHelper.GetSvgRect(mathFormula, svgMathFormula, rect);
			StiSvgHelper.DrawWithSvg(svgMathFormula, new RectangleF(0f, 0f, rect.Width, rect.Height), svgRect, mathFormula.Report.Info.Zoom, val2);
			SetImage(mathFormula, val);
		}
	}

	internal static void SetImage(StiMathFormula mathFormula, Bitmap image)
	{
		StiCacheKey key = default(StiCacheKey);
		key.Create(mathFormula);
		imageCache[key] = image;
	}

	internal static void SetLatextMatml(string latextContent, string matmMLContent)
	{
		latextMathMLCache.Add(latextContent, matmMLContent);
	}

	internal static string GetLatextMatml(string latextContent)
	{
		if (latextMathMLCache.ContainsKey(latextContent))
		{
			return latextMathMLCache[latextContent] as string;
		}
		return null;
	}
}
