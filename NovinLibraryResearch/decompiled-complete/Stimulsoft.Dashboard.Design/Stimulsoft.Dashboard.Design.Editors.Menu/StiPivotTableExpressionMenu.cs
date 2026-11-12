using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiPivotTableExpressionMenu : StiExpressionMenu
{
	protected StiPivotTableElement PivotTableElement => base.Element as StiPivotTableElement;

	protected StiPivotTableElementDesignerForm PivotTableEditorForm => base.EditorForm as StiPivotTableElementDesignerForm;

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
			if (PivotTableEditorForm.listBoxRows == base.MeterList)
			{
				PivotTableElement.Rows.Add(new StiPivotRow());
			}
			else if (PivotTableEditorForm.listBoxColumns == base.MeterList)
			{
				PivotTableElement.Columns.Add(new StiPivotColumn());
			}
			else if (PivotTableEditorForm.listBoxSummaries == base.MeterList)
			{
				PivotTableElement.Summaries.Add(new StiPivotSummary
				{
					Expression = "Sum()"
				});
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			PivotTableEditorForm.RefreshExpression();
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

	protected override IEnumerable<string> GetFuncs()
	{
		List<string> list = new List<string>();
		if (base.Meter == null)
		{
			return null;
		}
		if (base.Meter is IStiDimensionMeter && StiDataExpressionHelper.IsDateDataColumnInExpression(base.Dashboard, base.Meter.Expression))
		{
			list = list.Union(Funcs.GetDateDimensionFunctions()).ToList();
		}
		if (!(base.Meter is StiSparklinesColumn))
		{
			if (PivotTableElement.Columns.Contains(base.Meter) || PivotTableElement.Rows.Contains(base.Meter))
			{
				list = list.Union(Funcs.GetCommonMeasureFunctions()).ToList();
			}
			else if (StiDataExpressionHelper.IsNumericDataColumnInExpression(base.Dashboard, base.Meter.Expression))
			{
				list = list.Union(Funcs.GetAggregateMeasureFunctions()).ToList();
				list.Add("PercentOfGrandTotal");
			}
			else
			{
				list = list.Union(Funcs.GetCommonMeasureFunctions()).ToList();
			}
		}
		if (!list.Any())
		{
			return null;
		}
		return list;
	}

	public StiPivotTableExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
