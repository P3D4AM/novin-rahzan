using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxAttribute
{
	string Name { get; set; }

	object Value { get; set; }

	Point Position { get; set; }
}
