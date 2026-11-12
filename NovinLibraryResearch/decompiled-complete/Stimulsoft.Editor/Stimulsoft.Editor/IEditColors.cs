using System.Drawing;

namespace Stimulsoft.Editor;

public interface IEditColors
{
	Color ReadonlyForeColor { get; set; }

	Color ReadonlyBackColor { get; set; }

	Color DisabledForeColor { get; set; }

	Color DisabledBackColor { get; set; }
}
