using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public sealed class StiOffsetHelper
{
	public static Rectangle OffsetRect(Rectangle rect, Rectangle offsettingRectangle)
	{
		return new Rectangle(rect.X - offsettingRectangle.X, rect.Y - offsettingRectangle.Y, rect.Width + offsettingRectangle.Width, rect.Height + offsettingRectangle.Height);
	}
}
