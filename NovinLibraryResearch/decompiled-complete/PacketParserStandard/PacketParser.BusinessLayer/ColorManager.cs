using System.Drawing;

namespace PacketParser.BusinessLayer;

public class ColorManager
{
	public Color BackColor { get; private set; }

	public Color ForeColor { get; private set; }

	public short Ordinal { get; private set; }

	public ColorManager(Color bColor, Color fColor, short ordinal)
	{
		BackColor = bColor;
		ForeColor = fColor;
		Ordinal = ordinal;
	}
}
