using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Dashboard;

internal static class StiDashboardExport
{
	public static void ExportDocument(StiReport report, Stream stream)
	{
		if (!StiDashboardExportAssembly.IsAssemblyLoaded)
		{
			throw new Exception("Assembly 'Stimulsoft.Dashboard.Export' is not found!");
		}
		object obj = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1], new Type[1] { typeof(StiExportSettings) });
		Export(report, stream, (IStiDashboardExportSettings)obj);
	}

	public static void Export(StiReport report, Stream stream, StiExportSettings settings)
	{
		if (!StiDashboardExportAssembly.IsAssemblyLoaded)
		{
			throw new Exception("Assembly 'Stimulsoft.Dashboard.Export' is not found!");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		object obj = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { settings }, new Type[1] { typeof(StiExportSettings) });
		if (settings.GetExportFormat() == StiExportFormat.Pdf && ((StiPdfExportSettings)settings).AutoPrintMode == StiPdfAutoPrintMode.Dialog)
		{
			StiInvokeMethodsHelper.SetPropertyValue(obj, "AutoPrint", true);
		}
		Export(report, stream, (IStiDashboardExportSettings)obj);
	}

	public static void Export(StiReport report, Stream stream, IStiDashboardExportSettings settings)
	{
		if (!StiDashboardExportAssembly.IsAssemblyLoaded)
		{
			throw new Exception("Assembly 'Stimulsoft.Dashboard.Export' is not found!");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		object obj = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "StiDashboardExportTools", "ExportToStream", new object[2] { report, settings }, new Type[2]
		{
			typeof(IStiReport),
			StiInvokeMethodsHelper.GetType("Stimulsoft.Dashboard.Export", "Settings.StiDashboardExportSettings")
		});
		if (obj is MemoryStream memoryStream)
		{
			memoryStream.Position = 0L;
			memoryStream.CopyTo(stream);
		}
	}
}
