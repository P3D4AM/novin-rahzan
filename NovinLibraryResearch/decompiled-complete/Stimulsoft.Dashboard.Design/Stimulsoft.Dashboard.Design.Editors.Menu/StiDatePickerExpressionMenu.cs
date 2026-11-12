using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiDatePickerExpressionMenu : StiControlExpressionMenu
{
	protected StiDatePickerElement DatePickerElement => base.Element as StiDatePickerElement;

	protected StiDatePickerElementEditorForm DatePickerEditorForm => base.EditorForm as StiDatePickerElementEditorForm;

	protected override void CreateNewField()
	{
		if (DatePickerEditorForm.ValuesListBox == base.MeterList)
		{
			DatePickerElement.ValueMeter = new StiValueDatePickerMeter();
		}
		base.EditorForm.ReadPropertiesFromElement();
		base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
	}

	public StiDatePickerExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
