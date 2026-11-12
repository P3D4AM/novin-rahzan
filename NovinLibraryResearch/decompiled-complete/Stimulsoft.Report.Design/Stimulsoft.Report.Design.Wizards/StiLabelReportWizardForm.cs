using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Labels;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiLabelReportWizardForm : StiReportWizardForm
{
	private StiDataSourcesCollection availableDataSources;

	private StiBusinessObjectsCollection availableBusinessObjects;

	private StiCentimetersUnit cmUnit = new StiCentimetersUnit();

	private StiMillimetersUnit mmUnit = new StiMillimetersUnit();

	private StiInchesUnit inUnit = new StiInchesUnit();

	private StiHundredthsOfInchUnit hiUnit = new StiHundredthsOfInchUnit();

	private List<StiLabelInfo> labels = new List<StiLabelInfo>();

	private StiUnit latestUnit;

	private StiNodesMemory NodesMemory = new StiNodesMemory();

	private StiLabelInfo selectedLabel;

	private IContainer components;

	private ImageList ilButtons;

	private Label labelLabelType;

	private Label labelHeight;

	private Label labelWidth;

	private Label labelVertical;

	private Label labelHorizontal;

	private TextBox textBoxHeight;

	private TextBox textBoxWidth;

	private TextBox textBoxVertical;

	private TextBox textBoxHorizontal;

	private StiWizard Wizard;

	private StiWizardPage wpLabel;

	private StiWizardPage wpDataSource;

	private StiDataSourcePanel pnAvailableDataSources;

	private TextBox textBoxLeft;

	private TextBox textBoxTop;

	private ComboBox comboBoxUnit;

	private Label labelUnit1;

	private Label labelUnit0;

	private Label labelUnit3;

	private Label labelUnit2;

	private Label labelUnit5;

	private Label labelUnit4;

	private StiGroupLine groupBoxPreview;

	private Panel panelPreview;

	private Label labelTopMargin;

	private NumericUpDown numericNumberOfColumns;

	private Label labelNumberOfColumns;

	private Label labelNumberOfRows;

	private StiGroupLine groupLineLabelSize;

	private StiGroupLine groupLinePaper;

	private StiGroupLine groupLineNumbers;

	private TextBox textBoxPageHeight;

	private TextBox textBoxPageWidth;

	private NumericUpDown numericNumberOfRows;

	private Label labelPageWidth;

	private Label labelPageHeight;

	private Label labelLeftMargin;

	private Label labelUnit7;

	private Label labelUnit6;

	private ComboBox comboBoxPapers;

	private Label labelSize;

	private Label labelDirection;

	private ComboBox comboBoxDirection;

	private ErrorProvider errorProvider1;

	public StiTreeViewBox treeViewBoxLabelType;

	private StiLabelInfo SelectedLabel
	{
		get
		{
			return selectedLabel;
		}
		set
		{
			selectedLabel = value;
			((Control)textBoxWidth).Text = ConvertFromInch(value.LabelWidth);
			((Control)textBoxHeight).Text = ConvertFromInch(value.LabelHeight);
			((Control)textBoxLeft).Text = ConvertFromInch(value.LeftMargin);
			((Control)textBoxTop).Text = ConvertFromInch(value.TopMargin);
			((Control)textBoxHorizontal).Text = ConvertFromInch(value.HorizontalGap);
			((Control)textBoxVertical).Text = ConvertFromInch(value.VerticalGap);
			((Control)textBoxPageWidth).Text = ConvertFromInch(value.PaperWidth);
			((Control)textBoxPageHeight).Text = ConvertFromInch(value.PaperHeight);
			((Control)numericNumberOfColumns).Text = value.NumberOfColumns.ToString();
			((Control)numericNumberOfRows).Text = value.NumberOfRows.ToString();
			latestUnit = SelectedUnit;
		}
	}

	private StiUnit SelectedUnit
	{
		get
		{
			if (((ListControl)comboBoxUnit).SelectedIndex == 0)
			{
				return cmUnit;
			}
			if (((ListControl)comboBoxUnit).SelectedIndex == 1)
			{
				return mmUnit;
			}
			if (((ListControl)comboBoxUnit).SelectedIndex == 2)
			{
				return inUnit;
			}
			return hiUnit;
		}
	}

	public StiLabelReportWizardForm(StiReport report)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			base.Report = report ?? new StiReport();
			availableDataSources = base.Report.DataSources;
			availableBusinessObjects = base.Report.Dictionary.BusinessObjects;
			Localize();
			BuildDataSources();
			BuildLabels();
			BuildPapers();
			((Control)Wizard.FinishButton).Enabled = false;
			StiSettings.Load();
			((ListControl)comboBoxUnit).SelectedIndex = StiSettings.GetInt("Wizard", "Unit", (!RegionInfo.CurrentRegion.IsMetric) ? 2 : 0);
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Wizards", "title");
		((Control)Wizard.BackButton).Text = StiLocalization.Get("Wizards", "ButtonBack");
		Wizard.CancelText = StiLocalization.Get("Wizards", "ButtonCancel");
		Wizard.FinishText = StiLocalization.Get("Wizards", "ButtonFinish");
		Wizard.NextText = StiLocalization.Get("Wizards", "ButtonNext");
		((Control)wpDataSource).Text = StiLocalization.Get("Wizards", "DataSource");
		((Control)wpLabel).Text = StiLocalization.Get("Wizards", "LabelSettings");
		wpDataSource.Description = StiLocalization.Get("Wizards", "infoDataSource");
		wpLabel.Description = StiLocalization.Get("Wizards", "infoLabelSettings");
		comboBoxUnit.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportUnitTypeCentimeters"));
		comboBoxUnit.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportUnitTypeMillimeters"));
		comboBoxUnit.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportUnitTypeInches"));
		comboBoxUnit.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch"));
		if (RegionInfo.CurrentRegion.IsMetric)
		{
			((ListControl)comboBoxUnit).SelectedIndex = 0;
		}
		else
		{
			((ListControl)comboBoxUnit).SelectedIndex = 2;
		}
		((Control)labelNumberOfColumns).Text = StiLocalization.Get("Wizards", "LabelNumberOfColumns");
		((Control)labelNumberOfRows).Text = StiLocalization.Get("Wizards", "LabelNumberOfRows");
		((Control)labelWidth).Text = StiLocalization.Get("Wizards", "LabelWidth");
		((Control)labelHeight).Text = StiLocalization.Get("Wizards", "LabelHeight");
		((Control)labelSize).Text = StiLocalization.Get("Wizards", "LabelSize");
		((Control)labelPageWidth).Text = StiLocalization.Get("Wizards", "LabelPageWidth");
		((Control)labelPageHeight).Text = StiLocalization.Get("Wizards", "LabelPageHeight");
		((Control)labelHorizontal).Text = StiLocalization.Get("Wizards", "LabelHorizontalGap");
		((Control)labelVertical).Text = StiLocalization.Get("Wizards", "LabelVerticalGap");
		((Control)labelLeftMargin).Text = StiLocalization.Get("Wizards", "LabelLeftMargin");
		((Control)labelTopMargin).Text = StiLocalization.Get("Wizards", "LabelTopMargin");
		((Control)labelLabelType).Text = StiLocalization.Get("Wizards", "LabelLabelType");
		((Control)labelTopMargin).Text = StiLocalization.Get("Wizards", "LabelTopMargin");
		((Control)labelDirection).Text = StiLocalization.Get("Wizards", "LabelDirection");
		comboBoxDirection.Items.Clear();
		comboBoxDirection.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiColumnDirectionAcrossThenDown"));
		comboBoxDirection.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiColumnDirectionDownThenAcross"));
		((Control)comboBoxDirection).Text = StiLocalization.Get("PropertyEnum", "StiColumnDirectionAcrossThenDown");
		((Control)groupBoxPreview).Text = StiLocalization.Get("Wizards", "Preview");
	}

	private void BuildPapers()
	{
		comboBoxPapers.Items.Clear();
		foreach (object labelPageSize in StiOptions.Designer.Wizards.LabelPageSizes)
		{
			comboBoxPapers.Items.Add(((StiPaperLabelInfo)labelPageSize).Clone());
		}
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo(StiLocalization.Get("Wizards", "Custom"), StiPageOrientation.Portrait, 0.0, 0.0));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("A4", StiPageOrientation.Portrait, 21.0, 29.7));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("A4", StiPageOrientation.Landscape, 29.7, 21.0));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("A5", StiPageOrientation.Portrait, 14.8, 21.0));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("A5", StiPageOrientation.Landscape, 21.0, 14.8));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Letter", StiPageOrientation.Portrait, 21.59, 27.94));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Letter", StiPageOrientation.Landscape, 27.94, 21.59));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("B5", StiPageOrientation.Portrait, 18.2, 25.7));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("B5", StiPageOrientation.Landscape, 25.7, 18.2));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Mini", StiPageOrientation.Portrait, 10.795, 12.7));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Vertical Half Sheet", StiPageOrientation.Portrait, 10.795, 25.4));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Vertical Half Sheet", StiPageOrientation.Landscape, 25.4, 10.795));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Hagaki", StiPageOrientation.Portrait, 10.0, 14.8));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("Hagaki", StiPageOrientation.Landscape, 14.8, 10.0));
		comboBoxPapers.Items.Add((object)new StiPaperLabelInfo("B4 JIS", StiPageOrientation.Portrait, 25.7, 36.4));
		foreach (StiPaperLabelInfo item in comboBoxPapers.Items)
		{
			item.Unit = SelectedUnit;
		}
		((ListControl)comboBoxPapers).SelectedIndex = 0;
	}

	private void BuildDataSources()
	{
		pnAvailableDataSources.Build(availableDataSources, base.Report);
		pnAvailableDataSources.Build(availableBusinessObjects, clear: false);
	}

	private void BuildLabels()
	{
		Stream manifestResourceStream = typeof(StiReport).Assembly.GetManifestResourceStream("Stimulsoft.Report.Labels.Labels.data");
		StreamReader streamReader = new StreamReader(manifestResourceStream);
		streamReader.ReadLine();
		labels.Clear();
		foreach (object label in StiOptions.Designer.Wizards.Labels)
		{
			labels.Add(((StiLabelInfo)label).Clone() as StiLabelInfo);
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			while (true)
			{
				try
				{
					string text = streamReader.ReadLine();
					if (text == null)
					{
						break;
					}
					string[] array = text.Split(new char[1] { ';' });
					string manufacturer = array[0];
					string labelName = array[1];
					string s = array[2];
					string s2 = array[3];
					string s3 = array[4];
					string s4 = array[5];
					string s5 = array[6];
					string s6 = array[7];
					string s7 = array[8];
					string s8 = array[9];
					string s9 = array[10];
					string s10 = array[11];
					labels.Add(new StiLabelInfo(manufacturer, labelName, double.Parse(s), double.Parse(s2), double.Parse(s7), double.Parse(s8), double.Parse(s9), double.Parse(s10), double.Parse(s3), double.Parse(s4), int.Parse(s6), int.Parse(s5)));
					continue;
				}
				catch
				{
					continue;
				}
			}
			manifestResourceStream.Close();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
		foreach (StiLabelInfo label2 in labels)
		{
			label2.Unit = SelectedUnit;
		}
		foreach (StiLabelInfo label3 in labels)
		{
			label3.Unit = SelectedUnit;
		}
	}

	private string ConvertFromInch(double value)
	{
		if (!(SelectedUnit is StiInchesUnit))
		{
			value = inUnit.ConvertToHInches(value);
			value = SelectedUnit.ConvertFromHInches(value);
		}
		value = Math.Round(value, 2);
		return value.ToString();
	}

	private double ConvertToInch(double value)
	{
		if (!(SelectedUnit is StiInchesUnit))
		{
			value = SelectedUnit.ConvertToHInches(value);
			value = inUnit.ConvertFromHInches(value);
		}
		value = Math.Round(value, 2);
		return value;
	}

	private string ConvertUnit(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return str;
		}
		try
		{
			double value = double.Parse(str);
			value = latestUnit.ConvertToHInches(value);
			return Math.Round(SelectedUnit.ConvertFromHInches(value), 2).ToString();
		}
		catch
		{
			return "0";
		}
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/reports_designer_creating_reports_in_designer_overview_label_report_wizard.htm");
	}

	private void comboBoxPapers_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)treeViewBoxLabelType).Text = StiLocalization.Get("Wizards", "Custom");
		if (comboBoxPapers.SelectedItem is StiPaperLabelInfo { Width: not 0.0, Height: not 0.0 } stiPaperLabelInfo)
		{
			((Control)textBoxPageWidth).Text = ConvertFromInch(stiPaperLabelInfo.Width);
			((Control)textBoxPageHeight).Text = ConvertFromInch(stiPaperLabelInfo.Height);
		}
		((Control)panelPreview).Invalidate();
	}

	private void panelPreview_Paint(object sender, PaintEventArgs e)
	{
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Expected O, but got Unknown
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)panelPreview).ClientRectangle;
		graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
		try
		{
			if (((Control)textBoxWidth).Text.Trim().Length == 0 || ((Control)textBoxHeight).Text.Trim().Length == 0 || ((Control)textBoxLeft).Text.Trim().Length == 0 || ((Control)textBoxTop).Text.Trim().Length == 0 || ((Control)textBoxHorizontal).Text.Trim().Length == 0 || ((Control)textBoxVertical).Text.Trim().Length == 0 || ((Control)textBoxPageWidth).Text.Trim().Length == 0 || ((Control)textBoxPageHeight).Text.Trim().Length == 0)
			{
				return;
			}
			errorProvider1.Clear();
			double result = 0.0;
			if (!double.TryParse(((Control)textBoxWidth).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxWidth, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxHeight).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxHeight, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num2 = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxLeft).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxLeft, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num3 = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxTop).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxTop, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num4 = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxHorizontal).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxHorizontal, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num5 = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxVertical).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxVertical, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num6 = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxPageWidth).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxPageWidth, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num7 = ConvertToInch(result);
			result = 0.0;
			if (!double.TryParse(((Control)textBoxPageHeight).Text, out result))
			{
				errorProvider1.SetError((Control)(object)textBoxPageHeight, StiLocalization.GetValue("Errors", "NotCorrectFormat"));
				((Control)Wizard.FinishButton).Enabled = false;
				return;
			}
			double num8 = ConvertToInch(result);
			if (!((Control)Wizard.FinishButton).Enabled)
			{
				((Control)Wizard.FinishButton).Enabled = true;
			}
			double num9 = Math.Max(num7, num8);
			double num10 = clientRectangle.Width - 10;
			double num11 = clientRectangle.Height - 10;
			double num12 = num10 / num9;
			double num13 = (num10 - num7 * num12) / 2.0 + 5.0;
			double num14 = (num11 - num8 * num12) / 2.0 + 5.0;
			Rectangle rectangle = new Rectangle((int)num13, (int)num14, (int)(num7 * num12), (int)(num8 * num12));
			rectangle.Width++;
			rectangle.Height++;
			StiShadow.DrawShadow(graphics, rectangle);
			rectangle.Width--;
			rectangle.Height--;
			rectangle.Offset(1, 1);
			graphics.FillRectangle(Brushes.White, rectangle);
			graphics.DrawRectangle(Pens.DimGray, rectangle);
			double num15 = num3;
			double num16 = num4;
			graphics.SetClip(rectangle);
			int num17 = 1;
			Font val = new Font("Arial", (float)(num2 * num12 / 3.0));
			try
			{
				StringFormat val2 = new StringFormat();
				try
				{
					val2.LineAlignment = (StringAlignment)1;
					val2.Alignment = (StringAlignment)1;
					if (((ListControl)comboBoxDirection).SelectedIndex == 0)
					{
						for (int i = 0; (decimal)i < numericNumberOfRows.Value; i++)
						{
							num15 = num3;
							for (int j = 0; (decimal)j < numericNumberOfColumns.Value; j++)
							{
								RectangleF rectangleF = new RectangleF((float)(num15 * num12 + num13), (float)(num16 * num12 + num14), (float)(num * num12), (float)(num2 * num12));
								graphics.FillRectangle(Brushes.WhiteSmoke, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
								graphics.DrawRectangle(Pens.Black, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
								graphics.DrawString(num17.ToString(), val, Brushes.Black, rectangleF, val2);
								num15 += num + num5;
								num17++;
							}
							num16 += num2 + num6;
						}
					}
					else
					{
						for (int k = 0; (decimal)k < numericNumberOfColumns.Value; k++)
						{
							num16 = num4;
							for (int l = 0; (decimal)l < numericNumberOfRows.Value; l++)
							{
								RectangleF rectangleF2 = new RectangleF((float)(num15 * num12 + num13), (float)(num16 * num12 + num14), (float)(num * num12), (float)(num2 * num12));
								graphics.FillRectangle(Brushes.WhiteSmoke, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
								graphics.DrawRectangle(Pens.Black, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
								graphics.DrawString(num17.ToString(), val, Brushes.Black, rectangleF2, val2);
								num16 += num2 + num6;
								num17++;
							}
							num15 += num + num5;
						}
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			graphics.ResetClip();
			graphics.DrawRectangle(Pens.DimGray, rectangle);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set("Wizard", "Unit", ((ListControl)comboBoxUnit).SelectedIndex);
		StiSettings.Save();
	}

	private void ThisLoad(object sender, EventArgs e)
	{
		Wizard.UpdateWizardState();
	}

	private void Wizard_FinishClick(object sender, EventArgs e)
	{
		base.CreatedReport = StiReport.GetReport();
		if (SelectedUnit is StiCentimetersUnit)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.Centimeters;
		}
		if (SelectedUnit is StiMillimetersUnit)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.Millimeters;
		}
		if (SelectedUnit is StiInchesUnit)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.Inches;
		}
		if (SelectedUnit is StiHundredthsOfInchUnit)
		{
			base.CreatedReport.ReportUnit = StiReportUnitType.HundredthsOfInch;
		}
		base.CreatedReport.Dictionary = base.Report.Dictionary.Clone() as StiDictionary;
		base.CreatedReport.Dictionary.Report = base.CreatedReport;
		base.CreatedReport.RegData(base.Report.DataStore);
		StiOptions.Engine.GlobalEvents.InvokeWizardStart(base.CreatedReport, EventArgs.Empty);
		double num = double.Parse(((Control)textBoxHeight).Text) + double.Parse(((Control)textBoxVertical).Text);
		StiPage currentPage = base.CreatedReport.GetCurrentPage();
		double num2 = double.Parse(((Control)textBoxPageWidth).Text);
		double num3 = double.Parse(((Control)textBoxPageHeight).Text);
		if (num2 > num3)
		{
			currentPage.Orientation = StiPageOrientation.Landscape;
		}
		currentPage.PageWidth = num2;
		currentPage.PageHeight = num3;
		double left = double.Parse(((Control)textBoxLeft).Text);
		double right = double.Parse(((Control)textBoxLeft).Text);
		double num4 = double.Parse(((Control)textBoxTop).Text);
		double bottom = currentPage.PageHeight - num4 - num * (double)numericNumberOfRows.Value - currentPage.PageHeight * 0.01;
		currentPage.Margins = new StiMargins(left, right, num4, bottom);
		StiDataBand stiDataBand = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.DataBand) as StiDataBand;
		stiDataBand.Name = "LabelsDataBand";
		if (pnAvailableDataSources.SelectedDataSource != null)
		{
			stiDataBand.DataSourceName = pnAvailableDataSources.SelectedDataSource.Name;
		}
		else if (pnAvailableDataSources.SelectedBusinessObject != null)
		{
			stiDataBand.BusinessObjectGuid = pnAvailableDataSources.SelectedBusinessObject.Guid;
		}
		else
		{
			stiDataBand.CountData = (int)(numericNumberOfColumns.Value * numericNumberOfRows.Value);
		}
		stiDataBand.Height = double.Parse(((Control)textBoxHeight).Text) + double.Parse(((Control)textBoxVertical).Text);
		stiDataBand.ColumnWidth = double.Parse(((Control)textBoxWidth).Text);
		stiDataBand.ColumnGaps = double.Parse(((Control)textBoxHorizontal).Text);
		stiDataBand.Columns = (int)numericNumberOfColumns.Value;
		stiDataBand.ColumnDirection = ((((ListControl)comboBoxDirection).SelectedIndex == 0) ? StiColumnDirection.AcrossThenDown : StiColumnDirection.DownThenAcross);
		currentPage.Components.Add(stiDataBand);
		if (base.CreatedReport.EngineVersion == StiEngineVersion.EngineV1)
		{
			StiContainer stiContainer = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.ComplexComponents.Container) as StiContainer;
			stiContainer.Name = "Label_Area";
			stiContainer.Border.Side = StiBorderSides.All;
			stiContainer.Width = double.Parse(((Control)textBoxWidth).Text);
			stiContainer.Height = double.Parse(((Control)textBoxHeight).Text);
			stiDataBand.Components.Add(stiContainer);
		}
		else
		{
			Stimulsoft.Report.Components.StiPanel stiPanel = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.ComplexComponents.Panel) as Stimulsoft.Report.Components.StiPanel;
			stiPanel.Name = "Label_Area";
			stiPanel.Border.Side = StiBorderSides.All;
			stiPanel.Width = double.Parse(((Control)textBoxWidth).Text);
			stiPanel.Height = double.Parse(((Control)textBoxHeight).Text);
			stiDataBand.Components.Add(stiPanel);
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

	private void treeViewBoxLabelType_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		e.Nodes.Clear();
		((TreeView)e.TreeView).ImageList = ilButtons;
		Hashtable hashtable = new Hashtable();
		foreach (StiLabelInfo label in labels)
		{
			object? obj = hashtable[label.Manufacturer];
			TreeNode val = (TreeNode)((obj is TreeNode) ? obj : null);
			if (val == null)
			{
				val = new TreeNode(label.Manufacturer, 0, 0);
				e.Nodes.Add(val);
				hashtable[label.Manufacturer] = val;
			}
			TreeNode val2 = new TreeNode(label.ToString(), 1, 1)
			{
				Tag = label
			};
			val.Nodes.Add(val2);
		}
		((TreeView)e.TreeView).CollapseAll();
		foreach (TreeNode node in ((TreeView)e.TreeView).Nodes)
		{
			TreeNode val3 = node;
			if (val3.Text == "Avery")
			{
				val3.Expand();
				break;
			}
		}
		NodesMemory.LoadExpNodes((TreeView)(object)e.TreeView, restoreSelectedNode: true);
	}

	private void treeViewBoxLabelType_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		NodesMemory.SaveExpNodes((TreeView)(object)e.TreeView);
		if (((TreeView)e.TreeView).SelectedNode != null && ((TreeView)e.TreeView).SelectedNode.Tag == null)
		{
			return;
		}
		SelectedLabel = ((TreeView)e.TreeView).SelectedNode.Tag as StiLabelInfo;
		int num = 0;
		((ListControl)comboBoxPapers).SelectedIndex = 0;
		foreach (StiPaperLabelInfo item in comboBoxPapers.Items)
		{
			if (item.Width == SelectedLabel.PaperWidth && item.Height == SelectedLabel.PaperHeight)
			{
				((ListControl)comboBoxPapers).SelectedIndex = num;
			}
			num++;
		}
		((Control)groupBoxPreview).Enabled = true;
		((Control)groupLineLabelSize).Enabled = true;
		((Control)groupLineNumbers).Enabled = true;
		((Control)groupLinePaper).Enabled = true;
		((Control)Wizard.FinishButton).Enabled = true;
		((Control)panelPreview).Invalidate();
	}

	private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
	{
		Label obj = labelUnit0;
		Label obj2 = labelUnit1;
		Label obj3 = labelUnit2;
		Label obj4 = labelUnit3;
		Label obj5 = labelUnit4;
		Label obj6 = labelUnit5;
		Label obj7 = labelUnit6;
		string text = (((Control)labelUnit7).Text = SelectedUnit.ShortName);
		string text3 = (((Control)obj7).Text = text);
		string text5 = (((Control)obj6).Text = text3);
		string text7 = (((Control)obj5).Text = text5);
		string text9 = (((Control)obj4).Text = text7);
		string text11 = (((Control)obj3).Text = text9);
		string text13 = (((Control)obj2).Text = text11);
		((Control)obj).Text = text13;
		((Control)textBoxWidth).Text = ConvertUnit(((Control)textBoxWidth).Text);
		((Control)textBoxHeight).Text = ConvertUnit(((Control)textBoxHeight).Text);
		((Control)textBoxLeft).Text = ConvertUnit(((Control)textBoxLeft).Text);
		((Control)textBoxTop).Text = ConvertUnit(((Control)textBoxTop).Text);
		((Control)textBoxHorizontal).Text = ConvertUnit(((Control)textBoxHorizontal).Text);
		((Control)textBoxVertical).Text = ConvertUnit(((Control)textBoxVertical).Text);
		((Control)textBoxPageWidth).Text = ConvertUnit(((Control)textBoxPageWidth).Text);
		((Control)textBoxPageHeight).Text = ConvertUnit(((Control)textBoxPageHeight).Text);
		latestUnit = SelectedUnit;
		foreach (StiLabelInfo label in labels)
		{
			label.Unit = SelectedUnit;
		}
		foreach (StiPaperLabelInfo item in comboBoxPapers.Items)
		{
			item.Unit = SelectedUnit;
		}
		if (comboBoxPapers.SelectedItem != null)
		{
			((Control)comboBoxPapers).Invalidate();
		}
	}

	private void textBoxHeight_TextChanged(object sender, EventArgs e)
	{
		((Control)treeViewBoxLabelType).Text = StiLocalization.Get("Wizards", "Custom");
		((Control)panelPreview).Invalidate();
	}

	private void comboBoxDirection_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)panelPreview).Invalidate();
	}

	private void Wizard_NextClick(object sender, EventArgs e)
	{
		if (treeViewBoxLabelType.SelectedItem == null)
		{
			((Control)Wizard.FinishButton).Enabled = false;
		}
		else
		{
			((Control)Wizard.FinishButton).Enabled = true;
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_0b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_108b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1095: Expected O, but got Unknown
		//IL_1aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac4: Expected O, but got Unknown
		//IL_1aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af5: Expected O, but got Unknown
		//IL_1b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb8: Expected O, but got Unknown
		//IL_1c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df5: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiLabelReportWizardForm));
		ilButtons = new ImageList(components);
		groupLineLabelSize = new StiGroupLine();
		labelUnit1 = new Label();
		labelUnit0 = new Label();
		textBoxHeight = new TextBox();
		labelHeight = new Label();
		labelWidth = new Label();
		textBoxWidth = new TextBox();
		labelHorizontal = new Label();
		textBoxHorizontal = new TextBox();
		labelUnit2 = new Label();
		textBoxVertical = new TextBox();
		labelVertical = new Label();
		labelUnit3 = new Label();
		labelLabelType = new Label();
		wpLabel = new StiWizardPage();
		labelDirection = new Label();
		groupLineNumbers = new StiGroupLine();
		labelNumberOfColumns = new Label();
		labelNumberOfRows = new Label();
		numericNumberOfRows = new NumericUpDown();
		numericNumberOfColumns = new NumericUpDown();
		comboBoxDirection = new ComboBox();
		groupBoxPreview = new StiGroupLine();
		panelPreview = new Panel();
		comboBoxUnit = new ComboBox();
		groupLinePaper = new StiGroupLine();
		labelSize = new Label();
		comboBoxPapers = new ComboBox();
		textBoxPageHeight = new TextBox();
		textBoxPageWidth = new TextBox();
		labelPageWidth = new Label();
		labelUnit7 = new Label();
		labelPageHeight = new Label();
		labelUnit6 = new Label();
		textBoxTop = new TextBox();
		textBoxLeft = new TextBox();
		labelLeftMargin = new Label();
		labelUnit5 = new Label();
		labelTopMargin = new Label();
		labelUnit4 = new Label();
		treeViewBoxLabelType = new StiTreeViewBox();
		Wizard = new StiWizard();
		wpDataSource = new StiWizardPage();
		pnAvailableDataSources = new StiDataSourcePanel();
		errorProvider1 = new ErrorProvider(components);
		((Control)groupLineLabelSize).SuspendLayout();
		((Control)wpLabel).SuspendLayout();
		((Control)groupLineNumbers).SuspendLayout();
		((ISupportInitialize)numericNumberOfRows).BeginInit();
		((ISupportInitialize)numericNumberOfColumns).BeginInit();
		((Control)groupBoxPreview).SuspendLayout();
		((Control)groupLinePaper).SuspendLayout();
		((Control)Wizard).SuspendLayout();
		((Control)wpDataSource).SuspendLayout();
		((ISupportInitialize)errorProvider1).BeginInit();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelUnit1);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelUnit0);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)textBoxHeight);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelHeight);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelWidth);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)textBoxWidth);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelHorizontal);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)textBoxHorizontal);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelUnit2);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)textBoxVertical);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelVertical);
		((Control)groupLineLabelSize).Controls.Add((Control)(object)labelUnit3);
		((Control)groupLineLabelSize).Enabled = false;
		groupLineLabelSize.LinePosition = StiLinePosition.Top;
		groupLineLabelSize.LineStyle = StiLineStyle.Line;
		((Control)groupLineLabelSize).Location = new Point(8, 38);
		((Control)groupLineLabelSize).Name = "groupLineLabelSize";
		((Control)groupLineLabelSize).Size = new Size(264, 104);
		((Control)groupLineLabelSize).TabIndex = 1;
		((Control)groupLineLabelSize).TabStop = false;
		((Control)labelUnit1).ForeColor = SystemColors.ControlText;
		((Control)labelUnit1).Location = new Point(229, 32);
		((Control)labelUnit1).Name = "labelUnit1";
		((Control)labelUnit1).Size = new Size(24, 20);
		((Control)labelUnit1).TabIndex = 8;
		((Control)labelUnit1).Text = "cm";
		labelUnit1.TextAlign = (ContentAlignment)16;
		((Control)labelUnit0).ForeColor = SystemColors.ControlText;
		((Control)labelUnit0).Location = new Point(229, 8);
		((Control)labelUnit0).Name = "labelUnit0";
		((Control)labelUnit0).Size = new Size(24, 20);
		((Control)labelUnit0).TabIndex = 7;
		((Control)labelUnit0).Text = "cm";
		labelUnit0.TextAlign = (ContentAlignment)16;
		((Control)textBoxHeight).ForeColor = SystemColors.ControlText;
		((Control)textBoxHeight).Location = new Point(152, 32);
		((Control)textBoxHeight).Name = "textBoxHeight";
		((Control)textBoxHeight).Size = new Size(64, 20);
		((Control)textBoxHeight).TabIndex = 1;
		textBoxHeight.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxHeight).TextChanged += textBoxHeight_TextChanged;
		((Control)labelHeight).ForeColor = SystemColors.ControlText;
		((Control)labelHeight).Location = new Point(8, 32);
		((Control)labelHeight).Name = "labelHeight";
		((Control)labelHeight).Size = new Size(144, 20);
		((Control)labelHeight).TabIndex = 3;
		((Control)labelHeight).Text = "Height:";
		labelHeight.TextAlign = (ContentAlignment)16;
		((Control)labelWidth).ForeColor = SystemColors.ControlText;
		((Control)labelWidth).Location = new Point(8, 8);
		((Control)labelWidth).Name = "labelWidth";
		((Control)labelWidth).Size = new Size(144, 20);
		((Control)labelWidth).TabIndex = 2;
		((Control)labelWidth).Text = "Width:";
		labelWidth.TextAlign = (ContentAlignment)16;
		((Control)textBoxWidth).ForeColor = SystemColors.ControlText;
		((Control)textBoxWidth).Location = new Point(152, 8);
		((Control)textBoxWidth).Name = "textBoxWidth";
		((Control)textBoxWidth).Size = new Size(64, 20);
		((Control)textBoxWidth).TabIndex = 0;
		textBoxWidth.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxWidth).TextChanged += textBoxHeight_TextChanged;
		((Control)labelHorizontal).ForeColor = SystemColors.ControlText;
		((Control)labelHorizontal).Location = new Point(8, 56);
		((Control)labelHorizontal).Name = "labelHorizontal";
		((Control)labelHorizontal).Size = new Size(144, 20);
		((Control)labelHorizontal).TabIndex = 2;
		((Control)labelHorizontal).Text = "Horizontal Gap:";
		labelHorizontal.TextAlign = (ContentAlignment)16;
		((Control)textBoxHorizontal).ForeColor = SystemColors.ControlText;
		((Control)textBoxHorizontal).Location = new Point(152, 56);
		((Control)textBoxHorizontal).Name = "textBoxHorizontal";
		((Control)textBoxHorizontal).Size = new Size(64, 20);
		((Control)textBoxHorizontal).TabIndex = 2;
		textBoxHorizontal.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxHorizontal).TextChanged += textBoxHeight_TextChanged;
		((Control)labelUnit2).ForeColor = SystemColors.ControlText;
		((Control)labelUnit2).Location = new Point(229, 56);
		((Control)labelUnit2).Name = "labelUnit2";
		((Control)labelUnit2).Size = new Size(24, 20);
		((Control)labelUnit2).TabIndex = 9;
		((Control)labelUnit2).Text = "cm";
		labelUnit2.TextAlign = (ContentAlignment)16;
		((Control)textBoxVertical).ForeColor = SystemColors.ControlText;
		((Control)textBoxVertical).Location = new Point(152, 80);
		((Control)textBoxVertical).Name = "textBoxVertical";
		((Control)textBoxVertical).Size = new Size(64, 20);
		((Control)textBoxVertical).TabIndex = 3;
		textBoxVertical.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxVertical).TextChanged += textBoxHeight_TextChanged;
		((Control)labelVertical).ForeColor = SystemColors.ControlText;
		((Control)labelVertical).Location = new Point(8, 80);
		((Control)labelVertical).Name = "labelVertical";
		((Control)labelVertical).Size = new Size(144, 20);
		((Control)labelVertical).TabIndex = 3;
		((Control)labelVertical).Text = "Vertical Gap:";
		labelVertical.TextAlign = (ContentAlignment)16;
		((Control)labelUnit3).ForeColor = SystemColors.ControlText;
		((Control)labelUnit3).Location = new Point(229, 80);
		((Control)labelUnit3).Name = "labelUnit3";
		((Control)labelUnit3).Size = new Size(24, 20);
		((Control)labelUnit3).TabIndex = 10;
		((Control)labelUnit3).Text = "cm";
		labelUnit3.TextAlign = (ContentAlignment)16;
		((Control)labelLabelType).Anchor = (AnchorStyles)9;
		((Control)labelLabelType).Location = new Point(15, 8);
		((Control)labelLabelType).Name = "labelLabelType";
		((Control)labelLabelType).Size = new Size(96, 21);
		((Control)labelLabelType).TabIndex = 2;
		((Control)labelLabelType).Text = "Label Type:";
		labelLabelType.TextAlign = (ContentAlignment)16;
		((Control)wpLabel).BackColor = Color.White;
		((Control)wpLabel).Controls.Add((Control)(object)labelDirection);
		((Control)wpLabel).Controls.Add((Control)(object)groupLineNumbers);
		((Control)wpLabel).Controls.Add((Control)(object)groupBoxPreview);
		((Control)wpLabel).Controls.Add((Control)(object)comboBoxUnit);
		((Control)wpLabel).Controls.Add((Control)(object)labelLabelType);
		((Control)wpLabel).Controls.Add((Control)(object)groupLineLabelSize);
		((Control)wpLabel).Controls.Add((Control)(object)groupLinePaper);
		((Control)wpLabel).Controls.Add((Control)(object)treeViewBoxLabelType);
		wpLabel.Dock = (DockStyle)5;
		((Control)wpLabel).Location = new Point(200, 66);
		((Control)wpLabel).Name = "wpLabel";
		((Control)wpLabel).Padding = new Padding(2);
		((Control)wpLabel).Size = new Size(578, 358);
		((Control)wpLabel).TabIndex = 0;
		((Control)wpLabel).Text = "Label";
		wpLabel.Visible = false;
		((Control)labelDirection).ForeColor = SystemColors.ControlText;
		((Control)labelDirection).Location = new Point(16, 328);
		((Control)labelDirection).Name = "labelDirection";
		((Control)labelDirection).Size = new Size(104, 20);
		((Control)labelDirection).TabIndex = 27;
		((Control)labelDirection).Text = "Direction:";
		labelDirection.TextAlign = (ContentAlignment)16;
		((Control)groupLineNumbers).Controls.Add((Control)(object)labelNumberOfColumns);
		((Control)groupLineNumbers).Controls.Add((Control)(object)labelNumberOfRows);
		((Control)groupLineNumbers).Controls.Add((Control)(object)numericNumberOfRows);
		((Control)groupLineNumbers).Controls.Add((Control)(object)numericNumberOfColumns);
		((Control)groupLineNumbers).Controls.Add((Control)(object)comboBoxDirection);
		((Control)groupLineNumbers).Enabled = false;
		groupLineNumbers.LinePosition = StiLinePosition.Top;
		groupLineNumbers.LineStyle = StiLineStyle.Line;
		((Control)groupLineNumbers).Location = new Point(8, 272);
		((Control)groupLineNumbers).Name = "groupLineNumbers";
		((Control)groupLineNumbers).Size = new Size(264, 88);
		((Control)groupLineNumbers).TabIndex = 3;
		((Control)groupLineNumbers).TabStop = false;
		((Control)labelNumberOfColumns).ForeColor = SystemColors.ControlText;
		((Control)labelNumberOfColumns).Location = new Point(8, 8);
		((Control)labelNumberOfColumns).Name = "labelNumberOfColumns";
		((Control)labelNumberOfColumns).Size = new Size(144, 20);
		((Control)labelNumberOfColumns).TabIndex = 25;
		((Control)labelNumberOfColumns).Text = "Number of Columns:";
		labelNumberOfColumns.TextAlign = (ContentAlignment)16;
		((Control)labelNumberOfRows).ForeColor = SystemColors.ControlText;
		((Control)labelNumberOfRows).Location = new Point(8, 32);
		((Control)labelNumberOfRows).Name = "labelNumberOfRows";
		((Control)labelNumberOfRows).Size = new Size(144, 20);
		((Control)labelNumberOfRows).TabIndex = 26;
		((Control)labelNumberOfRows).Text = "Number of Rows:";
		labelNumberOfRows.TextAlign = (ContentAlignment)16;
		((Control)numericNumberOfRows).BackColor = SystemColors.Window;
		((Control)numericNumberOfRows).ForeColor = SystemColors.ControlText;
		((Control)numericNumberOfRows).Location = new Point(152, 32);
		((Control)numericNumberOfRows).Name = "numericNumberOfRows";
		((Control)numericNumberOfRows).Size = new Size(88, 20);
		((Control)numericNumberOfRows).TabIndex = 1;
		numericNumberOfRows.ValueChanged += textBoxHeight_TextChanged;
		((Control)numericNumberOfColumns).BackColor = SystemColors.Window;
		((Control)numericNumberOfColumns).ForeColor = SystemColors.ControlText;
		((Control)numericNumberOfColumns).Location = new Point(152, 8);
		((Control)numericNumberOfColumns).Name = "numericNumberOfColumns";
		((Control)numericNumberOfColumns).Size = new Size(88, 20);
		((Control)numericNumberOfColumns).TabIndex = 0;
		numericNumberOfColumns.ValueChanged += textBoxHeight_TextChanged;
		((Control)comboBoxDirection).BackColor = SystemColors.Window;
		comboBoxDirection.DropDownStyle = (ComboBoxStyle)2;
		comboBoxDirection.DropDownWidth = 300;
		((Control)comboBoxDirection).ForeColor = SystemColors.ControlText;
		((Control)comboBoxDirection).Location = new Point(112, 56);
		comboBoxDirection.MaxDropDownItems = 20;
		((Control)comboBoxDirection).Name = "comboBoxDirection";
		((Control)comboBoxDirection).Size = new Size(152, 21);
		((Control)comboBoxDirection).TabIndex = 2;
		comboBoxDirection.SelectedIndexChanged += comboBoxDirection_SelectedIndexChanged;
		((Control)groupBoxPreview).Controls.Add((Control)(object)panelPreview);
		((Control)groupBoxPreview).Enabled = false;
		groupBoxPreview.LinePosition = StiLinePosition.Top;
		groupBoxPreview.LineStyle = StiLineStyle.Line;
		((Control)groupBoxPreview).Location = new Point(280, 32);
		((Control)groupBoxPreview).Name = "groupBoxPreview";
		((Control)groupBoxPreview).Size = new Size(296, 320);
		((Control)groupBoxPreview).TabIndex = 5;
		((Control)groupBoxPreview).TabStop = false;
		((Control)groupBoxPreview).Text = "Preview";
		((Control)panelPreview).Anchor = (AnchorStyles)15;
		((Control)panelPreview).Location = new Point(8, 16);
		((Control)panelPreview).Name = "panelPreview";
		((Control)panelPreview).Size = new Size(280, 296);
		((Control)panelPreview).TabIndex = 0;
		((Control)panelPreview).Paint += new PaintEventHandler(panelPreview_Paint);
		((Control)comboBoxUnit).BackColor = SystemColors.Window;
		comboBoxUnit.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxUnit).ForeColor = SystemColors.ControlText;
		((Control)comboBoxUnit).Location = new Point(408, 8);
		comboBoxUnit.MaxDropDownItems = 20;
		((Control)comboBoxUnit).Name = "comboBoxUnit";
		((Control)comboBoxUnit).Size = new Size(152, 21);
		((Control)comboBoxUnit).TabIndex = 4;
		comboBoxUnit.SelectedIndexChanged += comboBoxUnit_SelectedIndexChanged;
		((Control)groupLinePaper).Controls.Add((Control)(object)labelSize);
		((Control)groupLinePaper).Controls.Add((Control)(object)comboBoxPapers);
		((Control)groupLinePaper).Controls.Add((Control)(object)textBoxPageHeight);
		((Control)groupLinePaper).Controls.Add((Control)(object)textBoxPageWidth);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelPageWidth);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelUnit7);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelPageHeight);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelUnit6);
		((Control)groupLinePaper).Controls.Add((Control)(object)textBoxTop);
		((Control)groupLinePaper).Controls.Add((Control)(object)textBoxLeft);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelLeftMargin);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelUnit5);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelTopMargin);
		((Control)groupLinePaper).Controls.Add((Control)(object)labelUnit4);
		((Control)groupLinePaper).Enabled = false;
		groupLinePaper.LinePosition = StiLinePosition.Top;
		groupLinePaper.LineStyle = StiLineStyle.Line;
		((Control)groupLinePaper).Location = new Point(8, 144);
		((Control)groupLinePaper).Name = "groupLinePaper";
		((Control)groupLinePaper).Size = new Size(264, 128);
		((Control)groupLinePaper).TabIndex = 2;
		((Control)groupLinePaper).TabStop = false;
		((Control)labelSize).ForeColor = SystemColors.ControlText;
		((Control)labelSize).Location = new Point(8, 8);
		((Control)labelSize).Name = "labelSize";
		((Control)labelSize).Size = new Size(104, 20);
		((Control)labelSize).TabIndex = 24;
		((Control)labelSize).Text = "Size:";
		labelSize.TextAlign = (ContentAlignment)16;
		((Control)comboBoxPapers).BackColor = SystemColors.Window;
		comboBoxPapers.DropDownStyle = (ComboBoxStyle)2;
		comboBoxPapers.DropDownWidth = 300;
		((Control)comboBoxPapers).ForeColor = SystemColors.ControlText;
		((Control)comboBoxPapers).Location = new Point(112, 8);
		comboBoxPapers.MaxDropDownItems = 20;
		((Control)comboBoxPapers).Name = "comboBoxPapers";
		((Control)comboBoxPapers).Size = new Size(152, 21);
		((Control)comboBoxPapers).TabIndex = 0;
		comboBoxPapers.SelectedIndexChanged += comboBoxPapers_SelectedIndexChanged;
		((Control)textBoxPageHeight).ForeColor = SystemColors.ControlText;
		((Control)textBoxPageHeight).Location = new Point(152, 56);
		((Control)textBoxPageHeight).Name = "textBoxPageHeight";
		((Control)textBoxPageHeight).Size = new Size(64, 20);
		((Control)textBoxPageHeight).TabIndex = 2;
		textBoxPageHeight.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxPageHeight).TextChanged += textBoxHeight_TextChanged;
		((Control)textBoxPageWidth).ForeColor = SystemColors.ControlText;
		((Control)textBoxPageWidth).Location = new Point(152, 32);
		((Control)textBoxPageWidth).Name = "textBoxPageWidth";
		((Control)textBoxPageWidth).Size = new Size(64, 20);
		((Control)textBoxPageWidth).TabIndex = 1;
		textBoxPageWidth.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxPageWidth).TextChanged += textBoxHeight_TextChanged;
		((Control)labelPageWidth).ForeColor = SystemColors.ControlText;
		((Control)labelPageWidth).Location = new Point(8, 32);
		((Control)labelPageWidth).Name = "labelPageWidth";
		((Control)labelPageWidth).Size = new Size(144, 20);
		((Control)labelPageWidth).TabIndex = 18;
		((Control)labelPageWidth).Text = "Page Width:";
		labelPageWidth.TextAlign = (ContentAlignment)16;
		((Control)labelUnit7).ForeColor = SystemColors.ControlText;
		((Control)labelUnit7).Location = new Point(229, 56);
		((Control)labelUnit7).Name = "labelUnit7";
		((Control)labelUnit7).Size = new Size(24, 20);
		((Control)labelUnit7).TabIndex = 22;
		((Control)labelUnit7).Text = "cm";
		labelUnit7.TextAlign = (ContentAlignment)16;
		((Control)labelPageHeight).ForeColor = SystemColors.ControlText;
		((Control)labelPageHeight).Location = new Point(8, 56);
		((Control)labelPageHeight).Name = "labelPageHeight";
		((Control)labelPageHeight).Size = new Size(144, 20);
		((Control)labelPageHeight).TabIndex = 21;
		((Control)labelPageHeight).Text = "Page Height:";
		labelPageHeight.TextAlign = (ContentAlignment)16;
		((Control)labelUnit6).ForeColor = SystemColors.ControlText;
		((Control)labelUnit6).Location = new Point(229, 32);
		((Control)labelUnit6).Name = "labelUnit6";
		((Control)labelUnit6).Size = new Size(24, 20);
		((Control)labelUnit6).TabIndex = 19;
		((Control)labelUnit6).Text = "cm";
		labelUnit6.TextAlign = (ContentAlignment)16;
		((Control)textBoxTop).ForeColor = SystemColors.ControlText;
		((Control)textBoxTop).Location = new Point(152, 104);
		((Control)textBoxTop).Name = "textBoxTop";
		((Control)textBoxTop).Size = new Size(64, 20);
		((Control)textBoxTop).TabIndex = 4;
		textBoxTop.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxTop).TextChanged += textBoxHeight_TextChanged;
		((Control)textBoxLeft).ForeColor = SystemColors.ControlText;
		((Control)textBoxLeft).Location = new Point(152, 80);
		((Control)textBoxLeft).Name = "textBoxLeft";
		((Control)textBoxLeft).Size = new Size(64, 20);
		((Control)textBoxLeft).TabIndex = 3;
		textBoxLeft.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxLeft).TextChanged += textBoxHeight_TextChanged;
		((Control)labelLeftMargin).ForeColor = SystemColors.ControlText;
		((Control)labelLeftMargin).Location = new Point(8, 80);
		((Control)labelLeftMargin).Name = "labelLeftMargin";
		((Control)labelLeftMargin).Size = new Size(144, 20);
		((Control)labelLeftMargin).TabIndex = 4;
		((Control)labelLeftMargin).Text = "Left Margin:";
		labelLeftMargin.TextAlign = (ContentAlignment)16;
		((Control)labelUnit5).ForeColor = SystemColors.ControlText;
		((Control)labelUnit5).Location = new Point(229, 104);
		((Control)labelUnit5).Name = "labelUnit5";
		((Control)labelUnit5).Size = new Size(24, 20);
		((Control)labelUnit5).TabIndex = 16;
		((Control)labelUnit5).Text = "cm";
		labelUnit5.TextAlign = (ContentAlignment)16;
		((Control)labelTopMargin).ForeColor = SystemColors.ControlText;
		((Control)labelTopMargin).Location = new Point(8, 104);
		((Control)labelTopMargin).Name = "labelTopMargin";
		((Control)labelTopMargin).Size = new Size(144, 20);
		((Control)labelTopMargin).TabIndex = 14;
		((Control)labelTopMargin).Text = "Top Margin:";
		labelTopMargin.TextAlign = (ContentAlignment)16;
		((Control)labelUnit4).ForeColor = SystemColors.ControlText;
		((Control)labelUnit4).Location = new Point(229, 80);
		((Control)labelUnit4).Name = "labelUnit4";
		((Control)labelUnit4).Size = new Size(24, 20);
		((Control)labelUnit4).TabIndex = 9;
		((Control)labelUnit4).Text = "cm";
		labelUnit4.TextAlign = (ContentAlignment)16;
		((Control)treeViewBoxLabelType).BackColor = SystemColors.Window;
		treeViewBoxLabelType.DropDownWidth = 400;
		treeViewBoxLabelType.Flat = false;
		((Control)treeViewBoxLabelType).ForeColor = SystemColors.ControlText;
		((Control)treeViewBoxLabelType).Location = new Point(120, 8);
		treeViewBoxLabelType.MaxDropDownHeight = 300;
		((Control)treeViewBoxLabelType).Name = "treeViewBoxLabelType";
		((Control)treeViewBoxLabelType).Size = new Size(280, 21);
		((Control)treeViewBoxLabelType).TabIndex = 0;
		treeViewBoxLabelType.ValueChanged += treeViewBoxLabelType_ValueChanged;
		treeViewBoxLabelType.TreeNodesFill += treeViewBoxLabelType_TreeNodesFill;
		Wizard.ContentColor = Color.Empty;
		((Control)Wizard).Controls.Add((Control)(object)wpDataSource);
		((Control)Wizard).Controls.Add((Control)(object)wpLabel);
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
		((Control)Wizard).Size = new Size(778, 470);
		((Control)Wizard).TabIndex = 0;
		((Control)Wizard).Text = "Introduction";
		Wizard.TreeProgressFont = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		Wizard.NextClick += Wizard_NextClick;
		Wizard.FinishClick += Wizard_FinishClick;
		((Control)wpDataSource).BackColor = Color.White;
		((Control)wpDataSource).Controls.Add((Control)(object)pnAvailableDataSources);
		wpDataSource.Dock = (DockStyle)5;
		((Control)wpDataSource).Location = new Point(200, 66);
		((Control)wpDataSource).Name = "wpDataSource";
		((Control)wpDataSource).Padding = new Padding(2);
		((Control)wpDataSource).Size = new Size(578, 358);
		((Control)wpDataSource).TabIndex = 1005;
		((Control)wpDataSource).Text = "Datasource";
		wpDataSource.Visible = false;
		((Control)pnAvailableDataSources).Anchor = (AnchorStyles)15;
		((Control)pnAvailableDataSources).BackColor = Color.White;
		((Control)pnAvailableDataSources).Location = new Point(0, 0);
		((Control)pnAvailableDataSources).Margin = new Padding(2);
		((Control)pnAvailableDataSources).Name = "pnAvailableDataSources";
		((Control)pnAvailableDataSources).Size = new Size(578, 358);
		((Control)pnAvailableDataSources).TabIndex = 1;
		errorProvider1.ContainerControl = (ContainerControl)(object)this;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).ClientSize = new Size(778, 470);
		((Control)this).Controls.Add((Control)(object)Wizard);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiLabelReportWizardForm";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "StiStandardReportWizardForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Form)this).Load += ThisLoad;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)groupLineLabelSize).ResumeLayout(false);
		((Control)groupLineLabelSize).PerformLayout();
		((Control)wpLabel).ResumeLayout(false);
		((Control)groupLineNumbers).ResumeLayout(false);
		((ISupportInitialize)numericNumberOfRows).EndInit();
		((ISupportInitialize)numericNumberOfColumns).EndInit();
		((Control)groupBoxPreview).ResumeLayout(false);
		((Control)groupLinePaper).ResumeLayout(false);
		((Control)groupLinePaper).PerformLayout();
		((Control)Wizard).ResumeLayout(false);
		((Control)wpDataSource).ResumeLayout(false);
		((ISupportInitialize)errorProvider1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
