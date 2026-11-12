using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Dashboard.Design.Editors.Helpers;

internal static class StiExpressionEditorHelper
{
	internal static bool RunExpressionEditor(StiElement element, ref HintTextBox textBox)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		string text = ((Control)textBox).Text;
		DialogResult val = StiTextEditor.EditText(element.Report, ref text, StiCodeEditorMode.EditorDashboardExpression, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)textBox).Text = text;
		}
		((Control)textBox).Focus();
		((TextBoxBase)textBox).Select(0, 0);
		return (int)val == 1;
	}
}
