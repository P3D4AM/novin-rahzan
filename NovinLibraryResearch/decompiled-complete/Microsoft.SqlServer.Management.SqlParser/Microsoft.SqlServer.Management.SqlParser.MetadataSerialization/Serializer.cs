using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class Serializer
{
	public static void SerializeToCode(IServer server, SerializationFilter filter, CodeGeneratorSettings settings)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (filter == null)
		{
			throw new ArgumentNullException("filter");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		CodeMetadataGenerator.GenerateCode(MetadataSerializer.Serialize(server, filter), settings);
	}
}
