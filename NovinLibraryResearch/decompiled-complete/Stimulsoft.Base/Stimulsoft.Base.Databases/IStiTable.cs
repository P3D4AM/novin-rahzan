using System;

namespace Stimulsoft.Base.Databases;

public interface IStiTable
{
	string Name { get; }

	Type ElementType { get; }

	StiDatabaseType? DatabaseType { get; }
}
