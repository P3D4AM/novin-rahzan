using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Design;

public class MonthCalendarAdvDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is MonthCalendarAdv monthCalendarAdv)
		{
			monthCalendarAdv.BackgroundStyle.Class = ElementStyleClassKeys.MonthCalendarAdvKey;
			monthCalendarAdv.NavigationBackgroundStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
			monthCalendarAdv.NavigationBackgroundStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
			monthCalendarAdv.NavigationBackgroundStyle.BackColorGradientAngle = 90;
			((Control)monthCalendarAdv).AutoSize = true;
		}
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}
}
