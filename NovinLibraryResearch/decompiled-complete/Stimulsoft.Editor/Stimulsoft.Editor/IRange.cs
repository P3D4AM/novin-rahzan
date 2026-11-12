using System.Drawing;

namespace Stimulsoft.Editor;

public interface IRange
{
	Point StartPoint { get; set; }

	Point EndPoint { get; set; }

	Size Size { get; set; }

	bool IsEmpty { get; }
}
