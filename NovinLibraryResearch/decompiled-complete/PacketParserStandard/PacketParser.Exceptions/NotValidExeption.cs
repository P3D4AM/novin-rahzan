using System;
using System.Collections.Generic;

namespace PacketParser.Exceptions;

public class NotValidExeption : Exception
{
	public List<string> ValidationErrors { get; set; }

	internal NotValidExeption()
	{
		ValidationErrors = new List<string>();
	}

	internal NotValidExeption(string message)
		: base(message)
	{
		ValidationErrors = new List<string>();
	}

	internal NotValidExeption(string message, Exception inner)
		: base(message, inner)
	{
		ValidationErrors = new List<string>();
	}
}
