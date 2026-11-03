using System;

namespace PacketParser.Entities;

public class WebPrintTemplate : Serializable<WebPrintTemplate>
{
	private Guid _guid = Guid.Empty;

	public Guid Guid
	{
		get
		{
			if (_guid == Guid.Empty)
			{
				_guid = Guid.NewGuid();
			}
			return _guid;
		}
		set
		{
			_guid = value;
		}
	}

	public string Name { get; set; }

	public string Html { get; set; }

	public PrintType PrintType { get; set; }

	public PrintFormat PrintFormat { get; set; }
}
