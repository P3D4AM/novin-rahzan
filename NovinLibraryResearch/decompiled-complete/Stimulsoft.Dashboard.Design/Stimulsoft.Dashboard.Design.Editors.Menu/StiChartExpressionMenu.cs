using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiChartExpressionMenu : StiExpressionMenu
{
	protected StiChartElement ChartElement => base.Element as StiChartElement;

	protected StiChartElementEditorForm ChartEditorForm => base.EditorForm as StiChartElementEditorForm;

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
			if (ChartEditorForm.ValuesListBox == base.MeterList)
			{
				StiValueChartMeter stiValueChartMeter = new StiValueChartMeter
				{
					Expression = "Sum()"
				};
				stiValueChartMeter.SeriesType = (ChartEditorForm.OldSeriesType.HasValue ? ChartEditorForm.OldSeriesType.Value : stiValueChartMeter.SeriesType);
				ChartElement.Values.Add(stiValueChartMeter);
			}
			else if (ChartEditorForm.EndValuesListBox == base.MeterList)
			{
				ChartElement.EndValues.Add(new StiEndValueChartMeter
				{
					Expression = "Sum()"
				});
			}
			else if (ChartEditorForm.CloseValuesListBox == base.MeterList)
			{
				ChartElement.CloseValues.Add(new StiCloseValueChartMeter
				{
					Expression = "Sum()"
				});
			}
			else if (ChartEditorForm.LowValuesListBox == base.MeterList)
			{
				ChartElement.LowValues.Add(new StiLowValueChartMeter
				{
					Expression = "Sum()"
				});
			}
			else if (ChartEditorForm.HighValuesListBox == base.MeterList)
			{
				ChartElement.HighValues.Add(new StiHighValueChartMeter
				{
					Expression = "Sum()"
				});
			}
			else if (ChartEditorForm.ArgumentsListBox == base.MeterList)
			{
				ChartElement.Arguments.Add(new StiArgumentChartMeter());
			}
			else if (ChartEditorForm.WeightsListBox == base.MeterList)
			{
				ChartElement.Weights.Add(new StiWeightChartMeter
				{
					Expression = "Sum()"
				});
			}
			else if (ChartEditorForm.SeriesListBox == base.MeterList)
			{
				ChartElement.Series = new StiSeriesChartMeter();
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			ChartEditorForm.RefreshExpression();
			ChartEditorForm.RefreshUiPanel();
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

	public StiChartExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
