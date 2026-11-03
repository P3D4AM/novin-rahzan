using System;

namespace PacketParser.Entities;

public class WebPrintTemplateMaping : Serializable<WebPrintTemplateMaping>
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

	public PrintType PrintType { get; set; }

	public PrintFormat PrintFormat { get; set; }

	public int UserId { get; set; }

	public Guid TemplateId { get; set; }

	public string TemplateName { get; set; }

	public int Delay { get; set; }

	public ConnectoinType ConnectoinType { get; set; }

	public string ConnectionValue { get; set; }

	public string PPrintType
	{
		get
		{
			return PrintType.GetDisplay();
		}
		set
		{
		}
	}

	public string PPrintFormat
	{
		get
		{
			return PrintFormat.GetDisplay();
		}
		set
		{
		}
	}

	public string PersianPrintType
	{
		get
		{
			return PrintType.GetDisplay();
		}
		set
		{
		}
	}
}
