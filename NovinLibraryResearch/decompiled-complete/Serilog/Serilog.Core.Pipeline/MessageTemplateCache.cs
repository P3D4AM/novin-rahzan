using System.Collections;
using Serilog.Events;

namespace Serilog.Core.Pipeline;

internal class MessageTemplateCache : IMessageTemplateParser
{
	private readonly IMessageTemplateParser _innerParser;

	private readonly object _templatesLock = new object();

	private readonly Hashtable _templates = new Hashtable(ByReferenceStringComparer.Instance);

	private const int MaxCacheItems = 1000;

	private const int MaxCachedTemplateLength = 1024;

	public MessageTemplateCache(IMessageTemplateParser innerParser)
	{
		_innerParser = Guard.AgainstNull(innerParser, "innerParser");
	}

	public MessageTemplate Parse(string messageTemplate)
	{
		Guard.AgainstNull(messageTemplate, "messageTemplate");
		if (messageTemplate.Length > 1024)
		{
			return _innerParser.Parse(messageTemplate);
		}
		MessageTemplate messageTemplate2 = (MessageTemplate)_templates[messageTemplate];
		if (messageTemplate2 != null)
		{
			return messageTemplate2;
		}
		messageTemplate2 = _innerParser.Parse(messageTemplate);
		lock (_templatesLock)
		{
			if (_templates.Count == 1000)
			{
				_templates.Clear();
			}
			_templates[messageTemplate] = messageTemplate2;
			return messageTemplate2;
		}
	}
}
