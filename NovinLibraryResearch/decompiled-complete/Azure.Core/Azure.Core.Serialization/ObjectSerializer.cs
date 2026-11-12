using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Serialization;

public abstract class ObjectSerializer
{
	public abstract void Serialize(Stream stream, object? value, Type inputType, CancellationToken cancellationToken);

	public abstract ValueTask SerializeAsync(Stream stream, object? value, Type inputType, CancellationToken cancellationToken);

	public abstract object Deserialize(Stream stream, Type returnType, CancellationToken cancellationToken);

	public abstract ValueTask<object> DeserializeAsync(Stream stream, Type returnType, CancellationToken cancellationToken);
}
