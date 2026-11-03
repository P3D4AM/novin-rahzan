using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Events;

public class BusinessEventsAggregator
{
	public TafsilEventAggregator Tafsil { get; private set; }

	public CusCompactEventAggregator CusCompact { get; private set; }

	public ChequeMEventAggregator ChequM { get; private set; }

	public ChequeShEventAggregator ChequeSh { get; set; }

	public OrderEventAggregator Order { get; private set; }

	public NoteEventAggregator Note { get; private set; }

	public AghsatMEventAggregator AghsatM { get; private set; }

	public AghsatShEventAggregator AghsatSh { get; private set; }

	public SanadEventAggregator Sanad { get; private set; }

	public MoeinEventAggregator Moein { get; private set; }

	public AttPersonLeaveEventAggregator PersonLeave { get; private set; }

	public BusinessEventsAggregator(IEnumerable<EnCacheConfig> configs)
	{
		if (configs != null && configs.Any((EnCacheConfig x) => x == EnCacheConfig.Global))
		{
			Tafsil = new TafsilEventAggregator();
			CusCompact = new CusCompactEventAggregator();
			ChequM = new ChequeMEventAggregator();
			ChequeSh = new ChequeShEventAggregator();
			Order = new OrderEventAggregator();
			Note = new NoteEventAggregator();
			AghsatM = new AghsatMEventAggregator();
			AghsatSh = new AghsatShEventAggregator();
			Sanad = new SanadEventAggregator();
			Moein = new MoeinEventAggregator();
		}
		if (configs != null && configs.Any((EnCacheConfig x) => x == EnCacheConfig.Attendance))
		{
			PersonLeave = new AttPersonLeaveEventAggregator();
		}
	}
}
