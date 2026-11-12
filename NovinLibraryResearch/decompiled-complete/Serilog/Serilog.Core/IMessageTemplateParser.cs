using Serilog.Events;

namespace Serilog.Core;

internal interface IMessageTemplateParser
{
	MessageTemplate Parse(string messageTemplate);
}
