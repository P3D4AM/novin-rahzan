using System;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace EntityCache.Interfaces;

public interface IDb_File
{
	long Crc { get; set; }

	string DateSh { get; }

	FileManagement_Parts FileManagementParts { get; set; }

	Guid Guid { get; set; }

	int Id { get; set; }

	DateTime InserTime { get; set; }

	int MaxChunk { get; set; }

	int MinChunk { get; set; }

	string Name { get; set; }

	int PartCount { get; set; }

	int PartSent { get; set; }

	Priority Priority { get; set; }

	DateTime Register { get; set; }

	short RetryCount { get; set; }

	string SubDirectory { get; set; }

	string UrlPath { get; set; }

	Task<int> GetPartSentCountAsync();

	Task<int> GetTotalPartCountAsync();
}
