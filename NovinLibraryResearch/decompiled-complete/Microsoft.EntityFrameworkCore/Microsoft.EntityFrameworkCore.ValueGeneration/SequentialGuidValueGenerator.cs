using System;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class SequentialGuidValueGenerator : ValueGenerator<Guid>
{
	private long _counter = DateTime.UtcNow.Ticks;

	public override bool GeneratesTemporaryValues => false;

	public override Guid Next(EntityEntry entry)
	{
		byte[] array = Guid.NewGuid().ToByteArray();
		byte[] bytes = BitConverter.GetBytes(Interlocked.Increment(ref _counter));
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes);
		}
		array[8] = bytes[1];
		array[9] = bytes[0];
		array[10] = bytes[7];
		array[11] = bytes[6];
		array[12] = bytes[5];
		array[13] = bytes[4];
		array[14] = bytes[3];
		array[15] = bytes[2];
		return new Guid(array);
	}
}
