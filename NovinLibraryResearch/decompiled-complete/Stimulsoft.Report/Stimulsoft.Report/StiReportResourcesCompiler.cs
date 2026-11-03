using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public class StiReportResourcesCompiler : IDisposable
{
	private string tempFolder;

	private StiReport report;

	private bool inMemory;

	private int reportIndex = -1;

	private List<StiResourceItem> resources;

	private List<string> files;

	public List<StiResourceItem> Resources => resources ?? (resources = new List<StiResourceItem>());

	public List<string> Files => files ?? (files = new List<string>());

	private void CreateResourceFiles()
	{
		if (Resources != null)
		{
			Resources.Clear();
		}
		if (report.ReportResources != null)
		{
			report.ReportResources.Clear();
		}
		StiReportResourcesFinder.GetResourcePages(report).ForEach(delegate(StiPage p)
		{
			CreateResourceFile(p.Name, p, p.Watermark.TakeImage(), "Watermark");
			p.Watermark.ResetImage();
		});
		(from image in StiReportResourcesFinder.GetResourceImageComponents(report)
			where image != null
			select image).ToList().ForEach(delegate(StiImage image)
		{
			if (!string.IsNullOrWhiteSpace(image.GlobalizedName) && image.Report != null && image.Report.GlobalizationManager != null)
			{
				object @object = image.Report.GlobalizationManager.GetObject(image.GlobalizedName);
				Image val = (Image)((@object is Image) ? @object : null);
				if (val != null)
				{
					CreateResourceFile(image.Name, image, StiImageConverter.ImageToBytes(val, false), "Image");
					val.Dispose();
					return;
				}
			}
			if (image.ExistImage())
			{
				CreateResourceFile(image.Name, image, image.TakeImage(), "Image");
				image.ResetImage();
			}
		});
		StiReportResourcesFinder.GetResourceVariables(report).ForEach(delegate(StiVariable v)
		{
			byte[] resource = ((v.ValueObject is Image) ? StiImageConverter.ImageToBytes((Image)/*isinst with value type is only supported in some contexts*/, false) : (v.ValueObject as byte[]));
			CreateResourceFile(v.Name, v, resource, "ValueObject");
			v.ValueObject = null;
		});
		StiReportResourcesFinder.GetResourceReport(report).ForEach(delegate(StiResource r)
		{
			CreateResourceFile(r.Name, r, r.Content, "Resource");
			r.Content = null;
		});
	}

	private void CreateResourceFile(string resourceName, object component, byte[] resource, string property)
	{
		string text = ((reportIndex > -1) ? (reportIndex + ".") : "");
		string text2 = text + resourceName + ".res";
		Resources.Add(new StiResourceItem(component, resource, text2, property));
		if (!inMemory)
		{
			report.ReportResources.Add(new StiReportResource(text2, resourceName, property));
			string text3 = Path.Combine(tempFolder, text + resourceName) + ".res";
			Files.Add(text3);
			File.WriteAllBytes(text3, resource);
		}
	}

	private void RestoreResourcesInReport()
	{
		if (Resources == null)
		{
			return;
		}
		foreach (StiResourceItem resource in Resources)
		{
			if (resource.Component is StiPage stiPage)
			{
				stiPage.Watermark.PutImage(resource.Resource);
			}
			if (resource.Component is StiImage stiImage)
			{
				stiImage.PutImage(resource.Resource);
			}
			if (resource.Component is StiVariable stiVariable)
			{
				if (StiTypeFinder.FindType(stiVariable.Type, typeof(Image)))
				{
					stiVariable.ValueObject = StiImageConverter.BytesToImage(resource.Resource, 200, 200, true, false);
				}
				else
				{
					stiVariable.ValueObject = resource.Resource;
				}
			}
			if (resource.Component is StiResource stiResource)
			{
				stiResource.Content = resource.Resource;
			}
		}
		Resources.Clear();
		resources = null;
	}

	private string GetResourcesFolder()
	{
		string text = StiOptions.Engine.ReportResources.ResourcesPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "Resources");
		}
		text = Path.Combine(text, StiGuidUtils.NewGuid());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public void ProcessReport(StiReport report)
	{
		if (this.report != null)
		{
			RestoreResourcesInReport();
		}
		reportIndex++;
		this.report = report;
		if (report != null && StiReportResourcesFinder.ResourcesExits(report))
		{
			if (!inMemory && tempFolder == null)
			{
				tempFolder = GetResourcesFolder();
			}
			CreateResourceFiles();
		}
	}

	public void Dispose()
	{
		if (report != null)
		{
			RestoreResourcesInReport();
		}
		if (!inMemory && Directory.Exists(tempFolder))
		{
			Directory.Delete(tempFolder, recursive: true);
		}
		files = null;
	}

	public StiReportResourcesCompiler(StiReport report, bool inMemory)
	{
		this.report = report;
		this.inMemory = inMemory;
		if (report != null && StiReportResourcesFinder.ResourcesExits(report))
		{
			if (!inMemory)
			{
				tempFolder = GetResourcesFolder();
			}
			CreateResourceFiles();
		}
	}
}
