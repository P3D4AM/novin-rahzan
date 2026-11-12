using System;

namespace Stimulsoft.Base.Drawing;

internal static class StiBrushCreator
{
	public static StiBrush New(string identName)
	{
		if (string.IsNullOrEmpty(identName))
		{
			return null;
		}
		if (!Enum.TryParse<StiBrushIdent>(identName, ignoreCase: true, out var result))
		{
			return null;
		}
		return New(result);
	}

	public static StiBrush New(StiBrushIdent ident)
	{
		return ident switch
		{
			StiBrushIdent.Empty => new StiEmptyBrush(), 
			StiBrushIdent.Solid => new StiSolidBrush(), 
			StiBrushIdent.Gradient => new StiGradientBrush(), 
			StiBrushIdent.Glare => new StiGlareBrush(), 
			StiBrushIdent.Glass => new StiGlassBrush(), 
			StiBrushIdent.Hatch => new StiHatchBrush(), 
			_ => throw new NotSupportedException(), 
		};
	}
}
