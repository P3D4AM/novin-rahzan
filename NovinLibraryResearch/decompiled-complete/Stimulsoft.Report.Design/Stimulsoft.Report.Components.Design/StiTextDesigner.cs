using System.Windows.Forms;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Components.Design;

public class StiTextDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (component.Report.IsPageDesigner)
		{
			string text = ((StiText)component).Text.Value;
			if (text == null)
			{
				text = string.Empty;
			}
			DialogResult result = StiTextEditor.EditText(component.Report, ref text, StiCodeEditorMode.EditorExpression);
			((StiText)component).Text.Value = text;
			((StiText)component).ExcelDataValue = null;
			return result;
		}
		return StiTextEditor.EditSystemText(component.Report, component as StiText);
	}

	public StiTextDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
