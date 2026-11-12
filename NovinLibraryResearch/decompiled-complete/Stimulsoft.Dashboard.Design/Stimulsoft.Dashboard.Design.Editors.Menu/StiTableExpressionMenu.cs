using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiTableExpressionMenu : StiExpressionMenu
{
	protected StiTableElement TableElement => base.Element as StiTableElement;

	public override void CreateItems()
	{
		base.ExpressionMenu.SubItems.Clear();
		CreateNewDimensionMenuItem();
		CreateNewMeasureMenuItem();
		base.CreateItems();
	}

	protected virtual void CreateNewDimensionMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Dimension(),
			Text = Loc.Get("Dashboard", "NewDimension")
		};
		buttonItem.Click += delegate
		{
			TableElement.Columns.Add(new StiDimensionColumn
			{
				HorAlignment = StiHorAlignment.Left
			});
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			((Control)base.ExpressionTextBox).Focus();
			((TextBoxBase)base.ExpressionTextBox).Select(0, 0);
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	protected virtual void CreateNewMeasureMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Measure(),
			Text = Loc.Get("Dashboard", "NewMeasure")
		};
		buttonItem.Click += delegate
		{
			TableElement.Columns.Add(new StiMeasureColumn
			{
				Expression = "Sum()",
				HorAlignment = StiHorAlignment.Right
			});
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			((Control)base.ExpressionTextBox).Focus();
			((TextBoxBase)base.ExpressionTextBox).Select(4, 0);
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	public StiTableExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
