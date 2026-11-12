using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiListBoxExpressionMenu : StiControlExpressionMenu
{
	protected StiListBoxElement ListBoxElement => base.Element as StiListBoxElement;

	protected StiListBoxElementEditorForm ListBoxEditorForm => base.EditorForm as StiListBoxElementEditorForm;

	protected override void CreateNewField()
	{
		if (ListBoxEditorForm.KeysListBox == base.MeterList)
		{
			ListBoxElement.KeyMeter = new StiKeyListBoxMeter();
		}
		else if (ListBoxEditorForm.NamesListBox == base.MeterList)
		{
			ListBoxElement.NameMeter = new StiNameListBoxMeter();
		}
		base.EditorForm.ReadPropertiesFromElement();
		base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
	}

	public StiListBoxExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
