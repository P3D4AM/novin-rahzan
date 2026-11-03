using System;

namespace Stimulsoft.Report.Engine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class StiV2BuilderAttribute : Attribute
{
	public string BuilderTypeName { get; }

	public StiV2BuilderAttribute(string builderTypeName)
	{
		BuilderTypeName = builderTypeName;
	}

	public StiV2BuilderAttribute(Type builderType)
	{
		BuilderTypeName = builderType.AssemblyQualifiedName;
	}
}
