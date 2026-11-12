using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiIndicatorExpressionMenu : StiExpressionMenu
{
	protected StiIndicatorElement IndicatorElement => base.Element as StiIndicatorElement;

	protected StiIndicatorElementEditorForm IndicatorEditorForm => base.EditorForm as StiIndicatorElementEditorForm;

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
			if (IndicatorEditorForm.ValuesListBox == base.MeterList)
			{
				IndicatorElement.Value = new StiValueIndicatorMeter
				{
					Expression = "Sum()"
				};
			}
			else if (IndicatorEditorForm.TargetsListBox == base.MeterList)
			{
				IndicatorElement.Target = new StiTargetIndicatorMeter
				{
					Expression = "Sum()"
				};
			}
			else if (IndicatorEditorForm.SeriesListBox == base.MeterList)
			{
				IndicatorElement.Series = new StiSeriesIndicatorMeter();
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			IndicatorEditorForm.RefreshExpression();
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

	public StiIndicatorExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
