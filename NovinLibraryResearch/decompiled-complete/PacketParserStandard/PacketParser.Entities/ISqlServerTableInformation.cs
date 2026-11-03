using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PacketParser.Entities;

public interface ISqlServerTableInformation
{
	long TableId { get; }

	string TableName { get; }

	string SchemaName { get; }

	DateTime CreateDate { get; }

	Task<List<ISqlServerTableInformation>> GetAllAsync(params string[] tableNames);
}
