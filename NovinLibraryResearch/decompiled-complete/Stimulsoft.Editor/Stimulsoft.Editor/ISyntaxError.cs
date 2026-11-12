using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxError
{
	string Name { get; set; }

	string Description { get; set; }

	Point Position { get; set; }

	Size Size { get; set; }

	IRange Range { get; set; }
}
