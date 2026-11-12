using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Design.DataTransformation.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.DataTransformation.Menu;

public class StiDataTransformationExpressionMenu
{
	private class StiDataColumnInfo
	{
		public string Name { get; set; }

		public Type Type { get; set; }

		public bool IsFunctionPresent { get; set; }
	}

	public bool IsExpressionMenu;

	protected StiDataTransformationColumn Column => ColumnList.SelectedDataColumnItem;

	protected StiDataTransformation Transformation => EditorForm.Transformation;

	protected StiReport Report => Transformation.Dictionary?.Report;

	protected StiDataTransformationEditorForm EditorForm { get; }

	protected ButtonItem ExpressionMenu { get; }

	protected TextBox ExpressionTextBox { get; }

	protected StiDataTransformColumnListBox ColumnList { get; }

	protected ComboBox ColumnTypeComboBox { get; }

	protected bool? UseAliases => Report?.Designer?.UseAliases;

	protected string LocField => Loc.Get("PropertyMain", "Field");

	public virtual void CreateItems()
	{
		ExpressionMenu.SubItems.Clear();
		if (!IsExpressionMenu)
		{
			CreateNewDimensionMenuItem();
			CreateNewMeasureMenuItem();
			CreateDuplicateFieldMenuItem();
		}
		CreateEditExpressionMenuItem();
		if (!IsExpressionMenu)
		{
			CreateRenameMenuItem();
			CreateRemoveMenuItem();
		}
		CreateFieldMenuItem();
		CreateFuncMenuItems();
		IsExpressionMenu = false;
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
			Transformation.Columns.Add(new StiDataTransformationColumn
			{
				Name = GetNewColumnName(Loc.Get("PropertyMain", "Column")),
				Mode = StiDataTransformationMode.Dimension,
				Type = typeof(object)
			});
			EditorForm.ReadPropertiesFromTransformation();
			ColumnList.SelectedDataColumnItem = ColumnList.LastItem();
			((Control)ExpressionTextBox).Focus();
			((TextBoxBase)ExpressionTextBox).Select(0, 0);
		};
		ExpressionMenu.SubItems.Add(buttonItem);
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
			Transformation.Columns.Add(new StiDataTransformationColumn
			{
				Name = GetNewColumnName(Loc.Get("PropertyMain", "Column")),
				Expression = "Sum()",
				Mode = StiDataTransformationMode.Measure,
				Type = typeof(decimal)
			});
			EditorForm.ReadPropertiesFromTransformation();
			ColumnList.SelectedDataColumnItem = ColumnList.LastItem();
			((Control)ExpressionTextBox).Focus();
			((TextBoxBase)ExpressionTextBox).Select(4, 0);
		};
		ExpressionMenu.SubItems.Add(buttonItem);
	}

	protected virtual void CreateFuncMenuItems()
	{
		IEnumerable<string> funcs = GetFuncs();
		if (funcs == null)
		{
			return;
		}
		int num = 0;
		string function = StiExpressionHelper.GetFunction(((Control)ExpressionTextBox).Text);
		foreach (string item in funcs)
		{
			bool flag = function != null && Funcs.ToLowerCase(item) == Funcs.ToLowerCase(function);
			ButtonItem buttonItem = new ButtonItem
			{
				AutoCollapseOnClick = false,
				AutoCheckOnClick = true,
				BeginGroup = (num == 0),
				Checked = flag,
				FontBold = flag,
				HotFontBold = flag,
				Text = Funcs.Localize(item),
				Tag = item
			};
			buttonItem.Click += delegate(object sender, EventArgs args)
			{
				ButtonItem senderMenuItem = sender as ButtonItem;
				(from ButtonItem b in senderMenuItem.Parent.SubItems
					where b != senderMenuItem
					select b).ToList().ForEach(delegate(ButtonItem b)
				{
					bool flag6 = (b.Checked = false);
					bool fontBold2 = (b.HotFontBold = flag6);
					b.FontBold = fontBold2;
				});
				ButtonItem buttonItem2 = senderMenuItem;
				ButtonItem buttonItem3 = senderMenuItem;
				bool flag3 = (senderMenuItem.Checked = true);
				bool fontBold = (buttonItem3.HotFontBold = flag3);
				buttonItem2.FontBold = fontBold;
				string function2 = StiExpressionHelper.GetFunction(((Control)ExpressionTextBox).Text);
				string text = (string)senderMenuItem.Tag;
				if (function2 != text)
				{
					((Control)ExpressionTextBox).Text = StiExpressionHelper.ReplaceFunction(((Control)ExpressionTextBox).Text, text);
					((TextBoxBase)ExpressionTextBox).Select(0, 0);
					switch (text)
					{
					case "First":
					case "Last":
						EditorForm.SelectedColumn.Type = typeof(object);
						break;
					case "Count":
					case "CountDistinct":
						EditorForm.SelectedColumn.Type = typeof(int);
						break;
					case "Sum":
					case "Min":
					case "Max":
					case "Avg":
						EditorForm.SelectedColumn.Type = typeof(decimal);
						break;
					}
					EditorForm.RefreshElementView();
					EditorForm.RefreshUiPanel();
				}
			};
			ExpressionMenu.SubItems.Add(buttonItem);
			num++;
		}
	}

	protected virtual void CreateFieldMenuItem()
	{
		if (Report.Dictionary.DataSources.ToList().Count((StiDataSource d) => d != Transformation) != 0)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = LocField,
				Visible = (ColumnList.SelectedDataColumnItem != null),
				BeginGroup = true,
				Tag = ((Report.Dictionary.DataSources.ToList().Count((StiDataSource d) => d != Transformation) == 1) ? Report.Dictionary.DataSources.ToList().FirstOrDefault() : null)
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
		}
	}

	private void MenuItemField_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		buttonItem.SubItems.Clear();
		IStiAppDataColumn dataColumnFromExpression = StiDataExpressionHelper.GetDataColumnFromExpression(Transformation, ((Control)ExpressionTextBox).Text);
		foreach (StiDataSource item in from d in Transformation.Dictionary.DataSources.ToList()
			where !(d is StiDataTransformation)
			select d)
		{
			bool flag = dataColumnFromExpression != null && item.Columns.Contains(dataColumnFromExpression as StiDataColumn);
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = item.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataSource && UseAliases.GetValueOrDefault(false)),
				Image = StiDataBuilder.Images.Images["DataSource"],
				FontBold = flag,
				HotFontBold = flag,
				Tag = item
			};
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2.SubItems.Add(new ButtonItem());
			buttonItem2.PopupOpen += MenuItemDataSource_PopupOpen;
		}
	}

	private void MenuItemDataSource_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiDataSource stiDataSource = buttonItem.Tag as StiDataSource;
		string function = StiExpressionHelper.GetFunction(((Control)ExpressionTextBox).Text);
		IStiAppDataColumn dataColumnFromExpression = StiDataExpressionHelper.GetDataColumnFromExpression(Transformation, ((Control)ExpressionTextBox).Text);
		buttonItem.SubItems.Clear();
		foreach (StiDataColumn column in stiDataSource.Columns)
		{
			string text = Funcs.ToExpression(column.DataSource.Name, column.Name);
			bool flag = dataColumnFromExpression == column;
			bool flag2 = !string.IsNullOrWhiteSpace(function);
			StiDataColumnInfo tag = new StiDataColumnInfo
			{
				IsFunctionPresent = flag2,
				Name = (flag2 ? (function + "(" + text + ")") : text),
				Type = column.Type
			};
			ButtonItem buttonItem2 = new ButtonItem
			{
				AutoCollapseOnClick = false,
				Checked = flag,
				Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromColumn(column)],
				FontBold = flag,
				HotFontBold = flag,
				Tag = tag,
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
			bool flag7 = (b.Checked = false);
			bool fontBold3 = (b.HotFontBold = flag7);
			b.FontBold = fontBold3;
		});
		item.Parent.Parent.SubItems.Cast<ButtonItem>().ToList().ForEach(delegate(ButtonItem b)
		{
			if (b.Tag is StiDataSource && b.Text != LocField)
			{
				bool flag4 = (b.Checked = false);
				bool fontBold2 = (b.HotFontBold = flag4);
				b.FontBold = fontBold2;
			}
		});
		ButtonItem buttonItem = item.Parent as ButtonItem;
		bool fontBold;
		if (buttonItem.Tag is StiDataSource && buttonItem.Text != LocField)
		{
			fontBold = (buttonItem.HotFontBold = true);
			buttonItem.FontBold = fontBold;
		}
		item.Checked = true;
		ButtonItem buttonItem2 = item;
		fontBold = (item.HotFontBold = true);
		buttonItem2.FontBold = fontBold;
		StiDataColumnInfo stiDataColumnInfo = (StiDataColumnInfo)item.Tag;
		((Control)ExpressionTextBox).Text = stiDataColumnInfo.Name;
		((TextBoxBase)ExpressionTextBox).Select(0, 0);
		if (!stiDataColumnInfo.IsFunctionPresent)
		{
			foreach (StiTypeWrapper item2 in ColumnTypeComboBox.Items)
			{
				if (item2.Type == stiDataColumnInfo.Type)
				{
					ColumnTypeComboBox.SelectedItem = item2;
					break;
				}
			}
		}
		EditorForm.RefreshElementView();
	}

	protected virtual void CreateDuplicateFieldMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Duplicate(),
			Text = Loc.Get("Dashboard", "DuplicateField"),
			Visible = (ColumnList.SelectedDataColumnItem != null),
			BeginGroup = true
		};
		buttonItem.Click += delegate
		{
			if (ColumnList.SelectedDataColumnItem != null && ColumnList.SelectedDataColumnItem != null)
			{
				ColumnList.DuplicateItem();
			}
		};
		ExpressionMenu.SubItems.Add(buttonItem);
	}

	protected virtual void CreateEditExpressionMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Edit(),
			Text = Loc.Get("Dashboard", "EditExpression"),
			Visible = (ColumnList.SelectedDataColumnItem != null)
		};
		buttonItem.Click += delegate
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			string text = ((Control)ExpressionTextBox).Text;
			DialogResult val = StiTextEditor.EditText(Transformation.Dictionary.Report, ref text, StiCodeEditorMode.EditorText | StiCodeEditorMode.ShowDictionary | StiCodeEditorMode.ShortLineTerminator);
			if ((int)val == 1)
			{
				((Control)ExpressionTextBox).Text = text;
			}
			((Control)ExpressionTextBox).Focus();
			((TextBoxBase)ExpressionTextBox).Select(0, 0);
			EditorForm.RefreshElementView();
		};
		ExpressionMenu.SubItems.Add(buttonItem);
	}

	protected virtual void CreateRemoveMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Remove(),
			Text = Loc.Get("Dashboard", "RemoveField"),
			Visible = (ColumnList.SelectedDataColumnItem != null)
		};
		buttonItem.Click += delegate
		{
			if (ColumnList.SelectedDataColumnItem != null)
			{
				ColumnList.DeleteItem();
			}
		};
		ExpressionMenu.SubItems.Add(buttonItem);
		ButtonItem buttonItem2 = new ButtonItem
		{
			Text = Loc.Get("Dashboard", "RemoveAllFields"),
			Visible = (((ListBox)ColumnList).Items.Count > 0)
		};
		buttonItem2.Click += delegate
		{
			ColumnList.Clear();
		};
		ExpressionMenu.SubItems.Add(buttonItem2);
	}

	protected virtual void CreateRenameMenuItem()
	{
		ButtonItem buttonItem = new ButtonItem
		{
			Image = StiReportDesignImages.Actions.Rename(),
			Text = Loc.Get("Buttons", "Rename"),
			Visible = (ColumnList.SelectedDataColumnItem != null)
		};
		buttonItem.Click += delegate
		{
			if (ColumnList.SelectedDataColumnItem != null)
			{
				ColumnList.BeginEdit();
			}
		};
		ExpressionMenu.SubItems.Add(buttonItem);
	}

	private IEnumerable<string> GetFuncs()
	{
		if (Column == null)
		{
			return null;
		}
		if (Column.Mode == StiDataTransformationMode.Dimension)
		{
			if (StiDataExpressionHelper.IsDateDataColumnInExpression(Transformation, Column.Expression))
			{
				return Funcs.GetDateDimensionFunctions();
			}
			return null;
		}
		if (StiDataExpressionHelper.IsNumericDataColumnInExpression(Transformation, Column.Expression))
		{
			return Funcs.GetAggregateMeasureFunctions();
		}
		return Funcs.GetCommonMeasureFunctions();
	}

	private string GetNewColumnName(string baseName)
	{
		IEnumerable<StiDataColumn> source = Transformation.Columns.ToList().Cast<StiDataColumn>();
		string name = baseName;
		int num = 1;
		while (!source.All((StiDataColumn c) => c.Name != name))
		{
			name = $"{baseName}{++num}";
		}
		return name;
	}

	public StiDataTransformationExpressionMenu(ButtonItem expressionMenu, TextBox expressionTextBox, StiDataTransformationEditorForm editorForm, StiDataTransformColumnListBox list, ComboBox types)
	{
		ExpressionMenu = expressionMenu;
		ExpressionTextBox = expressionTextBox;
		EditorForm = editorForm;
		ColumnList = list;
		ColumnTypeComboBox = types;
	}
}
