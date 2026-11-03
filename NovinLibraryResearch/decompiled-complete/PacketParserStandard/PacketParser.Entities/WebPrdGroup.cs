using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;

namespace PacketParser.Entities;

[Serializable]
public class WebPrdGroup : Serializable<WebPrdGroup>, IPrdGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, IDeliverableEntity
{
	private string _name;

	private string _description;

	private string _techInfoData;

	private string _pic;

	private string _htmlQuick;

	private string _htmlFull;

	private string _deliveryMessage;

	private string _deliveryShortMessage;

	private string _hostRefrenced;

	public short Id { get; set; }

	public short ParentId { get; set; }

	public string Name
	{
		get
		{
			return _name?.FixString();
		}
		set
		{
			_name = value;
		}
	}

	public string ShortName { get; set; }

	public string Description
	{
		get
		{
			return _description?.FixString();
		}
		set
		{
			_description = value;
		}
	}

	public int Level { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[JsonIgnore]
	public PrdGroupTechnicalInformations TechInfo
	{
		get
		{
			using (new JaegerMonitor("TechInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
			{
				try
				{
					return PrdGroupTechnicalInformations.FromString(TechInfoData);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "TechInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 46);
					return null;
				}
			}
		}
	}

	public string TechInfoData
	{
		get
		{
			return _techInfoData?.FixString();
		}
		set
		{
			_techInfoData = value;
		}
	}

	public Guid Guid { get; set; }

	[Obsolete("please use fileInfo by entityType and entityGuid", false)]
	public string Pic
	{
		get
		{
			return _pic?.FixString();
		}
		set
		{
			_pic = value;
		}
	}

	public string HtmlQuick
	{
		get
		{
			return _htmlQuick?.FixString();
		}
		set
		{
			_htmlQuick = value;
		}
	}

	public string HtmlFull
	{
		get
		{
			return _htmlFull?.FixString();
		}
		set
		{
			_htmlFull = value;
		}
	}

	[DefaultValue(0)]
	public long Code { get; set; }

	public long FullCode { get; set; }

	public bool Special { get; set; }

	public DateTime Modified { get; set; }

	public Guid? ParentGuid { get; set; }

	public DateTime ResetTime { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public string Slug { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage
	{
		get
		{
			return _deliveryMessage?.FixString();
		}
		set
		{
			_deliveryMessage = value;
		}
	}

	public string DeliveryShortMessage
	{
		get
		{
			return _deliveryShortMessage?.FixString();
		}
		set
		{
			_deliveryShortMessage = value;
		}
	}

	public string HostRefrenced
	{
		get
		{
			return _hostRefrenced?.FixString();
		}
		set
		{
			_hostRefrenced = value;
		}
	}

	public EnProductType Type { get; set; }

	public event Func<object, PrdGroupReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebPrdGroup>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebPrdGroup>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebPrdGroup>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeleteRequest;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				PrdGroupReadEventArgs prdGroupReadEventArgs = new PrdGroupReadEventArgs
				{
					Modified = modified
				};
				list.Add(prdGroupReadEventArgs.ToString());
				return new Packet(PacketType.ProductGroup, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 84);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<IPrdGroup> items, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			try
			{
				if (items == null || (items != null && items.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return items?.GeneratePackets(PacketType.ProductGroup, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 99);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> groups)
	{
		if (groups == null || !groups.Any())
		{
			throw new NullReferenceException("ProductGroups is null or empty!!!");
		}
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			try
			{
				return groups.GeneratePackets(PacketType.ProductGroup, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 113);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebPrdGroup> groups)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			if (groups == null || !groups.Any())
			{
				throw new NullReferenceException("Group list is null or empty!");
			}
			try
			{
				return groups.GeneratePackets(PacketType.ProductGroup, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 127);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebPrdGroup> groups, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			try
			{
				if (groups == null || !groups.Any())
				{
					throw new NullReferenceException();
				}
				string text = groups.Select((WebPrdGroup p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (groups == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = groups.GeneratePackets(PacketType.ProductGroup, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 142);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			try
			{
				if (responses == null || !responses.Any())
				{
					throw new NullReferenceException();
				}
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.ProductGroup, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 157);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebPrdGroup> groups)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			try
			{
				if (groups == null || !groups.Any())
				{
					throw new NullReferenceException();
				}
				return groups.GeneratePackets(PacketType.ProductGroup, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 171);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		PrdGroupActions prdGroupActions = (PrdGroupActions)action;
		return prdGroupActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((PrdGroupActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				switch ((PrdGroupActions)pack.PacketActions)
				{
				case PrdGroupActions.SaveRequest:
				{
					line = 10;
					ConnectionEventArgs<WebPrdGroup> args5 = new ConnectionEventArgs<WebPrdGroup>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args5, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs");
						break;
					}
					catch (Exception ex5)
					{
						WebErrorLog.Instance.StartLog(ex5, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 195);
						break;
					}
				}
				case PrdGroupActions.SaveResponse:
				{
					line = 20;
					SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs");
						break;
					}
					catch (Exception ex4)
					{
						WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 204);
						break;
					}
				}
				case PrdGroupActions.ReadRequest:
				{
					line = 30;
					PrdGroupReadEventArgs args3 = new PrdGroupReadEventArgs
					{
						ConnectionUniqueId = connectionUniqueId,
						Connection = connection
					};
					try
					{
						await Utilities.RaiseEvent(this.OnReadRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs");
						break;
					}
					catch (Exception ex3)
					{
						WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 217);
						break;
					}
				}
				case PrdGroupActions.ReadResponse:
				{
					line = 40;
					ConnectionEventArgs<WebPrdGroup> args2 = new ConnectionEventArgs<WebPrdGroup>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 227);
						break;
					}
				}
				case PrdGroupActions.DeleteRequest:
				{
					line = 40;
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 236);
						break;
					}
				}
				case (PrdGroupActions)3:
				case (PrdGroupActions)4:
				case PrdGroupActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex7)
			{
				WebErrorLog.Instance.StartLog(ex7, $"Line:{line}", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 242);
			}
		}
	}

	public override void Dispose(bool disposing)
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
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdGroup.cs", 259);
		}
	}
}
