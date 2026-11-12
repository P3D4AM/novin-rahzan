using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiTreeViewExpressionMenu : StiControlExpressionMenu
{
	protected StiTreeViewElement TreeViewElement => base.Element as StiTreeViewElement;

	protected StiTreeViewElementEditorForm TreeViewEditorForm => base.EditorForm as StiTreeViewElementEditorForm;

	protected override bool AllowMarkDataColumn => false;

	public override void CreateItems()
	{
		base.ExpressionMenu.SubItems.Clear();
		CreateNewFieldMenuItem();
		CreateRenameMenuItem();
		CreateRemoveMenuItem();
		CreateFieldMenuItem();
		CreateVariableMenuItem();
	}

	protected override void CreateNewFieldMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Dashboard", "NewField"),
			BeginGroup = true,
			Tag = ((base.Report.Dictionary.DataSources.Count == 1) ? base.Report.Dictionary.DataSources.ToList().FirstOrDefault() : null)
		};
		if (base.Report.Dictionary.DataSources.Count > 0)
		{
			if (buttonItem.Tag is StiDataSource)
			{
				buttonItem.PopupOpen += base.MenuItemDataSource_PopupOpen;
			}
			else
			{
				buttonItem.PopupOpen += base.MenuItemField_PopupOpen;
			}
			buttonItem.SubItems.Add(new ButtonItem());
		}
		else
		{
			buttonItem.Click += base.MenuItemField_Click;
		}
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	protected override void CreateNewField()
	{
		TreeViewElement.KeyMeters.Add(new StiKeyTreeViewMeter());
		base.EditorForm.ReadPropertiesFromElement();
		base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
	}

	protected override bool IsNeedCreateField(ButtonItem item)
	{
		if (base.Meter != null)
		{
			return IsNewFieldItem(item.Parent as ButtonItem);
		}
		return true;
	}

	public StiTreeViewExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
