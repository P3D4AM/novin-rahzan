using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class SwitchButtonColorTable
{
	public Color TextColor = ColorScheme.GetColor(1980763);

	public Color OffTextColor = ColorScheme.GetColor(6715534);

	public Color OnTextColor = ColorScheme.GetColor(6715534);

	public Color BorderColor = ColorScheme.GetColor(11649238);

	public Color OffBackColor = ColorScheme.GetColor(15726333);

	public Color OnBackColor = ColorScheme.GetColor(9621584);

	public Color SwitchBorderColor = ColorScheme.GetColor(9808068);

	public Color SwitchBackColor = ColorScheme.GetColor(14411764);

	public static SwitchButtonColorTable Disabled
	{
		get
		{
			SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
			switchButtonColorTable.BorderColor = ColorScheme.GetColor(13684944);
			switchButtonColorTable.OffBackColor = ColorScheme.GetColor(15132390);
			switchButtonColorTable.OffTextColor = ColorScheme.GetColor(13684944);
			switchButtonColorTable.OnBackColor = ColorScheme.GetColor(15132390);
			switchButtonColorTable.OnTextColor = ColorScheme.GetColor(13684944);
			switchButtonColorTable.SwitchBackColor = ColorScheme.GetColor(15132390);
			switchButtonColorTable.SwitchBorderColor = ColorScheme.GetColor(13684944);
			switchButtonColorTable.TextColor = ColorScheme.GetColor(8884895);
			return switchButtonColorTable;
		}
	}
}
