using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Connection.Class;

namespace TimeAttendanceDAL.Connection.BackGroundServices;

public abstract class BaseBackGroundService : IHasContext
{
	public ConnectionManager ConnectionManager { get; set; }

	public string ContextGuid { get; set; }

	public abstract Task InitializeAsync();

	protected BaseBackGroundService(string contextGuid, ConnectionManager manager)
	{
		ContextGuid = contextGuid;
		ConnectionManager = manager;
	}
}
