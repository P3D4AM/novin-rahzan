using System.Drawing;

namespace Stimulsoft.Editor;

public class Consts
{
	public static Color DefaultControlBackColor = SystemColors.Window;

	public static Color DefaultControlForeColor = SystemColors.WindowText;

	public static Color DefaultHotTrackColor = SystemColors.HotTrack;

	public static Color DefaultWindowFrameColor = SystemColors.WindowFrame;

	public static Color DefaultPressedBackColor = Color.FromArgb(152, 181, 226);

	public static Color DefaultHotTrackBackColor = Color.FromArgb(193, 210, 238);

	public static Color DefaultXPItemsForeColor = Color.FromArgb(66, 142, 255);

	public static Color DefaultXPHeaderForeColor = Color.FromArgb(33, 93, 198);

	public static Color BlueItemsAreaColor = Color.FromArgb(214, 223, 247);

	public static Color HomeSteadItemsAreaColor = Color.FromArgb(246, 246, 236);

	public static Color MetallicItemsAreaColor = Color.FromArgb(240, 241, 245);

	public const char Space = ' ';

	public const char CR = '\r';

	public const char LF = '\n';

	public const char Tab = '\t';

	public const char AveChar = 'x';

	public const string CRLF = "\r\n";

	public const string TabStr = "\t";

	public static char[] crlfArray = new char[2] { '\r', '\n' };

	private Consts()
	{
	}
}
