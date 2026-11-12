using System.IO;

namespace Serilog.Sinks.SystemConsole.Themes;

public abstract class ConsoleTheme
{
	public static ConsoleTheme None { get; } = new EmptyConsoleTheme();


	public abstract bool CanBuffer { get; }

	protected abstract int ResetCharCount { get; }

	public abstract int Set(TextWriter output, ConsoleThemeStyle style);

	public abstract void Reset(TextWriter output);

	internal StyleReset Apply(TextWriter output, ConsoleThemeStyle style, ref int invisibleCharacterCount)
	{
		invisibleCharacterCount += Set(output, style);
		invisibleCharacterCount += ResetCharCount;
		return new StyleReset(this, output);
	}
}
