using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Globalization;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design;

public class StiGlobalizationEditorForm : Form
{
	private StiReport report;

	private Hashtable cultures;

	private List<IGrouping<string, StiGlobalizationItem>> componentKeys;

	private bool locked;

	private bool needUpdateDataForDrawItem = true;

	private Hashtable allStringsFromReportList;

	private StiGlobalizationContainer currentGlobalizationContainer;

	private StiNoCultureControl noCultureControl;

	private IContainer components;

	public Bar tbTool;

	private ToolTip toolTip1;

	private ButtonItem buttonAddCulture;

	private ControlContainerItem tbAutoLocalizeContainer;

	private StiListBoxV2 listBoxCultures;

	private StiListBoxV2 listBoxProperties;

	private Splitter splitter1;

	private Splitter splitter2;

	private TextBox textBoxEditor;

	private StiRichTextBox richTextBoxEditor;

	private ButtonItem buttonAutoLocalizeReportOnRun;

	private StiCodeEditor expressionCodeEditor;

	private Panel panelSettings;

	private StiGroupLine stiGroupLine1;

	private StiOffice2013GroupBox groupBoxCulture;

	private StiOffice2013GroupBox groupBoxProperties;

	private StiOffice2013GroupBox groupBoxEditor;

	private ButtonItem buttonActions;

	private ButtonItem buttonRemoveCulture;

	private ButtonItem buttonGetCulture;

	private ButtonItem buttonSetCulture;

	private ButtonItem buttonOpen;

	private ButtonItem buttonSave;

	private StiListBoxV2 listBoxComponents;

	private Panel panelCultureAndGroups;

	private StiOffice2013GroupBox groupBoxComponents;

	private Splitter splitter3;

	private StiHtmlTextEditorControl htmlTextEditor;

	private StiGlobalizationContainer SelectedCulture
	{
		get
		{
			if (((ListControl)listBoxCultures).SelectedIndex == -1)
			{
				return null;
			}
			return report.GlobalizationStrings[((ListControl)listBoxCultures).SelectedIndex];
		}
	}

	private StiGlobalizationItem SelectedProperty => ((ListBox)listBoxProperties).SelectedItem as StiGlobalizationItem;

	private string SelectedComponent => ((ListBox)listBoxComponents).SelectedItem as string;

	public StiGlobalizationEditorForm(StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.report = report;
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		int @int = StiSettings.GetInt("StiGlobalizationEditorForm", "Spl", 200);
		if (@int != -1 && expressionCodeEditor.DictionaryControl != null)
		{
			((Control)expressionCodeEditor.DictionaryControl).Width = StiScaleUI.I(@int);
		}
		@int = StiSettings.GetInt("StiGlobalizationEditorForm", "Spl1");
		if (@int != -1)
		{
			((Control)panelCultureAndGroups).Width = StiScaleUI.I(@int);
		}
		@int = StiSettings.GetInt("StiGlobalizationEditorForm", "Spl2");
		if (@int != -1)
		{
			((Control)groupBoxProperties).Width = StiScaleUI.I(@int);
		}
		@int = StiSettings.GetInt("StiGlobalizationEditorForm", "Spl3");
		if (@int != -1)
		{
			((Control)groupBoxCulture).Height = StiScaleUI.I(@int);
		}
		Localize();
		DoScaling();
		listBoxProperties.IsBaseOwnerDraw = true;
		buttonAutoLocalizeReportOnRun.Checked = report.AutoLocalizeReportOnRun;
		CultureInfo[] array = StiOptions.Designer.InvokeGetCulturesListForGlobalizationEditor();
		if (array == null)
		{
			array = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		}
		cultures = new Hashtable();
		CultureInfo[] array2 = array;
		foreach (CultureInfo cultureInfo in array2)
		{
			if (cultureInfo.Name.Length > 0)
			{
				cultures[cultureInfo.Name] = cultureInfo;
				if (cultureInfo.Name == "no")
				{
					cultures["nn-NO"] = new CultureInfo("nn-NO");
					cultures["nb-NO"] = new CultureInfo("nb-NO");
				}
			}
		}
		InitializeExpressionCodeEditor(report);
		InitializeHtmlTextEditor();
		report.GlobalizationStrings.FillItemsFromReport();
		BuildCultures();
		((Form)this).ShowIcon = false;
		StiHtmlTextEditorControl stiHtmlTextEditorControl = htmlTextEditor;
		TextBox obj = textBoxEditor;
		StiRichTextBox stiRichTextBox = richTextBoxEditor;
		int num2 = (((Control)expressionCodeEditor).Left = 0);
		int num4 = (((Control)stiRichTextBox).Left = num2);
		int left = (((Control)obj).Left = num4);
		((Control)stiHtmlTextEditorControl).Left = left;
		StiHtmlTextEditorControl stiHtmlTextEditorControl2 = htmlTextEditor;
		TextBox obj2 = textBoxEditor;
		StiRichTextBox stiRichTextBox2 = richTextBoxEditor;
		num2 = (((Control)expressionCodeEditor).Width = ((Control)groupBoxEditor).Width);
		num4 = (((Control)stiRichTextBox2).Width = num2);
		left = (((Control)obj2).Width = num4);
		((Control)stiHtmlTextEditorControl2).Width = left;
		if (StiOptions.Designer.UseRightToLeftGlobalizationEditor)
		{
			((Control)textBoxEditor).RightToLeft = (RightToLeft)1;
		}
		buttonGetCulture.Enabled = false;
		buttonSetCulture.Enabled = false;
		ListBoxCultures_SelectedIndexChanged(null, EventArgs.Empty);
		needUpdateDataForDrawItem = true;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormGlobalizationEditor", "title");
		buttonActions.Text = StiLocalization.Get("FormDictionaryDesigner", "Actions");
		buttonSave.Text = StiLocalization.Get("MainMenu", "menuFileSaveAs");
		buttonOpen.Text = StiLocalization.Get("Buttons", "Open") + "...";
		buttonAddCulture.Text = StiLocalization.Get("FormGlobalizationEditor", "AddCulture");
		((Control)groupBoxCulture).Text = Loc.GetMain("Culture");
		((Control)groupBoxComponents).Text = Loc.Get("Report", "Components");
		((Control)groupBoxProperties).Text = Loc.GetCleaned("FormCrossTabDesigner", "Properties");
		((Control)groupBoxEditor).Text = Loc.Get("FormViewer", "Editor");
		buttonRemoveCulture.Tooltip = StiLocalization.Get("Buttons", "Remove");
		buttonAutoLocalizeReportOnRun.Tooltip = StiLocalization.Get("FormGlobalizationEditor", "AutoLocalizeReportOnRun");
		buttonSetCulture.Tooltip = StiLocalization.Get("FormGlobalizationEditor", "SetCulture");
		buttonGetCulture.Tooltip = StiLocalization.Get("FormGlobalizationEditor", "GetCulture");
	}

	private void InitializeExpressionCodeEditor(StiReport report)
	{
		expressionCodeEditor.Init(report, StiCodeEditorMode.EditorExpression);
		((Control)expressionCodeEditor.Editor).TextChanged += CodeEditorExpression_TextChanged;
	}

	private void InitializeHtmlTextEditor()
	{
		if (htmlTextEditor.TextBoxEditor != null)
		{
			((Control)htmlTextEditor.TextBoxEditor).TextChanged += delegate
			{
				SelectedProperty.Text = htmlTextEditor.GetText();
			};
			((Control)htmlTextEditor.TextBoxEditor).Leave += delegate
			{
				SelectedProperty.Text = htmlTextEditor.GetText();
			};
		}
		else
		{
			IStiHtmlTextEditorControl stiHtmlTextEditorControl = htmlTextEditor.HtmlTextEditor;
			((Control)((stiHtmlTextEditorControl is Control) ? stiHtmlTextEditorControl : null)).Leave += delegate
			{
				SelectedProperty.Text = htmlTextEditor.GetText();
			};
		}
	}

	private bool IsDashboardTextElementTextKey(string key)
	{
		if (string.IsNullOrWhiteSpace(key) || !key.Contains("."))
		{
			return false;
		}
		string[] keys = key.Split(new char[1] { '.' });
		if (keys.Length != 2 || string.IsNullOrWhiteSpace(keys[0]) || keys[1] != "Text")
		{
			return false;
		}
		return report.GetComponents().ToList().Any((StiComponent c) => c != null && c.Name == keys[0] && c is IStiTextElement);
	}

	private void UpdateNoCultureState()
	{
		if (noCultureControl == null)
		{
			StiNoCultureControl stiNoCultureControl = new StiNoCultureControl(this);
			((Control)stiNoCultureControl).Anchor = (AnchorStyles)0;
			((Control)stiNoCultureControl).Visible = false;
			noCultureControl = stiNoCultureControl;
			((Control)noCultureControl).Location = new Point((((Control)this).Width - ((Control)noCultureControl).Width) / 2, (((Control)this).Height - ((Control)noCultureControl).Height) / 2);
			((Control)this).Controls.Add((Control)(object)noCultureControl);
			((Control)this).SizeChanged += delegate
			{
				((Control)noCultureControl).Location = new Point((((Control)this).Width - ((Control)noCultureControl).Width) / 2, (((Control)this).Height - ((Control)noCultureControl).Height) / 2);
			};
		}
		((Control)noCultureControl).Visible = ((ListBox)listBoxCultures).Items.Count == 0;
	}

	private void DoScaling()
	{
		buttonAddCulture.Image = StiReportDesignImages.Actions.NewCulture();
		buttonRemoveCulture.Image = StiReportDesignImages.Actions.Remove();
		buttonGetCulture.Image = StiReportDesignImages.Actions.GetCulture();
		buttonSetCulture.Image = StiReportDesignImages.Actions.SetCulture();
		buttonOpen.Image = StiReportDesignImages.Actions.OpenFile();
		buttonSave.Image = StiReportDesignImages.Actions.SaveFile();
	}

	private void BuildCultures()
	{
		int num = ((ListControl)listBoxCultures).SelectedIndex;
		((ListBox)listBoxCultures).BeginUpdate();
		((ListBox)listBoxCultures).Items.Clear();
		foreach (StiGlobalizationContainer globalizationString in report.GlobalizationStrings)
		{
			if (cultures[globalizationString.CultureName] is CultureInfo cultureInfo)
			{
				((ListBox)listBoxCultures).Items.Add((object)(cultureInfo.DisplayName + " (" + globalizationString.CultureName + ")"));
			}
			else
			{
				((ListBox)listBoxCultures).Items.Add((object)globalizationString.CultureName);
			}
		}
		if (((ListBox)listBoxCultures).Items.Count > 0 && ((ListControl)listBoxCultures).SelectedIndex == -1)
		{
			((ListControl)listBoxCultures).SelectedIndex = 0;
		}
		((Control)panelSettings).Visible = ((ListBox)listBoxCultures).Items.Count > 0;
		UpdateNoCultureState();
		SelectedCulture?.Items.Sort();
		BuildComponents();
		if (((ListBox)listBoxCultures).Items.Count > 0 && num == -1)
		{
			num = 0;
		}
		if (((ListBox)listBoxCultures).Items.Count <= num)
		{
			num = ((ListBox)listBoxCultures).Items.Count - 1;
		}
		((ListControl)listBoxCultures).SelectedIndex = num;
		((ListBox)listBoxCultures).EndUpdate();
	}

	private void BuildComponents()
	{
		((ListBox)listBoxComponents).BeginUpdate();
		int num = ((ListControl)listBoxComponents).SelectedIndex;
		((ListBox)listBoxComponents).Items.Clear();
		componentKeys = GetComponentKeys();
		if (componentKeys != null)
		{
			foreach (IGrouping<string, StiGlobalizationItem> componentKey in componentKeys)
			{
				((ListBox)listBoxComponents).Items.Add((object)componentKey.Key);
			}
		}
		if (((ListBox)listBoxComponents).Items.Count > 0 && num == -1)
		{
			num = 0;
		}
		if (((ListBox)listBoxComponents).Items.Count <= num)
		{
			num = ((ListBox)listBoxComponents).Items.Count - 1;
		}
		((ListControl)listBoxComponents).SelectedIndex = num;
		((ListBox)listBoxComponents).EndUpdate();
	}

	private void BuildProperties(List<StiGlobalizationItem> items)
	{
		((ListBox)listBoxProperties).BeginUpdate();
		int num = ((ListControl)listBoxProperties).SelectedIndex;
		((ListBox)listBoxProperties).Items.Clear();
		foreach (StiGlobalizationItem item in items)
		{
			((ListBox)listBoxProperties).Items.Add((object)item);
		}
		if (((ListBox)listBoxProperties).Items.Count > 0 && num == -1)
		{
			num = 0;
		}
		if (((ListBox)listBoxProperties).Items.Count <= num)
		{
			num = ((ListBox)listBoxProperties).Items.Count - 1;
		}
		((ListControl)listBoxProperties).SelectedIndex = num;
		((ListBox)listBoxProperties).EndUpdate();
	}

	private List<IGrouping<string, StiGlobalizationItem>> GetComponentKeys()
	{
		if (SelectedCulture == null)
		{
			return null;
		}
		List<IGrouping<string, StiGlobalizationItem>> list = (from r in SelectedCulture.Items.ToList()
			group r by GetComponentKey(r.PropertyName) into c
			orderby c.Key
			select c).ToList();
		IGrouping<string, StiGlobalizationItem> grouping = list.FirstOrDefault((IGrouping<string, StiGlobalizationItem> g) => g.Key == "Report");
		if (grouping != null)
		{
			list.Remove(grouping);
			list.Insert(0, grouping);
		}
		IGrouping<string, StiGlobalizationItem> grouping2 = list.FirstOrDefault((IGrouping<string, StiGlobalizationItem> g) => g.Key == "Common");
		if (grouping2 != null)
		{
			list.Remove(grouping2);
			list.Add(grouping2);
		}
		return list;
	}

	private string GetComponentKey(string propertyName)
	{
		if (propertyName.StartsWith("Report"))
		{
			return "Report";
		}
		if (propertyName.Contains("."))
		{
			return propertyName.Substring(0, propertyName.IndexOf(".", StringComparison.Ordinal));
		}
		return "Common";
	}

	private void UpdateDataForDrawItem()
	{
		if (needUpdateDataForDrawItem)
		{
			int selectedIndex = ((ListControl)listBoxCultures).SelectedIndex;
			if (selectedIndex > -1)
			{
				currentGlobalizationContainer = report.GlobalizationStrings[selectedIndex];
				allStringsFromReportList = currentGlobalizationContainer.GetAllStringsForReport(report);
			}
			needUpdateDataForDrawItem = false;
		}
	}

	internal void ProcessAddCultureClick()
	{
		ButtonAddCulture_Click(null, null);
	}

	private void SaveChanges()
	{
		if (SelectedProperty != null)
		{
			if (StiOptions.Designer.UseSimpleGlobalizationEditor)
			{
				TextBoxEditor_Leave(null, null);
			}
			else if (IsDashboardTextElementTextKey(SelectedProperty.PropertyName))
			{
				SelectedProperty.Text = htmlTextEditor.GetText();
			}
			else if (SelectedProperty.Text.StartsWith("__LP__"))
			{
				RichTextBoxEditor_Leave(null, null);
			}
			else
			{
				CodeEditorExpression_Leave(null, null);
			}
		}
	}

	private void ButtonAddCulture_Click(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		StiAddCultureForm stiAddCultureForm = new StiAddCultureForm();
		try
		{
			if ((int)((Form)stiAddCultureForm).ShowDialog((IWin32Window)(object)this) == 1)
			{
				StiGlobalizationContainer stiGlobalizationContainer = new StiGlobalizationContainer(stiAddCultureForm.SelectedCultureName);
				stiGlobalizationContainer.FillItemsFromReport(report);
				report.GlobalizationStrings.Add(stiGlobalizationContainer);
				BuildCultures();
				((ListControl)listBoxCultures).SelectedIndex = ((ListBox)listBoxCultures).Items.Count - 1;
				((Control)panelSettings).Visible = true;
				UpdateNoCultureState();
			}
		}
		finally
		{
			((IDisposable)stiAddCultureForm)?.Dispose();
		}
		needUpdateDataForDrawItem = true;
	}

	private void ButtonRemoveCulture_Click(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		if (((ListControl)listBoxCultures).SelectedIndex != -1 && (int)MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Questions", "qnRemove"), ((Control)this).Text, (MessageBoxButtons)4) == 6)
		{
			locked = true;
			int num = ((ListControl)listBoxCultures).SelectedIndex;
			report.GlobalizationStrings.RemoveAt(num);
			((ListBox)listBoxCultures).Items.RemoveAt(num);
			if (num > 0)
			{
				num--;
			}
			if (num >= ((ListBox)listBoxCultures).Items.Count)
			{
				num = ((ListBox)listBoxCultures).Items.Count - 1;
			}
			BuildCultures();
			((ListControl)listBoxCultures).SelectedIndex = num;
			locked = false;
		}
		needUpdateDataForDrawItem = true;
	}

	private void ButtonGetCulture_Click(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("FormGlobalizationEditor", "qnGetCulture"), ((Control)this).Text, (MessageBoxButtons)4) == 6)
		{
			int selectedIndex = ((ListControl)listBoxCultures).SelectedIndex;
			StiGlobalizationContainer selectedCulture = SelectedCulture;
			selectedCulture.Items.Clear();
			selectedCulture.FillItemsFromReport(report);
			BuildCultures();
			((ListControl)listBoxCultures).SelectedIndex = selectedIndex;
			needUpdateDataForDrawItem = true;
		}
	}

	private void ButtonSetCulture_Click(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("FormGlobalizationEditor", "qnSetCulture"), ((Control)this).Text, (MessageBoxButtons)4) == 6)
		{
			report.LocalizeReport(SelectedCulture.CultureName);
			needUpdateDataForDrawItem = true;
		}
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "Excel2007Files");
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormGlobalizationEditor", "title"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && !string.IsNullOrEmpty(((FileDialog)val).FileName))
				{
					StiFileUtils.ProcessReadOnly(((FileDialog)val).FileName);
					report.GlobalizationStrings.LoadFromFile(((FileDialog)val).FileName);
					BuildCultures();
					MessageBox.Show("OK!");
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonSave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "Excel2007Files");
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormGlobalizationEditor", "title"));
				((FileDialog)val).FileName = Loc.GetMain("GlobalizationStrings");
				((FileDialog)val).DefaultExt = "xlsx";
				if ((int)((CommonDialog)val).ShowDialog() == 1 && !string.IsNullOrEmpty(((FileDialog)val).FileName))
				{
					StiFileUtils.ProcessReadOnly(((FileDialog)val).FileName);
					report.GlobalizationStrings.SaveToFile(((FileDialog)val).FileName);
					Process.Start(((FileDialog)val).FileName);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ListBoxCultures_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			locked = true;
			BuildComponents();
			locked = false;
		}
		bool enabled = ((ListBox)listBoxCultures).Items.Count > 0 && ((ListControl)listBoxCultures).SelectedIndex != -1;
		buttonRemoveCulture.Enabled = enabled;
		buttonGetCulture.Enabled = enabled;
		buttonSetCulture.Enabled = enabled;
		needUpdateDataForDrawItem = true;
	}

	private void ListBoxCultures_DoubleClick(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		StiAddCultureForm stiAddCultureForm = new StiAddCultureForm(SelectedCulture?.CultureName);
		try
		{
			if ((int)((Form)stiAddCultureForm).ShowDialog((IWin32Window)(object)this) == 1)
			{
				SelectedCulture.CultureName = stiAddCultureForm.SelectedCultureName;
				BuildCultures();
			}
		}
		finally
		{
			((IDisposable)stiAddCultureForm)?.Dispose();
		}
	}

	private void ListBoxComponents_SelectedIndexChanged(object sender, EventArgs e)
	{
		IGrouping<string, StiGlobalizationItem> source = componentKeys.FirstOrDefault((IGrouping<string, StiGlobalizationItem> g) => g.Key == SelectedComponent);
		BuildProperties(source.OrderBy((StiGlobalizationItem c) => c.PropertyName).ToList());
		ListBoxProperties_SelectedIndexChanged(null, null);
	}

	private void ListBoxProperties_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (StiOptions.Designer.UseSimpleGlobalizationEditor)
		{
			((Control)textBoxEditor).Visible = true;
			((Control)richTextBoxEditor).Visible = false;
			((Control)expressionCodeEditor).Visible = false;
			((Control)htmlTextEditor).Visible = false;
			((Control)textBoxEditor).Text = SelectedProperty.Text;
		}
		else if (IsDashboardTextElementTextKey(SelectedProperty.PropertyName))
		{
			((Control)richTextBoxEditor).Visible = false;
			((Control)textBoxEditor).Visible = false;
			((Control)expressionCodeEditor).Visible = false;
			((Control)htmlTextEditor).Visible = true;
			htmlTextEditor.SetText(SelectedProperty.Text);
		}
		else if (SelectedProperty.Text.StartsWith("__LP__"))
		{
			((Control)richTextBoxEditor).Visible = true;
			((Control)textBoxEditor).Visible = false;
			((Control)expressionCodeEditor).Visible = false;
			((Control)htmlTextEditor).Visible = false;
			((RichTextBox)richTextBoxEditor).Rtf = StiRichText.UnpackRtf(XmlConvert.DecodeName(SelectedProperty.Text));
		}
		else
		{
			((Control)richTextBoxEditor).Visible = false;
			((Control)textBoxEditor).Visible = false;
			((Control)expressionCodeEditor).Visible = true;
			((Control)htmlTextEditor).Visible = false;
			((Control)expressionCodeEditor).Text = SelectedProperty.Text;
		}
	}

	private void ListBoxProperties_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		UpdateDataForDrawItem();
		if (e.Index == -1)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		bool flag = (e.State & 1) > 0;
		Color color = (flag ? SystemColors.MenuHighlight : Color.White);
		Color color2 = (flag ? SystemColors.HighlightText : SystemColors.ControlText);
		Rectangle bounds = e.Bounds;
		bounds.Inflate(-2, -1);
		if (e.Index == 0)
		{
			bounds.Y++;
			bounds.Height--;
		}
		StiDrawing.FillRectangle(graphics, color, bounds);
		TextFormatFlags val = (TextFormatFlags)32804;
		int num = (int)(4.0 * StiScale.X);
		bounds.X += num;
		bounds.Width -= num;
		string text = ((ListBox)listBoxProperties).Items[e.Index].ToString();
		if (allStringsFromReportList != null && allStringsFromReportList.ContainsKey(text))
		{
			foreach (StiGlobalizationItem item in currentGlobalizationContainer.Items)
			{
				if (item.PropertyName == text && allStringsFromReportList[text].ToString() != item.Text)
				{
					Font val2 = StiFontUtils.ChangeFontStyleBold(e.Font, bold: true);
					try
					{
						TextRenderer.DrawText((IDeviceContext)(object)graphics, text, val2, bounds, color2, val);
						return;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
		}
		TextRenderer.DrawText((IDeviceContext)(object)graphics, text, e.Font, bounds, color2, val);
		e.DrawFocusRectangle();
	}

	private void CodeEditorExpression_TextChanged(object sender, EventArgs e)
	{
		if (SelectedProperty != null)
		{
			SelectedProperty.Text = ((Control)expressionCodeEditor.Editor).Text;
		}
	}

	private void CodeEditorExpression_Leave(object sender, EventArgs e)
	{
		if (SelectedProperty != null)
		{
			SelectedProperty.Text = StiExpressionPacker.UnPackExpression(((Control)expressionCodeEditor).Text, report.Designer, useBraces: false);
		}
	}

	private void TextBoxEditor_Leave(object sender, EventArgs e)
	{
		if (SelectedProperty != null)
		{
			SelectedProperty.Text = StiExpressionPacker.UnPackExpression(((Control)textBoxEditor).Text, report.Designer, useBraces: false);
		}
	}

	private void TextBoxEditor_TextChanged(object sender, EventArgs e)
	{
		if (SelectedProperty != null)
		{
			SelectedProperty.Text = ((Control)textBoxEditor).Text;
		}
	}

	private void RichTextBoxEditor_Leave(object sender, EventArgs e)
	{
		if (SelectedProperty != null)
		{
			SelectedProperty.Text = XmlConvert.EncodeName(StiRichText.PackRtf(((RichTextBox)richTextBoxEditor).Rtf));
		}
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		SaveChanges();
		report.AutoLocalizeReportOnRun = buttonAutoLocalizeReportOnRun.Checked;
		report.GlobalizationStrings.RemoveUnlocalizedItemsFromReport();
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		if (expressionCodeEditor.DictionaryControl != null)
		{
			StiSettings.Set("StiGlobalizationEditorForm", "Spl", (int)((double)((Control)expressionCodeEditor.DictionaryControl).Width / StiScaleUI.Factor));
			expressionCodeEditor.NodesMemoryForEditor.SaveExpNodes((TreeView)(object)expressionCodeEditor.DictionaryControl.tvData);
			expressionCodeEditor.DesignerLoadNodes();
		}
		StiSettings.Set("StiGlobalizationEditorForm", "Spl1", (int)((double)((Control)panelCultureAndGroups).Width / StiScaleUI.Factor));
		StiSettings.Set("StiGlobalizationEditorForm", "Spl2", (int)((double)((Control)groupBoxProperties).Width / StiScaleUI.Factor));
		StiSettings.Set("StiGlobalizationEditorForm", "Spl3", (int)((double)((Control)groupBoxCulture).Height / StiScaleUI.Factor));
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("User-Manual/index.html?reports_designer_template_globalization_editor.htm");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Expected O, but got Unknown
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0c: Expected O, but got Unknown
		//IL_0e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Expected O, but got Unknown
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Expected O, but got Unknown
		//IL_1010: Unknown result type (might be due to invalid IL or missing references)
		//IL_101a: Expected O, but got Unknown
		//IL_106c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1076: Expected O, but got Unknown
		//IL_1115: Unknown result type (might be due to invalid IL or missing references)
		//IL_111f: Expected O, but got Unknown
		//IL_1252: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGlobalizationEditorForm));
		toolTip1 = new ToolTip(components);
		splitter2 = new Splitter();
		splitter1 = new Splitter();
		panelSettings = new Panel();
		groupBoxEditor = new StiOffice2013GroupBox();
		htmlTextEditor = new StiHtmlTextEditorControl();
		textBoxEditor = new TextBox();
		expressionCodeEditor = new StiCodeEditor();
		richTextBoxEditor = new StiRichTextBox();
		groupBoxProperties = new StiOffice2013GroupBox();
		listBoxProperties = new StiListBoxV2();
		panelCultureAndGroups = new Panel();
		groupBoxComponents = new StiOffice2013GroupBox();
		listBoxComponents = new StiListBoxV2();
		splitter3 = new Splitter();
		groupBoxCulture = new StiOffice2013GroupBox();
		listBoxCultures = new StiListBoxV2();
		stiGroupLine1 = new StiGroupLine();
		tbTool = new Bar();
		buttonActions = new ButtonItem();
		buttonAddCulture = new ButtonItem();
		buttonRemoveCulture = new ButtonItem();
		buttonGetCulture = new ButtonItem();
		buttonSetCulture = new ButtonItem();
		buttonOpen = new ButtonItem();
		buttonSave = new ButtonItem();
		buttonAutoLocalizeReportOnRun = new ButtonItem();
		tbAutoLocalizeContainer = new ControlContainerItem();
		((Control)panelSettings).SuspendLayout();
		((Control)groupBoxEditor).SuspendLayout();
		((Control)groupBoxProperties).SuspendLayout();
		((Control)panelCultureAndGroups).SuspendLayout();
		((Control)groupBoxComponents).SuspendLayout();
		((Control)groupBoxCulture).SuspendLayout();
		((ISupportInitialize)tbTool).BeginInit();
		((Control)this).SuspendLayout();
		((Control)splitter2).BackColor = Color.White;
		((Control)splitter2).ForeColor = Color.Black;
		((Control)splitter2).Location = new Point(754, 0);
		((Control)splitter2).Margin = new Padding(4);
		((Control)splitter2).Name = "splitter2";
		((Control)splitter2).Size = new Size(24, 935);
		((Control)splitter2).TabIndex = 6;
		splitter2.TabStop = false;
		((Control)splitter1).BackColor = Color.White;
		((Control)splitter1).ForeColor = Color.Black;
		((Control)splitter1).Location = new Point(330, 0);
		((Control)splitter1).Margin = new Padding(4);
		((Control)splitter1).Name = "splitter1";
		((Control)splitter1).Size = new Size(24, 935);
		((Control)splitter1).TabIndex = 5;
		splitter1.TabStop = false;
		((Control)panelSettings).Anchor = (AnchorStyles)15;
		((Control)panelSettings).Controls.Add((Control)(object)groupBoxEditor);
		((Control)panelSettings).Controls.Add((Control)(object)splitter2);
		((Control)panelSettings).Controls.Add((Control)(object)groupBoxProperties);
		((Control)panelSettings).Controls.Add((Control)(object)splitter1);
		((Control)panelSettings).Controls.Add((Control)(object)panelCultureAndGroups);
		((Control)panelSettings).Location = new Point(24, 98);
		((Control)panelSettings).Margin = new Padding(4);
		((Control)panelSettings).Name = "panelSettings";
		((Control)panelSettings).Size = new Size(1822, 935);
		((Control)panelSettings).TabIndex = 9;
		((Control)groupBoxEditor).Controls.Add((Control)(object)htmlTextEditor);
		((Control)groupBoxEditor).Controls.Add((Control)(object)textBoxEditor);
		((Control)groupBoxEditor).Controls.Add((Control)(object)expressionCodeEditor);
		((Control)groupBoxEditor).Controls.Add((Control)(object)richTextBoxEditor);
		((Control)groupBoxEditor).Dock = (DockStyle)5;
		groupBoxEditor.HeaderHeight = 24;
		((Control)groupBoxEditor).Location = new Point(778, 0);
		((Control)groupBoxEditor).Margin = new Padding(6);
		((Control)groupBoxEditor).MinimumSize = new Size(500, 0);
		((Control)groupBoxEditor).Name = "groupBoxEditor";
		((Control)groupBoxEditor).Size = new Size(1044, 935);
		((Control)groupBoxEditor).TabIndex = 11;
		((Control)groupBoxEditor).TabStop = false;
		((Control)groupBoxEditor).Text = "Editor";
		((Control)htmlTextEditor).Anchor = (AnchorStyles)15;
		((Control)htmlTextEditor).BackColor = Color.White;
		((Control)htmlTextEditor).Location = new Point(536, 77);
		((Control)htmlTextEditor).Margin = new Padding(2);
		((Control)htmlTextEditor).Name = "htmlTextEditor";
		((Control)htmlTextEditor).Size = new Size(222, 854);
		((Control)htmlTextEditor).TabIndex = 9;
		((Control)textBoxEditor).Anchor = (AnchorStyles)15;
		((Control)textBoxEditor).BackColor = Color.White;
		((Control)textBoxEditor).Location = new Point(328, 77);
		((Control)textBoxEditor).Margin = new Padding(4);
		((TextBoxBase)textBoxEditor).Multiline = true;
		((Control)textBoxEditor).Name = "textBoxEditor";
		((Control)textBoxEditor).Size = new Size(188, 854);
		((Control)textBoxEditor).TabIndex = 7;
		((Control)textBoxEditor).Visible = false;
		((Control)textBoxEditor).TextChanged += TextBoxEditor_TextChanged;
		((Control)textBoxEditor).Leave += TextBoxEditor_Leave;
		expressionCodeEditor.AllowOutlining = false;
		expressionCodeEditor.AllowSQLFormatting = false;
		((Control)expressionCodeEditor).Anchor = (AnchorStyles)15;
		((Control)expressionCodeEditor).BackColor = Color.White;
		expressionCodeEditor.ForceShowAutoGeneratedCode = false;
		((Control)expressionCodeEditor).Location = new Point(0, 77);
		((Control)expressionCodeEditor).Margin = new Padding(4);
		((Control)expressionCodeEditor).Name = "expressionCodeEditor";
		((Control)expressionCodeEditor).Padding = new Padding(0, 2, 0, 2);
		expressionCodeEditor.ReplaceMode = false;
		expressionCodeEditor.ShowAutoGeneratedCode = false;
		expressionCodeEditor.ShowInsertSymbol = false;
		expressionCodeEditor.ShowLineNumbersInEditor = false;
		((Control)expressionCodeEditor).Size = new Size(140, 854);
		((Control)expressionCodeEditor).TabIndex = 3;
		expressionCodeEditor.TextFont = null;
		((Control)expressionCodeEditor).Leave += CodeEditorExpression_Leave;
		((Control)richTextBoxEditor).Anchor = (AnchorStyles)15;
		((Control)richTextBoxEditor).BackColor = Color.White;
		((Control)richTextBoxEditor).Location = new Point(148, 77);
		((Control)richTextBoxEditor).Margin = new Padding(4);
		((Control)richTextBoxEditor).Name = "richTextBoxEditor";
		richTextBoxEditor.SelectionAlignment = StiRtfSelectionAlign.Left;
		((Control)richTextBoxEditor).Size = new Size(168, 854);
		((Control)richTextBoxEditor).TabIndex = 8;
		((Control)richTextBoxEditor).Text = "";
		((Control)richTextBoxEditor).Visible = false;
		((Control)richTextBoxEditor).Leave += RichTextBoxEditor_Leave;
		((Control)groupBoxProperties).Controls.Add((Control)(object)listBoxProperties);
		((Control)groupBoxProperties).Dock = (DockStyle)3;
		groupBoxProperties.HeaderHeight = 24;
		((Control)groupBoxProperties).Location = new Point(354, 0);
		((Control)groupBoxProperties).Margin = new Padding(6);
		((Control)groupBoxProperties).MinimumSize = new Size(200, 0);
		((Control)groupBoxProperties).Name = "groupBoxProperties";
		((Control)groupBoxProperties).Size = new Size(400, 935);
		((Control)groupBoxProperties).TabIndex = 10;
		((Control)groupBoxProperties).TabStop = false;
		((Control)groupBoxProperties).Text = "Properties";
		((Control)listBoxProperties).Anchor = (AnchorStyles)15;
		((Control)listBoxProperties).BackColor = Color.White;
		((Control)listBoxProperties).Location = new Point(0, 77);
		((Control)listBoxProperties).Margin = new Padding(4);
		((Control)listBoxProperties).Name = "listBoxProperties";
		((Control)listBoxProperties).Size = new Size(400, 858);
		((Control)listBoxProperties).TabIndex = 2;
		((ListBox)listBoxProperties).DrawItem += new DrawItemEventHandler(ListBoxProperties_DrawItem);
		((ListBox)listBoxProperties).SelectedIndexChanged += ListBoxProperties_SelectedIndexChanged;
		((Control)panelCultureAndGroups).Controls.Add((Control)(object)groupBoxComponents);
		((Control)panelCultureAndGroups).Controls.Add((Control)(object)splitter3);
		((Control)panelCultureAndGroups).Controls.Add((Control)(object)groupBoxCulture);
		((Control)panelCultureAndGroups).Dock = (DockStyle)3;
		((Control)panelCultureAndGroups).Location = new Point(0, 0);
		((Control)panelCultureAndGroups).Margin = new Padding(4);
		((Control)panelCultureAndGroups).MinimumSize = new Size(200, 0);
		((Control)panelCultureAndGroups).Name = "panelCultureAndGroups";
		((Control)panelCultureAndGroups).Size = new Size(330, 935);
		((Control)panelCultureAndGroups).TabIndex = 12;
		((Control)groupBoxComponents).Controls.Add((Control)(object)listBoxComponents);
		((Control)groupBoxComponents).Dock = (DockStyle)5;
		groupBoxComponents.HeaderHeight = 24;
		((Control)groupBoxComponents).Location = new Point(0, 360);
		((Control)groupBoxComponents).Margin = new Padding(6);
		((Control)groupBoxComponents).Name = "groupBoxComponents";
		((Control)groupBoxComponents).Size = new Size(330, 575);
		((Control)groupBoxComponents).TabIndex = 10;
		((Control)groupBoxComponents).TabStop = false;
		((Control)groupBoxComponents).Text = "Components";
		((Control)listBoxComponents).Anchor = (AnchorStyles)15;
		((Control)listBoxComponents).BackColor = Color.White;
		((Control)listBoxComponents).Location = new Point(0, 77);
		((Control)listBoxComponents).Margin = new Padding(4);
		((Control)listBoxComponents).Name = "listBoxComponents";
		((Control)listBoxComponents).Size = new Size(330, 498);
		((Control)listBoxComponents).TabIndex = 1;
		((ListBox)listBoxComponents).SelectedIndexChanged += ListBoxComponents_SelectedIndexChanged;
		((Control)splitter3).Dock = (DockStyle)1;
		((Control)splitter3).Location = new Point(0, 337);
		((Control)splitter3).Margin = new Padding(4);
		((Control)splitter3).Name = "splitter3";
		((Control)splitter3).Size = new Size(330, 23);
		((Control)splitter3).TabIndex = 11;
		splitter3.TabStop = false;
		((Control)groupBoxCulture).Controls.Add((Control)(object)listBoxCultures);
		((Control)groupBoxCulture).Dock = (DockStyle)1;
		groupBoxCulture.HeaderHeight = 24;
		((Control)groupBoxCulture).Location = new Point(0, 0);
		((Control)groupBoxCulture).Margin = new Padding(6);
		((Control)groupBoxCulture).MinimumSize = new Size(0, 200);
		((Control)groupBoxCulture).Name = "groupBoxCulture";
		((Control)groupBoxCulture).Size = new Size(330, 337);
		((Control)groupBoxCulture).TabIndex = 9;
		((Control)groupBoxCulture).TabStop = false;
		((Control)groupBoxCulture).Text = "Culture";
		buttonActions.AutoExpandOnClick = true;
		buttonActions.Name = "buttonActions";
		buttonActions.SubItems.AddRange(new BaseItem[2] { buttonOpen, buttonSave });
		buttonActions.Text = "Actions";
		((Control)listBoxCultures).Anchor = (AnchorStyles)15;
		((Control)listBoxCultures).BackColor = Color.White;
		((Control)listBoxCultures).Location = new Point(0, 77);
		((Control)listBoxCultures).Margin = new Padding(4);
		((Control)listBoxCultures).Name = "listBoxCultures";
		((Control)listBoxCultures).Size = new Size(330, 260);
		((Control)listBoxCultures).TabIndex = 1;
		((ListBox)listBoxCultures).SelectedIndexChanged += ListBoxCultures_SelectedIndexChanged;
		((Control)listBoxCultures).DoubleClick += ListBoxCultures_DoubleClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Dock = (DockStyle)1;
		stiGroupLine1.LinePosition = StiLinePosition.Bottom;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 47);
		((Control)stiGroupLine1).Margin = new Padding(6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(1874, 8);
		((Control)stiGroupLine1).TabIndex = 21;
		((Control)tbTool).AccessibleDescription = "stiToolBar1 (tbTool)";
		((Control)tbTool).AccessibleName = "stiToolBar1";
		((Control)tbTool).AccessibleRole = (AccessibleRole)22;
		((Control)tbTool).BackColor = Color.White;
		tbTool.BarType = eBarType.MenuBar;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.DockTabStripHeight = 50;
		((Control)tbTool).Font = new Font("Segoe UI", 9f);
		tbTool.Items.AddRange(new BaseItem[6] { buttonActions, buttonAddCulture, buttonRemoveCulture, buttonGetCulture, buttonSetCulture, buttonAutoLocalizeReportOnRun });
		tbTool.Location = new Point(0, 0);
		((Control)tbTool).Margin = new Padding(6);
		tbTool.Name = "tbTool";
		tbTool.Padding = new Padding(12, 4, 4, 4);
		tbTool.RoundCorners = false;
		tbTool.SingleLineColor = Color.Empty;
		tbTool.Size = new Size(1874, 47);
		tbTool.Stretch = true;
		tbTool.Style = eDotNetBarStyle.StyleManagerControlled;
		tbTool.TabIndex = 4;
		tbTool.TabStop = false;
		((Control)tbTool).Text = "stiToolBar1";
		buttonAddCulture.ButtonStyle = eButtonStyle.ImageAndText;
		buttonAddCulture.Image = (Image)componentResourceManager.GetObject("buttonAddCulture.Image");
		buttonAddCulture.ImagePaddingHorizontal = 16;
		buttonAddCulture.ImagePaddingVertical = 12;
		buttonAddCulture.Name = "buttonAddCulture";
		buttonAddCulture.Text = "Add Culture";
		buttonAddCulture.Click += ButtonAddCulture_Click;
		buttonRemoveCulture.ButtonStyle = eButtonStyle.ImageAndText;
		buttonRemoveCulture.Image = (Image)componentResourceManager.GetObject("buttonRemoveCulture.Image");
		buttonRemoveCulture.ImagePaddingHorizontal = 16;
		buttonRemoveCulture.ImagePaddingVertical = 12;
		buttonRemoveCulture.Name = "buttonRemoveCulture";
		buttonRemoveCulture.Click += ButtonRemoveCulture_Click;
		buttonGetCulture.BeginGroup = true;
		buttonGetCulture.Image = (Image)componentResourceManager.GetObject("buttonGetCulture.Image");
		buttonGetCulture.ImagePaddingHorizontal = 16;
		buttonGetCulture.ImagePaddingVertical = 12;
		buttonGetCulture.Name = "buttonGetCulture";
		buttonGetCulture.Click += ButtonGetCulture_Click;
		buttonSetCulture.Image = (Image)componentResourceManager.GetObject("buttonSetCulture.Image");
		buttonSetCulture.ImagePaddingHorizontal = 16;
		buttonSetCulture.ImagePaddingVertical = 12;
		buttonSetCulture.Name = "buttonSetCulture";
		buttonSetCulture.Click += ButtonSetCulture_Click;
		buttonOpen.BeginGroup = true;
		buttonOpen.ImagePaddingHorizontal = 16;
		buttonOpen.ImagePaddingVertical = 12;
		buttonOpen.Name = "buttonOpen";
		buttonOpen.Click += ButtonOpen_Click;
		buttonSave.Image = (Image)componentResourceManager.GetObject("buttonSetCulture.Image");
		buttonSave.ImagePaddingHorizontal = 16;
		buttonSave.ImagePaddingVertical = 12;
		buttonSave.Name = "buttonSave";
		buttonSave.Click += ButtonSave_Click;
		buttonAutoLocalizeReportOnRun.AutoCheckOnClick = true;
		buttonAutoLocalizeReportOnRun.BeginGroup = true;
		buttonAutoLocalizeReportOnRun.ImagePaddingHorizontal = 16;
		buttonAutoLocalizeReportOnRun.ImagePaddingVertical = 12;
		buttonAutoLocalizeReportOnRun.Name = "buttonAutoLocalizeReportOnRun";
		buttonAutoLocalizeReportOnRun.Text = "Auto Localize Report on Run";
		tbAutoLocalizeContainer.AllowItemResize = true;
		tbAutoLocalizeContainer.MenuVisibility = eMenuVisibility.VisibleAlways;
		tbAutoLocalizeContainer.Name = "tbAutoLocalizeContainer";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(1874, 1062);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)panelSettings);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(1010, 873);
		((Control)this).Name = "StiGlobalizationEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Strings";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelSettings).ResumeLayout(false);
		((Control)groupBoxEditor).ResumeLayout(false);
		((Control)groupBoxEditor).PerformLayout();
		((Control)groupBoxProperties).ResumeLayout(false);
		((Control)panelCultureAndGroups).ResumeLayout(false);
		((Control)groupBoxComponents).ResumeLayout(false);
		((Control)groupBoxCulture).ResumeLayout(false);
		((ISupportInitialize)tbTool).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
