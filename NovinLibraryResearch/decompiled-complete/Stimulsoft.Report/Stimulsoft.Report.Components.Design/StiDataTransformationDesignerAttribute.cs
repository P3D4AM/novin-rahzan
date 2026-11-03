using System;

namespace Stimulsoft.Report.Components.Design;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiDataTransformationDesignerAttribute : Attribute
{
	public string DesignerTypeName { get; }

	public StiDataTransformationDesignerAttribute(string designerTypeName)
	{
		DesignerTypeName = designerTypeName;
	}

	public StiDataTransformationDesignerAttribute(Type type)
	{
		DesignerTypeName = type.AssemblyQualifiedName;
	}
}
