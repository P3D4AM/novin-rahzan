using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Babel;

public class MethodNameAndParamLocations
{
	private string name;

	private Location nameStartLocation;

	private Location nameEndLocation;

	private Location? paramStartLocation;

	private List<Location> paramSeperatorLocations;

	private Location? paramEndLocation;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public Location NameStartLocation
	{
		get
		{
			return nameStartLocation;
		}
		set
		{
			nameStartLocation = value;
		}
	}

	public Location NameEndLocation
	{
		get
		{
			return nameEndLocation;
		}
		set
		{
			nameEndLocation = value;
		}
	}

	public Location? ParamStartLocation
	{
		get
		{
			return paramStartLocation;
		}
		set
		{
			paramStartLocation = value;
		}
	}

	public List<Location> ParamSeperatorLocations
	{
		get
		{
			return paramSeperatorLocations;
		}
		set
		{
			paramSeperatorLocations = value;
		}
	}

	public Location? ParamEndLocation
	{
		get
		{
			return paramEndLocation;
		}
		set
		{
			paramEndLocation = value;
		}
	}

	public MethodNameAndParamLocations(string name)
	{
		this.name = name;
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

	internal bool IsLocationInArgumentList(Location testLocation)
	{
		bool num = paramStartLocation.HasValue && testLocation.CompareTo(paramStartLocation.Value) >= 0;
		bool flag = !ParamEndLocation.HasValue || testLocation.CompareTo(paramEndLocation.Value) <= 0;
		return num && flag;
	}

	internal void WriteXml(XmlWriter xmlWriter)
	{
		xmlWriter.WriteStartElement("MethodNameAndParamLocations");
		xmlWriter.WriteElementString("Name", Name);
		xmlWriter.WriteElementString("NameStartLocation", NameStartLocation.ToString());
		xmlWriter.WriteElementString("NameEndLocation", NameEndLocation.ToString());
		xmlWriter.WriteElementString("ParamStartLocation", ParamStartLocation.ToString());
		foreach (Location paramSeperatorLocation in ParamSeperatorLocations)
		{
			xmlWriter.WriteElementString("ParamSeperatorLocation", paramSeperatorLocation.ToString());
		}
		xmlWriter.WriteElementString("ParamEndLocation", ParamEndLocation.ToString());
		xmlWriter.WriteEndElement();
	}
}
