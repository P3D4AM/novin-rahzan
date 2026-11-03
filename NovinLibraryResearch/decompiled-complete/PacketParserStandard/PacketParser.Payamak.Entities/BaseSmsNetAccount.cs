using System;
using PacketParser.Entities;

namespace PacketParser.Payamak.Entities;

public class BaseSmsNetAccount : ISmsNetAccount
{
	public EnSmsSenderType Type { get; set; }

	public decimal GsmCrecit { get; set; }

	public DateTime? GsmUpdate { get; set; }

	public short GsmMaxSent { get; set; }

	public bool IsChecked { get; set; }

	public short St { get; set; }

	public string Description { get; set; }

	public string Username { get; set; }

	public int Id { get; set; }

	public string Title { get; set; }

	public string Password { get; set; }

	public bool GSM_Connection { get; set; }

	public bool GSM_On_Sendeng_Message { get; set; }

	public bool GSMPDUMode { get; set; }

	public int GSMNO { get; set; }

	public string GSMSignal { get; set; }

	public int GSMSendedSmsCount { get; set; }

	public virtual decimal? Account { get; }

	public virtual string Domain { get; set; }

	public virtual string FullDomain
	{
		get
		{
			if (!Domain.ToLower().Contains(".ir") && !Domain.ToLower().Contains(".com"))
			{
				return Domain + ".com";
			}
			return Domain;
		}
		set
		{
		}
	}

	public virtual string SenderNumber { get; set; }

	public SmsNetAccountMetaData MetaData { get; set; }
}
