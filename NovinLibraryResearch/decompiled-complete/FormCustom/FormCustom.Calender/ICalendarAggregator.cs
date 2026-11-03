using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Payamak.Entities;

namespace FormCustom.Calender;

public interface ICalendarAggregator
{
	Task<List<Guid>> GetCustomerGuidListAsync(string contextGuid, List<Guid> selectedGuidList, IWin32Window owner, CancellationToken token);

	Task ShowReportAsync(string contextGuid, IEnumerable<BaseSmsSent> _list, IWin32Window owner);
}
