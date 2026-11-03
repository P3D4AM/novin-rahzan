using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሣ : ሡ
{
	protected override void SetFont(MarkupDrawContext d)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		Font val = d.CurrentFont;
		FontStyle val2 = (FontStyle)(val.Style | 2);
		if (!val.Italic && d.CurrentFont.FontFamily.IsStyleAvailable(val2))
		{
			d.CurrentFont = new Font(val, val2);
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
