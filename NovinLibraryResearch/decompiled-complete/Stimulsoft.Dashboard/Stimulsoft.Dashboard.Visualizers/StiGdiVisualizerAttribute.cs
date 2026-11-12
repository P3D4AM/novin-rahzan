using System;

namespace Stimulsoft.Dashboard.Visualizers;

[AttributeUsage(AttributeTargets.Class)]
public class StiGdiVisualizerAttribute : Attribute
{
	public string VisualizerTypeName { get; }

	public StiGdiVisualizerAttribute(string visualizerTypeName)
	{
		VisualizerTypeName = visualizerTypeName;
	}

	public StiGdiVisualizerAttribute(Type visualizerType)
	{
		VisualizerTypeName = visualizerType.AssemblyQualifiedName;
	}
}
