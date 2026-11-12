using System;

namespace Stimulsoft.Dashboard.Export;

[AttributeUsage(AttributeTargets.Class)]
public class StiExportToolAttribute : Attribute
{
	public string ExportToolTypeName { get; }

	public StiExportToolAttribute(string exportToolTypeName)
	{
		ExportToolTypeName = exportToolTypeName;
	}

	public StiExportToolAttribute(Type exportToolType)
	{
		ExportToolTypeName = exportToolType.AssemblyQualifiedName;
	}
}
