using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Bmp.Export.ExportService.bmp")]
[StiServiceCategoryBitmap(typeof(StiExportService), "Stimulsoft.Report.Bmp.Export.ExportService.bmp")]
public abstract class StiExportService : StiService
{
	private string statusString = "";

	private bool isStopped;

	public sealed override Type ServiceType => typeof(StiExportService);

	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryExport");

	public object OwnerWindow { get; set; }

	public IStiProgressInformation Progress { get; set; }

	public string StatusString
	{
		get
		{
			return statusString;
		}
		set
		{
			if (!(statusString == value))
			{
				statusString = value;
				if (Progress != null)
				{
					Progress.Update(statusString);
				}
			}
		}
	}

	public bool IsStopped
	{
		get
		{
			if (Progress == null || !Progress.IsBreaked)
			{
				return isStopped;
			}
			return true;
		}
		set
		{
			isStopped = value;
		}
	}

	[Obsolete("IsStoped property is obsolete. Please use IsStopped property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsStoped
	{
		get
		{
			return IsStopped;
		}
		set
		{
			IsStopped = value;
		}
	}

	public abstract StiExportFormat ExportFormat { get; }

	public abstract string ExportNameInMenu { get; }

	public abstract string DefaultExtension { get; }

	public virtual bool MultipleFiles { get; set; }

	public abstract int Position { get; }

	public abstract string GroupCategory { get; }

	protected int RenderedPagesCount { get; set; }

	internal int CurrentPassNumber { get; set; }

	internal int MaximumPassNumber { get; set; }

	public event StiExportingEventHandler Exporting;

	public void InvokeExporting(StiPage page, StiPagesCollection pages, int currentPass, int maximumPass)
	{
		InvokeExporting(pages.IndexOf(page), pages.Count, currentPass, maximumPass);
	}

	public void InvokeExporting(int value, int maximum, int currentPass, int maximumPass)
	{
		InvokeExporting(new StiExportingEventArgs(value, maximum, currentPass, maximumPass));
	}

	public void InvokeExporting(StiExportingEventArgs e)
	{
		if (Progress != null)
		{
			Progress.SetProgressBar(e.Value, e.Maximum);
		}
		OnExporting(e);
	}

	public void OnExporting(StiExportingEventArgs e)
	{
		this.Exporting?.Invoke(this, e);
	}

	internal void StartExport(StiReport report, Stream stream, StiExportSettings settings, bool sendEMail, bool openAfterExport, string fileName, StiGuiMode guiMode)
	{
		StiExportInfo stiExportInfo = new StiExportInfo(report, settings, stream, sendEMail, openAfterExport, fileName);
		if (StiOptions.Engine.AllowProgressInThread)
		{
			ShowProgress();
			using BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += Worker_DoWork;
			backgroundWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
			backgroundWorker.RunWorkerAsync(stiExportInfo);
			return;
		}
		ShowProgress();
		try
		{
			DoExport(stiExportInfo);
		}
		finally
		{
			CloseProgress();
		}
		ProcessFile(stiExportInfo.SendEMail, stiExportInfo.OpenAfterExport, stiExportInfo.FileName, stiExportInfo.Report);
	}

	private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		StiExportInfo stiExportInfo = e.Result as StiExportInfo;
		CloseProgress();
		ProcessFile(stiExportInfo.SendEMail, stiExportInfo.OpenAfterExport, stiExportInfo.FileName, stiExportInfo.Report);
	}

	private void Worker_DoWork(object sender, DoWorkEventArgs e)
	{
		StiExportInfo info = (StiExportInfo)(e.Result = e.Argument as StiExportInfo);
		DoExport(info);
	}

	private void DoExport(StiExportInfo info)
	{
		try
		{
			foreach (StiPage renderedPage in info.Report.RenderedPages)
			{
				foreach (object component in renderedPage.GetComponents())
				{
					if (component is IStiChart stiChart)
					{
						stiChart.PreviousAnimations.Clear();
					}
				}
			}
			info.Report.ExportDocument(ExportFormat, this, info.Stream, info.Settings);
			InvokeExporting(100, 100, 1, 1);
		}
		catch (StopProgressException)
		{
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), "Save document...ERROR");
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			info.Stream.Flush();
			info.Stream.Close();
		}
	}

	internal void StartProgress(StiGuiMode guiMode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		isStopped = false;
		Progress = StiGuiOptions.GetProgressInformation(OwnerWindow, guiMode);
	}

	protected void ShowProgress()
	{
		Progress.Start(StiLocalization.Get("Export", "ExportingReport"));
	}

	protected void CloseProgress()
	{
		isStopped = Progress.IsBreaked;
		Progress.Close();
		Progress = null;
	}

	public void Export(StiReport report, string file)
	{
		Export(report, file, sendEMail: false, (StiGuiMode)0);
	}

	public void Export(StiReport report, string file, StiGuiMode guiMode)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		Export(report, file, sendEMail: false, guiMode);
	}

	public void Export(StiReport report, string file, bool sendEMail)
	{
		Export(report, file, sendEMail, (StiGuiMode)0);
	}

	public abstract void Export(StiReport report, string file, bool sendEMail, StiGuiMode guiMode);

	public void Export(StiReport report)
	{
		Export(report, null, sendEMail: false);
	}

	public abstract void ExportTo(StiReport report, Stream stream, StiExportSettings settings);

	protected void ProcessFile(bool sendEMail, bool openAfterExport, string fileName, StiReport report)
	{
		if (IsStopped)
		{
			File.Delete(fileName);
		}
		else if (sendEMail)
		{
			StiSendEMailEventArgs stiSendEMailEventArgs = new StiSendEMailEventArgs(report, string.Empty, (report.ReportAlias != null && report.ReportAlias.Trim().Length > 0) ? report.ReportAlias : report.ReportName, report.ReportDescription, fileName);
			StiOptions.Engine.GlobalEvents.InvokeSendEMailProcess(this, stiSendEMailEventArgs);
			if (stiSendEMailEventArgs.CallStandardHandler)
			{
				SendEMail(stiSendEMailEventArgs.Recipient, stiSendEMailEventArgs.Subject, stiSendEMailEventArgs.Body, stiSendEMailEventArgs.FilePath);
			}
		}
		else if (openAfterExport)
		{
			OpenFile(fileName);
		}
	}

	protected void SendEMail(string subject, string body, string filePath)
	{
		SendEMail(string.Empty, subject, body, filePath);
	}

	protected void SendEMail(string recipient, string subject, string body, string filePath)
	{
		string currentDirectory = Directory.GetCurrentDirectory();
		try
		{
			MAPI.SendEMail(recipient, subject, body, filePath);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "Send E-mail");
			StiLogService.Write(((object)this).GetType(), ex);
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
		finally
		{
			if (StiOptions.Engine.AllowSetCurrentDirectory)
			{
				Directory.SetCurrentDirectory(currentDirectory);
			}
		}
	}

	protected void OpenFile(string fileName)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiProcess.Start("\"" + fileName + "\"");
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "Open file");
			StiLogService.Write(((object)this).GetType(), ex);
			if (ex is Win32Exception { NativeErrorCode: 1155 })
			{
				MessageBox.Show(ex.Message, StiLocalization.Get("Export", "OpenAfterExport"));
			}
			else
			{
				StiExceptionProvider.Show(ex, false, (Form)null);
			}
		}
	}

	internal string CorrectFileName(string str)
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

	internal string GetFileName(StiReport report, bool sendEMail)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		if (sendEMail && StiOptions.Export.AutoGenerateFileNameInSendEMail)
		{
			string tempFileName = Path.GetTempFileName();
			tempFileName = Path.ChangeExtension(tempFileName, DefaultExtension);
			return Path.Combine(Path.GetTempPath(), tempFileName);
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			StiSettings.Load();
			string text = StiOptions.Viewer.Windows.ExportSaveLoadPath;
			if (string.IsNullOrEmpty(text))
			{
				text = StiSettings.GetStr("Viewer", "ExportSaveLoadPath", string.Empty);
			}
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				((FileDialog)val).InitialDirectory = text;
			}
			((FileDialog)val).Filter = GetFilter();
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).Title = StiHint.CreateHint(ExportNameInMenu);
			((FileDialog)val).FileName = CorrectFileName(report.ReportAlias);
			((FileDialog)val).DefaultExt = DefaultExtension;
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
			{
				string text2 = ((FileDialog)val).FileName;
				if (((FileDialog)val).FileName.LastIndexOf("." + ((FileDialog)val).DefaultExt, StringComparison.InvariantCulture) == -1)
				{
					text2 = text2 + "." + ((FileDialog)val).DefaultExt;
				}
				text = StiOptions.Viewer.Windows.ExportSaveLoadPath;
				if (string.IsNullOrEmpty(text))
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "ExportSaveLoadPath", Path.GetDirectoryName(text2));
					StiSettings.Save();
				}
				string text3 = text2;
				if (MultipleFiles)
				{
					string text4 = Path.GetExtension(text3).Substring(1);
					text3 = GetOrderFileName(text3.Substring(0, text3.Length - text4.Length - 1), 1, report.RenderedPages.Count, text4);
				}
				return text3;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return null;
	}

	public static string GetOrderFileName(string baseName, int index, int totalPagesCount, string extension)
	{
		string text = string.Empty;
		if (totalPagesCount > 1)
		{
			int num = 1;
			while (totalPagesCount >= 10)
			{
				num++;
				totalPagesCount /= 10;
			}
			text = index.ToString().PadLeft(num, '0');
		}
		if (!string.IsNullOrWhiteSpace(extension))
		{
			return baseName + text + "." + extension;
		}
		return text;
	}

	public static string GetFilters()
	{
		string text = "";
		bool flag = true;
		foreach (StiExportService item in StiOptions.Services.Exports.Where((StiExportService s) => ((StiService)s).ServiceEnabled))
		{
			if (!flag)
			{
				text += "|";
			}
			text += item.GetFilter();
			flag = false;
		}
		return text;
	}

	public abstract string GetFilter();
}
