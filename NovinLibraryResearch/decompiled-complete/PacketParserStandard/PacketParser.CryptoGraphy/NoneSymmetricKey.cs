using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace PacketParser.CryptoGraphy;

public static class NoneSymmetricKey
{
	public static string EncryptDataToBase64(string data, string publicKey)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		try
		{
			rSACryptoServiceProvider.FromXmlString(publicKey);
			return Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true));
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	public static string DecriptData(string base64data, string privateKey)
	{
		byte[] rgb = Convert.FromBase64String(base64data);
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		StringReader textReader = new StringReader(privateKey);
		RSAParameters parameters = (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(textReader);
		rSACryptoServiceProvider.ImportParameters(parameters);
		byte[] bytes = rSACryptoServiceProvider.Decrypt(rgb, fOAEP: false);
		return Encoding.Unicode.GetString(bytes);
	}
}
