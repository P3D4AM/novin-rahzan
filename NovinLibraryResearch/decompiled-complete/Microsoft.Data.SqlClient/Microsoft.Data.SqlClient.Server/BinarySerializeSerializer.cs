using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.SqlServer.Server;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class BinarySerializeSerializer : Serializer
{
	internal BinarySerializeSerializer(Type t)
		: base(t)
	{
	}

	public override void Serialize(Stream s, object o)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		BinaryWriter binaryWriter = new BinaryWriter(s);
		if (o is IBinarySerialize)
		{
			((IBinarySerialize)o).Write(binaryWriter);
		}
		else
		{
			((IBinarySerialize)o).Write(binaryWriter);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override object Deserialize(Stream s)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		object obj = Activator.CreateInstance(_type);
		BinaryReader binaryReader = new BinaryReader(s);
		if (obj is IBinarySerialize)
		{
			((IBinarySerialize)obj).Read(binaryReader);
		}
		else
		{
			((IBinarySerialize)obj).Read(binaryReader);
		}
		return obj;
	}
}
