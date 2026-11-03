using System;

namespace Stimulsoft.Report.Components.Design;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiDesignerAttribute : Attribute
{
	public string DesignerTypeName { get; }

	public StiDesignerAttribute(string designerTypeName)
	{
		DesignerTypeName = designerTypeName;
	}

	public StiDesignerAttribute(Type type)
	{
		DesignerTypeName = type.AssemblyQualifiedName;
	}
}
