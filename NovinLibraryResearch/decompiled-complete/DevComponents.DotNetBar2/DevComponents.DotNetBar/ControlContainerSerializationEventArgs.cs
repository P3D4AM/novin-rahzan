using System;
using System.Xml;

namespace DevComponents.DotNetBar;

public class ControlContainerSerializationEventArgs : EventArgs
{
	private readonly XmlElement _๕;

	public XmlElement XmlStorage => _๕;

	public ControlContainerSerializationEventArgs(XmlElement xmlstorage)
	{
		_๕ = xmlstorage;
	}
}
