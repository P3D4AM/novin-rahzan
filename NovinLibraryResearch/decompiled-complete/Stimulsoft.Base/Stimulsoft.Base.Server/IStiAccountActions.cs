using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Stimulsoft.Base.Server;

public interface IStiAccountActions
{
	bool SkipRequestChangesSaving { get; set; }

	event EventHandler SaveReportStart;

	event EventHandler SaveReportCompleted;

	event ProgressChangedEventHandler SaveReportProgressChanged;

	event EventHandler OpenReportStart;

	event EventHandler OpenReportCompleted;

	event ProgressChangedEventHandler OpenReportProgressChanged;

	StiLoadResult LoadReportByteArray(string itemKey);

	Task<StiLoadResult> LoadReportByteArrayAsync(string itemKey, IntPtr? intPtr = null);

	IStiReport OpenReport(string key);

	Task<IStiReport> OpenReportAsync(string key);

	Task SaveReportAsync(string itemKey, object report, IntPtr intPtr);

	Task SaveEncryptedReportAsync(string itemKey, object report, string password, IntPtr intPtr);
}
