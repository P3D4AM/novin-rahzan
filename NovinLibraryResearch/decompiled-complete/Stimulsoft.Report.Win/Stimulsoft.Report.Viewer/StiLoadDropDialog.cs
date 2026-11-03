using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Viewer;

public sealed class StiLoadDropDialog
{
	private StiViewerControl viewerControl;

	private bool isStoped;

	private StiReport report;

	private FileStream file;

	private StiDocumentSLService service;

	public void Load(string fileName)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		string[] array = new string[3] { ".mdc", ".mdz", ".mdx" };
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (fileName.EndsWith(array[i]))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		string key = string.Empty;
		if (fileName.Substring(fileName.Length - 4, 4) == ".mdx")
		{
			StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(fileName);
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
		viewerControl.Progress = StiGuiOptions.GetProgressInformation(((Control)viewerControl).FindForm(), (StiGuiMode)0);
		viewerControl.Progress.IsMarquee = true;
		file = new FileStream(fileName, FileMode.Open);
		if (file == null)
		{
			return;
		}
		Windows7Taskbar.ShowTaskbarProgress((object)viewerControl.Report.Designer, (ThumbnailProgressState)1);
		service = null;
		if (StiReport.IsPackedFile(file))
		{
			service = new StiPackedDocumentSLService();
		}
		else if (StiReport.IsEncryptedFile(file))
		{
			service = new StiEncryptedDocumentSLService();
			((StiEncryptedDocumentSLService)service).Key = key;
		}
		if (StiReport.IsJsonFile(file))
		{
			service = new StiJsonDocumentSLService();
		}
		else
		{
			service = new StiXmlDocumentSLService();
		}
		service.Loading += OnLoading;
		using (BackgroundWorker backgroundWorker = new BackgroundWorker())
		{
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += Worker_DoWork;
			backgroundWorker.RunWorkerAsync();
			viewerControl.Progress.Start(StiLocalization.Get("Report", "LoadingReport"));
			while (backgroundWorker.IsBusy)
			{
				Application.DoEvents();
			}
			service.Loading -= OnLoading;
			file.Flush();
			file.Close();
			viewerControl.Progress.Close();
			viewerControl.Progress = null;
			if (!isStoped)
			{
				viewerControl.Report = report;
				viewerControl.LoadedReportFileName = fileName;
			}
		}
		Windows7Taskbar.HideTaskbarProgress();
	}

	private void Worker_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			report = StiReport.GetReport();
			service.Load(report, file);
		}
		catch (StopProgressException)
		{
		}
		catch (Exception e2)
		{
			isStoped = true;
			StiLogService.Write(((object)viewerControl).GetType(), "Load document...ERROR");
			StiLogService.Write(((object)viewerControl).GetType(), e2);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	private void OnLoading(object sender, EventArgs e)
	{
		if (viewerControl.Progress.IsBreaked)
		{
			viewerControl.Progress.IsBreaked = false;
			isStoped = true;
			throw new StopProgressException();
		}
	}

	public StiLoadDropDialog(StiViewerControl viewerControl)
	{
		this.viewerControl = viewerControl;
	}
}
