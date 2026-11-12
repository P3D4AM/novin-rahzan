using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
internal sealed class StringFormatMethodAttribute : Attribute
{
	[JetBrains.Annotations.NotNull]
	public string FormatParameterName { get; }

	public StringFormatMethodAttribute([JetBrains.Annotations.NotNull] string formatParameterName)
	{
		FormatParameterName = formatParameterName;
	}
}
