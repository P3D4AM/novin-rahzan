using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiFilterMultiOptionActionCheckBox : StiActionCheckBox
{
	public StiFilterMultiOptionActionCheckBox(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		((CheckBox)this).Appearance = (Appearance)1;
		((Control)this).AutoSize = false;
		base.ToolTip = Loc.Get("PropertyMain", "FilterMode");
	}
}
