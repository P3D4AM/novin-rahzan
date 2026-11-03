using Newtonsoft.Json;

namespace PacketParser.BusinessLayer.Models;

public class ShabaNumberModel
{
	public string ShabaNumber { get; set; }

	[JsonIgnore]
	public string Shaba2
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 4)
			{
				return ShabaNumber.Substring(2, 2);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba3
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 7)
			{
				return ShabaNumber.Substring(4, 3);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba4
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 8)
			{
				return ShabaNumber.Substring(7, 1);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba5
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 12)
			{
				return ShabaNumber.Substring(8, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba6
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 16)
			{
				return ShabaNumber.Substring(12, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba7
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 20)
			{
				return ShabaNumber.Substring(16, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba8
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 24)
			{
				return ShabaNumber.Substring(20, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Shaba9
	{
		get
		{
			if (!string.IsNullOrEmpty(ShabaNumber) && ShabaNumber.Length >= 26)
			{
				return ShabaNumber.Substring(24, 2);
			}
			return "";
		}
	}
}
