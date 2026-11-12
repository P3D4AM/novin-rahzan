using System;

namespace Stimulsoft.Dashboard.Visualizers;

[AttributeUsage(AttributeTargets.Class)]
public class StiWpfVisualizerAttribute : Attribute
{
	public string VisualizerTypeName { get; }

	public StiWpfVisualizerAttribute(string visualizerTypeName)
	{
		VisualizerTypeName = visualizerTypeName;
	}

	public StiWpfVisualizerAttribute(Type visualizerType)
	{
		VisualizerTypeName = visualizerType.AssemblyQualifiedName;
	}
}
