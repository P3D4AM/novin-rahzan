using System;

namespace Stimulsoft.Editor;

public interface IColorThemes : ICloneable
{
	int ActiveThemeIndex { get; set; }

	IColorTheme ActiveTheme { get; set; }

	IColorTheme[] Themes { get; set; }

	void Add(IColorTheme colorTheme);

	bool Remove(int index);

	void Clear();
}
