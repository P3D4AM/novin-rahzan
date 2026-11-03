using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Win.Dashboard;

namespace Stimulsoft.Report.Viewer;

public class StiLoadDialog
{
	private bool isDocumentFile = true;

	private bool isEncryptedFile;

	private bool isDashboard;

	private StiViewerControl viewerControl;

	private FileStream file;

	private StiSLService service;

	private string fileName;

	private bool isStoped;

	private StiReport report;

	public void Load()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Invalid comparison between Unknown and I4
		isStoped = false;
		viewerControl.CloseEditor();
		if (StiOptions.Engine.GlobalEvents.InvokeLoadingDocumentInViewer(viewerControl.CurrentReport))
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
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
			{
				string text2 = ((FileDialog)val).FileName.Substring(((FileDialog)val).FileName.Length - 4, 4);
				switch (text2)
				{
				case ".mrt":
				case ".mrz":
				case ".mrx":
					isDocumentFile = false;
					break;
				}
				if (text2 == ".mdx" || text2 == ".mrx")
				{
					isEncryptedFile = true;
				}
				string key = string.Empty;
				if (isEncryptedFile)
				{
					StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(((FileDialog)val).FileName);
					try
					{
						if ((int)((Form)stiLoadPasswordForm).ShowDialog() != 1)
						{
							return;
						}
						key = stiLoadPasswordForm.Password;
					}
					finally
					{
						((IDisposable)stiLoadPasswordForm)?.Dispose();
					}
				}
				if (viewerControl.Report != null && viewerControl.Report.Designer != null)
				{
					Windows7Taskbar.ShowTaskbarProgress((object)viewerControl.Report.Designer, (ThumbnailProgressState)1);
				}
				text = StiOptions.Viewer.Windows.ReportSaveLoadPath;
				if (string.IsNullOrEmpty(text))
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "ReportSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
					StiSettings.Save();
				}
				viewerControl.Progress = StiGuiOptions.GetProgressInformation(((Control)viewerControl).FindForm(), (StiGuiMode)0);
				viewerControl.Progress.IsMarquee = true;
				file = new FileStream(((FileDialog)val).FileName, FileMode.Open);
				fileName = ((FileDialog)val).FileName;
				service = null;
				if (StiReport.IsPackedFile(file))
				{
					if (isDocumentFile)
					{
						service = new StiPackedDocumentSLService();
					}
					else
					{
						service = new StiPackedReportSLService();
					}
				}
				else if (StiReport.IsEncryptedFile(file))
				{
					if (isDocumentFile)
					{
						service = new StiEncryptedDocumentSLService
						{
							Key = key
						};
					}
					else
					{
						service = new StiEncryptedReportSLService
						{
							Key = key
						};
					}
				}
				else if (StiReport.IsJsonFile(file))
				{
					if (isDocumentFile)
					{
						service = new StiJsonDocumentSLService();
					}
					else
					{
						service = new StiJsonReportSLService();
					}
				}
				else if (isDocumentFile)
				{
					service = new StiXmlDocumentSLService();
				}
				else
				{
					service = new StiXmlReportSLService();
				}
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
						isDashboard = report.Pages.Cast<StiPage>().Any((StiPage p) => p.IsDashboard);
						if (!isDashboard)
						{
							viewerControl.Report = report;
							viewerControl.BuildThumbs();
							viewerControl.LoadedReportFileName = fileName;
							((Control)viewerControl).Refresh();
						}
					}
				}
				Windows7Taskbar.HideTaskbarProgress();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (isDashboard)
		{
			MessageBox.Show("Sorry, but this report contains a dashboard. You should open this report directly in the designer! You cannot do that from the report viewer!");
		}
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
				return;
			}
			((StiReportSLService)service).Load(report, file);
			report.ApplyStyles();
			report.LoadFonts();
			report.Render();
		}
		catch (StiDashboardNotSupportedException)
		{
			StiDashboardNotSupportedForm.Run((Control)(object)viewerControl);
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

	public StiLoadDialog(StiViewerControl viewerControl)
	{
		this.viewerControl = viewerControl;
	}
}
