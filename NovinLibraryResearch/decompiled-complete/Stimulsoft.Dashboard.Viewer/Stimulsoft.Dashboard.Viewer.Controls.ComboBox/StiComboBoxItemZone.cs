using System.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.ComboBox;

internal class StiComboBoxItemZone
{
	public string Text { get; private set; }

	public Rectangle Rect { get; private set; }

	public object Item { get; private set; }

	public bool IsAll { get; private set; }

	public bool IsStacked { get; private set; }

	internal static StiComboBoxItemZone MakeAll(Rectangle rect)
	{
		return new StiComboBoxItemZone
		{
			Text = StiLocHelper.LocAll,
			IsAll = true,
			Rect = rect
		};
	}

	internal static StiComboBoxItemZone MakeStacked(string text, Rectangle rect)
	{
		return new StiComboBoxItemZone
		{
			Text = text,
			IsStacked = true,
			Rect = rect
		};
	}

	internal static StiComboBoxItemZone Make(string text, Rectangle rect, object item)
	{
		return new StiComboBoxItemZone
		{
			Text = text,
			Rect = rect,
			Item = item
		};
	}

	private StiComboBoxItemZone()
	{
	}
}
