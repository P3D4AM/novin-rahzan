using System;
using Stimulsoft.Base.Json.Utilities;

namespace Stimulsoft.Base.Json.Bson;

public class BsonObjectId
{
	public byte[] Value { get; private set; }

	public BsonObjectId(byte[] value)
	{
		ValidationUtils.ArgumentNotNull(value, "value");
		if (value.Length != 12)
		{
			throw new ArgumentException("An ObjectId must be 12 bytes", "value");
		}
		Value = value;
	}
}
