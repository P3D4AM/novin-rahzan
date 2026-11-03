using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
public class StiSystemText : StiText
{
	public override StiComponentId ComponentId => StiComponentId.StiSystemText;

	public override int ToolboxPosition => 103;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiSystemText");

	public StiSystemText()
		: this(RectangleD.Empty, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiSystemText(RectangleD rect)
		: this(rect, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiSystemText(RectangleD rect, string text)
		: base(rect, text)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
