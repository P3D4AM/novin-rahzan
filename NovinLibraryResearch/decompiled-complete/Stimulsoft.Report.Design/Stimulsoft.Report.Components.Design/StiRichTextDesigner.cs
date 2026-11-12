using System.Windows.Forms;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Components.Design;

public class StiRichTextDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (component is StiRichText)
		{
			return StiRichTextEditor.EditSystemText(component.Report, (StiRichText)component);
		}
		string text = ((IStiText)component).Text.Value;
		DialogResult val = StiRichTextEditor.EditText(component.Report, ref text, showExpressionButton: true);
		if ((int)val == 1)
		{
			((IStiText)component).Text.Value = text;
		}
		return val;
	}

	public StiRichTextDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
