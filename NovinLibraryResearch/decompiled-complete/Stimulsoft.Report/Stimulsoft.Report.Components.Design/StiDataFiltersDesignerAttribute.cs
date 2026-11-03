using System;

namespace Stimulsoft.Report.Components.Design;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiDataFiltersDesignerAttribute : Attribute
{
	public string DesignerTypeName { get; }

	public StiDataFiltersDesignerAttribute(string designerTypeName)
	{
		DesignerTypeName = designerTypeName;
	}

	public StiDataFiltersDesignerAttribute(Type type)
	{
		DesignerTypeName = type.AssemblyQualifiedName;
	}
}
