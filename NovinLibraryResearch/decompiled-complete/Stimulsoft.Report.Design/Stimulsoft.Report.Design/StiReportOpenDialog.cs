using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Dashboard;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Design;

public static class StiReportOpenDialog
{
	public static async Task<bool> OpenReportAsync(StiDesignerControl designer)
	{
		_ = 1;
		try
		{
			StiReport report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
			if (!(await OpenReportAsync(designer, report)))
			{
				return false;
			}
			await designer.SetReportAsync(report);
			designer.ReportNeverSaved = false;
			return true;
		}
		finally
		{
			StiDictionaryPanelHelper.HideProgress(designer);
		}
	}

	public static bool OpenReport(StiDesignerControl designer)
	{
		StiReport report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		if (!OpenReport(designer, report))
		{
			return false;
		}
		designer.Report = report;
		designer.ReportNeverSaved = false;
		return true;
	}

	public static bool OpenReport(StiReport report)
	{
		return OpenReport(null, report);
	}

	private static async Task<bool> OpenReportAsync(StiDesignerControl designer, StiReport report)
	{
		bool reportOpenResult = false;
		try
		{
			if (designer == null || !designer.TestReportSave())
			{
				designer?.UndoRedo?.Clear();
				bool flag = false;
				if (designer != null)
				{
					flag = StiOptions.Engine.GlobalEvents.InvokeLoadingReportInDesigner(designer);
				}
				if (!flag)
				{
					OpenFileDialog of = new OpenFileDialog();
					try
					{
						StiSettings.Load();
						string text = StiOptions.Designer.ReportSaveLoadPath;
						if (string.IsNullOrEmpty(text))
						{
							text = StiSettings.GetStr("StiDesigner", "ReportSaveLoadPath", string.Empty);
						}
						if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
						{
							((FileDialog)of).InitialDirectory = text;
						}
						((FileDialog)of).Filter = StiSLService.GetReportFilters(StiSLActions.Load);
						((FileDialog)of).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportOpen"));
						((FileDialog)of).RestoreDirectory = true;
						if ((int)((CommonDialog)of).ShowDialog() == 1 && ((FileDialog)of).FileName.Length > 0)
						{
							reportOpenResult = await OpenReportAsync(designer, report, ((FileDialog)of).FileName);
						}
					}
					finally
					{
						((IDisposable)of)?.Dispose();
					}
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiReportOpenDialog), "Loading report...ERROR");
			StiLogService.Write(typeof(StiReportOpenDialog), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			if (designer != null && designer.Report != null)
			{
				StiDictionaryPanelHelper.ShowProgress(designer, Loc.Get("DesignerFx", "LoadingData"), useTimer: false);
				Application.DoEvents();
				await designer.Report.Dictionary.ConnectAsync(loadData: false);
				designer.Builder.BuildReport();
				designer.InvokeBuidingToolbox();
				designer.Builder.InvalidateRulers();
			}
		}
		if (reportOpenResult)
		{
			StiOptions.Engine.GlobalEvents.InvokeLoadedReportInDesigner(designer);
		}
		Windows7Taskbar.HideTaskbarProgress();
		return reportOpenResult;
	}

	private static bool OpenReport(StiDesignerControl designer, StiReport report)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		bool flag = false;
		try
		{
			if (designer == null || !designer.TestReportSave())
			{
				designer?.UndoRedo?.Clear();
				bool flag2 = false;
				if (designer != null)
				{
					flag2 = StiOptions.Engine.GlobalEvents.InvokeLoadingReportInDesigner(designer);
				}
				if (!flag2)
				{
					OpenFileDialog val = new OpenFileDialog();
					try
					{
						StiSettings.Load();
						string text = StiOptions.Designer.ReportSaveLoadPath;
						if (string.IsNullOrEmpty(text))
						{
							text = StiSettings.GetStr("StiDesigner", "ReportSaveLoadPath", string.Empty);
						}
						if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
						{
							((FileDialog)val).InitialDirectory = text;
						}
						((FileDialog)val).Filter = StiSLService.GetReportFilters(StiSLActions.Load);
						((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportOpen"));
						((FileDialog)val).RestoreDirectory = true;
						if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
						{
							flag = OpenReport(designer, report, ((FileDialog)val).FileName);
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiReportOpenDialog), "Loading report...ERROR");
			StiLogService.Write(typeof(StiReportOpenDialog), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			if (designer != null && designer.Report != null)
			{
				designer.Report.Dictionary.Connect(loadData: false);
				designer.Builder.BuildReport();
				designer.InvokeBuidingToolbox();
				designer.Builder.InvalidateRulers();
			}
		}
		if (flag)
		{
			StiOptions.Engine.GlobalEvents.InvokeLoadedReportInDesigner(designer);
		}
		Windows7Taskbar.HideTaskbarProgress();
		return flag;
	}

	internal static async Task<bool> OpenReportAsync(StiDesignerControl designer, StiReport report, string fileName)
	{
		string key = string.Empty;
		if (fileName.Substring(fileName.Length - 4, 4) == ".mrx")
		{
			StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(fileName);
			try
			{
				if ((int)((Form)stiLoadPasswordForm).ShowDialog() != 1)
				{
					return false;
				}
				key = stiLoadPasswordForm.Password;
			}
			finally
			{
				((IDisposable)stiLoadPasswordForm)?.Dispose();
			}
		}
		Windows7Taskbar.ShowTaskbarProgress(report, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
		string reportSaveLoadPath = StiOptions.Designer.ReportSaveLoadPath;
		if (string.IsNullOrEmpty(reportSaveLoadPath))
		{
			StiSettings.Load();
			StiSettings.Set("StiDesigner", "ReportSaveLoadPath", Path.GetDirectoryName(fileName));
			StiSettings.Save();
		}
		if (designer != null && designer.Report == null)
		{
			StiDictionaryPanelHelper.ShowProgress(designer, Loc.Get("DesignerFx", "LoadingData"));
			if (report != null)
			{
				await designer.SetReportAsync(report);
			}
			else
			{
				await designer.SetReportAsync(StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport);
				report = designer.Report;
			}
			StiDictionaryPanelHelper.HideProgress(designer);
		}
		if (designer != null)
		{
			designer.Report.CurrentPage = 0;
			designer.TabControl.SelectedTabIndex = 0;
			StiCacheCleaner.Clean(designer.Report);
		}
		try
		{
			if (fileName.EndsWith(".mrx"))
			{
				report.LoadEncryptedReport(fileName, key);
			}
			else
			{
				report.Load(fileName);
			}
			StiAdapterHelper.CheckDownloadAdapter(report, designer);
		}
		catch (StiDashboardNotSupportedException)
		{
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				designer?.AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
			return false;
		}
		StiDictionaryPanelService dictionaryService = designer.GetDictionaryService();
		if (dictionaryService != null)
		{
			switch (dictionaryService.CurrentSort)
			{
			case StiDictionarySortOrder.Asc:
				report.Dictionary.SortItems(StiSortOrder.Asc);
				break;
			case StiDictionarySortOrder.Desc:
				report.Dictionary.SortItems(StiSortOrder.Desc);
				break;
			}
		}
		report.Pages.SortByPriority();
		report.ApplyStyles();
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		if (designer.UndoRedo != null)
		{
			designer.UndoRedo.Clear();
		}
		report.ResetSelection();
		report.Pages[0].Select();
		if (StiDesigner.CanDesignerChangeReportFileName)
		{
			designer.ReportFileName = fileName;
		}
		else
		{
			report.IsModified = true;
		}
		designer.AddRecentFile(fileName, null);
		designer.ReportNeverSaved = false;
		return true;
	}

	internal static bool OpenReport(StiDesignerControl designer, StiReport report, string fileName)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		string key = string.Empty;
		if (fileName.Substring(fileName.Length - 4, 4) == ".mrx")
		{
			StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(fileName);
			try
			{
				if ((int)((Form)stiLoadPasswordForm).ShowDialog() != 1)
				{
					return false;
				}
				key = stiLoadPasswordForm.Password;
			}
			finally
			{
				((IDisposable)stiLoadPasswordForm)?.Dispose();
			}
		}
		Windows7Taskbar.ShowTaskbarProgress(report, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
		string reportSaveLoadPath = StiOptions.Designer.ReportSaveLoadPath;
		if (string.IsNullOrEmpty(reportSaveLoadPath))
		{
			StiSettings.Load();
			StiSettings.Set("StiDesigner", "ReportSaveLoadPath", Path.GetDirectoryName(fileName));
			StiSettings.Save();
		}
		if (designer != null && designer.Report == null)
		{
			if (report == null)
			{
				designer.Report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
				report = designer.Report;
			}
			else
			{
				designer.Report = report;
			}
		}
		if (designer != null)
		{
			designer.Report.CurrentPage = 0;
			designer.TabControl.SelectedTabIndex = 0;
			StiCacheCleaner.Clean(designer.Report);
		}
		try
		{
			if (fileName.EndsWith(".mrx"))
			{
				report.LoadEncryptedReport(fileName, key);
			}
			else
			{
				report.Load(fileName);
			}
			StiAdapterHelper.CheckDownloadAdapter(report, designer);
		}
		catch (StiDashboardNotSupportedException)
		{
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				designer?.AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
			return false;
		}
		StiDictionaryPanelService dictionaryService = designer.GetDictionaryService();
		if (dictionaryService != null)
		{
			switch (dictionaryService.CurrentSort)
			{
			case StiDictionarySortOrder.Asc:
				report.Dictionary.SortItems(StiSortOrder.Asc);
				break;
			case StiDictionarySortOrder.Desc:
				report.Dictionary.SortItems(StiSortOrder.Desc);
				break;
			}
		}
		report.Pages.SortByPriority();
		report.ApplyStyles();
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		if (designer.UndoRedo != null)
		{
			designer.UndoRedo.Clear();
		}
		report.ResetSelection();
		report.Pages[0].Select();
		if (StiDesigner.CanDesignerChangeReportFileName)
		{
			designer.ReportFileName = fileName;
		}
		else
		{
			report.IsModified = true;
		}
		designer.AddRecentFile(fileName, null);
		designer.ReportNeverSaved = false;
		return true;
	}
}
