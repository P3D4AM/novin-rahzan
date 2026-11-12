using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiRichTextBoxControlDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		string text = ((StiRichTextBoxControl)component).Text;
		DialogResult val = StiRichTextEditor.EditText(component.Report, ref text, showExpressionButton: false);
		if ((int)val == 1)
		{
			((StiRichTextBoxControl)component).Text = text;
		}
		return val;
	}

	public StiRichTextBoxControlDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
