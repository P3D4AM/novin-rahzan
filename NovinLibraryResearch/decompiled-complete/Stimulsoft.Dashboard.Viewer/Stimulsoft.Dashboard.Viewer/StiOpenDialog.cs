using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Dashboard.Viewer;

public class StiOpenDialog
{
	private bool isDocumentFile = true;

	private bool isEncryptedFile;

	private StiDashboardViewerControl viewerControl;

	private FileStream file;

	private StiSLService service;

	private bool isStoped;

	private StiReport report;

	public void Load()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		isStoped = false;
		if (StiOptions.Engine.GlobalEvents.InvokeLoadingDocumentInViewer(viewerControl.Report))
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			StiSettings.Load();
			string text = StiOptions.Viewer.Windows.ReportSaveLoadPath;
			if (string.IsNullOrEmpty(text))
			{
				text = StiSettings.GetStr("Viewer", "ReportSaveLoadPath", string.Empty);
			}
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				((FileDialog)val).InitialDirectory = text;
			}
			string documentFilters = StiSLService.GetDocumentFilters(StiSLActions.Load);
			string reportFilters = StiSLService.GetReportFilters(StiSLActions.Load);
			((FileDialog)val).Filter = string.Format("{0}|{1}|{2}|{3}", StiLocalization.Get("Cloud", "TextReports"), "*mdc;*.mdz;*.mdx;*mrt;*.mrz;*.mrx", documentFilters, reportFilters);
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).AutoUpgradeEnabled = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1 || ((FileDialog)val).FileName.Length <= 0)
			{
				return;
			}
			string extension = Path.GetExtension(((FileDialog)val).FileName);
			isDocumentFile = IsDocument(extension);
			isEncryptedFile = IsEncrypted(extension);
			string text2 = string.Empty;
			if (isEncryptedFile)
			{
				text2 = RequestPasswordForEncryptedFile(((FileDialog)val).FileName);
				if (text2 == null)
				{
					return;
				}
			}
			if (viewerControl.Report?.Designer != null)
			{
				Windows7Taskbar.ShowTaskbarProgress(viewerControl.Report.Designer, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
			}
			text = SetReportSaveLoadPath(((FileDialog)val).FileName);
			viewerControl.Progress = StiGuiOptions.GetProgressInformation(((Control)viewerControl).FindForm(), StiGuiMode.Gdi);
			viewerControl.Progress.IsMarquee = true;
			file = new FileStream(((FileDialog)val).FileName, FileMode.Open);
			service = GetLoadingService(text2);
			service.Loading += OnLoading;
			BackgroundWorker backgroundWorker = null;
			try
			{
				if (StiOptions.Engine.AllowProgressInThread)
				{
					backgroundWorker = new BackgroundWorker
					{
						WorkerSupportsCancellation = true
					};
					backgroundWorker.DoWork += delegate
					{
						DoLoad();
					};
					backgroundWorker.RunWorkerAsync();
				}
				viewerControl.Progress.Start(StiLocalization.Get("Report", "LoadingReport"));
				if (backgroundWorker != null)
				{
					while (backgroundWorker.IsBusy)
					{
						Application.DoEvents();
					}
				}
				else
				{
					DoLoad();
				}
			}
			finally
			{
				backgroundWorker?.Dispose();
				service.Loading -= OnLoading;
				file.Close();
				viewerControl.Progress.Close();
				viewerControl.Progress = null;
				if (!isStoped)
				{
					viewerControl.ReportsCache.Clear();
					StiCacheCleaner.Clean(viewerControl.Report);
					viewerControl.Report = report;
					viewerControl.RefreshViewer();
				}
			}
			Windows7Taskbar.HideTaskbarProgress();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private string RequestPasswordForEncryptedFile(string fileName)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(fileName);
		try
		{
			return ((int)((Form)stiLoadPasswordForm).ShowDialog() == 1) ? stiLoadPasswordForm.Password : null;
		}
		finally
		{
			((IDisposable)stiLoadPasswordForm)?.Dispose();
		}
	}

	private bool IsEncrypted(string fileExtension)
	{
		if (fileExtension == ".mdx" || fileExtension == ".mrx")
		{
			return true;
		}
		return false;
	}

	private bool IsDocument(string fileExtension)
	{
		switch (fileExtension)
		{
		case ".mrt":
		case ".mrz":
		case ".mrx":
			return false;
		default:
			return true;
		}
	}

	private string SetReportSaveLoadPath(string fileName)
	{
		string reportSaveLoadPath = StiOptions.Viewer.Windows.ReportSaveLoadPath;
		if (!string.IsNullOrEmpty(reportSaveLoadPath))
		{
			return reportSaveLoadPath;
		}
		StiSettings.Load();
		StiSettings.Set("Viewer", "ReportSaveLoadPath", Path.GetDirectoryName(fileName));
		StiSettings.Save();
		return reportSaveLoadPath;
	}

	private StiSLService GetLoadingService(string key)
	{
		if (StiReport.IsPackedFile(file))
		{
			if (isDocumentFile)
			{
				return new StiPackedDocumentSLService();
			}
			return new StiPackedReportSLService();
		}
		if (StiReport.IsEncryptedFile(file))
		{
			if (isDocumentFile)
			{
				return new StiEncryptedDocumentSLService
				{
					Key = key
				};
			}
			return new StiEncryptedReportSLService
			{
				Key = key
			};
		}
		if (StiReport.IsJsonFile(file))
		{
			if (isDocumentFile)
			{
				return new StiJsonDocumentSLService();
			}
			return new StiJsonReportSLService();
		}
		if (isDocumentFile)
		{
			return new StiXmlDocumentSLService();
		}
		return new StiXmlReportSLService();
	}

	private void DoLoad()
	{
		try
		{
			report = StiReport.GetReport();
			if (isDocumentFile)
			{
				((StiDocumentSLService)service).Load(report, file);
				report.LoadDocumentFonts();
			}
			else
			{
				((StiReportSLService)service).Load(report, file);
				report.LoadFonts();
				report.Render();
			}
		}
		catch (StopProgressException)
		{
		}
		catch (Exception e)
		{
			isStoped = true;
			StiLogService.Write(((object)viewerControl).GetType(), "Load document...ERROR");
			StiLogService.Write(((object)viewerControl).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	private void OnLoading(object sender, EventArgs e)
	{
		if (!StiOptions.Engine.AllowProgressInThread)
		{
			Application.DoEvents();
		}
		if (viewerControl.Progress.IsBreaked)
		{
			viewerControl.Progress.IsBreaked = false;
			isStoped = true;
			throw new StopProgressException();
		}
	}

	public void Clear()
	{
		service = null;
		viewerControl = null;
		report = null;
		file = null;
	}

	public StiOpenDialog(StiDashboardViewerControl viewerControl)
	{
		this.viewerControl = viewerControl;
	}
}
