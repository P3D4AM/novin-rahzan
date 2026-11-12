using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiDataFiltersExpressionMenu : StiControlExpressionMenu
{
	protected IStiDataFilters DateFilters => base.Element as IStiDataFilters;

	protected StiDataFiltersEditorForm DataFilterEditorForm => base.EditorForm as StiDataFiltersEditorForm;

	public override void CreateItems()
	{
		base.ExpressionMenu.SubItems.Clear();
		CreateAddFilterMenuItem();
		CreateRemoveMenuItem();
		CreateFieldMenuItem();
	}

	protected virtual void CreateAddFilterMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Chart", "AddFilter"),
			BeginGroup = true
		};
		buttonItem.Click += delegate
		{
			if (DataFilterEditorForm.FiltersListBox == base.FilterList)
			{
				DateFilters.DataFilters.Add(new StiDataFilterRule
				{
					Operation = (base.EditorForm as StiDataFiltersEditorForm).GetSelectedFilterOperation()
				});
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.FilterList.SelectedFilterItem = base.FilterList.LastItem();
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	public StiDataFiltersExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiDataFilterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
