namespace Stimulsoft.Controls.Win.DotNetBar;

internal interface IThemeCache
{
	ThemeWindow ThemeWindow { get; }

	ThemeRebar ThemeRebar { get; }

	ThemeToolbar ThemeToolbar { get; }

	ThemeHeader ThemeHeader { get; }

	ThemeScrollBar ThemeScrollBar { get; }

	ThemeExplorerBar ThemeExplorerBar { get; }

	ThemeProgress ThemeProgress { get; }

	ThemeButton ThemeButton { get; }
}
