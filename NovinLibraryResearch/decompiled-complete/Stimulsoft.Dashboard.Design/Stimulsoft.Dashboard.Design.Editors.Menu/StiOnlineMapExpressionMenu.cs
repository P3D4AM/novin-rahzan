using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiOnlineMapExpressionMenu : StiExpressionMenu
{
	protected StiOnlineMapElement MapElement => base.Element as StiOnlineMapElement;

	protected StiOnlineMapElementEditorForm OnlineMapEditorForm => base.EditorForm as StiOnlineMapElementEditorForm;

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
			if (OnlineMapEditorForm.GisListBox == base.MeterList)
			{
				MapElement.Gis = new StiGisMapMeter();
			}
			if (OnlineMapEditorForm.GisDescriptionListBox == base.MeterList)
			{
				MapElement.Description = new StiGisMapDescriptionMeter();
			}
			else if (OnlineMapEditorForm.GisColorListBox == base.MeterList)
			{
				MapElement.GisColor = new StiGisMapColorMeter();
			}
			else if (OnlineMapEditorForm.GisLineSizeListBox == base.MeterList)
			{
				MapElement.LineSize = new StiGisMapLineSizeMeter();
			}
			else if (OnlineMapEditorForm.GisDescriptionListBox == base.MeterList)
			{
				MapElement.Description = new StiGisMapDescriptionMeter();
			}
			else if (OnlineMapEditorForm.LatitudeListBox == base.MeterList)
			{
				MapElement.Latitude = new StiLatitudeMapMeter();
			}
			else if (OnlineMapEditorForm.LongitudeListBox == base.MeterList)
			{
				MapElement.Longitude = new StiLongitudeMapMeter();
			}
			else if (OnlineMapEditorForm.LocationListBox == base.MeterList)
			{
				MapElement.Location = new StiLocationMapMeter();
			}
			else if (OnlineMapEditorForm.LocationValueListBox == base.MeterList)
			{
				MapElement.LocationValue = new StiLocationValueMapMeter();
			}
			else if (OnlineMapEditorForm.LocationColorMeterListBox == base.MeterList)
			{
				MapElement.LocationColorMeter = new StiLocationColorMapMeter();
			}
			else if (OnlineMapEditorForm.LocationArgumentMeterListBox == base.MeterList)
			{
				MapElement.LocationArgument = new StiLocationArgumentMapMeter();
			}
			base.EditorForm.ReadPropertiesFromElement();
			base.MeterList.SelectedMeterItem = base.MeterList.LastItem();
			((Control)base.ExpressionTextBox).Focus();
			((TextBoxBase)base.ExpressionTextBox).Select(0, 0);
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	public StiOnlineMapExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
