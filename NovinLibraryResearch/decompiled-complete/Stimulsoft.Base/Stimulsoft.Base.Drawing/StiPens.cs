using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public static class StiPens
{
	public static Pen ControlText { get; } = new Pen(StiColors.ControlText);


	public static Pen Selected { get; } = new Pen(StiColors.Selected);


	public static Pen SelectedText { get; } = new Pen(StiColors.SelectedText);

}
