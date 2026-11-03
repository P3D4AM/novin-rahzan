using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Viewer;

public class StiSaveDialog
{
	private StiViewerControl viewerControl;

	private bool sendEMail;

	private FileStream file;

	private StiDocumentSLService service;

	private bool isStoped;

	public void Save()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Invalid comparison between Unknown and I4
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Invalid comparison between Unknown and I4
		isStoped = false;
		if (viewerControl.CurrentReport == null || (viewerControl.CurrentReport.RenderedPages != null && viewerControl.CurrentReport.RenderedPages.CacheMode))
		{
			return;
		}
		viewerControl.CloseEditor();
		if (StiOptions.Engine.GlobalEvents.InvokeSavingDocumentInViewer(viewerControl.CurrentReport))
		{
			return;
		}
		StiSettings.Load();
		string text = StiOptions.Viewer.Windows.ReportSaveLoadPath;
		if (string.IsNullOrEmpty(text))
		{
			text = StiSettings.GetStr("Viewer", "ReportSaveLoadPath", string.Empty);
		}
		string fileName = ((viewerControl.LoadedReportFileName == null) ? viewerControl.CurrentReport.ReportAlias : Path.GetFileNameWithoutExtension(viewerControl.LoadedReportFileName));
		StiSaveDocumentEventArgs stiSaveDocumentEventArgs = new StiSaveDocumentEventArgs(viewerControl.CurrentReport, text, fileName);
		StiOptions.Engine.GlobalEvents.InvokeSaveDocumentProcess(this, stiSaveDocumentEventArgs);
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			if (!string.IsNullOrEmpty(stiSaveDocumentEventArgs.FilePath) && Directory.Exists(stiSaveDocumentEventArgs.FilePath))
			{
				((FileDialog)val).InitialDirectory = stiSaveDocumentEventArgs.FilePath;
			}
			((FileDialog)val).Filter = StiSLService.GetDocumentFilters(StiSLActions.Save);
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
			((FileDialog)val).FileName = CorrectFileName(stiSaveDocumentEventArgs.FileName);
			((FileDialog)val).DefaultExt = "mdc";
			((FileDialog)val).FilterIndex = StiSettings.GetInt("Viewer", "SaveDialogFilterIndex", 1);
			if ((int)((CommonDialog)val).ShowDialog() != 1 || ((FileDialog)val).FileName.Length <= 0)
			{
				return;
			}
			StiSettings.Set("Viewer", "SaveDialogFilterIndex", ((FileDialog)val).FilterIndex);
			string key = string.Empty;
			if (((FileDialog)val).FileName.Substring(((FileDialog)val).FileName.Length - 4, 4) == ".mdx")
			{
				StiSavePasswordForm stiSavePasswordForm = new StiSavePasswordForm();
				if ((int)((Form)stiSavePasswordForm).ShowDialog() != 1)
				{
					return;
				}
				key = stiSavePasswordForm.Password;
			}
			Windows7Taskbar.ShowTaskbarProgress((object)viewerControl.Report.Designer, (ThumbnailProgressState)1);
			text = StiOptions.Viewer.Windows.ReportSaveLoadPath;
			if (string.IsNullOrEmpty(text))
			{
				StiSettings.Load();
				StiSettings.Set("Viewer", "ReportSaveLoadPath", Path.GetDirectoryName(((FileDialog)val).FileName));
				StiSettings.Save();
			}
			service = StiSLService.GetDocumentSLServices(StiSLActions.Save)[((FileDialog)val).FilterIndex - 1];
			if (service is StiEncryptedDocumentSLService)
			{
				((StiEncryptedDocumentSLService)service).Key = key;
			}
			fileName = ((FileDialog)val).FileName;
			if (service.MultiplePages)
			{
				string text2 = "";
				int num = fileName.IndexOf('.');
				if (num >= 0)
				{
					text2 = fileName.Substring(num, fileName.Length - num);
					fileName = fileName.Substring(0, num);
				}
				fileName += "001";
				fileName += text2;
			}
			viewerControl.View.IsLockPaint = true;
			((Control)viewerControl.View).Invalidate();
			Application.DoEvents();
			viewerControl.Progress = StiGuiOptions.GetProgressInformation(((Control)viewerControl).FindForm(), (StiGuiMode)0);
			viewerControl.Progress.IsMarquee = true;
			StiFileUtils.ProcessReadOnly(text);
			file = new FileStream(fileName, FileMode.Create);
			service.Saving += OnSaving;
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
						DoSave();
					};
					backgroundWorker.RunWorkerAsync();
				}
				viewerControl.Progress.Start(StiLocalization.Get("Report", "SavingReport"));
				if (backgroundWorker != null)
				{
					while (backgroundWorker.IsBusy)
					{
						Application.DoEvents();
					}
				}
				else
				{
					DoSave();
				}
			}
			finally
			{
				backgroundWorker?.Dispose();
				service.Saving -= OnSaving;
				file.Flush();
				file.Close();
				if (isStoped)
				{
					File.Delete(fileName);
				}
				viewerControl.Progress.Close();
				viewerControl.Progress = null;
				viewerControl.View.IsLockPaint = false;
				((Control)viewerControl.View).Invalidate();
				if (sendEMail)
				{
					MAPI.SendEMail((viewerControl.CurrentReport.ReportAlias != null && viewerControl.CurrentReport.ReportAlias.Trim().Length > 0) ? viewerControl.CurrentReport.ReportAlias : viewerControl.CurrentReport.ReportName, viewerControl.CurrentReport.ReportDescription, fileName);
				}
			}
			Windows7Taskbar.HideTaskbarProgress();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private string CorrectFileName(string str)
	{
		string text = "/\\:*?\"<>|";
		char[] array = str.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			int num = text.IndexOf(array[i]);
			if (num != -1)
			{
				array[i] = '_';
			}
		}
		return new string(array);
	}

	private void DoSave()
	{
		try
		{
			service.Save(viewerControl.CurrentReport, file);
		}
		catch (StopProgressException)
		{
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)viewerControl).GetType(), "Save document...ERROR");
			StiLogService.Write(((object)viewerControl).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	private void OnSaving(object sender, EventArgs e)
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
		file = null;
	}

	public StiSaveDialog(StiViewerControl viewerControl, bool sendEMail)
	{
		this.viewerControl = viewerControl;
		this.sendEMail = sendEMail;
	}
}
