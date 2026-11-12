using Serilog.Core;
using Serilog.Core.Pipeline;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Capturing;

internal class MessageTemplateProcessor : ILogEventPropertyFactory, ILogEventPropertyValueFactory
{
	private readonly MessageTemplateCache _parser = new MessageTemplateCache(new MessageTemplateParser());

	private readonly PropertyBinder _propertyBinder;

	private readonly PropertyValueConverter _propertyValueConverter;

	public MessageTemplateProcessor(PropertyValueConverter propertyValueConverter)
	{
		_propertyValueConverter = propertyValueConverter;
		_propertyBinder = new PropertyBinder(_propertyValueConverter);
	}

	public void Process(string messageTemplate, object?[] messageTemplateParameters, out MessageTemplate parsedTemplate, out EventProperty[] properties)
	{
		parsedTemplate = _parser.Parse(messageTemplate);
		properties = _propertyBinder.ConstructProperties(parsedTemplate, messageTemplateParameters);
	}

	public LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false)
	{
		return _propertyValueConverter.CreateProperty(name, value, destructureObjects);
	}

	public LogEventPropertyValue CreatePropertyValue(object? value, bool destructureObjects = false)
	{
		return _propertyValueConverter.CreatePropertyValue(value, destructureObjects);
	}
}
