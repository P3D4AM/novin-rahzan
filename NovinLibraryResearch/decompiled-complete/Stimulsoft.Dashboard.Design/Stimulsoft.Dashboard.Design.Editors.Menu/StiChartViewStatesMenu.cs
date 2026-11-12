using System;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiChartViewStatesMenu : StiExpressionMenu
{
	protected StiChartElement ChartElement => base.Element as StiChartElement;

	protected StiChartElementEditorForm ChartEditorForm => base.EditorForm as StiChartElementEditorForm;

	public override void CreateItems()
	{
		base.ExpressionMenu.SubItems.Clear();
		CreateNewItemMenuItem();
		CreateDuplicateItemMenuItem();
		if (ChartElement.UserViewStates.Count <= 1)
		{
			return;
		}
		int num = 0;
		foreach (StiUserViewState userViewState in ChartElement.UserViewStates)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				BeginGroup = (num == 0),
				Text = userViewState.Name,
				Checked = (ChartElement.SelectedViewStateKey == userViewState.Key),
				Tag = userViewState
			};
			buttonItem.Click += Item_Click;
			base.ExpressionMenu.SubItems.Add(buttonItem);
			num++;
		}
	}

	private void Item_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiUserViewState stiUserViewState = buttonItem.Tag as StiUserViewState;
		ChartElement.SwitchSelectedViewState(stiUserViewState?.Key);
		ChartEditorForm.ReadPropertiesFromElement();
		ChartEditorForm.RefreshUiPanel();
	}

	protected virtual void CreateNewItemMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Gui", "colorpickerdialog_newcolorlabel"),
			Enabled = (ChartElement.UserViewStates != null && ChartElement.UserViewStates.Count < 5)
		};
		buttonItem.Click += delegate
		{
			CreateNewItem();
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	protected virtual void CreateDuplicateItemMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Buttons", "Duplicate"),
			Enabled = (ChartElement.UserViewStates != null && ChartElement.UserViewStates.Count < 5)
		};
		buttonItem.Click += delegate
		{
			CreateNewItem(isDuplicating: true);
		};
		base.ExpressionMenu.SubItems.Add(buttonItem);
	}

	private void CreateNewItem(bool isDuplicating = false)
	{
		JObject jObject = (isDuplicating ? StiChartUserViewStatesHelper.SaveToJsonObject(ChartElement) : null);
		string text = Loc.Get("PropertyCategory", "ViewCategory");
		if (ChartElement.UserViewStates.Count == 0)
		{
			ChartElement.UserViewStates.Add(new StiUserViewState
			{
				SeriesType = ((ChartEditorForm.ChartValue == null) ? StiChartSeriesType.ClusteredColumn : ChartEditorForm.ChartValue.SeriesType),
				Name = text + " 1"
			});
		}
		string name = GetName(text);
		StiUserViewState stiUserViewState = new StiUserViewState();
		stiUserViewState.Name = name;
		stiUserViewState.SeriesType = ((ChartEditorForm.ChartValue == null) ? StiChartSeriesType.ClusteredColumn : ChartEditorForm.ChartValue.SeriesType);
		ChartElement.UserViewStates.Add(stiUserViewState);
		ChartElement.SwitchSelectedViewState(stiUserViewState?.Key);
		if (isDuplicating)
		{
			StiChartUserViewStatesHelper.LoadFromJsonObject(ChartElement, jObject);
		}
		ChartEditorForm.ReadPropertiesFromElement();
		ChartEditorForm.RefreshUiPanel();
		((Control)base.ExpressionTextBox).Focus();
		((TextBoxBase)base.ExpressionTextBox).Select(0, 0);
	}

	private string GetName(string itemStr)
	{
		int num = 2;
		string name = $"{itemStr} {num}";
		while (ChartElement.UserViewStates.Any((StiUserViewState v) => v.Name == name))
		{
			name = $"{itemStr} {++num}";
		}
		return name;
	}

	public StiChartViewStatesMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm form, StiMeterListBox list)
		: base(expressionMenu, expressionTextBox, form, list)
	{
	}
}
