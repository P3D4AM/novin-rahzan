using System.IO;
using System.Linq;
using Serilog.Parsing;

namespace Serilog.Rendering;

internal static class Padding
{
	private static readonly char[] PaddingChars = Enumerable.Repeat(' ', 80).ToArray();

	public static void Apply(TextWriter output, string value, in Alignment? alignment)
	{
		if (!alignment.HasValue || value.Length >= alignment.Value.Width)
		{
			output.Write(value);
			return;
		}
		int num = alignment.Value.Width - value.Length;
		if (alignment.Value.Direction == AlignmentDirection.Left)
		{
			output.Write(value);
		}
		if (num <= PaddingChars.Length)
		{
			output.Write(PaddingChars, 0, num);
		}
		else
		{
			output.Write(new string(' ', num));
		}
		if (alignment.Value.Direction == AlignmentDirection.Right)
		{
			output.Write(value);
		}
	}
}
