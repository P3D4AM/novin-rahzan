using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IPrinting
{
	PrintDocument PrintDocument { get; }

	PrinterSettings PrinterSettings { get; }

	PrintOptions Options { get; set; }

	PrintOptions AllowedOptions { get; set; }

	IPageHeader Header { get; set; }

	IPageHeader Footer { get; set; }

	PrintDialog PrintDialog { get; }

	PageSetupDialog PageSetupDialog { get; }

	PrintPreviewDialog PrintPreviewDialog { get; }

	IPrintOptionsDialog PrintOptionsDialog { get; set; }

	event CreatePrintEditEvent CreatePrintEdit;

	event EventHandler Initialized;

	void Assign(IPrinting source);

	void Print();

	DialogResult ExecutePrintPreviewDialog();

	DialogResult ExecutePrintDialog();

	DialogResult ExecutePageSetupDialog();

	DialogResult ExecutePrintOptionsDialog();

	void ResetOptions();

	void ResetAllowedOptions();
}
