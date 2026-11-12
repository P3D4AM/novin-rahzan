namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeProgressParts : ThemePart
{
	public static ThemeProgressParts Bar = new ThemeProgressParts(1);

	public static ThemeProgressParts BarVert = new ThemeProgressParts(2);

	public static ThemeProgressParts Chunk = new ThemeProgressParts(3);

	public static ThemeProgressParts ChunkVert = new ThemeProgressParts(4);

	protected ThemeProgressParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
