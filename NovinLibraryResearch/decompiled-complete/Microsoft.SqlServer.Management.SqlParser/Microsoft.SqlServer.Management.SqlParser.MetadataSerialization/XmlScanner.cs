using System;
using System.Xml;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class XmlScanner
{
	private readonly XmlReader reader;

	private bool openedEmptyElement;

	private bool endOfChildren;

	private bool contentRead;

	private XmlToken nameToken;

	private bool nameTokenRead;

	public bool IsEndOfFile => reader.ReadState == ReadState.EndOfFile;

	public XmlNodeType NodeType => reader.NodeType;

	public bool IsTextNodeType
	{
		get
		{
			if (NodeType != XmlNodeType.Text)
			{
				return NodeType == XmlNodeType.CDATA;
			}
			return true;
		}
	}

	public bool IsEmptyElement => reader.IsEmptyElement;

	public bool IsEndOfChildren => endOfChildren;

	public string Name => reader.Name;

	public XmlToken NameToken
	{
		get
		{
			if (!nameTokenRead)
			{
				nameToken = XmlTokenLookup.Find(reader.LocalName);
				nameTokenRead = true;
			}
			return nameToken;
		}
	}

	public string Namespace => reader.NamespaceURI;

	public string Value => reader.Value;

	public XmlScanner(XmlReader reader)
	{
		this.reader = reader;
		MoveToNextToken();
	}

	public bool MoveToFirstChild()
	{
		if (IsEmptyElement)
		{
			openedEmptyElement = true;
			endOfChildren = true;
		}
		else
		{
			MoveToNextToken();
			endOfChildren = NodeType == XmlNodeType.EndElement;
		}
		ResetXmlTokens();
		return !endOfChildren;
	}

	public bool MoveToNext()
	{
		if (contentRead)
		{
			contentRead = false;
			MoveToContentToken();
		}
		else
		{
			MoveToNextToken();
		}
		endOfChildren = NodeType == XmlNodeType.EndElement;
		ResetXmlTokens();
		return !endOfChildren;
	}

	public void MoveUp()
	{
		endOfChildren = false;
		openedEmptyElement = false;
		ResetXmlTokens();
	}

	public string GetAttribute(XmlQualifiedName qName)
	{
		return reader.GetAttribute(qName.Name, qName.Namespace);
	}

	public string ReadContentAsString()
	{
		string result = reader.ReadContentAsString();
		contentRead = true;
		return result;
	}

	public bool MatchElement(XmlQualifiedNameEx qName)
	{
		return MatchElement(qName, throwOnError: false);
	}

	public bool MatchElement(XmlQualifiedNameEx qName, bool throwOnError)
	{
		bool num = NodeType == XmlNodeType.Element && NameToken == qName.NameToken && Namespace.Equals(qName.Namespace, StringComparison.Ordinal);
		if (!num && throwOnError)
		{
			throw new InvalidXmlException(SR.DeserializationInvalidXmlElement2(Name, qName.ToString()));
		}
		return num;
	}

	private void MoveToNextToken()
	{
		reader.Read();
		MoveToContentToken();
	}

	private void ResetXmlTokens()
	{
		nameTokenRead = false;
	}

	private void MoveToContentToken()
	{
		while (true)
		{
			switch (reader.ReadState)
			{
			case ReadState.Interactive:
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element || (uint)(nodeType - 3) <= 1u || nodeType == XmlNodeType.EndElement)
				{
					return;
				}
				break;
			}
			case ReadState.EndOfFile:
				return;
			default:
				throw new Exception();
			}
			reader.Read();
		}
	}
}
