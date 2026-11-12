using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiProgressExpressionMenu : StiExpressionMenu
{
	protected StiProgressElement ProgressElement => base.Element as StiProgressElement;

	protected StiProgressElementEditorForm ProgressEditorForm => base.EditorForm as StiProgressElementEditorForm;

	public override void CreateItems()
	{
		base.ExpressionMenu.SubItems.Clear();
		CreateNewFieldMenuItem();
		base.CreateItems();
	}

	protected virtual void CreateNewFieldMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Dashboard", "NewField")
		};
		buttonItem.Click += delegate
		{
			if (ProgressEditorForm.ValuesListBox == base.MeterList)
			{
				ProgressElement.Value = new StiValueProgressMeter
				{
					Expression = "Sum()"
				};
			}
			else if (ProgressEditorForm.TargetsListBox == base.MeterList)
			{
				ProgressElement.Target = new StiTargetProgressMeter
				{
					Expression = "Sum()"
				};
			}
			else if (ProgressEditorForm.SeriesListBox == base.MeterList)
			{
				ProgressElement.Series = new StiSeriesProgressMeter();
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			ProgressEditorForm.RefreshExpression();
			((Control)base.ExpressionTextBox).Focus();
			if (((Control)base.ExpressionTextBox).Text == "Sum()")
			{
				((TextBoxBase)base.ExpressionTextBox).Select(4, 0);
			}
			else
			{
				((TextBoxBase)base.ExpressionTextBox).Select(0, 0);
			}
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	public StiProgressExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
