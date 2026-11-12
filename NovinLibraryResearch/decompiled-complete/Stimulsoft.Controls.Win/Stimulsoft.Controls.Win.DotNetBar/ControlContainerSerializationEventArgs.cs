using System;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ControlContainerSerializationEventArgs : EventArgs
{
	private readonly XmlElement xmlstore;

	public XmlElement XmlStorage => xmlstore;

	public ControlContainerSerializationEventArgs(XmlElement xmlstorage)
	{
		xmlstore = xmlstorage;
	}
}
