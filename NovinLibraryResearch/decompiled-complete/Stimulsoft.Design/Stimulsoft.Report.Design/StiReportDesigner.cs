using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Design;

public class StiReportDesigner : ComponentDesigner
{
	private StiReport report;

	private DesignerVerb reportWizard;

	private DesignerVerb designReport;

	private DesignerVerb showReport;

	private DesignerVerb chooseDataSources;

	private DesignerVerb openReportFromFile;

	private DesignerVerb saveReportToFile;

	private static bool markerOfChanges;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[6] { reportWizard, designReport, showReport, chooseDataSources, openReportFromFile, saveReportToFile });

	private void OnReportWizard(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		if (report.IsDesigning)
		{
			MessageBox.Show("You can't run report wizard in this mode!");
			return;
		}
		try
		{
			if ((int)GetChooseDataSourcesResult() != 1)
			{
				return;
			}
			report.Dictionary.DataStore.Clear();
			report.RegReportDataSources();
			StiStandardWizardService stiStandardWizardService = new StiStandardWizardService();
			report.Dictionary.Synchronize();
			StiReport stiReport = stiStandardWizardService.CreateReport(report);
			if (stiReport != null)
			{
				string text = stiReport.SavePackedReportToString();
				LoadReport(report, text);
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(report).Find("ReportSource", ignoreCase: false);
				if (report.SaveReportInResources)
				{
					((ComponentDesigner)this).RaiseComponentChanged((MemberDescriptor)propertyDescriptor, (object)report.ReportSource, (object)report.SavePackedReportToString());
					report.ReportSource = text;
				}
				OnDesignReport(sender, e);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private static void OnSaveReport(object sender, StiSavingObjectEventArgs e)
	{
		try
		{
			if (e.SaveAs)
			{
				e.Processed = false;
				return;
			}
			StiReport stiReport = (sender as StiDesignerControl).Report;
			if (stiReport.SaveReportInResources)
			{
				stiReport.SetReportSource(stiReport.SavePackedReportToString());
				markerOfChanges = true;
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnDesignReport(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if (report.IsDesigning)
		{
			MessageBox.Show("Designer already running!");
			return;
		}
		try
		{
			StiDesigner.RunInDesignMode = true;
			StiDesigner.CanDesignerChangeReportFileName = !report.SaveReportInResources;
			StiOptions.Engine.HideExceptions = false;
			StiOptions.Engine.HideMessages = false;
			StiOptions.Engine.EmulateData = true;
			markerOfChanges = false;
			string reportStr = ((!string.IsNullOrEmpty(report.ReportSource)) ? report.ReportSource : report.SavePackedReportToString());
			bool saveReportInResources = report.SaveReportInResources;
			if (report.SaveReportInResources)
			{
				StiDesigner.SavingReport += OnSaveReport;
				if (!string.IsNullOrEmpty(report.ReportSource))
				{
					LoadReport(report, report.ReportSource);
				}
			}
			report.Dictionary.DataStore.Clear();
			report.RegReportDataSources();
			if (report.SaveReportInResources)
			{
				if (!string.IsNullOrEmpty(report?.Site?.Name))
				{
					report.ReportFile = report.Site.Name;
				}
				else
				{
					report.ReportFile = "Component";
				}
			}
			else
			{
				report.ReportFile = string.Empty;
			}
			try
			{
				report.Design(dialogForm: true);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			finally
			{
				report.Designer = null;
			}
			if (!markerOfChanges)
			{
				LoadReport(report, reportStr);
			}
			report.SaveReportInResources = saveReportInResources;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(report).Find("ReportSource", ignoreCase: false);
			if (report.SaveReportInResources)
			{
				((ComponentDesigner)this).RaiseComponentChanged((MemberDescriptor)propertyDescriptor, (object)report.ReportSource, (object)report.SavePackedReportToString());
				StiDesigner.SavingReport -= OnSaveReport;
			}
		}
		catch (Exception exception2)
		{
			StiExceptionProvider.Show(exception2);
		}
	}

	private void OnShowReport(object sender, EventArgs e)
	{
		try
		{
			StiOptions.Engine.EmulateData = false;
			if (report.SaveReportInResources && !string.IsNullOrEmpty(report.ReportSource))
			{
				LoadReport(report, report.ReportSource);
			}
			report.Dictionary.DataStore.Clear();
			report.RegReportDataSources();
			report.Show();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnChooseDataSources(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GetChooseDataSourcesResult();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnOpenReportFromFile(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiSLService.GetReportFilters(StiSLActions.Load);
				((FileDialog)val).Title = "Open Report";
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					report.Load(((FileDialog)val).FileName);
					report.ReportSource = report.SavePackedReportToString();
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(report).Find("ReportSource", ignoreCase: false);
					if (report.SaveReportInResources)
					{
						((ComponentDesigner)this).RaiseComponentChanged((MemberDescriptor)propertyDescriptor, (object)report.ReportSource, (object)report.SavePackedReportToString());
					}
					MessageBox.Show("Report has been opened from file successfully.");
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

	private void OnSaveReportToFile(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = StiSLService.GetReportFilters(StiSLActions.Save);
				((FileDialog)val).Title = "Save report";
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).DefaultExt = "mrt";
				((FileDialog)val).FileName = report.ReportName;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					report.Save(((FileDialog)val).FileName);
					MessageBox.Show("Report has been saved to file successfully.");
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

	private DialogResult GetChooseDataSourcesResult()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (report.IsDesigning)
		{
			MessageBox.Show("You can't choose the data sources in this mode!");
			return (DialogResult)0;
		}
		StiReportDataSourcesEditorForm stiReportDataSourcesEditorForm = new StiReportDataSourcesEditorForm((IReferenceService)((ComponentDesigner)this).GetService(typeof(IReferenceService)), report);
		try
		{
			if ((int)((Form)stiReportDataSourcesEditorForm).ShowDialog() == 1)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(report).Find("ReportDataSources", ignoreCase: false);
				((ComponentDesigner)this).RaiseComponentChanged((MemberDescriptor)propertyDescriptor, (object)report.ReportDataSources, (object)report.ReportDataSources);
			}
			return ((Form)stiReportDataSourcesEditorForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiReportDataSourcesEditorForm)?.Dispose();
		}
	}

	private static void LoadReport(StiReport report, string reportStr)
	{
		if (reportStr[0] == '<' && reportStr[1] == '?')
		{
			report.LoadFromString(reportStr);
		}
		else
		{
			report.LoadPackedReportFromString(reportStr);
		}
	}

	public override void Initialize(IComponent component)
	{
		((ComponentDesigner)this).Initialize(component);
		report = (StiReport)component;
	}

	public StiReportDesigner()
	{
		reportWizard = new DesignerVerb("Report Wizard", OnReportWizard);
		designReport = new DesignerVerb("Design Report", OnDesignReport);
		showReport = new DesignerVerb("Show Report", OnShowReport);
		chooseDataSources = new DesignerVerb("Choose Data Sources", OnChooseDataSources);
		openReportFromFile = new DesignerVerb("Load Report from File", OnOpenReportFromFile);
		saveReportToFile = new DesignerVerb("Save Report to File", OnSaveReportToFile);
	}
}
