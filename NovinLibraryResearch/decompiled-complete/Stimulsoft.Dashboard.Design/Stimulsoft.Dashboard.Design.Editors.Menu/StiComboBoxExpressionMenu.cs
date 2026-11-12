using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiComboBoxExpressionMenu : StiControlExpressionMenu
{
	protected StiComboBoxElement ComboBoxElement => base.Element as StiComboBoxElement;

	protected StiComboBoxElementEditorForm ComboBoxEditorForm => base.EditorForm as StiComboBoxElementEditorForm;

	protected override void CreateNewField()
	{
		if (ComboBoxEditorForm.KeysListBox == base.MeterList)
		{
			ComboBoxElement.KeyMeter = new StiKeyComboBoxMeter();
		}
		else if (ComboBoxEditorForm.NamesListBox == base.MeterList)
		{
			ComboBoxElement.NameMeter = new StiNameComboBoxMeter();
		}
		base.EditorForm.ReadPropertiesFromElement();
		base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
	}

	public StiComboBoxExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
