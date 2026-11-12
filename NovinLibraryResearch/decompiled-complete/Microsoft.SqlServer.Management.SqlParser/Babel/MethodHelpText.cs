using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Babel;

public class MethodHelpText
{
	private string name;

	private string description;

	private string type;

	private bool shouldShowParentheses;

	private bool isVarArg;

	private IList<Parameter> parameters;

	public string Name => name;

	public string Description => description;

	public string Type => type;

	public bool ShouldShowParentheses => shouldShowParentheses;

	public bool IsVarArg => isVarArg;

	public IList<Parameter> Parameters => parameters;

	public MethodHelpText(string name, string description, string type, bool shouldShowParentheses, bool isVarArg)
	{
		this.name = name;
		this.description = description;
		this.type = type;
		this.shouldShowParentheses = shouldShowParentheses;
		this.isVarArg = isVarArg;
		parameters = new List<Parameter>();
	}

	public string ToXml()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
		{
			WriteXml(xmlWriter);
		}
		return stringWriter.ToString();
	}

	internal void WriteXml(XmlWriter xmlWriter)
	{
		xmlWriter.WriteStartElement("Method");
		xmlWriter.WriteElementString("Name", name);
		xmlWriter.WriteElementString("Description", Description);
		xmlWriter.WriteElementString("Type", type);
		if (parameters != null)
		{
			foreach (Parameter parameter in parameters)
			{
				parameter.WriteXml(xmlWriter);
			}
		}
		xmlWriter.WriteEndElement();
	}
}
