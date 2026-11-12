using System.Globalization;
using System.Text;
using System.Xml;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class Token
{
	private int id;

	private string text;

	private Location startLocation;

	private Location endLocation;

	public string Text => text;

	public int Id => id;

	public string Type
	{
		get
		{
			if (id >= 127 && id < 995)
			{
				return ((Tokens)Id).ToString();
			}
			return ((char)Id).ToString();
		}
	}

	public Location StartLocation => startLocation;

	public Location EndLocation => endLocation;

	public bool IsSignificant => TokenManager.IsSignificantToken(id);

	public string Xml
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.OmitXmlDeclaration = true;
			xmlWriterSettings.Indent = true;
			using (XmlWriter writer = XmlWriter.Create(stringBuilder, xmlWriterSettings))
			{
				WriteXml(writer);
			}
			return stringBuilder.ToString();
		}
	}

	internal Token(int id, string text, Location startLocation, Location endLocation)
	{
		this.id = ((id < 0) ? (~id) : id);
		this.text = text;
		this.startLocation = startLocation;
		this.endLocation = endLocation;
	}

	public void WriteXml(XmlWriter writer)
	{
		if (writer != null)
		{
			writer.WriteStartElement("Token");
			writer.WriteAttributeString("location", string.Format(CultureInfo.CurrentCulture, "({0}, {1})", StartLocation, EndLocation));
			writer.WriteAttributeString("id", Id.ToString(CultureInfo.CurrentCulture));
			writer.WriteAttributeString("type", Type);
			string text = Text;
			text = text.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r")
				.Replace("\t", "\\t");
			writer.WriteString(text);
			writer.WriteEndElement();
		}
	}

	public override string ToString()
	{
		return Xml;
	}
}
