using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiStandardReportWizardForm : StiReportWizardForm
{
	private int TabIndexColumns = 1;

	private int TabIndexOrder = 2;

	private int TabIndexSort = 3;

	private int TabIndexFilters = 4;

	private int TabIndexGroup = 5;

	private int TabIndexRelation = 6;

	private int TabIndexTotals = 7;

	private StiFiltersCollection Filters = new StiFiltersCollection();

	private StiDataSourcesCollection availableDataSources;

	private StiBusinessObjectsCollection availableBusinessObjects;

	private ArrayList sorts = new ArrayList();

	private ArrayList filters = new ArrayList();

	private ArrayList totals = new ArrayList();

	private ArrayList relations = new ArrayList();

	private bool isMasterDetailWizard;

	private IContainer components;

	private ImageList ilButtons;

	private StiDataSourcePanel pnAvailableDataSources;

	private StiColumnsSelection columnsSelection;

	private StiColumnsOrder columnsOrder;

	private StiColumnsGroup columnsGroup;

	private StiColumnsThemes columnsThemes;

	private StiWizard Wizard;

	private StiWizardPage wpDataSource;

	private StiWizardPage wpSelectColumns;

	private StiWizardPage wpColumsOrder;

	private StiWizardPage wpSort;

	private StiWizardPage wpTotals;

	private StiWizardPage wpThemes;

	private StiWizardPage wpFilters;

	private StiWizardPage wpLayout;

	internal StiReportLayout reportLayout;

	private StiWizardPage wpRelation;

	private StiWizardPage wpGroups;

	public bool IsMasterDetailWizard
	{
		get
		{
			return isMasterDetailWizard;
		}
		set
		{
			isMasterDetailWizard = value;
		}
	}

	public StiStandardReportWizardForm(StiReport report, bool isMasterDetailWizard)
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (report == null)
			{
				availableDataSources = new StiDataSourcesCollection(null);
				availableBusinessObjects = new StiBusinessObjectsCollection(null, null);
			}
			else
			{
				availableDataSources = report.DataSources;
				availableBusinessObjects = report.Dictionary.BusinessObjects;
			}
			base.Report = report;
			this.isMasterDetailWizard = isMasterDetailWizard;
			Localize();
			BuildDataSources();
			if (!isMasterDetailWizard)
			{
				((TreeView)pnAvailableDataSources.tvDataSources).AfterSelect += new TreeViewEventHandler(OnUpdateWizardState);
			}
			else
			{
				((TreeView)pnAvailableDataSources.tvDataSources).AfterCheck += new TreeViewEventHandler(OnUpdateWizardState);
			}
			columnsSelection.tvColumns.AfterCheck += new TreeViewEventHandler(OnUpdateWizardState);
			((TreeView)pnAvailableDataSources.tvDataSources).CheckBoxes = IsMasterDetailWizard;
			if (StiOptions.Designer.AllowUseDragDrop)
			{
				((Control)pnAvailableDataSources.tvDataSources).AllowDrop = IsMasterDetailWizard;
			}
			columnsGroup.IsMasterDetailWizard = IsMasterDetailWizard;
			if (!IsMasterDetailWizard)
			{
				TabIndexRelation = -99;
				((Control)Wizard).Controls.Remove((Control)(object)wpRelation);
				TabIndexTotals--;
			}
			if (report != null)
			{
				columnsThemes.AddStyles(report.Styles);
			}
			StiSettings.Load();
			reportLayout.rbCentimetres.Checked = StiSettings.GetBool("Wizard", "Centimetres", RegionInfo.CurrentRegion.IsMetric);
			reportLayout.rbMillimeters.Checked = StiSettings.GetBool("Wizard", "Millimeters", nullValue: false);
			reportLayout.rbInch.Checked = StiSettings.GetBool("Wizard", "Inch", !RegionInfo.CurrentRegion.IsMetric);
			reportLayout.rbHundredthsOfInch.Checked = StiSettings.GetBool("Wizard", "HundredthsOfInch", nullValue: false);
			reportLayout.rbCSharp.Checked = StiSettings.GetBool("Wizard", "CSharp", nullValue: true);
			reportLayout.rbVB.Checked = StiSettings.GetBool("Wizard", "VB", nullValue: false);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Wizards", "title");
		Wizard.BackText = StiLocalization.Get("Wizards", "ButtonBack");
		Wizard.CancelText = StiLocalization.Get("Wizards", "ButtonCancel");
		Wizard.FinishText = StiLocalization.Get("Wizards", "ButtonFinish");
		Wizard.NextText = StiLocalization.Get("Wizards", "ButtonNext");
		((Control)wpColumsOrder).Text = StiLocalization.Get("Wizards", "ColumnsOrder");
		((Control)wpDataSource).Text = (IsMasterDetailWizard ? StiLocalization.Get("Wizards", "DataSources") : StiLocalization.Get("Wizards", "DataSource"));
		((Control)wpGroups).Text = StiLocalization.Get("Wizards", "Groups");
		((Control)wpSelectColumns).Text = StiLocalization.Get("Wizards", "SelectColumns");
		((Control)wpSort).Text = StiLocalization.Get("Wizards", "Sort");
		((Control)wpFilters).Text = StiLocalization.Get("Wizards", "Filters");
		((Control)wpRelation).Text = StiLocalization.Get("Wizards", "DataRelation");
		((Control)wpTotals).Text = StiLocalization.Get("Wizards", "Totals");
		((Control)wpThemes).Text = StiLocalization.Get("Wizards", "Themes");
		((Control)wpLayout).Text = StiLocalization.Get("Wizards", "Layout");
		wpColumsOrder.Description = StiLocalization.Get("Wizards", "infoColumnsOrder");
		wpDataSource.Description = StiLocalization.Get("Wizards", "infoDataSource");
		wpDataSource.Description = (IsMasterDetailWizard ? StiLocalization.Get("Wizards", "infoDataSources") : StiLocalization.Get("Wizards", "infoDataSource"));
		wpGroups.Description = StiLocalization.Get("Wizards", "infoGroups");
		wpSelectColumns.Description = StiLocalization.Get("Wizards", "infoSelectColumns");
		wpSort.Description = StiLocalization.Get("Wizards", "infoSort");
		wpRelation.Description = StiLocalization.Get("Wizards", "infoRelation");
		wpTotals.Description = StiLocalization.Get("Wizards", "infoTotals");
		wpThemes.Description = StiLocalization.Get("Wizards", "infoThemes");
		wpFilters.Description = StiLocalization.Get("Wizards", "infoFilters");
		wpLayout.Description = StiLocalization.Get("Wizards", "infoLayout");
	}

	private void BuildDataSources()
	{
		pnAvailableDataSources.Build(availableDataSources, base.Report);
		pnAvailableDataSources.Build(availableBusinessObjects, clear: false);
	}

	private string GetDataSourceName(string text)
	{
		int num = text.IndexOf(".", StringComparison.InvariantCulture);
		if (num == -1)
		{
			return string.Empty;
		}
		return text.Substring(0, num);
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

	private void OnUpdateWizardState(object sender, TreeViewEventArgs e)
	{
		Wizard.UpdateWizardState();
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		string url = (isMasterDetailWizard ? "user-manual/reports_designer_creating_reports_in_designer_overview_master_detail_report_wizard.htm" : "user-manual/reports_designer_creating_reports_in_designer_overview_standard_report_wizard.htm");
		StiHelpProvider.ShowHelpViewer(url);
	}

	private void Wizard_CanFinish(object sender, StiCanFinishEventArgs e)
	{
		e.CanFinish = Wizard.SelectedIndex > 1;
	}

	private void StiStandardReportWizardForm_Closed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set("Wizard", "Centimetres", reportLayout.rbCentimetres.Checked);
		StiSettings.Set("Wizard", "Millimeters", reportLayout.rbMillimeters.Checked);
		StiSettings.Set("Wizard", "Inch", reportLayout.rbInch.Checked);
		StiSettings.Set("Wizard", "HundredthsOfInch", reportLayout.rbHundredthsOfInch.Checked);
		StiSettings.Set("Wizard", "CSharp", reportLayout.rbCSharp.Checked);
		StiSettings.Set("Wizard", "VB", reportLayout.rbVB.Checked);
		StiSettings.Save();
	}

	private void Wizard_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be7: Expected O, but got Unknown
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Expected O, but got Unknown
		if (Wizard.SelectedIndex == TabIndexColumns)
		{
			if (pnAvailableDataSources.CheckedDataSourcesOrBusinessObject)
			{
				columnsSelection.SetCheckedDataSourcesOrBusinessObject(pnAvailableDataSources.CheckedDataSources, pnAvailableDataSources.CheckedBusinessObjects);
			}
			else
			{
				columnsSelection.SetDataSourcesOrBusinessObject(pnAvailableDataSources.SelectedDataSource, pnAvailableDataSources.SelectedBusinessObject);
			}
		}
		else if (Wizard.SelectedIndex == TabIndexOrder)
		{
			columnsOrder.SelectedColumns = columnsSelection.Columns;
		}
		else if (Wizard.SelectedIndex == TabIndexSort)
		{
			if (!pnAvailableDataSources.CheckedDataSourcesOrBusinessObject)
			{
				StiColumnsSort stiColumnsSort = null;
				if (((ArrangedElementCollection)((Control)wpSort).Controls).Count > 0 && ((Control)wpSort).Controls[0] is StiColumnsSort)
				{
					stiColumnsSort = ((Control)wpSort).Controls[0] as StiColumnsSort;
				}
				else
				{
					((Control)wpSort).Controls.Clear();
					StiColumnsSort stiColumnsSort2 = new StiColumnsSort();
					((Control)stiColumnsSort2).Dock = (DockStyle)5;
					stiColumnsSort = stiColumnsSort2;
					((Control)wpSort).Controls.Add((Control)(object)stiColumnsSort);
				}
				stiColumnsSort.SetDataSourceOrBusinessObject(pnAvailableDataSources.SelectedDataSource, pnAvailableDataSources.SelectedBusinessObject);
				sorts.Clear();
				sorts.Add(stiColumnsSort);
				return;
			}
			StiTabulator stiTabulator = null;
			if (((ArrangedElementCollection)((Control)wpSort).Controls).Count > 0 && ((Control)wpSort).Controls[0] is StiTabulator)
			{
				stiTabulator = ((Control)wpSort).Controls[0] as StiTabulator;
			}
			else
			{
				((Control)wpSort).Controls.Clear();
				StiTabulator obj = new StiTabulator
				{
					TabTitlePosition = StiTabTitlePosition.RightHorizontal
				};
				((Control)obj).Dock = (DockStyle)5;
				stiTabulator = obj;
				stiTabulator.ControlStyle = StiControlStyle.Office2013Blue;
				((Control)wpSort).Controls.Add((Control)(object)stiTabulator);
			}
			sorts.Clear();
			Hashtable hashtable = new Hashtable();
			foreach (Control item in (ArrangedElementCollection)((Control)stiTabulator).Controls)
			{
				Control val = item;
				hashtable[val.Text] = val;
			}
			((Control)stiTabulator).Controls.Clear();
			StiDataSourcesCollection checkedDataSources = pnAvailableDataSources.CheckedDataSources;
			if (checkedDataSources != null)
			{
				foreach (StiDataSource item2 in checkedDataSources)
				{
					StiTabulatorPage stiTabulatorPage = hashtable[item2.Name] as StiTabulatorPage;
					if (stiTabulatorPage == null)
					{
						StiTabulatorPage stiTabulatorPage2 = new StiTabulatorPage();
						((Control)stiTabulatorPage2).Text = item2.Name;
						stiTabulatorPage = stiTabulatorPage2;
						StiColumnsSort stiColumnsSort3 = new StiColumnsSort();
						((Control)stiColumnsSort3).Dock = (DockStyle)5;
						stiColumnsSort3.DataSource = item2;
						StiColumnsSort stiColumnsSort4 = stiColumnsSort3;
						((Control)stiTabulatorPage).Controls.Add((Control)(object)stiColumnsSort4);
						sorts.Add(stiColumnsSort4);
					}
					else
					{
						sorts.Add(((Control)stiTabulatorPage).Controls[0]);
					}
					((Control)stiTabulator).Controls.Add((Control)(object)stiTabulatorPage);
				}
			}
			StiBusinessObjectsCollection checkedBusinessObjects = pnAvailableDataSources.CheckedBusinessObjects;
			if (checkedBusinessObjects != null)
			{
				foreach (StiBusinessObject item3 in checkedBusinessObjects)
				{
					StiTabulatorPage stiTabulatorPage3 = hashtable[item3.Name] as StiTabulatorPage;
					if (stiTabulatorPage3 == null)
					{
						StiTabulatorPage stiTabulatorPage4 = new StiTabulatorPage();
						((Control)stiTabulatorPage4).Text = item3.Name;
						stiTabulatorPage3 = stiTabulatorPage4;
						StiColumnsSort stiColumnsSort5 = new StiColumnsSort();
						((Control)stiColumnsSort5).Dock = (DockStyle)5;
						stiColumnsSort5.BusinessObject = item3;
						StiColumnsSort stiColumnsSort6 = stiColumnsSort5;
						((Control)stiTabulatorPage3).Controls.Add((Control)(object)stiColumnsSort6);
						sorts.Add(stiColumnsSort6);
					}
					else
					{
						sorts.Add(((Control)stiTabulatorPage3).Controls[0]);
					}
					((Control)stiTabulator).Controls.Add((Control)(object)stiTabulatorPage3);
				}
			}
			hashtable.Clear();
		}
		else if (Wizard.SelectedIndex == TabIndexFilters)
		{
			if (!pnAvailableDataSources.CheckedDataSourcesOrBusinessObject)
			{
				StiColumnsFilters stiColumnsFilters = null;
				if (((ArrangedElementCollection)((Control)wpFilters).Controls).Count > 0 && ((Control)wpFilters).Controls[0] is StiColumnsFilters)
				{
					stiColumnsFilters = ((Control)wpFilters).Controls[0] as StiColumnsFilters;
				}
				else
				{
					((Control)wpFilters).Controls.Clear();
					StiColumnsFilters stiColumnsFilters2 = new StiColumnsFilters();
					((Control)stiColumnsFilters2).Dock = (DockStyle)5;
					stiColumnsFilters = stiColumnsFilters2;
					((Control)wpFilters).Controls.Add((Control)(object)stiColumnsFilters);
				}
				stiColumnsFilters.DataSource = pnAvailableDataSources.SelectedDataSource;
				stiColumnsFilters.BusinessObject = pnAvailableDataSources.SelectedBusinessObject;
				filters.Clear();
				filters.Add(stiColumnsFilters);
				return;
			}
			StiTabulator stiTabulator2 = null;
			if (((ArrangedElementCollection)((Control)wpFilters).Controls).Count > 0 && ((Control)wpFilters).Controls[0] is StiTabulator)
			{
				stiTabulator2 = ((Control)wpFilters).Controls[0] as StiTabulator;
			}
			else
			{
				((Control)wpFilters).Controls.Clear();
				stiTabulator2 = new StiTabulator();
				stiTabulator2.TabTitlePosition = StiTabTitlePosition.RightHorizontal;
				stiTabulator2.ControlStyle = StiControlStyle.Office2013Blue;
				((Control)stiTabulator2).Dock = (DockStyle)5;
				((Control)wpFilters).Controls.Add((Control)(object)stiTabulator2);
			}
			filters.Clear();
			Hashtable hashtable2 = new Hashtable();
			foreach (Control item4 in (ArrangedElementCollection)((Control)stiTabulator2).Controls)
			{
				Control val2 = item4;
				hashtable2[val2.Text] = val2;
			}
			((Control)stiTabulator2).Controls.Clear();
			StiDataSourcesCollection checkedDataSources2 = pnAvailableDataSources.CheckedDataSources;
			if (checkedDataSources2 != null)
			{
				foreach (StiDataSource item5 in checkedDataSources2)
				{
					StiTabulatorPage stiTabulatorPage5 = hashtable2[item5.Name] as StiTabulatorPage;
					if (stiTabulatorPage5 == null)
					{
						stiTabulatorPage5 = new StiTabulatorPage();
						((Control)stiTabulatorPage5).Text = item5.Name;
						StiColumnsFilters stiColumnsFilters3 = new StiColumnsFilters();
						((Control)stiColumnsFilters3).Dock = (DockStyle)5;
						stiColumnsFilters3.DataSource = item5;
						stiColumnsFilters3.BuildFilters(Filters);
						((Control)stiTabulatorPage5).Controls.Add((Control)(object)stiColumnsFilters3);
						sorts.Add(stiColumnsFilters3);
					}
					filters.Add(((Control)stiTabulatorPage5).Controls[0]);
					((Control)stiTabulator2).Controls.Add((Control)(object)stiTabulatorPage5);
				}
			}
			StiBusinessObjectsCollection checkedBusinessObjects2 = pnAvailableDataSources.CheckedBusinessObjects;
			if (checkedBusinessObjects2 != null)
			{
				foreach (StiBusinessObject item6 in checkedBusinessObjects2)
				{
					StiTabulatorPage stiTabulatorPage6 = hashtable2[item6.Name] as StiTabulatorPage;
					if (stiTabulatorPage6 == null)
					{
						StiTabulatorPage stiTabulatorPage7 = new StiTabulatorPage();
						((Control)stiTabulatorPage7).Text = item6.Name;
						stiTabulatorPage6 = stiTabulatorPage7;
						StiColumnsFilters stiColumnsFilters4 = new StiColumnsFilters();
						((Control)stiColumnsFilters4).Dock = (DockStyle)5;
						stiColumnsFilters4.BusinessObject = item6;
						StiColumnsFilters stiColumnsFilters5 = stiColumnsFilters4;
						stiColumnsFilters5.BuildFilters(Filters);
						((Control)stiTabulatorPage6).Controls.Add((Control)(object)stiColumnsFilters5);
						sorts.Add(stiColumnsFilters5);
					}
					filters.Add(((Control)stiTabulatorPage6).Controls[0]);
					((Control)stiTabulator2).Controls.Add((Control)(object)stiTabulatorPage6);
				}
			}
			hashtable2.Clear();
		}
		else if (Wizard.SelectedIndex == TabIndexGroup)
		{
			columnsGroup.SelectedColumns = columnsOrder.OrderedColumns;
		}
		else if (Wizard.SelectedIndex == TabIndexRelation)
		{
			if (pnAvailableDataSources.CheckedDataSources != null && pnAvailableDataSources.CheckedDataSources.Count > 1)
			{
				StiTabulator stiTabulator3 = null;
				if (((ArrangedElementCollection)((Control)wpRelation).Controls).Count > 0 && ((Control)wpRelation).Controls[0] is StiTabulator)
				{
					stiTabulator3 = ((Control)wpRelation).Controls[0] as StiTabulator;
				}
				else
				{
					((Control)wpRelation).Controls.Clear();
					stiTabulator3 = new StiTabulator();
					stiTabulator3.TabTitlePosition = StiTabTitlePosition.RightHorizontal;
					stiTabulator3.ControlStyle = StiControlStyle.Office2013Blue;
					((Control)stiTabulator3).Dock = (DockStyle)5;
					((Control)wpRelation).Controls.Add((Control)(object)stiTabulator3);
				}
				relations.Clear();
				Hashtable hashtable3 = new Hashtable();
				foreach (Control item7 in (ArrangedElementCollection)((Control)stiTabulator3).Controls)
				{
					Control val3 = item7;
					hashtable3[val3.Text] = val3;
				}
				((Control)stiTabulator3).Controls.Clear();
				StiDataSource masterDataSource = null;
				int num = 0;
				foreach (StiDataSource checkedDataSource in pnAvailableDataSources.CheckedDataSources)
				{
					if (num != 0)
					{
						StiTabulatorPage stiTabulatorPage8 = hashtable3[checkedDataSource.Name] as StiTabulatorPage;
						if (stiTabulatorPage8 == null)
						{
							StiTabulatorPage stiTabulatorPage9 = new StiTabulatorPage();
							((Control)stiTabulatorPage9).Text = checkedDataSource.Name;
							stiTabulatorPage8 = stiTabulatorPage9;
							StiColumnsRelation stiColumnsRelation = new StiColumnsRelation();
							((Control)stiColumnsRelation).Dock = (DockStyle)5;
							stiColumnsRelation.MasterDataSource = masterDataSource;
							stiColumnsRelation.DataSource = checkedDataSource;
							StiColumnsRelation stiColumnsRelation2 = stiColumnsRelation;
							((Control)stiTabulatorPage8).Controls.Add((Control)(object)stiColumnsRelation2);
							sorts.Add(stiColumnsRelation2);
						}
						relations.Add(((Control)stiTabulatorPage8).Controls[0]);
						((Control)stiTabulator3).Controls.Add((Control)(object)stiTabulatorPage8);
					}
					else
					{
						masterDataSource = checkedDataSource;
					}
					num++;
				}
				hashtable3.Clear();
			}
			else if (((Control)Wizard.NextButton).Focused)
			{
				Wizard.SelectedIndex = TabIndexTotals;
			}
			else if (((Control)Wizard.BackButton).Focused)
			{
				Wizard.SelectedIndex = TabIndexGroup;
			}
		}
		else
		{
			if (Wizard.SelectedIndex != TabIndexTotals)
			{
				return;
			}
			if (!pnAvailableDataSources.CheckedDataSourcesOrBusinessObject)
			{
				StiColumnsTotals stiColumnsTotals = null;
				if (((ArrangedElementCollection)((Control)wpTotals).Controls).Count > 0 && ((Control)wpTotals).Controls[0] is StiColumnsTotals)
				{
					stiColumnsTotals = ((Control)wpTotals).Controls[0] as StiColumnsTotals;
				}
				else
				{
					((Control)wpTotals).Controls.Clear();
					StiColumnsTotals stiColumnsTotals2 = new StiColumnsTotals();
					((Control)stiColumnsTotals2).Dock = (DockStyle)5;
					stiColumnsTotals = stiColumnsTotals2;
					((Control)wpTotals).Controls.Add((Control)(object)stiColumnsTotals);
				}
				stiColumnsTotals.DataSource = pnAvailableDataSources.SelectedDataSource;
				stiColumnsTotals.BusinessObject = pnAvailableDataSources.SelectedBusinessObject;
				stiColumnsTotals.SelectedColumns = columnsOrder.OrderedColumns;
				totals.Clear();
				totals.Add(stiColumnsTotals);
				return;
			}
			StiTabulator stiTabulator4 = null;
			if (((ArrangedElementCollection)((Control)wpTotals).Controls).Count > 0 && ((Control)wpTotals).Controls[0] is StiTabulator)
			{
				stiTabulator4 = ((Control)wpTotals).Controls[0] as StiTabulator;
			}
			else
			{
				((Control)wpTotals).Controls.Clear();
				StiTabulator obj2 = new StiTabulator
				{
					TabTitlePosition = StiTabTitlePosition.RightHorizontal
				};
				((Control)obj2).Dock = (DockStyle)5;
				stiTabulator4 = obj2;
				stiTabulator4.ControlStyle = StiControlStyle.Office2013Blue;
				((Control)wpTotals).Controls.Add((Control)(object)stiTabulator4);
			}
			totals.Clear();
			Hashtable hashtable4 = new Hashtable();
			foreach (Control item8 in (ArrangedElementCollection)((Control)stiTabulator4).Controls)
			{
				Control val4 = item8;
				hashtable4[val4.Text] = val4;
			}
			((Control)stiTabulator4).Controls.Clear();
			StiDataSourcesCollection checkedDataSources3 = pnAvailableDataSources.CheckedDataSources;
			if (checkedDataSources3 != null)
			{
				foreach (StiDataSource item9 in checkedDataSources3)
				{
					StiTabulatorPage stiTabulatorPage10 = hashtable4[item9.Name] as StiTabulatorPage;
					if (stiTabulatorPage10 == null)
					{
						stiTabulatorPage10 = new StiTabulatorPage();
						((Control)stiTabulatorPage10).Text = item9.Name;
						StiColumnsTotals stiColumnsTotals3 = new StiColumnsTotals();
						((Control)stiColumnsTotals3).Dock = (DockStyle)5;
						stiColumnsTotals3.DataSource = item9;
						stiColumnsTotals3.SelectedColumns = columnsOrder.OrderedColumns;
						StiColumnsTotals stiColumnsTotals4 = stiColumnsTotals3;
						((Control)stiTabulatorPage10).Controls.Add((Control)(object)stiColumnsTotals4);
						totals.Add(stiColumnsTotals4);
					}
					else
					{
						totals.Add(((Control)stiTabulatorPage10).Controls[0]);
					}
					((Control)stiTabulator4).Controls.Add((Control)(object)stiTabulatorPage10);
				}
			}
			StiBusinessObjectsCollection checkedBusinessObjects3 = pnAvailableDataSources.CheckedBusinessObjects;
			if (checkedBusinessObjects3 != null)
			{
				foreach (StiBusinessObject item10 in checkedBusinessObjects3)
				{
					StiTabulatorPage stiTabulatorPage11 = hashtable4[item10.Name] as StiTabulatorPage;
					if (stiTabulatorPage11 == null)
					{
						stiTabulatorPage11 = new StiTabulatorPage();
						((Control)stiTabulatorPage11).Text = item10.Name;
						StiColumnsTotals stiColumnsTotals5 = new StiColumnsTotals();
						((Control)stiColumnsTotals5).Dock = (DockStyle)5;
						stiColumnsTotals5.BusinessObject = item10;
						stiColumnsTotals5.SelectedColumns = columnsOrder.OrderedColumns;
						StiColumnsTotals stiColumnsTotals6 = stiColumnsTotals5;
						((Control)stiTabulatorPage11).Controls.Add((Control)(object)stiColumnsTotals6);
						totals.Add(stiColumnsTotals6);
					}
					else
					{
						totals.Add(((Control)stiTabulatorPage11).Controls[0]);
					}
					((Control)stiTabulator4).Controls.Add((Control)(object)stiTabulatorPage11);
				}
			}
			hashtable4.Clear();
		}
	}

	private void Wizard_CanNext(object sender, StiCanNextEventArgs e)
	{
		if (Wizard.SelectedIndex == 0)
		{
			if (IsMasterDetailWizard)
			{
				e.CanNext = pnAvailableDataSources.CheckedDataSourcesOrBusinessObject;
			}
			else
			{
				e.CanNext = pnAvailableDataSources.SelectedDataSourceOrBusinessObject;
			}
		}
		else if (Wizard.SelectedIndex == 1)
		{
			e.CanNext = columnsSelection.ColumnIsSelect();
		}
	}

	private void ThisLoad(object sender, EventArgs e)
	{
		Wizard.UpdateWizardState();
	}

	private void Wizard_FinishClick(object sender, EventArgs e)
	{
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Expected O, but got Unknown
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_1718: Expected O, but got Unknown
		//IL_1857: Unknown result type (might be due to invalid IL or missing references)
		//IL_185e: Expected O, but got Unknown
		//IL_17f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ff: Expected O, but got Unknown
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_193c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1946: Expected O, but got Unknown
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5b: Expected O, but got Unknown
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8c: Expected O, but got Unknown
		//IL_127e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1288: Expected O, but got Unknown
		//IL_0cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Expected O, but got Unknown
		//IL_1df0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df7: Expected O, but got Unknown
		//IL_23d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e1: Expected O, but got Unknown
		//IL_1e69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e73: Expected O, but got Unknown
		//IL_15a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ab: Expected O, but got Unknown
		//IL_1193: Unknown result type (might be due to invalid IL or missing references)
		//IL_119d: Expected O, but got Unknown
		//IL_26e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f1: Expected O, but got Unknown
		//IL_22ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f6: Expected O, but got Unknown
		base.CreatedReport = StiReport.GetReport();
		base.CreatedReport.ScriptLanguage = ((!reportLayout.rbCSharp.Checked) ? StiReportLanguageType.VB : StiReportLanguageType.CSharp);
		if (reportLayout.rbCentimetres.Checked)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.Centimeters;
		}
		if (reportLayout.rbMillimeters.Checked)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.Millimeters;
		}
		if (reportLayout.rbInch.Checked)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.Inches;
		}
		if (reportLayout.rbHundredthsOfInch.Checked)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.HundredthsOfInch;
		}
		base.CreatedReport.Dictionary = base.Report.Dictionary.Clone() as StiDictionary;
		base.CreatedReport.Dictionary.Report = base.CreatedReport;
		base.CreatedReport.RegData(base.Report.DataStore);
		StiOptions.Engine.GlobalEvents.InvokeWizardStart(base.CreatedReport, EventArgs.Empty);
		StiPage currentPage = base.CreatedReport.GetCurrentPage();
		currentPage.Orientation = ((!reportLayout.rbPortrait.Checked) ? StiPageOrientation.Landscape : StiPageOrientation.Portrait);
		StiDataSourcesCollection stiDataSourcesCollection = pnAvailableDataSources.CheckedDataSources;
		if (stiDataSourcesCollection == null)
		{
			stiDataSourcesCollection = new StiDataSourcesCollection(null);
		}
		if (stiDataSourcesCollection.Count == 0 && pnAvailableDataSources.SelectedDataSource != null)
		{
			stiDataSourcesCollection.Add(pnAvailableDataSources.SelectedDataSource);
		}
		StiBusinessObjectsCollection stiBusinessObjectsCollection = pnAvailableDataSources.CheckedBusinessObjects;
		if (stiBusinessObjectsCollection == null)
		{
			stiBusinessObjectsCollection = new StiBusinessObjectsCollection(null, null);
		}
		if (stiBusinessObjectsCollection.Count == 0 && pnAvailableDataSources.SelectedBusinessObject != null)
		{
			stiBusinessObjectsCollection.Add(pnAvailableDataSources.SelectedBusinessObject);
		}
		bool flag = true;
		string text = string.Empty;
		foreach (StiDataSource item in stiDataSourcesCollection)
		{
			if (!flag)
			{
				text += ", ";
			}
			text += item.Alias;
			flag = false;
		}
		foreach (StiBusinessObject item2 in stiBusinessObjectsCollection)
		{
			if (!flag)
			{
				text += ", ";
			}
			text += item2.Alias;
			flag = false;
		}
		StiReportTitleBand stiReportTitleBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.ReportTitleBand) as StiReportTitleBand;
		stiReportTitleBand.Name = "ReportTitle";
		stiReportTitleBand.Height = AlignToMaxGrid(currentPage, 50.0, converted: true);
		currentPage.Components.Add(stiReportTitleBand);
		StiText stiText = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
		stiText.HorAlignment = StiTextHorAlignment.Center;
		stiText.VertAlignment = StiVertAlignment.Center;
		stiText.Font = new Font("Arial", 20f, (FontStyle)1);
		stiText.Name = "ReportTitleText";
		stiText.Text = text;
		stiText.Top = 0.0;
		stiText.Left = 0.0;
		stiText.Width = currentPage.Width;
		stiText.Height = stiReportTitleBand.Height;
		stiReportTitleBand.Components.Add(stiText);
		StiDataBand masterComponent = null;
		Hashtable hashtable = new Hashtable();
		foreach (StiAggregateFunctionService item3 in StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => s.ServiceEnabled))
		{
			hashtable[item3.ServiceName] = item3;
		}
		int num = 0;
		foreach (StiDataSource item4 in stiDataSourcesCollection)
		{
			StiHeaderBand stiHeaderBand = null;
			StiDataBand stiDataBand = null;
			StiTable stiTable = null;
			StiFooterBand stiFooterBand = null;
			string text2 = StiNameValidator.CorrectName(item4.Name, base.Report);
			if (stiDataSourcesCollection.Count == 1)
			{
				text2 = string.Empty;
				if (item4.Name == "Header" || item4.Name == "Footer" || item4.Name == "Data")
				{
					text2 = "Band";
				}
			}
			TreeNodeCollection val = columnsOrder.OrderedColumns.Nodes;
			if (stiDataSourcesCollection.Count > 1)
			{
				bool flag2 = false;
				foreach (TreeNode node in columnsOrder.OrderedColumns.Nodes)
				{
					TreeNode val2 = node;
					if (item4.Name == val2.Text)
					{
						val = val2.Nodes;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					val = null;
				}
			}
			if (reportLayout.rbDataBand.Checked)
			{
				stiHeaderBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.HeaderBand) as StiHeaderBand;
				stiHeaderBand.Name = "Header" + text2;
				stiHeaderBand.Height = AlignToMaxGrid(currentPage, 20.0, converted: true);
				currentPage.Components.Add(stiHeaderBand);
			}
			TreeNodeCollection nodes = columnsGroup.tvColumnsGroup.Nodes;
			if (stiDataSourcesCollection.Count > 1)
			{
				foreach (TreeNode node2 in columnsGroup.tvColumnsGroup.Nodes)
				{
					TreeNode val3 = node2;
					if (item4.Name == val3.Text)
					{
						nodes = val3.Nodes;
						break;
					}
				}
			}
			int num2 = 0;
			foreach (TreeNode item5 in nodes)
			{
				TreeNode val4 = item5;
				if (val4.Checked)
				{
					StiGroupHeaderBand stiGroupHeaderBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.GroupHeaderBand) as StiGroupHeaderBand;
					stiGroupHeaderBand.Name = $"GroupHeader{text2}{num2}";
					stiGroupHeaderBand.Height = AlignToMaxGrid(currentPage, 30.0, converted: true);
					stiGroupHeaderBand.Condition.Value = "{" + val4.Tag?.ToString() + "}";
					currentPage.Components.Add(stiGroupHeaderBand);
					StiText stiText2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
					stiText2.Text = "{" + val4.Tag?.ToString() + "}";
					stiText2.VertAlignment = StiVertAlignment.Center;
					stiText2.Font = new Font("Arial", 16f, (FontStyle)1);
					stiText2.Name = $"GroupHeaderText{text2}{num2}";
					stiText2.Top = 0.0;
					stiText2.Left = 0.0;
					stiText2.Width = currentPage.Width;
					stiText2.Height = stiGroupHeaderBand.Height;
					stiText2.CanGrow = true;
					stiText2.WordWrap = true;
					stiGroupHeaderBand.Components.Add(stiText2);
					num2++;
				}
			}
			if (reportLayout.rbDataBand.Checked)
			{
				stiDataBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.DataBand) as StiDataBand;
				if (sorts.Count > 0)
				{
					stiDataBand.Sort = ((StiColumnsSort)sorts[num]).Sort;
				}
				stiDataBand.Name = "Data" + text2;
				stiDataBand.DataSourceName = item4.Name;
				stiDataBand.Height = AlignToMaxGrid(currentPage, 20.0, converted: true);
				stiDataBand.MasterComponent = masterComponent;
				currentPage.Components.Add(stiDataBand);
				if (num > 0 && relations.Count > 0)
				{
					StiColumnsRelation stiColumnsRelation = relations[num - 1] as StiColumnsRelation;
					StiDataRelation stiDataRelation = null;
					if (stiColumnsRelation.tvRelations.SelectedNode != null && stiColumnsRelation.tvRelations.SelectedNode.Index != 0 && stiColumnsRelation.tvRelations.SelectedNode.Tag is StiDataRelation)
					{
						stiDataRelation = stiColumnsRelation.tvRelations.SelectedNode.Tag as StiDataRelation;
					}
					if (stiDataRelation != null)
					{
						stiDataBand.DataRelationName = stiDataRelation.NameInSource;
					}
				}
				if (num == 0)
				{
					masterComponent = stiDataBand;
				}
			}
			else
			{
				stiTable = new StiTable
				{
					Name = "Data" + text2,
					RowCount = 3,
					HeaderRowsCount = 1,
					FooterRowsCount = 1,
					ColumnCount = val.Count
				};
				if (sorts.Count > 0)
				{
					stiTable.Sort = ((StiColumnsSort)sorts[num]).Sort;
				}
				stiTable.DataSourceName = item4.Name;
				currentPage.Components.Add(stiTable);
				stiTable.Height = currentPage.GridSize * 9.0;
				stiTable.Width = currentPage.Width;
				stiTable.MasterComponent = masterComponent;
				stiTable.AutoSizeCells();
				if (num > 0 && relations.Count > 0)
				{
					StiColumnsRelation stiColumnsRelation2 = relations[num - 1] as StiColumnsRelation;
					StiDataRelation stiDataRelation2 = null;
					if (stiColumnsRelation2.tvRelations.SelectedNode != null && stiColumnsRelation2.tvRelations.SelectedNode.Index != 0 && stiColumnsRelation2.tvRelations.SelectedNode.Tag is StiDataRelation)
					{
						stiDataRelation2 = stiColumnsRelation2.tvRelations.SelectedNode.Tag as StiDataRelation;
					}
					if (stiDataRelation2 != null)
					{
						stiTable.DataRelationName = stiDataRelation2.NameInSource;
					}
				}
				if (num == 0)
				{
					masterComponent = stiTable;
				}
			}
			if (filters.Count > 0)
			{
				if (reportLayout.rbDataBand.Checked)
				{
					stiDataBand.FilterMode = ((!((RadioButton)((StiColumnsFilters)filters[num]).rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
					((StiColumnsFilters)filters[num]).FillFilters(stiDataBand.Filters);
					stiDataBand.FilterOn = ((CheckBox)((StiColumnsFilters)filters[num]).cbFilterOn).Checked;
				}
				else
				{
					stiTable.FilterMode = ((!((RadioButton)((StiColumnsFilters)filters[num]).rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
					((StiColumnsFilters)filters[num]).FillFilters(stiTable.Filters);
					stiTable.FilterOn = ((CheckBox)((StiColumnsFilters)filters[num]).cbFilterOn).Checked;
				}
			}
			num2 = 0;
			foreach (TreeNode item6 in nodes)
			{
				TreeNode val5 = item6;
				if (val5.Checked)
				{
					StiGroupFooterBand stiGroupFooterBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.GroupFooterBand) as StiGroupFooterBand;
					stiGroupFooterBand.Name = $"GroupFooter{text2}{num2}";
					stiGroupFooterBand.Height = AlignToMaxGrid(currentPage, 10.0, converted: true);
					currentPage.Components.Add(stiGroupFooterBand);
					num2++;
				}
			}
			if (reportLayout.rbDataBand.Checked)
			{
				stiFooterBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.FooterBand) as StiFooterBand;
				stiFooterBand.Name = "Footer" + text2;
				stiFooterBand.Height = AlignToMaxGrid(currentPage, 20.0, converted: true);
				currentPage.Components.Add(stiFooterBand);
			}
			if (val != null)
			{
				double num3 = AlignToGrid(currentPage, currentPage.Width / (double)val.Count, converted: false);
				if (num3 * (double)val.Count > currentPage.Width && num3 > base.CreatedReport.Info.GridSize * 2.0)
				{
					num3 -= base.CreatedReport.Info.GridSize;
				}
				double num4 = 0.0;
				int num5 = 1;
				int num6 = 1;
				int num7 = 1;
				int num8 = 1;
				if (!reportLayout.rbDataBand.Checked)
				{
					num6 = 0;
					num7 = stiTable.ColumnCount;
					num8 = stiTable.ColumnCount * 2;
				}
				foreach (TreeNode item7 in val)
				{
					TreeNode val6 = item7;
					double num9 = num3;
					if (num5 == val.Count)
					{
						num9 = AlignToGrid(currentPage, currentPage.Width - num4, converted: false);
						if (num9 <= 0.0)
						{
							num9 = currentPage.GridSize;
						}
					}
					if (reportLayout.rbDataBand.Checked)
					{
						StiText stiText3 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
						stiText3.VertAlignment = StiVertAlignment.Center;
						stiText3.Font = new Font("Arial", 10f, (FontStyle)1);
						stiText3.Name = $"HeaderText{text2}{num6}";
						stiText3.Top = 0.0;
						stiText3.Left = num4;
						stiText3.Width = num9;
						stiText3.Height = stiHeaderBand.Height;
						stiText3.CanGrow = true;
						stiText3.WordWrap = true;
						if (val.Count > 1)
						{
							stiText3.GrowToHeight = true;
						}
						int num10 = val6.Text.LastIndexOf('.');
						StiDataColumn columnFromPath = StiDataBuilder.GetColumnFromPath(val6.Tag as string, base.CreatedReport.Dictionary);
						if (columnFromPath != null)
						{
							stiText3.Text = columnFromPath.Alias;
						}
						else if (num10 == -1)
						{
							stiText3.Text = val6.Text;
						}
						else
						{
							stiText3.Text = val6.Text.Substring(num10 + 1);
						}
						num6++;
						stiHeaderBand.Components.Add(stiText3);
						StiDataColumn columnFromPath2 = StiDataBuilder.GetColumnFromPath(val6.Tag as string, stiDataBand.Report.Dictionary);
						if (columnFromPath2 != null && (columnFromPath2.Type == typeof(Image) || columnFromPath2.Type == typeof(byte[])))
						{
							StiImage stiImage = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image) as StiImage;
							stiImage.Name = $"DataImage{text2}{num7}";
							stiImage.Top = 0.0;
							stiImage.Left = num4;
							stiImage.Width = num9;
							stiImage.Height = stiDataBand.Height;
							stiImage.DataColumn = val6.Tag as string;
							stiImage.CanGrow = true;
							if (val.Count > 1)
							{
								stiImage.GrowToHeight = true;
							}
							stiDataBand.Components.Add(stiImage);
						}
						else if (columnFromPath2 != null && columnFromPath2.Type == typeof(bool))
						{
							Stimulsoft.Report.Components.StiCheckBox stiCheckBox = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.CheckBox) as Stimulsoft.Report.Components.StiCheckBox;
							stiCheckBox.Name = $"DataCheck{text2}{num7}";
							stiCheckBox.Top = 0.0;
							stiCheckBox.Left = num4;
							stiCheckBox.Width = num9;
							stiCheckBox.Height = stiDataBand.Height;
							stiCheckBox.Checked.Value = "{" + val6.Tag?.ToString() + "}";
							if (val.Count > 1)
							{
								stiCheckBox.GrowToHeight = true;
							}
							stiDataBand.Components.Add(stiCheckBox);
						}
						else
						{
							StiText stiText4 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
							stiText4.Name = $"DataText{text2}{num7}";
							stiText4.TextQuality = StiTextQuality.Wysiwyg;
							stiText4.VertAlignment = StiVertAlignment.Center;
							stiText4.Top = 0.0;
							stiText4.Left = num4;
							stiText4.Width = num9;
							stiText4.Height = stiDataBand.Height;
							stiText4.Text = "{" + val6.Tag?.ToString() + "}";
							stiText4.CanGrow = true;
							if (val.Count > 1)
							{
								stiText4.GrowToHeight = true;
							}
							stiText4.WordWrap = true;
							stiDataBand.Components.Add(stiText4);
						}
						num7++;
						string empty = string.Empty;
						if (totals.Count > 0 && totals[num] is StiColumnsTotals stiColumnsTotals && stiColumnsTotals.Totals.Count >= num5)
						{
							empty = stiColumnsTotals.Totals[num5 - 1];
							if (empty != StiLocalization.Get("Wizards", "NoFunction"))
							{
								StiAggregateFunctionService stiAggregateFunctionService = hashtable[empty] as StiAggregateFunctionService;
								if (totals.Count > 0)
								{
									empty = ((stiAggregateFunctionService == null || !stiAggregateFunctionService.RecureParam) ? (empty + "()") : (empty + "(" + (string)val6.Tag + ")"));
								}
								StiText stiText5 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
								stiText5.VertAlignment = StiVertAlignment.Center;
								stiText5.HorAlignment = StiTextHorAlignment.Right;
								stiText5.Font = new Font("Arial", 10f, (FontStyle)1);
								stiText5.Name = $"FooterText{text2}{num8}";
								stiText5.Top = 0.0;
								stiText5.Left = num4;
								stiText5.Width = num9;
								stiText5.Height = stiFooterBand.Height;
								stiText5.CanGrow = true;
								if (val.Count > 1)
								{
									stiText5.GrowToHeight = true;
								}
								stiText5.WordWrap = true;
								stiText5.Text = ((totals.Count > 0) ? ("{" + empty + "}") : string.Empty);
								num8++;
								stiFooterBand.Components.Add(stiText5);
							}
						}
					}
					else
					{
						StiTableCell stiTableCell = (StiTableCell)stiTable.Components[num6];
						stiTableCell.VertAlignment = StiVertAlignment.Center;
						stiTableCell.Font = new Font("Arial", 10f, (FontStyle)1);
						stiTableCell.WordWrap = true;
						int num11 = val6.Text.LastIndexOf('.');
						StiDataColumn columnFromPath3 = StiDataBuilder.GetColumnFromPath(val6.Tag as string, base.CreatedReport.Dictionary);
						if (columnFromPath3 != null)
						{
							stiTableCell.Text = columnFromPath3.Alias;
						}
						else if (num11 == -1)
						{
							stiTableCell.Text = val6.Text;
						}
						else
						{
							stiTableCell.Text = val6.Text.Substring(num11 + 1);
						}
						num6++;
						StiDataColumn columnFromPath4 = StiDataBuilder.GetColumnFromPath(val6.Tag as string, stiTable.Report.Dictionary);
						if (columnFromPath4 != null && (columnFromPath4.Type == typeof(Image) || columnFromPath4.Type == typeof(byte[])))
						{
							((IStiTableCell)stiTable.Components[num7]).CellType = StiTablceCellType.Image;
							StiTableCellImage stiTableCellImage = (StiTableCellImage)stiTable.Components[num7];
							stiTableCellImage.VertAlignment = StiVertAlignment.Center;
							stiTableCellImage.CanGrow = true;
							stiTableCellImage.DataColumn = val6.Tag as string;
						}
						else if (columnFromPath4 != null && columnFromPath4.Type == typeof(bool))
						{
							((IStiTableCell)stiTable.Components[num7]).CellType = StiTablceCellType.CheckBox;
							StiTableCellCheckBox stiTableCellCheckBox = (StiTableCellCheckBox)stiTable.Components[num7];
							stiTableCellCheckBox.CanGrow = true;
							stiTableCellCheckBox.Checked.Value = "{" + val6.Tag?.ToString() + "}";
						}
						else
						{
							StiTableCell stiTableCell2 = (StiTableCell)stiTable.Components[num7];
							stiTableCell2.CanGrow = true;
							stiTableCell2.VertAlignment = StiVertAlignment.Center;
							stiTableCell2.Text = "{" + val6.Tag?.ToString() + "}";
							stiTableCell2.WordWrap = true;
						}
						num7++;
						string empty2 = string.Empty;
						if (totals.Count > 0 && totals[num] is StiColumnsTotals stiColumnsTotals2 && stiColumnsTotals2.Totals.Count >= num5)
						{
							empty2 = stiColumnsTotals2.Totals[num5 - 1];
							if (empty2 != StiLocalization.Get("Wizards", "NoFunction"))
							{
								StiAggregateFunctionService stiAggregateFunctionService2 = hashtable[empty2] as StiAggregateFunctionService;
								if (totals.Count > 0)
								{
									empty2 = ((stiAggregateFunctionService2 == null || !stiAggregateFunctionService2.RecureParam) ? (empty2 + "()") : (empty2 + "(" + (string)val6.Tag + ")"));
								}
								StiTableCell stiTableCell3 = (StiTableCell)stiTable.Components[num8];
								stiTableCell3.VertAlignment = StiVertAlignment.Center;
								stiTableCell3.HorAlignment = StiTextHorAlignment.Right;
								stiTableCell3.Font = new Font("Arial", 10f, (FontStyle)1);
								stiTableCell3.WordWrap = true;
								stiTableCell3.Text = ((totals.Count > 0) ? ("{" + empty2 + "}") : string.Empty);
								num8++;
							}
						}
					}
					num4 += num3;
					num5++;
				}
			}
			num++;
		}
		num = 0;
		foreach (StiBusinessObject item8 in stiBusinessObjectsCollection)
		{
			StiHeaderBand stiHeaderBand2 = null;
			StiDataBand stiDataBand2 = null;
			StiTable stiTable2 = null;
			StiFooterBand stiFooterBand2 = null;
			string text3 = StiNameValidator.CorrectName(item8.Name, base.Report);
			if (stiBusinessObjectsCollection.Count == 1)
			{
				text3 = string.Empty;
				if (item8.Name == "Header" || item8.Name == "Footer" || item8.Name == "Data")
				{
					text3 = "Band";
				}
			}
			TreeNodeCollection val7 = columnsOrder.OrderedColumns.Nodes;
			if (stiDataSourcesCollection.Count > 1)
			{
				bool flag3 = false;
				foreach (TreeNode node3 in columnsOrder.OrderedColumns.Nodes)
				{
					TreeNode val8 = node3;
					if (item8.Name == val8.Text)
					{
						val7 = val8.Nodes;
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					val7 = null;
				}
			}
			if (reportLayout.rbDataBand.Checked)
			{
				stiHeaderBand2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.HeaderBand) as StiHeaderBand;
				stiHeaderBand2.Name = "Header" + text3;
				stiHeaderBand2.Height = AlignToMaxGrid(currentPage, 20.0, converted: true);
				currentPage.Components.Add(stiHeaderBand2);
			}
			TreeNodeCollection nodes2 = columnsGroup.tvColumnsGroup.Nodes;
			if (stiDataSourcesCollection.Count > 1)
			{
				foreach (TreeNode node4 in columnsGroup.tvColumnsGroup.Nodes)
				{
					TreeNode val9 = node4;
					if (item8.Name == val9.Text)
					{
						nodes2 = val9.Nodes;
						break;
					}
				}
			}
			int num12 = 0;
			foreach (TreeNode item9 in nodes2)
			{
				TreeNode val10 = item9;
				if (val10.Checked)
				{
					StiGroupHeaderBand stiGroupHeaderBand2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.GroupHeaderBand) as StiGroupHeaderBand;
					stiGroupHeaderBand2.Name = $"GroupHeader{text3}{num12}";
					stiGroupHeaderBand2.Height = AlignToMaxGrid(currentPage, 30.0, converted: true);
					stiGroupHeaderBand2.Condition.Value = "{" + val10.Tag?.ToString() + "}";
					currentPage.Components.Add(stiGroupHeaderBand2);
					StiText stiText6 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
					stiText6.Text = "{" + val10.Tag?.ToString() + "}";
					stiText6.VertAlignment = StiVertAlignment.Center;
					stiText6.Font = new Font("Arial", 16f, (FontStyle)1);
					stiText6.Name = $"GroupHeaderText{text3}{num12}";
					stiText6.Top = 0.0;
					stiText6.Left = 0.0;
					stiText6.Width = currentPage.Width;
					stiText6.Height = stiGroupHeaderBand2.Height;
					stiText6.CanGrow = true;
					stiText6.WordWrap = true;
					stiGroupHeaderBand2.Components.Add(stiText6);
					num12++;
				}
			}
			if (reportLayout.rbDataBand.Checked)
			{
				stiDataBand2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.DataBand) as StiDataBand;
				if (sorts.Count > 0)
				{
					stiDataBand2.Sort = ((StiColumnsSort)sorts[num]).Sort;
				}
				stiDataBand2.Name = "Data" + text3;
				stiDataBand2.BusinessObjectGuid = item8.Guid;
				stiDataBand2.Height = AlignToMaxGrid(currentPage, 20.0, converted: true);
				stiDataBand2.MasterComponent = masterComponent;
				currentPage.Components.Add(stiDataBand2);
				masterComponent = stiDataBand2;
			}
			else
			{
				stiTable2 = new StiTable
				{
					Name = "Data" + text3,
					RowCount = 3,
					HeaderRowsCount = 1,
					FooterRowsCount = 1,
					ColumnCount = val7.Count
				};
				if (sorts.Count > 0)
				{
					stiTable2.Sort = ((StiColumnsSort)sorts[num]).Sort;
				}
				stiTable2.BusinessObjectGuid = item8.Guid;
				currentPage.Components.Add(stiTable2);
				stiTable2.Height = currentPage.GridSize * 9.0;
				stiTable2.Width = currentPage.Width;
				stiTable2.MasterComponent = masterComponent;
				stiTable2.AutoSizeCells();
				masterComponent = stiTable2;
			}
			if (filters.Count > 0)
			{
				if (reportLayout.rbDataBand.Checked)
				{
					stiDataBand2.FilterMode = ((!((RadioButton)((StiColumnsFilters)filters[num]).rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
					((StiColumnsFilters)filters[num]).FillFilters(stiDataBand2.Filters);
					stiDataBand2.FilterOn = ((CheckBox)((StiColumnsFilters)filters[num]).cbFilterOn).Checked;
				}
				else
				{
					stiTable2.FilterMode = ((!((RadioButton)((StiColumnsFilters)filters[num]).rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
					((StiColumnsFilters)filters[num]).FillFilters(stiTable2.Filters);
					stiTable2.FilterOn = ((CheckBox)((StiColumnsFilters)filters[num]).cbFilterOn).Checked;
				}
			}
			num12 = 0;
			foreach (TreeNode item10 in nodes2)
			{
				TreeNode val11 = item10;
				if (val11.Checked)
				{
					StiGroupFooterBand stiGroupFooterBand2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.GroupFooterBand) as StiGroupFooterBand;
					stiGroupFooterBand2.Name = $"GroupFooter{text3}{num12}";
					stiGroupFooterBand2.Height = AlignToMaxGrid(currentPage, 10.0, converted: true);
					currentPage.Components.Add(stiGroupFooterBand2);
					num12++;
				}
			}
			if (reportLayout.rbDataBand.Checked)
			{
				stiFooterBand2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.FooterBand) as StiFooterBand;
				stiFooterBand2.Name = "Footer" + text3;
				stiFooterBand2.Height = AlignToMaxGrid(currentPage, 20.0, converted: true);
				currentPage.Components.Add(stiFooterBand2);
			}
			if (val7 != null)
			{
				double num13 = AlignToGrid(currentPage, currentPage.Width / (double)val7.Count, converted: false);
				if (num13 * (double)val7.Count > currentPage.Width && num13 > base.CreatedReport.Info.GridSize * 2.0)
				{
					num13 -= base.CreatedReport.Info.GridSize;
				}
				double num14 = 0.0;
				int num15 = 1;
				int num16 = 1;
				int num17 = 1;
				int num18 = 1;
				if (!reportLayout.rbDataBand.Checked)
				{
					num16 = 0;
					num17 = stiTable2.ColumnCount;
					num18 = stiTable2.ColumnCount * 2;
				}
				foreach (TreeNode item11 in val7)
				{
					TreeNode val12 = item11;
					double num19 = num13;
					if (num15 == val7.Count)
					{
						num19 = AlignToGrid(currentPage, currentPage.Width - num14, converted: false);
						if (num19 <= 0.0)
						{
							num19 = currentPage.GridSize;
						}
					}
					if (reportLayout.rbDataBand.Checked)
					{
						StiText stiText7 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
						stiText7.VertAlignment = StiVertAlignment.Center;
						stiText7.Font = new Font("Arial", 10f, (FontStyle)1);
						stiText7.Name = $"HeaderText{text3}{num16}";
						stiText7.Top = 0.0;
						stiText7.Left = num14;
						stiText7.Width = num19;
						stiText7.Height = stiHeaderBand2.Height;
						stiText7.CanGrow = true;
						stiText7.WordWrap = true;
						if (val7.Count > 1)
						{
							stiText7.GrowToHeight = true;
						}
						int num20 = val12.Text.LastIndexOf('.');
						StiDataColumn columnFromPath5 = StiDataBuilder.GetColumnFromPath(val12.Tag as string, base.CreatedReport.Dictionary);
						if (columnFromPath5 != null)
						{
							stiText7.Text = columnFromPath5.Alias;
						}
						else if (num20 == -1)
						{
							stiText7.Text = val12.Text;
						}
						else
						{
							stiText7.Text = val12.Text.Substring(num20 + 1);
						}
						num16++;
						stiHeaderBand2.Components.Add(stiText7);
						StiDataColumn columnFromPath6 = StiDataBuilder.GetColumnFromPath(val12.Tag as string, stiDataBand2.Report.Dictionary);
						if (columnFromPath6 != null && (columnFromPath6.Type == typeof(Image) || columnFromPath6.Type == typeof(byte[])))
						{
							StiImage stiImage2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image) as StiImage;
							stiImage2.Name = $"DataImage{text3}{num17}";
							stiImage2.Top = 0.0;
							stiImage2.Left = num14;
							stiImage2.Width = num19;
							stiImage2.Height = stiDataBand2.Height;
							stiImage2.DataColumn = val12.Tag as string;
							stiImage2.CanGrow = true;
							if (val7.Count > 1)
							{
								stiImage2.GrowToHeight = true;
							}
							stiDataBand2.Components.Add(stiImage2);
						}
						else if (columnFromPath6 != null && columnFromPath6.Type == typeof(bool))
						{
							Stimulsoft.Report.Components.StiCheckBox stiCheckBox2 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.CheckBox) as Stimulsoft.Report.Components.StiCheckBox;
							stiCheckBox2.Name = $"DataCheck{text3}{num17}";
							stiCheckBox2.Top = 0.0;
							stiCheckBox2.Left = num14;
							stiCheckBox2.Width = num19;
							stiCheckBox2.Height = stiDataBand2.Height;
							stiCheckBox2.Checked.Value = "{" + val12.Tag?.ToString() + "}";
							stiDataBand2.Components.Add(stiCheckBox2);
						}
						else
						{
							StiText stiText8 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
							stiText8.Name = $"DataText{text3}{num17}";
							stiText8.TextQuality = StiTextQuality.Wysiwyg;
							stiText8.VertAlignment = StiVertAlignment.Center;
							stiText8.Top = 0.0;
							stiText8.Left = num14;
							stiText8.Width = num19;
							stiText8.Height = stiDataBand2.Height;
							stiText8.Text = "{" + val12.Tag?.ToString() + "}";
							stiText8.CanGrow = true;
							if (val7.Count > 1)
							{
								stiText8.GrowToHeight = true;
							}
							stiText8.WordWrap = true;
							stiDataBand2.Components.Add(stiText8);
						}
						num17++;
						string empty3 = string.Empty;
						if (totals.Count > 0 && totals[num] is StiColumnsTotals stiColumnsTotals3 && stiColumnsTotals3.Totals.Count > num15)
						{
							empty3 = stiColumnsTotals3.Totals[num15 - 1];
							if (empty3 != StiLocalization.Get("Wizards", "NoFunction"))
							{
								StiAggregateFunctionService stiAggregateFunctionService3 = hashtable[empty3] as StiAggregateFunctionService;
								if (totals.Count > 0)
								{
									empty3 = ((stiAggregateFunctionService3 == null || !stiAggregateFunctionService3.RecureParam) ? (empty3 + "()") : (empty3 + "(" + (string)val12.Tag + ")"));
								}
								StiText stiText9 = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
								stiText9.VertAlignment = StiVertAlignment.Center;
								stiText9.HorAlignment = StiTextHorAlignment.Right;
								stiText9.Font = new Font("Arial", 10f, (FontStyle)1);
								stiText9.Name = $"FooterText{text3}{num18}";
								stiText9.Top = 0.0;
								stiText9.Left = num14;
								stiText9.Width = num19;
								stiText9.Height = stiFooterBand2.Height;
								stiText9.CanGrow = true;
								if (val7.Count > 1)
								{
									stiText9.GrowToHeight = true;
								}
								stiText9.WordWrap = true;
								stiText9.Text = ((totals.Count > 0) ? ("{" + empty3 + "}") : string.Empty);
								num18++;
								stiFooterBand2.Components.Add(stiText9);
							}
						}
					}
					else
					{
						StiTableCell stiTableCell4 = (StiTableCell)stiTable2.Components[num16];
						stiTableCell4.VertAlignment = StiVertAlignment.Center;
						stiTableCell4.Font = new Font("Arial", 10f, (FontStyle)1);
						stiTableCell4.WordWrap = true;
						int num21 = val12.Text.LastIndexOf('.');
						StiDataColumn columnFromPath7 = StiDataBuilder.GetColumnFromPath(val12.Tag as string, base.CreatedReport.Dictionary);
						if (columnFromPath7 != null)
						{
							stiTableCell4.Text = columnFromPath7.Alias;
						}
						else if (num21 == -1)
						{
							stiTableCell4.Text = val12.Text;
						}
						else
						{
							stiTableCell4.Text = val12.Text.Substring(num21 + 1);
						}
						num16++;
						StiDataColumn columnFromPath8 = StiDataBuilder.GetColumnFromPath(val12.Tag as string, stiTable2.Report.Dictionary);
						if (columnFromPath8 != null && (columnFromPath8.Type == typeof(Image) || columnFromPath8.Type == typeof(byte[])))
						{
							((IStiTableCell)stiTable2.Components[num17]).CellType = StiTablceCellType.Image;
							StiTableCellImage stiTableCellImage2 = (StiTableCellImage)stiTable2.Components[num17];
							stiTableCellImage2.VertAlignment = StiVertAlignment.Center;
							stiTableCellImage2.DataColumn = val12.Tag as string;
						}
						else if (columnFromPath8 != null && columnFromPath8.Type == typeof(bool))
						{
							((IStiTableCell)stiTable2.Components[num17]).CellType = StiTablceCellType.CheckBox;
							StiTableCellCheckBox stiTableCellCheckBox2 = (StiTableCellCheckBox)stiTable2.Components[num17];
							stiTableCellCheckBox2.Checked.Value = "{" + val12.Tag?.ToString() + "}";
						}
						else
						{
							StiTableCell stiTableCell5 = (StiTableCell)stiTable2.Components[num17];
							stiTableCell5.CanGrow = true;
							stiTableCell5.VertAlignment = StiVertAlignment.Center;
							stiTableCell5.Text = "{" + val12.Tag?.ToString() + "}";
							stiTableCell5.WordWrap = true;
						}
						num17++;
						string empty4 = string.Empty;
						if (totals.Count > 0 && totals[num] is StiColumnsTotals stiColumnsTotals4 && stiColumnsTotals4.Totals.Count > num15)
						{
							empty4 = stiColumnsTotals4.Totals[num15 - 1];
							if (empty4 != StiLocalization.Get("Wizards", "NoFunction"))
							{
								StiAggregateFunctionService stiAggregateFunctionService4 = hashtable[empty4] as StiAggregateFunctionService;
								if (totals.Count > 0)
								{
									empty4 = ((stiAggregateFunctionService4 == null || !stiAggregateFunctionService4.RecureParam) ? (empty4 + "()") : (empty4 + "(" + (string)val12.Tag + ")"));
								}
								StiTableCell stiTableCell6 = (StiTableCell)stiTable2.Components[num18];
								stiTableCell6.VertAlignment = StiVertAlignment.Center;
								stiTableCell6.HorAlignment = StiTextHorAlignment.Right;
								stiTableCell6.Font = new Font("Arial", 10f, (FontStyle)1);
								stiTableCell6.WordWrap = true;
								stiTableCell6.Text = ((totals.Count > 0) ? ("{" + empty4 + "}") : string.Empty);
								num18++;
							}
						}
					}
					num14 += num13;
					num15++;
				}
			}
			num++;
		}
		StiStylesCollection selectedStylesCollection = columnsThemes.SelectedStylesCollection;
		if (selectedStylesCollection != null)
		{
			foreach (StiBaseStyle item12 in selectedStylesCollection)
			{
				base.CreatedReport.Styles.Add(item12);
			}
			string collectionName = null;
			foreach (StiBaseStyle item13 in selectedStylesCollection)
			{
				collectionName = item13.CollectionName;
			}
			base.CreatedReport.ApplyStyleCollection(collectionName);
		}
		StiOptions.Engine.GlobalEvents.InvokeWizardFinish(base.CreatedReport, EventArgs.Empty);
		base.CreatedReport.IsModified = true;
	}

	private void ThisKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Expected O, but got Unknown
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc6: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiStandardReportWizardForm));
		pnAvailableDataSources = new StiDataSourcePanel();
		columnsSelection = new StiColumnsSelection();
		columnsOrder = new StiColumnsOrder();
		columnsGroup = new StiColumnsGroup();
		columnsThemes = new StiColumnsThemes();
		ilButtons = new ImageList(components);
		Wizard = new StiWizard();
		wpDataSource = new StiWizardPage();
		wpSelectColumns = new StiWizardPage();
		wpColumsOrder = new StiWizardPage();
		wpSort = new StiWizardPage();
		wpFilters = new StiWizardPage();
		wpGroups = new StiWizardPage();
		wpRelation = new StiWizardPage();
		wpTotals = new StiWizardPage();
		wpThemes = new StiWizardPage();
		wpLayout = new StiWizardPage();
		reportLayout = new StiReportLayout();
		((Control)Wizard).SuspendLayout();
		((Control)wpDataSource).SuspendLayout();
		((Control)wpSelectColumns).SuspendLayout();
		((Control)wpColumsOrder).SuspendLayout();
		((Control)wpGroups).SuspendLayout();
		((Control)wpThemes).SuspendLayout();
		((Control)wpLayout).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pnAvailableDataSources).Anchor = (AnchorStyles)15;
		((Control)pnAvailableDataSources).BackColor = Color.White;
		((Control)pnAvailableDataSources).Location = new Point(0, 0);
		((Control)pnAvailableDataSources).Margin = new Padding(2);
		((Control)pnAvailableDataSources).Name = "pnAvailableDataSources";
		((Control)pnAvailableDataSources).Size = new Size(544, 318);
		((Control)pnAvailableDataSources).TabIndex = 0;
		((Control)columnsSelection).Anchor = (AnchorStyles)15;
		((Control)columnsSelection).BackColor = Color.White;
		columnsSelection.CheckedBusinessObjects = null;
		columnsSelection.CheckedDataSources = null;
		((Control)columnsSelection).Location = new Point(0, 0);
		((Control)columnsSelection).Margin = new Padding(0);
		((Control)columnsSelection).Name = "columnsSelection";
		columnsSelection.SelectedBusinessObject = null;
		columnsSelection.SelectedDataSource = null;
		((Control)columnsSelection).Size = new Size(544, 318);
		((Control)columnsSelection).TabIndex = 0;
		((Control)columnsOrder).Anchor = (AnchorStyles)15;
		((Control)columnsOrder).BackColor = Color.White;
		((Control)columnsOrder).Location = new Point(0, 0);
		((Control)columnsOrder).Name = "columnsOrder";
		columnsOrder.SelectedColumns = null;
		((Control)columnsOrder).Size = new Size(544, 318);
		((Control)columnsOrder).TabIndex = 0;
		((Control)columnsGroup).Anchor = (AnchorStyles)15;
		columnsGroup.IsMasterDetailWizard = false;
		((Control)columnsGroup).Location = new Point(0, 0);
		((Control)columnsGroup).Name = "columnsGroup";
		columnsGroup.SelectedColumns = null;
		((Control)columnsGroup).Size = new Size(544, 318);
		((Control)columnsGroup).TabIndex = 0;
		((Control)columnsThemes).Anchor = (AnchorStyles)15;
		((Control)columnsThemes).Location = new Point(0, 0);
		((Control)columnsThemes).Name = "columnsThemes";
		((Control)columnsThemes).Size = new Size(544, 318);
		((Control)columnsThemes).TabIndex = 0;
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		ilButtons.Images.SetKeyName(3, "");
		Wizard.ContentColor = Color.White;
		((Control)Wizard).Controls.Add((Control)(object)wpDataSource);
		((Control)Wizard).Controls.Add((Control)(object)wpSelectColumns);
		((Control)Wizard).Controls.Add((Control)(object)wpColumsOrder);
		((Control)Wizard).Controls.Add((Control)(object)wpSort);
		((Control)Wizard).Controls.Add((Control)(object)wpFilters);
		((Control)Wizard).Controls.Add((Control)(object)wpGroups);
		((Control)Wizard).Controls.Add((Control)(object)wpRelation);
		((Control)Wizard).Controls.Add((Control)(object)wpTotals);
		((Control)Wizard).Controls.Add((Control)(object)wpThemes);
		((Control)Wizard).Controls.Add((Control)(object)wpLayout);
		Wizard.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)Wizard).Dock = (DockStyle)5;
		Wizard.HeaderBackgroundImage = null;
		Wizard.HeaderColor = Color.White;
		Wizard.HeaderDescriptionFont = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		Wizard.HeaderTextColor = Color.Empty;
		Wizard.HeaderTitleFont = new Font("Times New Roman", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		Wizard.HelpButtonVisible = false;
		((Control)Wizard).Location = new Point(0, 0);
		Wizard.MarkLastPage = false;
		((Control)Wizard).Name = "Wizard";
		((Control)Wizard).Padding = new Padding(200, 66, 0, 46);
		Wizard.ProgressTreeOneLevel = true;
		Wizard.ShowProgressLine = false;
		((Control)Wizard).Size = new Size(744, 430);
		((Control)Wizard).TabIndex = 2;
		((Control)Wizard).Text = "Introduction";
		Wizard.TreeProgressFont = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		Wizard.SelectedIndexChanged += Wizard_SelectedIndexChanged;
		Wizard.FinishClick += Wizard_FinishClick;
		Wizard.CanNext += Wizard_CanNext;
		Wizard.CanFinish += Wizard_CanFinish;
		((Control)wpDataSource).Controls.Add((Control)(object)pnAvailableDataSources);
		wpDataSource.Dock = (DockStyle)5;
		((Control)wpDataSource).Location = new Point(200, 66);
		((Control)wpDataSource).Name = "wpDataSource";
		((Control)wpDataSource).Padding = new Padding(2);
		((Control)wpDataSource).Size = new Size(544, 318);
		((Control)wpDataSource).TabIndex = 1006;
		((Control)wpDataSource).Text = "Data Source";
		wpDataSource.Visible = false;
		((Control)wpSelectColumns).Controls.Add((Control)(object)columnsSelection);
		wpSelectColumns.Dock = (DockStyle)5;
		((Control)wpSelectColumns).Location = new Point(200, 66);
		((Control)wpSelectColumns).Name = "wpSelectColumns";
		((Control)wpSelectColumns).Padding = new Padding(2);
		((Control)wpSelectColumns).Size = new Size(544, 318);
		((Control)wpSelectColumns).TabIndex = 1007;
		((Control)wpSelectColumns).Text = "Select Columns";
		wpSelectColumns.Visible = false;
		((Control)wpColumsOrder).Controls.Add((Control)(object)columnsOrder);
		wpColumsOrder.Dock = (DockStyle)5;
		((Control)wpColumsOrder).Location = new Point(200, 66);
		((Control)wpColumsOrder).Name = "wpColumsOrder";
		((Control)wpColumsOrder).Padding = new Padding(2);
		((Control)wpColumsOrder).Size = new Size(544, 318);
		((Control)wpColumsOrder).TabIndex = 1008;
		((Control)wpColumsOrder).Text = "Colums Order";
		wpColumsOrder.Visible = false;
		wpSort.Dock = (DockStyle)5;
		((Control)wpSort).Location = new Point(200, 66);
		((Control)wpSort).Name = "wpSort";
		((Control)wpSort).Padding = new Padding(2);
		((Control)wpSort).Size = new Size(544, 318);
		((Control)wpSort).TabIndex = 1010;
		((Control)wpSort).Text = "Sort";
		wpSort.Visible = false;
		wpFilters.Dock = (DockStyle)5;
		((Control)wpFilters).Location = new Point(200, 66);
		((Control)wpFilters).Name = "wpFilters";
		((Control)wpFilters).Padding = new Padding(2);
		((Control)wpFilters).Size = new Size(544, 318);
		((Control)wpFilters).TabIndex = 1012;
		((Control)wpFilters).Text = "Filters";
		wpFilters.Visible = false;
		((Control)wpGroups).Controls.Add((Control)(object)columnsGroup);
		wpGroups.Dock = (DockStyle)5;
		((Control)wpGroups).Location = new Point(200, 66);
		((Control)wpGroups).Name = "wpGroups";
		((Control)wpGroups).Padding = new Padding(2);
		((Control)wpGroups).Size = new Size(544, 318);
		((Control)wpGroups).TabIndex = 1009;
		((Control)wpGroups).Text = "Groups";
		wpGroups.Visible = false;
		wpRelation.Dock = (DockStyle)5;
		((Control)wpRelation).Location = new Point(200, 66);
		((Control)wpRelation).Name = "wpRelation";
		((Control)wpRelation).Padding = new Padding(2);
		((Control)wpRelation).Size = new Size(544, 318);
		((Control)wpRelation).TabIndex = 1014;
		((Control)wpRelation).Text = "Relation";
		wpRelation.Visible = false;
		wpTotals.Dock = (DockStyle)5;
		((Control)wpTotals).Location = new Point(200, 66);
		((Control)wpTotals).Name = "wpTotals";
		((Control)wpTotals).Padding = new Padding(2);
		((Control)wpTotals).Size = new Size(544, 318);
		((Control)wpTotals).TabIndex = 1011;
		((Control)wpTotals).Text = "Totals";
		wpTotals.Visible = false;
		((Control)wpThemes).Controls.Add((Control)(object)columnsThemes);
		wpThemes.Dock = (DockStyle)5;
		((Control)wpThemes).Location = new Point(200, 66);
		((Control)wpThemes).Name = "wpThemes";
		((Control)wpThemes).Padding = new Padding(2);
		((Control)wpThemes).Size = new Size(544, 318);
		((Control)wpThemes).TabIndex = 1015;
		((Control)wpThemes).Text = "Themes";
		wpThemes.Visible = false;
		((Control)wpLayout).BackColor = Color.White;
		((Control)wpLayout).Controls.Add((Control)(object)reportLayout);
		wpLayout.Dock = (DockStyle)5;
		((Control)wpLayout).Location = new Point(200, 66);
		((Control)wpLayout).Name = "wpLayout";
		((Control)wpLayout).Padding = new Padding(2);
		((Control)wpLayout).Size = new Size(544, 318);
		((Control)wpLayout).TabIndex = 1013;
		((Control)wpLayout).Text = "Layout";
		wpLayout.Visible = false;
		((Control)reportLayout).Dock = (DockStyle)5;
		((Control)reportLayout).Location = new Point(2, 2);
		((Control)reportLayout).Name = "reportLayout";
		((Control)reportLayout).Size = new Size(540, 314);
		((Control)reportLayout).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).ClientSize = new Size(744, 430);
		((Control)this).Controls.Add((Control)(object)Wizard);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiStandardReportWizardForm";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "StiStandardReportWizardForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += StiStandardReportWizardForm_Closed;
		((Form)this).Load += ThisLoad;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)Wizard).ResumeLayout(false);
		((Control)wpDataSource).ResumeLayout(false);
		((Control)wpSelectColumns).ResumeLayout(false);
		((Control)wpColumsOrder).ResumeLayout(false);
		((Control)wpGroups).ResumeLayout(false);
		((Control)wpThemes).ResumeLayout(false);
		((Control)wpLayout).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
