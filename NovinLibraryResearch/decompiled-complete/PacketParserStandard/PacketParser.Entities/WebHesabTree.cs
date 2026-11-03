using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.DefaultsCoding;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebHesabTree : Serializable<WebHesabTree>, IHesabTree, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private List<WebGardeshTafsili> _GardeshTafsili;

	private string kolName;

	private string name;

	private string moeinName;

	private Guid _tafsilLevelGuid = Guid.Empty;

	public int Id { get; set; }

	[DefaultValue(HesabType.All)]
	public HesabType HType { get; set; }

	[DefaultValue(HesabType.All)]
	public HesabType A1
	{
		get
		{
			return HType;
		}
		set
		{
			HType = value;
		}
	}

	public int HId { get; set; }

	[DefaultValue(ENHesabLevel.Tafsil)]
	public ENHesabLevel Level { get; set; } = ENHesabLevel.Tafsil;


	public string Code { get; set; } = "";


	public string FullCode { get; set; } = "";


	public int Parent { get; set; }

	public bool IsUpdated { get; set; }

	public int KolId { get; set; }

	public int MoeinId { get; set; }

	public int TafsilId { get; set; }

	public string Name
	{
		get
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "نام حساب";
			}
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string ParentName { get; set; } = "";


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public bool Special { get; set; }

	public string KolName
	{
		get
		{
			if (string.IsNullOrEmpty(kolName))
			{
				kolName = "حساب کل";
			}
			return kolName;
		}
		set
		{
			kolName = value;
		}
	}

	public string MoeinName
	{
		get
		{
			if (string.IsNullOrEmpty(moeinName))
			{
				moeinName = "حساب معین";
			}
			return moeinName;
		}
		set
		{
			moeinName = value;
		}
	}

	[DefaultValue(ENMahiat.Debit_Credit)]
	public ENMahiat Mahiat { get; set; } = ENMahiat.Debit_Credit;


	public string MahiatName { get; set; } = "";


	public string CodeKol { get; set; } = "";


	public string CodeMoein { get; set; } = "";


	public string CodeTafsil { get; set; }

	public decimal Account { get; set; }

	public short TafsilLevel { get; set; }

	public bool Tedadi { get; set; }

	public Guid Guid { get; set; } = Guid.Empty;


	public DateTime Modified { get; set; }

	public short GroupId { get; set; }

	public int VisitorId { get; set; }

	public Guid KolGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid TafsilGroupGuid
	{
		get
		{
			if (_tafsilLevelGuid != Guid.Empty)
			{
				return _tafsilLevelGuid;
			}
			return HType switch
			{
				HesabType.Customers => ParentDefaults.TafsilLevelGuids.PersonsAndCompany, 
				HesabType.Bank => ParentDefaults.TafsilLevelGuids.Banks, 
				HesabType.Sandogh => ParentDefaults.TafsilLevelGuids.Sandooghs, 
				HesabType.Bardasht => ParentDefaults.TafsilLevelGuids.Bardashts, 
				HesabType.Hazine => ParentDefaults.TafsilLevelGuids.Hazineha, 
				_ => ParentDefaults.TafsilLevelGuids.Others, 
			};
		}
		set
		{
			_tafsilLevelGuid = value;
		}
	}

	public Guid VisitorGuid { get; set; }

	public Guid ParentGuid { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public bool Peygiri { get; set; }

	public bool System { get; set; }

	public List<WebGardeshTafsili> GardeshTafsili
	{
		get
		{
			return _GardeshTafsili?.ToList();
		}
		set
		{
			try
			{
				decimal rem = default(decimal);
				if (value != null)
				{
					foreach (WebGardeshTafsili item in value)
					{
						rem += item.Credit - item.Debit;
						item.Rem = rem;
					}
				}
				_GardeshTafsili = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GardeshTafsili", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 197);
			}
		}
	}

	public List<WebGardeshTafsili> D1
	{
		get
		{
			return GardeshTafsili?.ToList();
		}
		set
		{
			GardeshTafsili = value;
		}
	}

	public Guid GroupGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public string SerializedData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; } = ServerStatusForClient.Delivered;


	public event Func<object, HesabTreeReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebHesabTree>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebHesabTree>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebHesabTree>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				HesabTreeReadEventArgs hesabTreeReadEventArgs = new HesabTreeReadEventArgs
				{
					Modified = modified
				};
				list.Add(hesabTreeReadEventArgs.ToString());
				return new Packet(PacketType.HesabTree, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 246);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebHesabTree> items, bool isSync, int partitionId)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs"))
		{
			try
			{
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.HesabTree, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 265);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebHesabTree> hesabs, short partition)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs"))
		{
			try
			{
				if (hesabs == null || (hesabs != null && hesabs.Count() == 0))
				{
					throw new NullReferenceException();
				}
				string text = hesabs.Select((WebHesabTree p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (hesabs == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = hesabs.GeneratePackets(PacketType.HesabTree, 1, null, isSync: false, null, partition, deliveryRequst: true, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 286);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses, Guid RequestId)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs"))
		{
			try
			{
				if (responses == null || (responses != null && responses.Count() == 0))
				{
					throw new NullReferenceException();
				}
				string extraDataForDebug = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				return responses?.GeneratePackets(PacketType.HesabTree, 2, null, isSync: false, RequestId, 0, deliveryRequst: false, extraDataForDebug);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 306);
				throw;
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((HesabActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 342);
			return $"Error({action})";
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			HesabActions hesabActions = (HesabActions)action;
			return hesabActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 355);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs"))
		{
			_ = 4;
			try
			{
				switch ((HesabActions)pack.PacketActions)
				{
				case HesabActions.SaveRequest:
				{
					ConnectionEventArgs<WebHesabTree> args3 = new ConnectionEventArgs<WebHesabTree>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs");
					break;
				}
				case HesabActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs");
					break;
				}
				case HesabActions.ReadRequest:
				{
					HesabTreeReadEventArgs hesabTreeReadEventArgs = pack.Args[1].FromJson<HesabTreeReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 378);
					hesabTreeReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					hesabTreeReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, hesabTreeReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs");
					break;
				}
				case HesabActions.ReadResponse:
				{
					ConnectionEventArgs<WebHesabTree> args = new ConnectionEventArgs<WebHesabTree>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs");
						}
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 394);
						break;
					}
				}
				case (HesabActions)3:
				case (HesabActions)4:
				case HesabActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 416);
			}
		}
	}

	public new void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
				D1 = null;
				GardeshTafsili = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Web_Hesabtree.cs", 441);
		}
	}
}
