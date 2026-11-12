using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface IColorTheme : ICloneable
{
	string Name { get; set; }

	bool ReadOnly { get; set; }

	Font Font { get; set; }

	LexStyle[] LexStyles { get; set; }

	ILexStyle this[string lexStyleName] { get; set; }

	bool Contains(string lexStyleName);
}
