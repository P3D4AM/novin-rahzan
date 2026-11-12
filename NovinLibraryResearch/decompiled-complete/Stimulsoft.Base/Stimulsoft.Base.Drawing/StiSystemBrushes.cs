using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public sealed class StiSystemBrushes
{
	public static Brush Window { get; } = (Brush)new SolidBrush(SystemColors.Window);

}
