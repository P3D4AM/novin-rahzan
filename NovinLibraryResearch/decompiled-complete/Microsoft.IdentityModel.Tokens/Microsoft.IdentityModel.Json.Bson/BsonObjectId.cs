using System;
using Microsoft.IdentityModel.Json.Utilities;

namespace Microsoft.IdentityModel.Json.Bson;

[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Microsoft.IdentityModel.Json.Bson for more details.")]
internal class BsonObjectId
{
	public byte[] Value { get; }

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
