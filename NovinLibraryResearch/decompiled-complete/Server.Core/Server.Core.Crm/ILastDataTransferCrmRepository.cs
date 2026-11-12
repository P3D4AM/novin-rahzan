using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.CRM;

namespace Server.Core.Crm;

public interface ILastDataTransferCrmRepository
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebLastDataTransferCrm>>> GetDataAsync(Guid tafsilGuid, int counts, DateTime PassedDate);
}
