using System;

namespace Stimulsoft.Base.Drawing;

internal static class StiBorderCreator
{
	public static StiBorder New(string identName)
	{
		if (string.IsNullOrEmpty(identName))
		{
			return null;
		}
		if (!Enum.TryParse<StiBorderIdent>(identName, ignoreCase: true, out var result))
		{
			return null;
		}
		return New(result);
	}

	public static StiBorder New(StiBorderIdent ident)
	{
		return ident switch
		{
			StiBorderIdent.Border => new StiBorder(), 
			StiBorderIdent.AdvancedBorder => new StiAdvancedBorder(), 
			_ => throw new NotSupportedException(), 
		};
	}
}
