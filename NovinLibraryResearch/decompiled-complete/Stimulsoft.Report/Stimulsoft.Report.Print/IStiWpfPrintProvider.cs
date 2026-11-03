namespace Stimulsoft.Report.Print;

public interface IStiWpfPrintProvider
{
	void Print(object printTicketObject, bool showPrintDialog, int fromPage, int toPage, int copies, string printerName);
}
