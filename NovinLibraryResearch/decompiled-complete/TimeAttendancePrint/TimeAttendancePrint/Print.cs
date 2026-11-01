namespace TimeAttendancePrint;

public class Print
{
	public enum EnPrinterType : short
	{
		AllA4Printers,
		AllA5Printers
	}

	public enum EnfrmPrint : short
	{
		NotShow,
		DisplayForAll,
		DisplayForMotefareghe
	}

	public static bool ShowDesign { get; set; }
}
