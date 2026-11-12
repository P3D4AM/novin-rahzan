using System;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Base.Licenses;

public abstract class StiLicenseObject
{
	protected virtual string EncryptKey => "rXtkxg№g3o9P@9BfNt#vu";

	protected internal void LoadFromString(string str)
	{
		StiJsonHelper.LoadFromJsonString(str, this);
	}

	protected internal string SaveToString()
	{
		return StiJsonHelper.SaveToJsonString(this);
	}

	protected internal void LoadFromBytes(byte[] bytes)
	{
		string str = StiBytesToStringConverter.ConvertBytesToString(bytes);
		LoadFromString(str);
	}

	protected internal byte[] SaveToBytes()
	{
		string str = SaveToString();
		return StiBytesToStringConverter.ConvertStringToBytes(str);
	}

	protected internal void DecryptFromBytes(byte[] bytes)
	{
		byte[] bytes2 = StiCryptHelper.Decrypt(bytes, EncryptKey);
		string str = StiBytesToStringConverter.ConvertBytesToString(bytes2);
		LoadFromString(str);
	}

	protected internal byte[] EncryptToBytes()
	{
		byte[] bytes = SaveToBytes();
		return StiCryptHelper.Encrypt(bytes, EncryptKey);
	}

	protected internal string EncryptToString()
	{
		byte[] inArray = EncryptToBytes();
		return Convert.ToBase64String(inArray);
	}

	protected internal void DecryptFromString(string str)
	{
		byte[] bytes = Convert.FromBase64String(str);
		DecryptFromBytes(bytes);
	}
}
