using System;

namespace Stimulsoft.Report.Print;

public class StiPrintPageEventArgs : EventArgs
{
	public int CurrentPage { get; set; }

	public int FromPage { get; set; }

	public int ToPage { get; set; }

	internal StiPrintPageEventArgs(int currentPage, int fromPage, int toPage)
	{
		CurrentPage = currentPage;
		FromPage = fromPage;
		ToPage = toPage;
	}
}
