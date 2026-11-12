using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Plans;

namespace Stimulsoft.Base.Licenses;

public class StiLicenseKey : StiLicenseObject, ICloneable
{
	[JsonProperty(Order = 1)]
	public DateTime ActivationDate { get; set; }

	[JsonProperty(Order = 2)]
	public string Signature { get; set; }

	[DefaultValue("")]
	[JsonProperty(Order = 3)]
	public string Owner { get; set; }

	[DefaultValue("")]
	[JsonProperty(Order = 4)]
	public string UserName { get; set; }

	[JsonProperty(Order = 6)]
	public DateTime? StartDate { get; set; }

	[JsonProperty(Order = 7)]
	public DateTime? EndDate { get; set; }

	[JsonProperty(Order = 8)]
	public string DeviceId { get; set; }

	[DefaultValue(null)]
	[JsonProperty(Order = 9)]
	public StiPlanIdent? PlanId { get; set; }

	[JsonProperty(Order = 5)]
	public List<StiLicenseProduct> Products { get; set; }

	[JsonProperty(Order = 10)]
	public string ProductName { get; set; }

	[JsonProperty(Order = 11)]
	public byte[] ProductLogo { get; set; }

	[JsonProperty(Order = 12)]
	public byte[] ProductFavIcon { get; set; }

	[JsonProperty(Order = 13)]
	public string ProductDescription { get; set; }

	[JsonProperty(Order = 14)]
	public string ProductUrl { get; set; }

	[JsonIgnore]
	public bool IsServerLicense => PlanId.HasValue;

	[JsonIgnore]
	public bool IsProductLicense
	{
		get
		{
			if (Products != null)
			{
				return Products.Count > 0;
			}
			return false;
		}
	}

	public object Clone()
	{
		StiLicenseKey stiLicenseKey = new StiLicenseKey();
		stiLicenseKey.LoadFromString(SaveToString());
		return stiLicenseKey;
	}

	public byte[] GetCheckBytes()
	{
		StiLicenseKey stiLicenseKey = Clone() as StiLicenseKey;
		stiLicenseKey.Signature = null;
		return stiLicenseKey.SaveToBytes();
	}

	public byte[] GetSignatureBytes()
	{
		return Convert.FromBase64String(Signature);
	}

	public static StiLicenseKey Get(byte[] bytes)
	{
		StiLicenseKey stiLicenseKey = new StiLicenseKey();
		stiLicenseKey.DecryptFromBytes(bytes);
		return stiLicenseKey;
	}

	public static StiLicenseKey Get(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		StiLicenseKey stiLicenseKey = new StiLicenseKey();
		stiLicenseKey.DecryptFromString(str);
		return stiLicenseKey;
	}

	public string GetCSharpCode(int maxSymbols = 100, bool onlyKey = false)
	{
		StringBuilder code = GetCode(maxSymbols, onlyKey);
		code = code.Append(";");
		return code.ToString();
	}

	public string GetVbNetCode(int maxSymbols = 100, bool onlyKey = false)
	{
		return GetCode(maxSymbols, onlyKey).ToString();
	}

	private StringBuilder GetCode(int maxSymbols = 100, bool onlyKey = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!onlyKey)
		{
			stringBuilder = stringBuilder.Append("StiLicense.Key = ");
		}
		int length = stringBuilder.Length;
		bool flag = !onlyKey;
		string text = EncryptToString();
		while (true)
		{
			int num = (flag ? (maxSymbols - length) : maxSymbols);
			if (text.Length <= num)
			{
				break;
			}
			stringBuilder = stringBuilder.AppendFormat("\"{0}\"+\r\n", text.Substring(0, num));
			text = text.Substring(num);
			flag = false;
		}
		return stringBuilder.AppendFormat("\"{0}\"", text);
	}

	public StiLicenseKey()
	{
		ActivationDate = DateTime.Today;
		Owner = string.Empty;
		UserName = string.Empty;
	}
}
