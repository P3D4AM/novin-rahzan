using System;
using System.Collections;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Extensions.Logging;

internal class CachingMessageTemplateParser
{
	private readonly MessageTemplateParser _innerParser = new MessageTemplateParser();

	private readonly object _templatesLock = new object();

	private readonly Hashtable _templates = new Hashtable();

	private const int MaxCacheItems = 1000;

	private const int MaxCachedTemplateLength = 1024;

	public MessageTemplate Parse(string messageTemplate)
	{
		if (messageTemplate == null)
		{
			throw new ArgumentNullException("messageTemplate");
		}
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
