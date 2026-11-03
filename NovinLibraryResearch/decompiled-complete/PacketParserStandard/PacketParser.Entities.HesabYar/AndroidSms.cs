using System;
using PacketParser.Entities.Interfaces.HesabYar;

namespace PacketParser.Entities.HesabYar;

public class AndroidSms : IAndoidSmsEntity
{
	private string message;

	private string sender;

	private const string dontLoseStarChar = "aasdasdf";

	public DateTime ProcessDate { get; set; } = DateTime.Now;


	public Guid Guid { get; set; }

	public string Sender
	{
		get
		{
			return sender;
		}
		set
		{
			sender = value.FixString().ToLower().Trim();
		}
	}

	public string Message
	{
		get
		{
			return message;
		}
		set
		{
			message = value.Replace("*", "aasdasdf").FixString().Replace("aasdasdf", "*")
				.KeepCharactersForSmsParser();
		}
	}

	public DateTime UtcTime { get; set; }
}
