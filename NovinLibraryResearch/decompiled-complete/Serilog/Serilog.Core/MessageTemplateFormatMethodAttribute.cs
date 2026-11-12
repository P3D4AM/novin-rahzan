using System;

namespace Serilog.Core;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
public sealed class MessageTemplateFormatMethodAttribute : Attribute
{
	public string MessageTemplateParameterName { get; private set; }

	public MessageTemplateFormatMethodAttribute(string messageTemplateParameterName)
	{
		MessageTemplateParameterName = messageTemplateParameterName;
	}
}
