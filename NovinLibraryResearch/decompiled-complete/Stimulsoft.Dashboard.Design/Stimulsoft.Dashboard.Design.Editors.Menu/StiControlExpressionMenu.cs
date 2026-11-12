using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public abstract class StiControlExpressionMenu : StiExpressionMenu
{
	protected override bool AllowCreateField => true;

	public override void CreateItems()
	{
		base.ExpressionMenu.SubItems.Clear();
		CreateNewFieldMenuItem();
		CreateRenameMenuItem();
		CreateRemoveMenuItem();
		CreateFieldMenuItem();
		CreateVariableMenuItem();
	}

	protected virtual void CreateNewFieldMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Dashboard", "NewField"),
			BeginGroup = true,
			Visible = (base.SelectedItem == null),
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

	protected override ButtonItem CreateRenameMenuItem()
	{
		ButtonItem buttonItem = base.CreateRenameMenuItem();
		buttonItem.BeginGroup = true;
		return buttonItem;
	}

	public StiControlExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiDataFilterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}

	public StiControlExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
