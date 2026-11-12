using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.CRM;

namespace Server.Core.Crm;

public interface ILastErrorReportCrmRepository
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebLastErrorReportCrm>>> GetDataAsync(Guid tafsilGuid, int counts, DateTime PassedDate);
}
