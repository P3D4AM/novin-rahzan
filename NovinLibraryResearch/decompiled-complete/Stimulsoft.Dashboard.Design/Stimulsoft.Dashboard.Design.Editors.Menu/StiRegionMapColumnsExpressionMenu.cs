using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.RegionMap;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiRegionMapColumnsExpressionMenu : StiExpressionMenu
{
	protected StiRegionMapElement MapElement => base.Element as StiRegionMapElement;

	protected StiRegionMapElementEditorForm RegionMapEditorForm => base.EditorForm as StiRegionMapElementEditorForm;

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
			StiRegionMapDataControl regionControl = RegionMapEditorForm.RegionControl;
			if (regionControl != null)
			{
				StiRegionMapDataColumnsControl columnsControl = regionControl.ColumnsControl;
				if (columnsControl != null)
				{
					if (columnsControl.listBoxKey == base.MeterList)
					{
						MapElement.KeyMeter = new StiKeyMapMeter();
					}
					else if (columnsControl.listBoxName == base.MeterList)
					{
						MapElement.NameMeter = new StiNameMapMeter();
					}
					else if (columnsControl.listBoxValue == base.MeterList)
					{
						MapElement.ValueMeter = new StiValueMapMeter();
					}
					else if (columnsControl.listBoxGroup == base.MeterList)
					{
						MapElement.GroupMeter = new StiGroupMapMeter();
					}
					else if (columnsControl.listBoxColor == base.MeterList)
					{
						MapElement.ColorMeter = new StiColorMapMeter();
					}
					base.EditorForm.ReadPropertiesFromElement();
					base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
					((Control)base.ExpressionTextBox).Focus();
					if (((Control)base.ExpressionTextBox).Text == "Sum()")
					{
						((TextBoxBase)base.ExpressionTextBox).Select(4, 0);
					}
					else
					{
						((TextBoxBase)base.ExpressionTextBox).Select(0, 0);
					}
				}
			}
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	public StiRegionMapColumnsExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
