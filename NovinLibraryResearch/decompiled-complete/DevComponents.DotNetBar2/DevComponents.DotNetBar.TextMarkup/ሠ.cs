using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሠ : ሡ
{
	protected override void SetFont(MarkupDrawContext d)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		Font val = d.CurrentFont;
		FontStyle val2 = (FontStyle)(d.CurrentFont.Style | 1);
		if (!val.Bold && val.FontFamily.IsStyleAvailable(val2))
		{
			d.CurrentFont = new Font(d.CurrentFont, val2);
		}
		else
		{
			val = null;
		}
		if (val != null)
		{
			ሢ = val;
		}
		base.SetFont(d);
	}
}
