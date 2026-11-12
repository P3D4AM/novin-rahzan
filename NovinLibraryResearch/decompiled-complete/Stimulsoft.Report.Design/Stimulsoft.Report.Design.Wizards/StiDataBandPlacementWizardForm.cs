using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Wizards;

public class StiDataBandPlacementWizardForm : Form
{
	private StiDataBand dataBand;

	private IStiTableElement tableElement;

	private IContainer components;

	private StiGroupLine panelButtons;

	private StiColumnsThemes columnsThemes;

	private Button btCancel;

	private Button btOk;

	private StiTabulator tabControl;

	private StiTabulatorPage pageData;

	private StiTabulatorPage pageStyles;

	private StiGroupLine stiGroupLine2;

	private CheckBox checkBoxHeader;

	private CheckBox checkBoxFooter;

	private RadioButton rbDataBand;

	private RadioButton rbTable;

	private StiColumnsSelection columnsSelection;

	private StiColumnsOrder columnsOrder;

	private Panel panelSeparator;

	public StiTable Table { get; private set; }

	public StiPlacementComponentWizard PlacementComponent { get; private set; }

	public StiDataBandPlacementWizardForm(StiDataBand dataBand, StiDataSource dataSource)
		: this(dataBand, null, dataSource)
	{
	}

	public StiDataBandPlacementWizardForm(IStiTableElement tableElement, StiDataSource dataSource)
		: this(null, tableElement, dataSource)
	{
	}

	private StiDataBandPlacementWizardForm(StiDataBand dataBand, IStiTableElement tableElement, StiDataSource dataSource)
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		((Form)this).ShowIcon = false;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.dataBand = dataBand;
		this.tableElement = tableElement;
		if (this.dataBand != null)
		{
			if (this.dataBand.Report != null)
			{
				columnsThemes.AddStyles(this.dataBand.Report.Styles);
			}
			columnsSelection.SelectedDataSource = dataBand.DataSource;
			columnsSelection.SelectedBusinessObject = dataBand.BusinessObject;
		}
		else
		{
			StiColumnsSelection stiColumnsSelection = columnsSelection;
			((Control)stiColumnsSelection).Height = ((Control)stiColumnsSelection).Height + ((Control)stiGroupLine2).Height;
			StiColumnsOrder stiColumnsOrder = columnsOrder;
			((Control)stiColumnsOrder).Height = ((Control)stiColumnsOrder).Height + ((Control)stiGroupLine2).Height;
			((Control)stiGroupLine2).Visible = false;
			((Control)tabControl).Controls.Remove((Control)(object)pageStyles);
			columnsSelection.SelectedDataSource = dataSource;
		}
		columnsSelection.tvColumns.AfterCheck += new TreeViewEventHandler(TreeViewColumns_AfterCheck);
		Localize();
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		checkBoxHeader.Checked = StiSettings.GetBool(((Control)this).Name, "Header", checkBoxHeader.Checked);
		checkBoxFooter.Checked = StiSettings.GetBool(((Control)this).Name, "Footer", checkBoxFooter.Checked);
		tabControl.SelectedIndex = 0;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiDataBand");
		((Control)checkBoxHeader).Text = StiLocalization.Get("Components", "StiHeaderBand");
		((Control)checkBoxFooter).Text = StiLocalization.Get("Components", "StiFooterBand");
		((Control)rbDataBand).Text = StiLocalization.Get("Components", "StiDataBand");
		((Control)rbTable).Text = StiLocalization.Get("Components", "StiTable");
		((Control)pageData).Text = StiLocalization.Get("PropertyMain", "Data");
		((Control)pageStyles).Text = StiLocalization.Get("PropertyMain", "Styles");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private double AlignToMaxGrid(StiPage page, double value, bool converted)
	{
		if (converted)
		{
			value = page.Unit.ConvertFromHInches(value);
		}
		return StiAlignValue.AlignToMaxGrid(value, page.GridSize, page.Report.Info.AlignToGrid);
	}

	private double AlignToGrid(StiPage page, double value, bool converted)
	{
		if (converted)
		{
			value = page.Unit.ConvertFromHInches(value);
		}
		return StiAlignValue.AlignToGrid(value, page.GridSize, page.Report.Info.AlignToGrid);
	}

	private void AddTableElement()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		foreach (TreeNode node in columnsOrder.OrderedColumns.Nodes)
		{
			TreeNode val = node;
			StiDataColumn columnFromPath = StiDataBuilder.GetColumnFromPath(val.Tag as string, tableElement.Report.Dictionary);
			if (columnFromPath != null)
			{
				tableElement.CreateMeter(columnFromPath);
			}
		}
	}

	private void AddBataBand()
	{
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		List<StiComponent> list = new List<StiComponent>();
		list.Add(dataBand);
		TreeNodeCollection nodes = columnsOrder.OrderedColumns.Nodes;
		string text = StiNameValidator.CorrectName(dataBand.DataSourceName, dataBand.Report);
		double num = ((dataBand.Parent is StiPage) ? dataBand.Page.GetColumnWidth() : dataBand.Parent.Width);
		StiHeaderBand stiHeaderBand = null;
		if (checkBoxHeader.Checked)
		{
			stiHeaderBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.HeaderBand) as StiHeaderBand;
			string str = (dataBand.Report.Info.GenerateLocalizedName ? (stiHeaderBand.LocalizedName + text) : ("Header" + text));
			stiHeaderBand.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
			stiHeaderBand.Height = AlignToMaxGrid(dataBand.Page, 30.0, converted: true);
			int index = dataBand.Parent.Components.IndexOf(dataBand);
			dataBand.Parent.Components.Insert(index, stiHeaderBand);
			list.Add(stiHeaderBand);
		}
		StiFooterBand stiFooterBand = null;
		if (checkBoxFooter.Checked)
		{
			stiFooterBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.FooterBand) as StiFooterBand;
			string str2 = (dataBand.Report.Info.GenerateLocalizedName ? (stiFooterBand.LocalizedName + text) : ("Footer" + text));
			stiFooterBand.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str2, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
			stiFooterBand.Height = AlignToMaxGrid(dataBand.Page, 30.0, converted: true);
			int num2 = dataBand.Parent.Components.IndexOf(dataBand);
			dataBand.Parent.Components.Insert(num2 + 1, stiFooterBand);
			list.Add(stiFooterBand);
		}
		double num3 = AlignToGrid(dataBand.Page, num / (double)nodes.Count, converted: false);
		if (!(dataBand.Parent is StiPage))
		{
			num3 = AlignToGrid(dataBand.Page, num / (double)nodes.Count, converted: false);
		}
		double num4 = 0.0;
		int num5 = 1;
		foreach (TreeNode item in nodes)
		{
			TreeNode val = item;
			if (num5 == nodes.Count)
			{
				num3 = AlignToGrid(dataBand.Page, num - num4, converted: false);
				if (num3 <= 0.0)
				{
					num3 = dataBand.Page.GridSize;
				}
			}
			if (checkBoxHeader.Checked)
			{
				StiText stiText = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
				stiText.VertAlignment = StiVertAlignment.Center;
				stiText.Font = new Font("Arial", 10f, (FontStyle)1);
				string str3 = (dataBand.Report.Info.GenerateLocalizedName ? (stiHeaderBand.LocalizedName + val.Text) : ("Header" + val.Text));
				stiText.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str3, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				stiText.Top = 0.0;
				stiText.Left = num4;
				stiText.Width = num3;
				stiText.Height = stiHeaderBand.Height;
				stiText.WordWrap = true;
				int num6 = val.Text.LastIndexOf('.');
				StiDataColumn columnFromPath = StiDataBuilder.GetColumnFromPath(val.Tag as string, dataBand.Report.Dictionary);
				if (columnFromPath != null)
				{
					stiText.Text = columnFromPath.Alias;
				}
				else if (num6 == -1)
				{
					stiText.Text = val.Text;
				}
				else
				{
					stiText.Text = val.Text.Substring(num6 + 1);
				}
				stiHeaderBand.Components.Add(stiText);
				list.Add(stiText);
			}
			StiDataColumn columnFromPath2 = StiDataBuilder.GetColumnFromPath(val.Tag as string, dataBand.Report.Dictionary);
			if (columnFromPath2 != null && (columnFromPath2.Type == typeof(Image) || columnFromPath2.Type == typeof(byte[])))
			{
				StiImage stiImage = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image) as StiImage;
				string str4 = (dataBand.Report.Info.GenerateLocalizedName ? (dataBand.LocalizedName + val.Text) : ("Data" + val.Text));
				stiImage.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str4, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				stiImage.Top = 0.0;
				stiImage.Left = num4;
				stiImage.Width = num3;
				stiImage.Height = dataBand.Height;
				stiImage.DataColumn = val.Text;
				stiImage.CanGrow = true;
				dataBand.Components.Add(stiImage);
				list.Add(stiImage);
			}
			else if (columnFromPath2 != null && columnFromPath2.Type == typeof(bool))
			{
				Stimulsoft.Report.Components.StiCheckBox stiCheckBox = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.CheckBox) as Stimulsoft.Report.Components.StiCheckBox;
				string str5 = (dataBand.Report.Info.GenerateLocalizedName ? (dataBand.LocalizedName + val.Text) : ("Data" + val.Text));
				stiCheckBox.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str5, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				stiCheckBox.Top = 0.0;
				stiCheckBox.Left = num4;
				stiCheckBox.Width = num3;
				stiCheckBox.Height = dataBand.Height;
				stiCheckBox.Checked.Value = "{" + val.Tag?.ToString() + "}";
				dataBand.Components.Add(stiCheckBox);
				list.Add(stiCheckBox);
			}
			else
			{
				StiText stiText2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
				string str6 = (dataBand.Report.Info.GenerateLocalizedName ? (dataBand.LocalizedName + val.Text) : ("Data" + val.Text));
				stiText2.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str6, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				stiText2.VertAlignment = StiVertAlignment.Center;
				stiText2.Top = 0.0;
				stiText2.Left = num4;
				stiText2.Width = num3;
				stiText2.Height = dataBand.Height;
				stiText2.Text = "{" + val.Tag?.ToString() + "}";
				stiText2.CanGrow = true;
				stiText2.WordWrap = true;
				dataBand.Components.Add(stiText2);
				list.Add(stiText2);
			}
			if (checkBoxFooter.Checked)
			{
				StiText stiText3 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
				stiText3.VertAlignment = StiVertAlignment.Center;
				stiText3.HorAlignment = StiTextHorAlignment.Right;
				stiText3.Font = new Font("Arial", 10f, (FontStyle)1);
				string text2 = (dataBand.Report.Info.GenerateLocalizedName ? (stiFooterBand.LocalizedName + val.Text) : ("Footer" + val.Text));
				stiText3.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName("Footer" + val.Text, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
				stiText3.Top = 0.0;
				stiText3.Left = num4;
				stiText3.Width = num3;
				stiText3.Height = stiFooterBand.Height;
				stiText3.WordWrap = true;
				stiFooterBand.Components.Add(stiText3);
				list.Add(stiText3);
			}
			num4 += num3;
			num5++;
		}
		StiStylesCollection selectedStylesCollection = columnsThemes.SelectedStylesCollection;
		if (selectedStylesCollection == null)
		{
			return;
		}
		List<StiBaseStyle> list2 = selectedStylesCollection.ToList();
		foreach (StiBaseStyle item2 in list2)
		{
			dataBand.Report.Styles[item2.Name] = item2;
		}
		string collectionName = null;
		foreach (StiBaseStyle item3 in list2)
		{
			collectionName = item3.CollectionName;
		}
		dataBand.Report.ApplyStyleCollection(collectionName, list);
	}

	private void AddTable()
	{
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		TreeNodeCollection nodes = columnsOrder.OrderedColumns.Nodes;
		if (nodes.Count == 0)
		{
			PlacementComponent = StiPlacementComponentWizard.DataBand;
			return;
		}
		Table = new StiTable
		{
			DataSourceName = dataBand.DataSourceName,
			DataRelationName = dataBand.DataRelationName,
			BusinessObjectGuid = dataBand.BusinessObjectGuid
		};
		string text = ((dataBand.DataSourceName == null) ? StiNameValidator.CorrectName(dataBand.BusinessObject.Name, dataBand.Report) : StiNameValidator.CorrectName(dataBand.DataSourceName, dataBand.Report));
		string str = (dataBand.Report.Info.GenerateLocalizedName ? (Table.LocalizedName + text) : ("Table" + text));
		Table.Name = StiNameCreation.CreateName(dataBand.Report, StiNameValidator.CorrectName(str, dataBand.Report), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
		double width = ((dataBand.Parent is StiPage) ? dataBand.Page.GetColumnWidth() : dataBand.Parent.Width);
		int num = 1;
		if (checkBoxHeader.Checked)
		{
			num++;
		}
		if (checkBoxFooter.Checked)
		{
			num++;
		}
		Table.Page = dataBand.Page;
		Table.Parent = dataBand.Parent;
		Table.RowCount = num;
		Table.ColumnCount = nodes.Count;
		Table.HeaderRowsCount = (checkBoxHeader.Checked ? 1 : 0);
		Table.FooterRowsCount = (checkBoxFooter.Checked ? 1 : 0);
		Table.Height = dataBand.Page.GridSize * 4.0 * (double)num;
		Table.Width = width;
		int num2 = 0;
		int num3 = (checkBoxHeader.Checked ? nodes.Count : 0);
		foreach (TreeNode item in nodes)
		{
			TreeNode val = item;
			if (checkBoxHeader.Checked)
			{
				StiTableCell stiTableCell = (StiTableCell)Table.Components[num2];
				int num4 = val.Text.LastIndexOf('.');
				StiDataColumn columnFromPath = StiDataBuilder.GetColumnFromPath(val.Tag as string, dataBand.Report.Dictionary);
				if (columnFromPath != null)
				{
					stiTableCell.Text = columnFromPath.Alias;
				}
				else if (num4 == -1)
				{
					stiTableCell.Text = val.Text;
				}
				else
				{
					stiTableCell.Text = val.Text.Substring(num4 + 1);
				}
				num2++;
			}
			StiDataColumn columnFromPath2 = StiDataBuilder.GetColumnFromPath(val.Tag as string, dataBand.Report.Dictionary);
			if (columnFromPath2 != null && (columnFromPath2.Type == typeof(Image) || columnFromPath2.Type == typeof(byte[])))
			{
				((IStiTableCell)Table.Components[num3]).CellType = StiTablceCellType.Image;
				StiTableCellImage stiTableCellImage = (StiTableCellImage)Table.Components[num3];
				stiTableCellImage.DataColumn = val.Text;
				num3++;
			}
			else if (columnFromPath2 != null && columnFromPath2.Type == typeof(bool))
			{
				((IStiTableCell)Table.Components[num3]).CellType = StiTablceCellType.CheckBox;
				StiTableCellCheckBox stiTableCellCheckBox = (StiTableCellCheckBox)Table.Components[num3];
				stiTableCellCheckBox.Checked.Value = "{" + val.Tag?.ToString() + "}";
				num3++;
			}
			else
			{
				StiTableCell stiTableCell2 = (StiTableCell)Table.Components[num3];
				stiTableCell2.Text = "{" + val.Tag?.ToString() + "}";
				num3++;
			}
		}
		PlacementComponent = StiPlacementComponentWizard.Table;
	}

	private void TreeViewColumns_AfterCheck(object sender, TreeViewEventArgs e)
	{
		columnsOrder.SelectedColumns = columnsSelection.Columns;
	}

	private void PanelSeparator_Paint(object sender, PaintEventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		Pen val = new Pen(Color.Gray);
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(0, 0);
			Point point2 = new Point(0, ((Control)panelSeparator).Height);
			e.Graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (tableElement != null)
		{
			AddTableElement();
		}
		else if (rbDataBand.Checked)
		{
			AddBataBand();
		}
		else
		{
			AddTable();
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "Header", checkBoxHeader.Checked);
		StiSettings.Set(((Control)this).Name, "Footer", checkBoxFooter.Checked);
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/reports_designer_creating_reports_in_designer_overview_label_report_wizard.htm");
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		panelButtons = new StiGroupLine();
		btCancel = new Button();
		btOk = new Button();
		tabControl = new StiTabulator();
		pageData = new StiTabulatorPage();
		panelSeparator = new Panel();
		stiGroupLine2 = new StiGroupLine();
		checkBoxHeader = new CheckBox();
		checkBoxFooter = new CheckBox();
		rbDataBand = new RadioButton();
		rbTable = new RadioButton();
		pageStyles = new StiTabulatorPage();
		columnsSelection = new StiColumnsSelection();
		columnsOrder = new StiColumnsOrder();
		columnsThemes = new StiColumnsThemes();
		((Control)panelButtons).SuspendLayout();
		((Control)tabControl).SuspendLayout();
		((Control)pageData).SuspendLayout();
		((Control)stiGroupLine2).SuspendLayout();
		((Control)pageStyles).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelButtons).BackColor = Color.White;
		((Control)panelButtons).Controls.Add((Control)(object)btCancel);
		((Control)panelButtons).Controls.Add((Control)(object)btOk);
		((Control)panelButtons).Dock = (DockStyle)2;
		panelButtons.LinePosition = StiLinePosition.Top;
		panelButtons.LineStyle = StiLineStyle.Dot;
		((Control)panelButtons).Location = new Point(0, 379);
		((Control)panelButtons).Name = "panelButtons";
		((Control)panelButtons).Size = new Size(584, 46);
		((Control)panelButtons).TabIndex = 6;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(497, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(417, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)tabControl).Anchor = (AnchorStyles)15;
		((Control)tabControl).Controls.Add((Control)(object)pageData);
		((Control)tabControl).Controls.Add((Control)(object)pageStyles);
		((Control)tabControl).Location = new Point(12, 12);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 22, 0, 0);
		((Control)tabControl).Size = new Size(559, 352);
		((Control)tabControl).TabIndex = 7;
		tabControl.TitleBackColor = Color.White;
		((Control)pageData).Controls.Add((Control)(object)columnsSelection);
		((Control)pageData).Controls.Add((Control)(object)panelSeparator);
		((Control)pageData).Controls.Add((Control)(object)columnsOrder);
		((Control)pageData).Controls.Add((Control)(object)stiGroupLine2);
		pageData.Dock = (DockStyle)5;
		pageData.InvisibleVisible = true;
		((Control)pageData).Location = new Point(0, 22);
		((Control)pageData).Name = "pageData";
		((Control)pageData).Padding = new Padding(2);
		((Control)pageData).Size = new Size(559, 330);
		((Control)pageData).TabIndex = 0;
		((Control)pageData).Text = "Data";
		((Control)pageData).Visible = false;
		((Control)panelSeparator).Dock = (DockStyle)4;
		((Control)panelSeparator).Location = new Point(295, 2);
		((Control)panelSeparator).Name = "panelSeparator";
		((Control)panelSeparator).Size = new Size(4, 257);
		((Control)panelSeparator).TabIndex = 11;
		((Control)panelSeparator).Paint += new PaintEventHandler(PanelSeparator_Paint);
		((Control)stiGroupLine2).BackColor = Color.White;
		((Control)stiGroupLine2).Controls.Add((Control)(object)checkBoxHeader);
		((Control)stiGroupLine2).Controls.Add((Control)(object)checkBoxFooter);
		((Control)stiGroupLine2).Controls.Add((Control)(object)rbDataBand);
		((Control)stiGroupLine2).Controls.Add((Control)(object)rbTable);
		((Control)stiGroupLine2).Dock = (DockStyle)2;
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(2, 259);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(555, 69);
		((Control)stiGroupLine2).TabIndex = 8;
		((Control)checkBoxHeader).ForeColor = SystemColors.ControlText;
		((Control)checkBoxHeader).Location = new Point(177, 12);
		((Control)checkBoxHeader).Name = "checkBoxHeader";
		((Control)checkBoxHeader).Size = new Size(280, 18);
		((Control)checkBoxHeader).TabIndex = 0;
		((Control)checkBoxHeader).Text = "Header";
		((Control)checkBoxFooter).ForeColor = SystemColors.ControlText;
		((Control)checkBoxFooter).Location = new Point(177, 40);
		((Control)checkBoxFooter).Name = "checkBoxFooter";
		((Control)checkBoxFooter).Size = new Size(280, 18);
		((Control)checkBoxFooter).TabIndex = 1;
		((Control)checkBoxFooter).Text = "Footer";
		((Control)rbDataBand).AutoSize = true;
		rbDataBand.Checked = true;
		((Control)rbDataBand).Location = new Point(12, 12);
		((Control)rbDataBand).Name = "rbDataBand";
		((Control)rbDataBand).Size = new Size(76, 17);
		((Control)rbDataBand).TabIndex = 2;
		rbDataBand.TabStop = true;
		((Control)rbDataBand).Text = "Data Band";
		((ButtonBase)rbDataBand).UseVisualStyleBackColor = true;
		((Control)rbTable).AutoSize = true;
		((Control)rbTable).Location = new Point(12, 41);
		((Control)rbTable).Name = "rbTable";
		((Control)rbTable).Size = new Size(52, 17);
		((Control)rbTable).TabIndex = 3;
		((Control)rbTable).Text = "Table";
		((ButtonBase)rbTable).UseVisualStyleBackColor = true;
		((Control)pageStyles).Controls.Add((Control)(object)columnsThemes);
		pageStyles.Dock = (DockStyle)5;
		pageStyles.InvisibleVisible = false;
		((Control)pageStyles).Location = new Point(0, 23);
		((Control)pageStyles).Name = "pageStyles";
		((Control)pageStyles).Padding = new Padding(0, 0, 1, 0);
		((Control)pageStyles).Size = new Size(559, 329);
		((Control)pageStyles).TabIndex = 1;
		((Control)pageStyles).Text = "Styles";
		((Control)pageStyles).Visible = false;
		((Control)columnsSelection).BackColor = Color.White;
		columnsSelection.CheckedBusinessObjects = null;
		columnsSelection.CheckedDataSources = null;
		((Control)columnsSelection).Dock = (DockStyle)5;
		((Control)columnsSelection).Location = new Point(2, 2);
		((Control)columnsSelection).Margin = new Padding(6);
		((Control)columnsSelection).Name = "columnsSelection";
		columnsSelection.SelectedBusinessObject = null;
		columnsSelection.SelectedDataSource = null;
		((Control)columnsSelection).Size = new Size(293, 257);
		((Control)columnsSelection).TabIndex = 9;
		((Control)columnsOrder).BackColor = Color.White;
		((Control)columnsOrder).Dock = (DockStyle)4;
		((Control)columnsOrder).Location = new Point(299, 2);
		((Control)columnsOrder).Margin = new Padding(6);
		((Control)columnsOrder).Name = "columnsOrder";
		columnsOrder.SelectedColumns = null;
		((Control)columnsOrder).Size = new Size(258, 257);
		((Control)columnsOrder).TabIndex = 10;
		((Control)columnsThemes).Anchor = (AnchorStyles)15;
		((Control)columnsThemes).Location = new Point(4, 4);
		((Control)columnsThemes).Margin = new Padding(6);
		((Control)columnsThemes).Name = "columnsThemes";
		((Control)columnsThemes).Size = new Size(551, 321);
		((Control)columnsThemes).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(584, 425);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)panelButtons);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(600, 464);
		((Control)this).Name = "StiDataBandPlacementWizardForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Wizard";
		((Form)this).Closed += This_Closed;
		((Control)panelButtons).ResumeLayout(false);
		((Control)tabControl).ResumeLayout(false);
		((Control)pageData).ResumeLayout(false);
		((Control)stiGroupLine2).ResumeLayout(false);
		((Control)stiGroupLine2).PerformLayout();
		((Control)pageStyles).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
