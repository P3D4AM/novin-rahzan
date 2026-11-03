using System;
using System.Drawing.Printing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public class StiPageHelper
{
	public static PaperSize GetPaperSizeFromPaperKind(PaperKind paperKind)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if ((int)paperKind == 0 || !StiOptions.Print.AllowUsePaperSizesFromPrinterSettings)
		{
			return null;
		}
		try
		{
			PaperSizeCollection val = StiOptions.Print.CustomPaperSizes;
			if (val == null)
			{
				PrinterSettings val2 = new PrinterSettings();
				val = val2.PaperSizes;
			}
			foreach (PaperSize item in val)
			{
				PaperSize val3 = item;
				if (val3.Kind == paperKind)
				{
					return val3;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public static SizeD GetPaperSize(StiPage page, PaperSize paperSize)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		double value = paperSize.Width;
		double value2 = paperSize.Height;
		if (page.Unit is StiCentimetersUnit || page.Unit is StiMillimetersUnit)
		{
			value = PrinterUnitConvert.Convert(paperSize.Width, (PrinterUnit)1, (PrinterUnit)3);
			value2 = PrinterUnitConvert.Convert(paperSize.Height, (PrinterUnit)1, (PrinterUnit)3);
			if (page.Unit is StiCentimetersUnit)
			{
				value /= 10.0;
				value2 /= 10.0;
			}
		}
		else
		{
			value = Math.Round(page.ConvertFromHInches(page.Unit, value), 2);
			value2 = Math.Round(page.ConvertFromHInches(page.Unit, value2), 2);
		}
		if (page.Orientation == StiPageOrientation.Landscape)
		{
			return new SizeD(value2, value);
		}
		return new SizeD(value, value2);
	}
}
