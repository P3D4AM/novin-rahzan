using System.IO;

namespace Microsoft.Data.SqlClient.Server;

public interface IBinarySerialize
{
	void Read(BinaryReader r);

	void Write(BinaryWriter w);
}
