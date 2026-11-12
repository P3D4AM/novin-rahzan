using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiGaugeExpressionMenu : StiExpressionMenu
{
	protected StiGaugeElement GaugeElement => base.Element as StiGaugeElement;

	protected StiGaugeElementEditorForm GaugeEditorForm => base.EditorForm as StiGaugeElementEditorForm;

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
			if (GaugeEditorForm.ValuesListBox == base.MeterList)
			{
				GaugeElement.Value = new StiValueGaugeMeter
				{
					Expression = "Sum()"
				};
			}
			else if (GaugeEditorForm.TargetListBox == base.MeterList)
			{
				GaugeElement.Target = new StiTargetGaugeMeter
				{
					Expression = "Sum()"
				};
			}
			else if (GaugeEditorForm.SeriesListBox == base.MeterList)
			{
				GaugeElement.Series = new StiSeriesGaugeMeter();
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			GaugeEditorForm.RefreshExpression();
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

	public StiGaugeExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
