using System;
using System.IO;

namespace Microsoft.Data.SqlClient.Server;

internal abstract class Serializer
{
	protected Type _type;

	public abstract object Deserialize(Stream s);

	public abstract void Serialize(Stream s, object o);

	protected Serializer(Type t)
	{
		_type = t;
	}
}
