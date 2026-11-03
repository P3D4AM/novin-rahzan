using System.Drawing;
using System.IO;
using System.Reflection;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report;

public static class StiReportResourcesLoader
{
	public static void LoadReportResourcesFromAssembly(StiReport report)
	{
		if (report.ReportResources == null)
		{
			return;
		}
		Assembly assembly = report.GetType().Assembly;
		foreach (StiReportResource reportResource in report.ReportResources)
		{
			Stream manifestResourceStream = assembly.GetManifestResourceStream(reportResource.Resource);
			if (manifestResourceStream != null)
			{
				byte[] array = new byte[manifestResourceStream.Length];
				manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
				if (!LoadPageWatermarkImageFromContent(report, reportResource, array) && !LoadVariableImageFromContent(report, reportResource, array))
				{
					LoadReportResourceFromContent(report, reportResource, array);
				}
			}
		}
	}

	public static void LoadReportResourcesFromReport(StiReport source, StiReport dest)
	{
		StiReportResourcesFinder.GetResourcePages(source).ForEach(delegate(StiPage p)
		{
			dest.Pages[p.Name]?.Watermark.PutImage(p.Watermark.TakeImage().Clone() as byte[]);
		});
		StiReportResourcesFinder.GetResourceImageComponents(source).ForEach(delegate(StiImage i)
		{
			if (dest.GetComponentByName(i.Name) is StiImage stiImage && i.ExistImage())
			{
				stiImage.PutImage(i.TakeImage().Clone() as byte[]);
			}
		});
		StiReportResourcesFinder.GetResourceVariables(source).ForEach(delegate(StiVariable v)
		{
			string value = v.Value;
			StiVariable stiVariable = dest.Dictionary.Variables[v.Name];
			if (stiVariable == null)
			{
				dest.Dictionary.Variables.Add(new StiVariable(null, v.Name, value));
			}
			else
			{
				stiVariable.Value = value;
				if (!dest.CheckNeedsCompiling())
				{
					dest[v.Name] = stiVariable.ValueObject;
				}
			}
		});
		StiReportResourcesFinder.GetResourceReport(source).ForEach(delegate(StiResource r)
		{
			byte[] content = StiArrayCloner.Clone(r.Content);
			StiResource stiResource = dest.Dictionary.Resources[r.Name];
			if (stiResource != null)
			{
				stiResource.Content = content;
			}
			else
			{
				dest.Dictionary.Resources.Add(new StiResource(r.Name, r.Type, content));
			}
		});
	}

	private static bool LoadVariableImageFromContent(StiReport report, StiReportResource resource, object content)
	{
		StiVariable stiVariable = report.Dictionary.Variables[resource.Component];
		if (stiVariable != null)
		{
			stiVariable.ValueObject = content as byte[];
			if (!report.CheckNeedsCompiling())
			{
				string component = resource.Component;
				object valueObject = stiVariable.ValueObject;
				report[component] = ((valueObject is Image) ? valueObject : null);
			}
			return true;
		}
		return false;
	}

	private static bool LoadPageWatermarkImageFromContent(StiReport report, StiReportResource resource, byte[] content)
	{
		StiComponent componentByName = report.GetComponentByName(resource.Component);
		if (componentByName != null)
		{
			if (componentByName is StiPage stiPage && resource.Property == "Watermark")
			{
				stiPage.Watermark.PutImage(content);
			}
			if (componentByName is StiImage stiImage && resource.Property == "Image")
			{
				stiImage.PutImage(content);
			}
			return true;
		}
		return false;
	}

	private static bool LoadReportResourceFromContent(StiReport report, StiReportResource resource, object content)
	{
		StiResource stiResource = report.Dictionary.Resources[resource.Component];
		if (stiResource != null)
		{
			stiResource.Content = content as byte[];
			return true;
		}
		return false;
	}
}
