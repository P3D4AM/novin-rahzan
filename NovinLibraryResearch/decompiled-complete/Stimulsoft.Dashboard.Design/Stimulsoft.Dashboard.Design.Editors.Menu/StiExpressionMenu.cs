using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public abstract class StiExpressionMenu
{
	protected StiMeter Meter => MeterList?.SelectedMeterItem?.Meter;

	protected StiDataFilterRule Filter => FilterList?.SelectedFilterItem?.Filter;

	protected IStiElement Element => EditorForm.Element;

	protected IStiDashboard Dashboard => Element.Page as IStiDashboard;

	protected StiReport Report => Element.Report?.Dictionary?.Report;

	protected StiElementEditorForm EditorForm { get; }

	protected ButtonItem ExpressionMenu { get; }

	protected TextBox ExpressionTextBox { get; }

	protected StiDataFilterListBox FilterList { get; }

	protected StiMeterListBox MeterList { get; }

	protected StiMeterItem SelectedMeterItem => MeterList?.SelectedMeterItem;

	protected StiDataFilterItem SelectedFilterItem => FilterList?.SelectedFilterItem;

	protected object SelectedItem
	{
		get
		{
			if (SelectedMeterItem == null)
			{
				return SelectedFilterItem;
			}
			return SelectedMeterItem;
		}
	}

	protected bool? UseAliases => Report?.Designer?.UseAliases;

	protected string LocField => Loc.GetMain("Field");

	protected string LocVariable => Loc.GetMain("Variable");

	protected virtual bool AllowCreateField => false;

	protected virtual bool AllowMarkDataColumn => true;

	protected virtual bool AllowMarkVariable => true;

	public virtual void CreateItems()
	{
		CreateDuplicateFieldMenuItem();
		CreateRenameMenuItem();
		CreateRemoveMenuItem();
		CreateRemoveAllFieldsMenuItem();
		CreateFieldMenuItem();
		CreateVariableMenuItem();
		CreateFuncMenuItems();
	}

	protected virtual void CreateFuncMenuItems()
	{
		IEnumerable<string> funcs = GetFuncs();
		if (funcs == null)
		{
			return;
		}
		string text = Funcs.GetDateDimensionFunctions().LastOrDefault();
		int num = 0;
		string function = StiExpressionHelper.GetFunction(((Control)ExpressionTextBox).Text);
		string text2 = null;
		foreach (string item in funcs)
		{
			bool flag = function != null && Funcs.ToLowerCase(item) == Funcs.ToLowerCase(function);
			ButtonItem buttonItem = new ButtonItem
			{
				AutoCollapseOnClick = false,
				AutoCheckOnClick = true,
				BeginGroup = (num == 0 || text2 == text),
				Checked = flag,
				FontBold = flag,
				HotFontBold = flag,
				Text = Funcs.Localize(item),
				Tag = item
			};
			buttonItem.Click += delegate(object sender, EventArgs args)
			{
				ButtonItem senderMenuItem = sender as ButtonItem;
				senderMenuItem.Checked = true;
				(from ButtonItem b in senderMenuItem.Parent.SubItems
					where b != senderMenuItem
					select b).ToList().ForEach(delegate(ButtonItem b)
				{
					bool flag6 = (b.Checked = false);
					bool fontBold2 = (b.HotFontBold = flag6);
					b.FontBold = fontBold2;
				});
				ButtonItem buttonItem2 = senderMenuItem;
				bool fontBold = (senderMenuItem.HotFontBold = true);
				buttonItem2.FontBold = fontBold;
				string text3 = (string)senderMenuItem.Tag;
				bool flag3 = Funcs.GetDateDimensionFunctions().Contains(text3);
				bool flag4 = Funcs.GetAggregateMeasureFunctions().Contains(text3);
				if (EditorForm is StiTableElementEditorForm stiTableElementEditorForm)
				{
					if (flag3 && Meter is IStiMeasureMeter)
					{
						stiTableElementEditorForm.SwitchCurrentMeterToDimension();
					}
					else if (flag4 && Meter is IStiDimensionMeter)
					{
						stiTableElementEditorForm.SwitchCurrentMeterToMeasure();
					}
					if (text3.ToLowerInvariant() == "percentofgrandtotal")
					{
						stiTableElementEditorForm.SwitchToPercentOfGrandTotal();
					}
				}
				if (EditorForm is StiPivotTableElementDesignerForm stiPivotTableElementDesignerForm && text3.ToLowerInvariant() == "percentofgrandtotal")
				{
					stiPivotTableElementDesignerForm.SwitchToPercentOfGrandTotal();
				}
				((Control)ExpressionTextBox).Text = StiExpressionHelper.ReplaceFunction(((Control)ExpressionTextBox).Text, text3);
				((TextBoxBase)ExpressionTextBox).Select(0, 0);
				EditorForm.RefreshElementView();
			};
			ExpressionMenu.SubItems.Add(buttonItem);
			num++;
			text2 = item;
		}
	}

	protected virtual ButtonItem CreateFieldMenuItem()
	{
		if (Report.Dictionary.DataSources.Count == 0)
		{
			return null;
		}
		ButtonItem buttonItem = new ButtonItem
		{
			Text = LocField,
			Visible = (SelectedItem != null),
			BeginGroup = true,
			Tag = ((Report.Dictionary.DataSources.Count == 1) ? Report.Dictionary.DataSources.ToList().FirstOrDefault() : null)
		};
		if (buttonItem.Tag is StiDataSource)
		{
			buttonItem.PopupOpen += MenuItemDataSource_PopupOpen;
		}
		else
		{
			buttonItem.PopupOpen += MenuItemField_PopupOpen;
		}
		buttonItem.SubItems.Add(new ButtonItem());
		ExpressionMenu.SubItems.Add(buttonItem);
		return buttonItem;
	}

	protected virtual ButtonItem CreateVariableMenuItem()
	{
		List<StiVariable> source = (from v in Report.Dictionary.Variables.ToList()
			where !v.IsCategory
			select v).ToList();
		if (!source.Any())
		{
			return null;
		}
		ButtonItem buttonItem = new ButtonItem
		{
			Text = LocVariable,
			Visible = (SelectedItem != null)
		};
		buttonItem.PopupOpen += MenuItemVariable_PopupOpen;
		buttonItem.SubItems.Add(new ButtonItem());
		ExpressionMenu.SubItems.Add(buttonItem);
		return buttonItem;
	}

	protected bool IsNewFieldItem(ButtonItem parent)
	{
		string text = Loc.Get("Dashboard", "NewField");
		if (!(parent?.Text == text))
		{
			return parent?.Parent?.Text == text;
		}
		return true;
	}

	private static string ExtractVariableName(string variableName)
	{
		if (variableName == null)
		{
			return null;
		}
		if (variableName.StartsWith("["))
		{
			variableName = variableName.Substring(1);
		}
		if (variableName.EndsWith("]"))
		{
			variableName = variableName.Substring(0, variableName.Length - 1);
		}
		return variableName;
	}

	protected virtual ButtonItem CreateDuplicateFieldMenuItem()
	{
		if (MeterList == null)
		{
			return null;
		}
		if (!MeterList.AllowMultipleElements)
		{
			return null;
		}
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Duplicate(),
			Text = Loc.Get("Dashboard", "DuplicateField"),
			Visible = (SelectedItem != null),
			BeginGroup = true
		};
		buttonItem.Click += delegate
		{
			if (SelectedItem != null)
			{
				MeterList.DuplicateItem();
			}
		};
		ExpressionMenu.SubItems.Add(buttonItem);
		return buttonItem;
	}

	protected virtual ButtonItem CreateRemoveMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem();
		buttonItem.Image = StiReportDesignImages.Actions.Remove();
		buttonItem.Text = ((this is StiDataFiltersExpressionMenu) ? Loc.GetCleaned("FormBand", "RemoveFilter") : Loc.GetCleaned("Dashboard", "RemoveField"));
		buttonItem.Visible = SelectedItem != null;
		ButtonItem buttonItem2 = buttonItem;
		buttonItem2.Click += delegate
		{
			if (SelectedMeterItem != null)
			{
				MeterList.DeleteItem();
			}
			if (SelectedFilterItem != null)
			{
				FilterList.DeleteItem();
			}
		};
		ExpressionMenu.SubItems.Add(buttonItem2);
		return buttonItem2;
	}

	protected virtual ButtonItem CreateRemoveAllFieldsMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Text = Loc.Get("Dashboard", "RemoveAllFields"),
			Visible = (((ListBox)MeterList).Items.Count > 0)
		};
		buttonItem.Click += delegate
		{
			MeterList.Clear();
		};
		ExpressionMenu.SubItems.Add(buttonItem);
		return buttonItem;
	}

	protected virtual ButtonItem CreateRenameMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			BeginGroup = false,
			Image = StiReportDesignImages.Actions.Rename(),
			Text = Loc.Get("Buttons", "Rename"),
			Visible = (SelectedItem != null)
		};
		buttonItem.Click += delegate
		{
			if (SelectedItem != null)
			{
				MeterList.BeginEdit();
			}
		};
		ExpressionMenu.SubItems.Add(buttonItem);
		return buttonItem;
	}

	protected virtual IEnumerable<string> GetFuncs()
	{
		List<string> list = new List<string>();
		if (Meter == null)
		{
			return null;
		}
		if (Meter is IStiDimensionMeter && StiDataExpressionHelper.IsDateDataColumnInExpression(Dashboard, Meter.Expression))
		{
			list = list.Union(Funcs.GetDateDimensionFunctions()).ToList();
		}
		if (!(Meter is StiSparklinesColumn))
		{
			if (StiDataExpressionHelper.IsNumericDataColumnInExpression(Dashboard, Meter.Expression))
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

	protected virtual void CreateNewField()
	{
	}

	protected virtual bool IsNeedCreateField(ButtonItem item)
	{
		if (Meter == null)
		{
			return Filter == null;
		}
		return false;
	}

	protected void MenuItemField_Click(object sender, EventArgs e)
	{
		CreateNewField();
		TextBox expressionTextBox = ExpressionTextBox;
		if (expressionTextBox != null)
		{
			((TextBoxBase)expressionTextBox).Select(0, 0);
		}
		TextBox expressionTextBox2 = ExpressionTextBox;
		if (expressionTextBox2 != null)
		{
			((Control)expressionTextBox2).Focus();
		}
		EditorForm.RefreshElementView();
	}

	protected void MenuItemField_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		buttonItem.SubItems.Clear();
		IStiDashboard dashboard = Dashboard;
		TextBox expressionTextBox = ExpressionTextBox;
		IStiAppDataColumn dataColumnFromExpression = StiDataExpressionHelper.GetDataColumnFromExpression(dashboard, (expressionTextBox != null) ? ((Control)expressionTextBox).Text : null);
		foreach (StiDataSource dataSource in Element.Report.Dictionary.DataSources)
		{
			bool flag = dataColumnFromExpression != null && dataSource.Columns.Contains(dataColumnFromExpression as StiDataColumn) && AllowMarkDataColumn;
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = dataSource.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataSource && UseAliases.GetValueOrDefault(false)),
				Image = StiDataBuilder.Images.Images["DataSource"],
				FontBold = flag,
				HotFontBold = flag,
				Tag = dataSource
			};
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2.SubItems.Add(new ButtonItem());
			buttonItem2.PopupOpen += MenuItemDataSource_PopupOpen;
		}
	}

	protected void MenuItemVariable_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		buttonItem.SubItems.Clear();
		TextBox expressionTextBox = ExpressionTextBox;
		string text = ExtractVariableName((expressionTextBox != null) ? ((Control)expressionTextBox).Text : null);
		foreach (StiVariable variable in Element.Report.Dictionary.Variables)
		{
			bool flag = text == variable.Name;
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = variable.Name,
				Image = StiDataBuilder.Images.Images["Variable"],
				FontBold = flag,
				HotFontBold = flag,
				Tag = variable
			};
			buttonItem2.Click += MenuItemVariable_Click;
			buttonItem.SubItems.Add(buttonItem2);
		}
	}

	private void MenuItemVariable_Click(object sender, EventArgs e)
	{
		ButtonItem item = (ButtonItem)sender;
		(from ButtonItem b in item.Parent.SubItems
			where b != item
			select b).ToList().ForEach(delegate(ButtonItem b)
		{
			bool flag2 = (b.Checked = false);
			bool fontBold2 = (b.HotFontBold = flag2);
			b.FontBold = fontBold2;
		});
		item.Checked = true;
		ButtonItem buttonItem = item;
		bool fontBold = (item.HotFontBold = AllowMarkDataColumn);
		buttonItem.FontBold = fontBold;
		StiVariable stiVariable = item.Tag as StiVariable;
		((Control)ExpressionTextBox).Text = Funcs.ToExpression(stiVariable.Name);
		((TextBoxBase)ExpressionTextBox).Select(0, 0);
		if (MeterList != null)
		{
			MeterList.SetSelectedText(StiLabelHelper.GetLabel(Meter));
		}
		if (FilterList != null && Filter != null)
		{
			FilterList.SetSelectedText(Filter.GetStringRepresentation());
		}
		if (EditorForm is StiDataFiltersEditorForm)
		{
			((StiDataFiltersEditorForm)EditorForm).ClearLoadedFilterValues();
		}
		if (EditorForm is StiTableElementEditorForm)
		{
			((StiTableElementEditorForm)EditorForm).RefreshNodeImage();
		}
		EditorForm.RefreshElementView();
		EditorForm.RefreshUiPanel();
	}

	protected void MenuItemDataSource_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiDataSource stiDataSource = buttonItem.Tag as StiDataSource;
		TextBox expressionTextBox = ExpressionTextBox;
		string function = StiExpressionHelper.GetFunction((expressionTextBox != null) ? ((Control)expressionTextBox).Text : null);
		IStiDashboard dashboard = Dashboard;
		TextBox expressionTextBox2 = ExpressionTextBox;
		IStiAppDataColumn dataColumnFromExpression = StiDataExpressionHelper.GetDataColumnFromExpression(dashboard, (expressionTextBox2 != null) ? ((Control)expressionTextBox2).Text : null);
		buttonItem.SubItems.Clear();
		foreach (StiDataColumn column in stiDataSource.Columns)
		{
			string text = stiDataSource.Name + "." + column.NameInSource;
			string text2 = StiCodeDomSerializator.ReplaceSymbols(stiDataSource.Name) + "." + StiCodeDomSerializator.ReplaceSymbols(column.NameInSource);
			if (text2 != text)
			{
				text = "[" + text + "]";
			}
			bool flag = dataColumnFromExpression == column && AllowMarkDataColumn;
			ButtonItem buttonItem2 = new ButtonItem
			{
				AutoCollapseOnClick = IsNewFieldItem(buttonItem),
				Checked = flag,
				Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromColumn(column)],
				FontBold = flag,
				HotFontBold = flag,
				Tag = (string.IsNullOrWhiteSpace(function) ? text : (function + "(" + text + ")")),
				Text = column.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataColumn && UseAliases.GetValueOrDefault(false))
			};
			buttonItem2.Click += MenuItemDataColumn_Click;
			buttonItem.SubItems.Add(buttonItem2);
		}
	}

	private void MenuItemDataColumn_Click(object sender, EventArgs e)
	{
		ButtonItem item = (ButtonItem)sender;
		(from ButtonItem b in item.Parent.SubItems
			where b != item
			select b).ToList().ForEach(delegate(ButtonItem b)
		{
			bool flag6 = (b.Checked = false);
			bool fontBold3 = (b.HotFontBold = flag6);
			b.FontBold = fontBold3;
		});
		item.Parent.Parent.SubItems.Cast<ButtonItem>().ToList().ForEach(delegate(ButtonItem b)
		{
			if (b.Tag is StiDataSource && b.Text != LocField)
			{
				bool flag3 = (b.Checked = false);
				bool fontBold2 = (b.HotFontBold = flag3);
				b.FontBold = fontBold2;
			}
		});
		ButtonItem buttonItem = item.Parent as ButtonItem;
		bool fontBold;
		if (buttonItem.Tag is StiDataSource && buttonItem.Text != LocField && AllowMarkDataColumn)
		{
			fontBold = (buttonItem.HotFontBold = true);
			buttonItem.FontBold = fontBold;
		}
		item.Checked = true;
		ButtonItem buttonItem2 = item;
		fontBold = (item.HotFontBold = AllowMarkDataColumn);
		buttonItem2.FontBold = fontBold;
		if (IsNeedCreateField(item))
		{
			CreateNewField();
			if (Meter != null)
			{
				Meter.Expression = item.Tag as string;
				Meter.Label = item.Text;
			}
			else if (Filter != null)
			{
				Filter.Path = item.Tag as string;
			}
		}
		if (Meter != null && ((Control)ExpressionTextBox).Text.EndsWith("." + Meter.Label))
		{
			Meter.Label = item.Text;
		}
		((Control)ExpressionTextBox).Text = item.Tag as string;
		((TextBoxBase)ExpressionTextBox).Select(0, 0);
		if (MeterList != null)
		{
			MeterList.SetSelectedText(StiLabelHelper.GetLabel(Meter));
		}
		if (FilterList != null && Filter != null)
		{
			FilterList.SetSelectedText(Filter.GetStringRepresentation());
		}
		if (EditorForm is StiDataFiltersEditorForm)
		{
			((StiDataFiltersEditorForm)EditorForm).ClearLoadedFilterValues();
		}
		if (EditorForm is StiTableElementEditorForm)
		{
			((StiTableElementEditorForm)EditorForm).RefreshNodeImage();
		}
		EditorForm.RefreshElementView();
		EditorForm.RefreshUiPanel();
	}

	public StiExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm editorForm, StiDataFilterListBox list)
	{
		ExpressionMenu = expressionMenu;
		ExpressionTextBox = expressionTextBox;
		EditorForm = editorForm;
		FilterList = list;
	}

	public StiExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiElementEditorForm editorForm, StiMeterListBox list)
	{
		ExpressionMenu = expressionMenu;
		ExpressionTextBox = expressionTextBox;
		EditorForm = editorForm;
		MeterList = list;
	}
}
