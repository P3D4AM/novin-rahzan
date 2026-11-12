using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.Cache;

internal static class AdalCacheOperations
{
	private const int SchemaVersion = 3;

	private const string Delimiter = ":::";

	public static byte[] Serialize(ICoreLogger logger, IDictionary<AdalTokenCacheKey, AdalResultWrapper> tokenCacheDictionary)
	{
		using Stream stream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(stream);
		binaryWriter.Write(3);
		logger.Info(string.Format(CultureInfo.CurrentCulture, "Serializing token cache with {0} items.", tokenCacheDictionary.Count));
		binaryWriter.Write(tokenCacheDictionary.Count);
		foreach (KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> item in tokenCacheDictionary)
		{
			binaryWriter.Write(string.Format(CultureInfo.InvariantCulture, "{1}{0}{2}{0}{3}{0}{4}", ":::", item.Key.Authority, item.Key.Resource, item.Key.ClientId, (int)item.Key.TokenSubjectType));
			binaryWriter.Write(item.Value.Serialize());
		}
		int count = (int)stream.Position;
		stream.Position = 0L;
		return new BinaryReader(stream).ReadBytes(count);
	}

	public static IDictionary<AdalTokenCacheKey, AdalResultWrapper> Deserialize(ICoreLogger logger, byte[] state)
	{
		IDictionary<AdalTokenCacheKey, AdalResultWrapper> dictionary = new Dictionary<AdalTokenCacheKey, AdalResultWrapper>();
		if (state == null || state.Length == 0)
		{
			return dictionary;
		}
		using Stream stream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(stream);
		binaryWriter.Write(state);
		binaryWriter.Flush();
		stream.Position = 0L;
		BinaryReader binaryReader = new BinaryReader(stream);
		if (binaryReader.ReadInt32() != 3)
		{
			logger.Warning("The version of the persistent state of the cache does not match the current schema, so skipping deserialization.");
			return dictionary;
		}
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string[] array = binaryReader.ReadString().Split(new string[1] { ":::" }, StringSplitOptions.None);
			AdalResultWrapper adalResultWrapper = AdalResultWrapper.Deserialize(binaryReader.ReadString());
			AdalTokenCacheKey key = new AdalTokenCacheKey(array[0], array[1], array[2], (TokenSubjectType)int.Parse(array[3], CultureInfo.CurrentCulture), adalResultWrapper.Result.UserInfo);
			dictionary[key] = adalResultWrapper;
		}
		logger.Info(string.Format(CultureInfo.CurrentCulture, "Deserialized {0} items to token cache.", num));
		return dictionary;
	}
}
