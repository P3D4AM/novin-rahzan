using System.Text.RegularExpressions;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime.Misc;

internal static class RegexOptionsHelper
{
	public static readonly RegexOptions Compiled;

	static RegexOptionsHelper()
	{
		Compiled = RegexOptions.Compiled;
	}
}
