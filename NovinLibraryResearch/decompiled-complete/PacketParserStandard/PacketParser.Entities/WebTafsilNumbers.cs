using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Mapping;

namespace PacketParser.Entities;

public class WebTafsilNumbers : ITafsilNumbers, IItemAsyncMapper<WebTafsilNumbers, WebTafsilNumbers>
{
	private string _originalNumber;

	public Guid TafsilGuid { get; set; }

	public long OriginalNumber
	{
		get
		{
			return _originalNumber.ParseToLong(0L);
		}
		set
		{
			_originalNumber = value.ToString();
			ProcessCallerIdNumber(_originalNumber);
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public short Ordinal { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public short Prefix { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool Mobile { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public long CalcedNumber { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsValid
	{
		get
		{
			if (string.IsNullOrEmpty(_originalNumber))
			{
				return false;
			}
			if (Mobile || (OriginalNumber >= 1000000000 && OriginalNumber <= 9999999999L) || (!_originalNumber.StartsWith("0") && OriginalNumber > 10000000 && OriginalNumber <= 99999999))
			{
				return true;
			}
			return false;
		}
	}

	public string Title { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	[Obsolete("please use orginal method", true)]
	public EnCellPhoneOperatorType CellPhoneOperatorType { get; set; }

	public WebTafsilNumbers()
	{
	}

	public WebTafsilNumbers(string number)
	{
		ProcessCallerIdNumber(number);
	}

	public WebTafsilNumbers(long number)
	{
		OriginalNumber = number;
	}

	private void ProcessCallerIdNumber(string calleIdNumber)
	{
		checked
		{
			try
			{
				calleIdNumber = calleIdNumber.RemoveNoNumbers();
				_originalNumber = calleIdNumber;
				while (calleIdNumber.StartsWith("0"))
				{
					calleIdNumber = calleIdNumber.Substring(1, calleIdNumber.Length - 1);
				}
				if (calleIdNumber.Length == 12 && calleIdNumber.StartsWith("98"))
				{
					calleIdNumber = calleIdNumber.Substring(2, calleIdNumber.Length - 2);
				}
				if (calleIdNumber.Length <= 7)
				{
					Prefix = 0;
					CalcedNumber = calleIdNumber.ParseToLong(0L);
					Mobile = false;
				}
				else if (calleIdNumber.Length == 10 && calleIdNumber.Substring(0, 1) == "9")
				{
					CalcedNumber = calleIdNumber.ParseToLong(0L);
					Prefix = 0;
					Mobile = true;
				}
				else
				{
					CalcedNumber = calleIdNumber.Substring(calleIdNumber.Length - 8, 8).ParseToLong(0L);
					Prefix = calleIdNumber.Substring(0, calleIdNumber.Length - 8).ParseToShort(0);
					Mobile = false;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "data:" + calleIdNumber, "ProcessCallerIdNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilNumbers.cs", 88);
			}
		}
	}

	public Task<WebTafsilNumbers> MapAsync(string contextGuid, WebTafsilNumbers source, WebTafsilNumbers destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebTafsilNumbers();
			}
			destination._originalNumber = source._originalNumber;
			destination.TafsilGuid = source.TafsilGuid;
			destination.OriginalNumber = source.OriginalNumber;
			destination.Prefix = source.Prefix;
			destination.Mobile = source.Mobile;
			destination.CalcedNumber = source.CalcedNumber;
			destination.Title = source.Title;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilNumbers.cs", 111);
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebTafsilNumbers>> MapListAsync(string contextGuid, IEnumerable<WebTafsilNumbers> source, List<WebTafsilNumbers> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
