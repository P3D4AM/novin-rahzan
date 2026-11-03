using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;
using PacketParser.SiteManagementApiModel;

namespace PacketParser.Entities;

[Serializable]
public class WebCus : Serializable<WebCus>, ICus, ICusCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, ISyncFromClient2Server<WebCus>, IDeliverableEntity, IEditableDelivery, INpDisposable, IDisposable, IItemAsyncMapper<WebCus, WebCus>
{
	private static string apiUrl = HttpHelpers.ApiUrl + "Department/Customer/";

	private long _code;

	private short _ID_Group;

	private DateTime date = DateTime.MaxValue;

	private string picFace = "";

	private string picSign = "";

	private List<WebTafsilNumbers> _tafsilNumbers;

	private string wpRoleName;

	public short Version { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public Guid TafsilGroupGuid { get; set; }

	public TypeTax TaxType { get; set; }

	public string FullName
	{
		get
		{
			if (!string.IsNullOrEmpty(Name))
			{
				return Name + " " + Family;
			}
			return Family;
		}
		set
		{
		}
	}

	public int Id { get; set; }

	[Obsolete("Use AreaGuid")]
	public int IdPath { get; set; }

	public Guid? AreaGuid { get; set; }

	public string Name { get; set; } = "";


	public string Family { get; set; } = "";


	public Guid SupervisorGuid { get; set; }

	public DateTime? SendToServerDate { get; set; }

	public string Tel1 { get; set; } = "";


	public string Tel2 { get; set; } = "";


	public string Fax { get; set; } = "";


	public int VisitorId { get; set; }

	public string Mob1 { get; set; } = "";


	public string Mob2 { get; set; } = "";


	public string Address1 { get; set; } = "";


	public string Address2 { get; set; } = "";


	public string Description { get; set; } = "";


	public string Zip { get; set; } = "";


	public decimal Account { get; set; }

	public string Tell1 { get; set; }

	public Guid? RoleGuid { get; set; }

	public ReturnedState ChequeBargashtiValidation { get; set; }

	public ReturnedState OrderTasvieTimeValidation { get; set; }

	public List<WebCusCard> CardList { get; set; }

	public int ClientVersion { get; set; }

	public List<WebTafsilNumbers> TafsilNumbers
	{
		get
		{
			return _tafsilNumbers;
		}
		set
		{
			_tafsilNumbers = value;
		}
	}

	[JsonIgnore]
	public List<WebTafsilNumbers> Numbers
	{
		get
		{
			List<WebTafsilNumbers> tafsilNumbers = TafsilNumbers;
			if (tafsilNumbers != null && tafsilNumbers.Any())
			{
				return TafsilNumbers;
			}
			List<WebTafsilNumbers> list = new List<WebTafsilNumbers>();
			if (Tel1.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = Tel1.ParseToLong(0L),
					Title = "تلفن1"
				});
			}
			if (Tel2.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = Tel2.ParseToLong(0L),
					Title = "تلفن2"
				});
			}
			if (Mob1.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = Mob1.ParseToLong(0L),
					Title = "همراه1"
				});
			}
			if (Mob2.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = Mob2.ParseToLong(0L),
					Title = "همراه2"
				});
			}
			if (Fax.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = Fax.ParseToLong(0L),
					Title = "فکس"
				});
			}
			if (RT1.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT1.ParseToLong(0L),
					Title = "تلفن3"
				});
			}
			if (RT2.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT2.ParseToLong(0L),
					Title = "تلفن4"
				});
			}
			if (RT3.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT3.ParseToLong(0L),
					Title = "تلفن5"
				});
			}
			if (RT4.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT4.ParseToLong(0L),
					Title = "تلفن6"
				});
			}
			if (RT5.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT5.ParseToLong(0L),
					Title = "تلفن7"
				});
			}
			if (RT6.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT6.ParseToLong(0L),
					Title = "تلفن8"
				});
			}
			if (RT7.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT7.ParseToLong(0L),
					Title = "تلفن9"
				});
			}
			if (RT8.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT8.ParseToLong(0L),
					Title = "تلفن10"
				});
			}
			if (RT9.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT9.ParseToLong(0L),
					Title = "تلفن11"
				});
			}
			if (RT10.ParseToLong(0L) > 99)
			{
				list.Add(new WebTafsilNumbers
				{
					TafsilGuid = Guid,
					OriginalNumber = RT10.ParseToLong(0L),
					Title = "تلفن12"
				});
			}
			return list;
		}
		set
		{
			_tafsilNumbers = value;
			Tel1 = GetNumber(0);
			Tel2 = GetNumber(1);
			Mob1 = GetNumber(2);
			Mob2 = GetNumber(3);
			Fax = GetNumber(4);
			RT1 = GetNumber(5);
			RT2 = GetNumber(6);
			RT3 = GetNumber(7);
			RT4 = GetNumber(8);
			RT5 = GetNumber(9);
			RT6 = GetNumber(10);
			RT7 = GetNumber(11);
			RT8 = GetNumber(12);
			RT9 = GetNumber(13);
			RT10 = GetNumber(14);
			string GetNumber(short index)
			{
				try
				{
					if (value == null || index >= value.Count)
					{
						return "";
					}
					return value[index].OriginalNumber.ToString();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Numbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 119);
					return "";
				}
			}
		}
	}

	public List<WebMetaData> MetaDataList { get; set; }

	public short IdGroup
	{
		get
		{
			if (_ID_Group <= 0 && GroupGuid == Guid.Empty)
			{
				return 1000;
			}
			return _ID_Group;
		}
		set
		{
			_ID_Group = value;
		}
	}

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[JsonIgnore]
	public string StatusForDisplay
	{
		get
		{
			if (!Status)
			{
				return "غیرفعال";
			}
			return "فعال";
		}
	}

	public short Title { get; set; }

	public decimal EtebarCheck { get; set; }

	public string EghtesadiNo { get; set; } = "";


	public string CodeMeli { get; set; } = "";


	public string Company { get; set; } = "";


	public string Job { get; set; } = "";


	public decimal Etebar { get; set; }

	public long LongCode
	{
		get
		{
			return _code;
		}
		set
		{
			_code = value;
		}
	}

	[Obsolete("Use LongCode")]
	public string Code
	{
		get
		{
			return _code.ToString();
		}
		set
		{
			_code = value.ParseToLong(0L);
		}
	}

	public DateTime Date
	{
		get
		{
			if (date.Year <= DateTime.Now.Year)
			{
				return date;
			}
			return DateTime.Now;
		}
		set
		{
			date = value;
		}
	}

	public string PicFace
	{
		get
		{
			if (!picFace.StartsWith("https://secure.gravatar.com"))
			{
				return picFace;
			}
			return "";
		}
		set
		{
			picFace = value ?? "";
		}
	}

	public string PicSign
	{
		get
		{
			if (!picSign.StartsWith("https://secure.gravatar.com"))
			{
				return picSign;
			}
			return "";
		}
		set
		{
			picSign = value ?? "";
		}
	}

	public string Password { get; set; } = "";


	public string User { get; set; } = "";


	[Obsolete]
	public EnPriceType DefPrice { get; set; }

	public bool IsBazaryab { get; set; }

	public bool IsSupervisor { get; set; }

	public bool IsEtebari { get; set; }

	public int SupervisorId { get; set; }

	public string RSTR1 { get; set; } = "";


	public string RSTR2 { get; set; } = "";


	public string RSTR3 { get; set; } = "";


	public string RSTR4 { get; set; } = "";


	public string RSTR5 { get; set; } = "";


	public string RSTR6 { get; set; } = "";


	public string RSTR7 { get; set; } = "";


	public string RSTR8 { get; set; } = "";


	public string RSTR9
	{
		get
		{
			return Email;
		}
		set
		{
			Email = value;
		}
	}

	public string RSTR10 { get; set; } = "";


	public string RT1 { get; set; } = "";


	public string RT2 { get; set; } = "";


	public string RT3 { get; set; } = "";


	public string RT4 { get; set; } = "";


	public string RT5 { get; set; } = "";


	public string RT6 { get; set; } = "";


	public string RT7 { get; set; } = "";


	public string RT8 { get; set; } = "";


	public string RT9 { get; set; } = "";


	public string RT10 { get; set; } = "";


	public DateTime? Rdate1 { get; set; }

	public DateTime? RDate2 { get; set; }

	public DateTime? RDate3 { get; set; }

	public DateTime? Rdate4 { get; set; }

	public DateTime? RDate5 { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public string Location { get; set; } = "";


	public bool Special { get; set; }

	public CustomerType CustType { get; set; }

	public int TafsilId { get; set; }

	public short TafsilLevel { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid TitleGuid { get; set; }

	public Guid VisitorGuid { get; set; }

	public decimal BPercent { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime ResetTime { get; set; }

	public bool InPlacePayment { get; set; }

	public string Email { get; set; }

	[JsonIgnore]
	public DateTime? DeliveryDate { get; set; }

	[JsonIgnore]
	public int DeliveryId { get; set; }

	[JsonIgnore]
	public string DeliveryMessage { get; set; } = "";


	[JsonIgnore]
	public string DeliveryShortMessage { get; set; } = "";


	[JsonIgnore]
	public string HostRefrenced { get; set; } = "";


	public List<string> ListWpRoles => new List<string> { "customer", "administrator", "author" };

	public string WpRoleName
	{
		get
		{
			try
			{
				if (ClientVersion <= 1)
				{
					return wpRoleName;
				}
				if (string.IsNullOrEmpty(wpRoleName))
				{
					return "customer";
				}
				if (!ListWpRoles.Contains(wpRoleName.ToLower()))
				{
					return "customer";
				}
				return wpRoleName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "data:\r\n" + Json.ToStringJson(this), "WpRoleName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 223);
				return "";
			}
		}
		set
		{
			wpRoleName = value;
		}
	}

	public event Func<object, CustomerReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebCus>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeleteRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnDeleteResponse;

	public event Func<object, ConnectionEventArgs<WebCus>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebCus>, Task> OnSync;

	public event Func<object, CustomerTempResponseEventArgs, Task> OnTempCommitResponse;

	public event Func<object, CustomerCommitRequestEventArgs, Task> OnTempCommitRequest;

	public event Func<object, CustomerTempSaveRequestEventArgs, Task> OnTempSaveRequest;

	public event Func<object, CustomerTempResponseEventArgs, Task> OnTempSaveResponse;

	public event Func<object, CustomerPasswordRecoveryRequestEventArgs, Task> OnPasswordRecoveryRequest;

	public event Func<object, CustomerPasswordRecoveryResponseEventArgs, Task> OnPasswordRecoveryResponse;

	public string GetSafePassword()
	{
		return Password.CalcSafeHashBase64(Guid.ToString());
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			CustomerActions customerActions = (CustomerActions)action;
			return customerActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 256);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((CustomerActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 268);
			return $"Error({action})";
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				CustomerReadEventArgs customerReadEventArgs = new CustomerReadEventArgs
				{
					Modified = modified
				};
				list.Add(customerReadEventArgs.ToString());
				return new Packet(PacketType.Customer, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 290);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebCus> customers, int partition, bool isSync = false)
	{
		if (customers == null || !customers.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				if (customers == null || !customers.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (customers == null)
				{
					result = null;
				}
				else
				{
					result = customers.GeneratePackets(PacketType.Customer, 6, null, isSync, null, partition, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 305);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> customers, int partition)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			if (customers == null || !customers.Any())
			{
				throw new NullReferenceException();
			}
			try
			{
				object result;
				if (customers == null)
				{
					result = null;
				}
				else
				{
					result = customers.GeneratePackets(PacketType.Customer, 14, null, isSync: false, null, partition, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 319);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebCus> customers, int partition)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			if (customers == null || !customers.Any())
			{
				throw new NullReferenceException();
			}
			try
			{
				object result;
				if (customers == null)
				{
					result = null;
				}
				else
				{
					result = customers.GeneratePackets(PacketType.Customer, 14, null, isSync: false, null, partition, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 333);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebCus> customers, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			if (customers == null || !customers.Any())
			{
				throw new NullReferenceException("List is null or empty!");
			}
			if (customers != null && customers.Any((WebCus p) => p == null))
			{
				throw new NullReferenceException("list contains customer than null objects!!!!");
			}
			if (customers != null && customers.Any((WebCus p) => !p.CheckValidations()))
			{
				throw new NullReferenceException("list contains customer than revoke validations");
			}
			try
			{
				string text = customers.Select((WebCus p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (customers == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = customers.GeneratePackets(PacketType.Customer, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 351);
				return null;
			}
		}
	}

	private bool CheckValidations()
	{
		if (string.IsNullOrEmpty(Family))
		{
			throw new ArgumentException("family is empty!");
		}
		return true;
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
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
					result = responses.GeneratePackets(PacketType.Customer, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 372);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebCus> customers)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			if (customers == null || !customers.Any())
			{
				throw new NullReferenceException();
			}
			try
			{
				if (customers == null || customers.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return customers?.GeneratePackets(PacketType.Customer, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 387);
				return null;
			}
		}
	}

	public static Packet GenerateCommitRequest(Guid guid, string SmsCode)
	{
		using (new JaegerMonitor("GenerateCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)8).ToString() };
				CustomerCommitRequestEventArgs customerCommitRequestEventArgs = new CustomerCommitRequestEventArgs
				{
					Guid = guid,
					SmsCode = SmsCode
				};
				list.Add(customerCommitRequestEventArgs.ToString());
				return new Packet(PacketType.Customer, 8, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 410);
				return null;
			}
		}
	}

	public static Packet GenerateCommitResponse(CusTmpCmtRsp status, Guid RequestedCusGuid)
	{
		using (new JaegerMonitor("GenerateCommitResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)9).ToString() };
				CustomerTempResponseEventArgs customerTempResponseEventArgs = new CustomerTempResponseEventArgs
				{
					Part = 0,
					TotalParts = 1,
					PacketReferenceId = Guid.Empty,
					CustomerSaveResponse = new CustomerSaveResponse
					{
						Status = status
					},
					RequestedCustomerGuid = RequestedCusGuid
				};
				list.Add(customerTempResponseEventArgs.ToString());
				return new Packet(PacketType.Customer, 9, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateCommitResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 439);
				return null;
			}
		}
	}

	public static Packet GenerateTempSaveRequest(WebCus customer, string ip)
	{
		if (customer == null || customer.Guid == Guid.Empty)
		{
			throw new ArgumentException("ip:" + ip + ",");
		}
		using (new JaegerMonitor("GenerateTempSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)10).ToString() };
				CustomerTempSaveRequestEventArgs customerTempSaveRequestEventArgs = new CustomerTempSaveRequestEventArgs
				{
					PacketReferenceId = Guid.Empty,
					ClientIp = ip
				};
				list.Add(customerTempSaveRequestEventArgs.ToString());
				return new Packet(PacketType.Customer, 10, Json.ToBytesJson(customer), list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTempSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 464);
				return null;
			}
		}
	}

	public static Packet GenerateTempSaveResponse(CusTmpCmtRsp status, Guid RequestedCusGuid, string code = "", string lineNumber = "")
	{
		using (new JaegerMonitor("GenerateTempSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)13).ToString() };
				CustomerSaveResponse customerSaveResponse = new CustomerSaveResponse
				{
					LineNumber = lineNumber,
					Status = status,
					VerificationCode = code
				};
				CustomerTempResponseEventArgs customerTempResponseEventArgs = new CustomerTempResponseEventArgs
				{
					PacketReferenceId = Guid.Empty,
					CustomerSaveResponse = customerSaveResponse,
					RequestedCustomerGuid = RequestedCusGuid
				};
				list.Add(customerTempResponseEventArgs.ToString());
				return new Packet(PacketType.Customer, 13, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTempSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 494);
				return null;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>>> PostToApi(CustomerTempSaveModel item)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<CustomerSaveResponse, CustomerTempSaveModel>(item, apiUrl + "SaveTempAsync/", null, strongDeserialize: false, 15000, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 505);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 511);
			ret.AddReturnedValue(ex, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 512);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 516);
			ret.AddReturnedValue(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 517);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> Commit(string smsCode, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> ret = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<CusTmpCmtRsp>> returnedSaveFuncInfoWithValue = await (apiUrl + $"SaveCommitAsync/{Guid}/{smsCode}").GetFromApi<CusTmpCmtRsp>(token, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Commit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 527);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Commit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 533);
			ret.AddReturnedValue(ex, "", "Commit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 534);
		}
		return ret;
	}

	public static Packet GeneratePasswordRecoveryRequest(string username, string mobileNumber)
	{
		using (new JaegerMonitor("GeneratePasswordRecoveryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)11).ToString() };
				CustomerPasswordRecoveryRequestEventArgs customerPasswordRecoveryRequestEventArgs = new CustomerPasswordRecoveryRequestEventArgs
				{
					MobileNumber = mobileNumber,
					UserName = username
				};
				list.Add(customerPasswordRecoveryRequestEventArgs.ToString());
				return new Packet(PacketType.Customer, 11, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GeneratePasswordRecoveryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 557);
				throw;
			}
		}
	}

	public static Packet GeneratePasswordRecoveryResponse(bool success, string message)
	{
		using (new JaegerMonitor("GeneratePasswordRecoveryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)12).ToString() };
				CustomerPasswordRecoveryResponseEventArgs customerPasswordRecoveryResponseEventArgs = new CustomerPasswordRecoveryResponseEventArgs
				{
					Success = success,
					Message = message
				};
				list.Add(customerPasswordRecoveryResponseEventArgs.ToString());
				return new Packet(PacketType.Customer, 12, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GeneratePasswordRecoveryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 580);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebCus>>> GetByNumberAsync(long number, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebCus>> ret = new ReturnedSaveFuncInfoWithValue<List<WebCus>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebCus>>> returnedSaveFuncInfoWithValue = await $"https://crm.novinp.ir/api/Customer/List/{number}".GetFromApi<List<WebCus>>(token, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 592);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Number:{number}", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 598);
			ret.AddReturnedValue(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 599);
		}
		return ret;
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs"))
		{
			_ = 11;
			try
			{
				int num;
				if (connection == null)
				{
					num = 1;
				}
				else
				{
					_ = connection.cancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return;
				}
				connection?.cancellationToken.ThrowIfCancellationRequested();
				switch ((CustomerActions)pack.PacketActions)
				{
				case CustomerActions.SaveRequest:
				{
					ConnectionEventArgs<WebCus> args2 = new ConnectionEventArgs<WebCus>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.SaveResponse:
				{
					SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.ReadRequest:
				{
					CustomerReadEventArgs customerReadEventArgs = pack.Args[1].FromJson<CustomerReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 627);
					customerReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, customerReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.ReadResponse:
				{
					ConnectionEventArgs<WebCus> args3 = new ConnectionEventArgs<WebCus>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 641);
						break;
					}
				}
				case CustomerActions.DeleteRequest:
				{
					if (this.OnDeleteRequest == null)
					{
						break;
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 651);
						break;
					}
				}
				case CustomerActions.CommitRequest:
				{
					CustomerCommitRequestEventArgs customerCommitRequestEventArgs = pack.Args[1].FromJson<CustomerCommitRequestEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 658);
					customerCommitRequestEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerCommitRequestEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnTempCommitRequest, this, customerCommitRequestEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.CommitResponse:
				{
					CustomerTempResponseEventArgs customerTempResponseEventArgs2 = pack.Args[1].FromJson<CustomerTempResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 665);
					customerTempResponseEventArgs2.ConnectionUniqueId = connectionUniqueId;
					customerTempResponseEventArgs2.Connection = connection;
					await Utilities.RaiseEvent(this.OnTempCommitResponse, this, customerTempResponseEventArgs2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.TempSaveRequest:
				{
					CustomerTempSaveRequestEventArgs customerTempSaveRequestEventArgs = pack.Args[1].FromJson<CustomerTempSaveRequestEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 671);
					customerTempSaveRequestEventArgs.TempCustomer = Json.FromJson<WebCus>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 672);
					customerTempSaveRequestEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerTempSaveRequestEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnTempSaveRequest, this, customerTempSaveRequestEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.PasswordRecoveryRequest:
				{
					CustomerPasswordRecoveryRequestEventArgs customerPasswordRecoveryRequestEventArgs = pack.Args[1].FromJson<CustomerPasswordRecoveryRequestEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 679);
					customerPasswordRecoveryRequestEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerPasswordRecoveryRequestEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnPasswordRecoveryRequest, this, customerPasswordRecoveryRequestEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.PasswordRecoveryResponse:
				{
					CustomerPasswordRecoveryResponseEventArgs customerPasswordRecoveryResponseEventArgs = pack.Args[1].FromJson<CustomerPasswordRecoveryResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 686);
					customerPasswordRecoveryResponseEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerPasswordRecoveryResponseEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnPasswordRecoveryResponse, this, customerPasswordRecoveryResponseEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				case CustomerActions.TempSaveResponse:
				{
					CustomerTempResponseEventArgs customerTempResponseEventArgs = pack.Args[1].FromJson<CustomerTempResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 693);
					customerTempResponseEventArgs.ConnectionUniqueId = connectionUniqueId;
					customerTempResponseEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnTempSaveResponse, this, customerTempResponseEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs");
					break;
				}
				default:
					throw new Exception($"pack.PacketActions:{pack.PacketActions}//{pack.ToString()}");
				case CustomerActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 705);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
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
				if (this.OnDeleteRequest != null)
				{
					this.OnDeleteRequest = null;
				}
				if (this.OnDeleteResponse != null)
				{
					this.OnDeleteResponse = null;
				}
				if (this.OnPasswordRecoveryRequest != null)
				{
					this.OnPasswordRecoveryRequest = null;
				}
				if (this.OnPasswordRecoveryResponse != null)
				{
					this.OnPasswordRecoveryResponse = null;
				}
				if (this.OnTempSaveResponse != null)
				{
					this.OnTempSaveResponse = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 738);
		}
	}

	public async Task<WebCus> MapAsync(string contextGuid, WebCus source, WebCus destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebCus();
			}
			WebCus webCus = destination;
			webCus.Numbers = await new WebTafsilNumbers().MapListAsync(contextGuid, source?.Numbers, new List<WebTafsilNumbers>(), tranName);
			destination.Email = source.Email;
			destination.Id = source.Id;
			destination.AreaGuid = source.AreaGuid;
			destination.Name = source.Name;
			destination.Family = source.Family;
			destination.SupervisorGuid = source.SupervisorGuid;
			destination.Tel1 = source.Tel1;
			destination.Tel2 = source.Tel2;
			destination.Fax = source.Fax;
			destination.VisitorId = source.VisitorId;
			destination.Mob1 = source.Mob1;
			destination.Mob2 = source.Mob2;
			destination.Address1 = source.Address1;
			destination.Address2 = source.Address2;
			destination.Description = source.Description;
			destination.Zip = source.Zip;
			destination.Account = source.Account;
			destination.Tell1 = source.Tell1;
			destination.Status = source.Status;
			destination.Id = source.Id;
			destination.Title = source.Title;
			destination.EtebarCheck = source.EtebarCheck;
			destination.EghtesadiNo = source.EghtesadiNo;
			destination.CodeMeli = source.CodeMeli;
			destination.Company = source.Company;
			destination.Job = source.Job;
			destination.Etebar = source.Etebar;
			destination.Code = source.Code;
			destination.LongCode = source.LongCode;
			destination.Date = source.Date;
			destination.PicFace = source.PicFace;
			destination.PicSign = source.PicSign;
			destination.Password = source.Password;
			destination.User = source.User;
			destination.DefPrice = source.DefPrice;
			destination.IsBazaryab = source.IsBazaryab;
			destination.IsSupervisor = source.IsSupervisor;
			destination.IsEtebari = source.IsEtebari;
			destination.SupervisorId = source.SupervisorId;
			destination.RSTR1 = source.RSTR1;
			destination.RSTR2 = source.RSTR2;
			destination.RSTR3 = source.RSTR3;
			destination.RSTR4 = source.RSTR4;
			destination.RSTR5 = source.RSTR5;
			destination.RSTR6 = source.RSTR6;
			destination.RSTR7 = source.RSTR7;
			destination.RSTR8 = source.RSTR8;
			destination.RSTR10 = source.RSTR10;
			destination.RT1 = source.RT1;
			destination.RT2 = source.RT2;
			destination.RT3 = source.RT3;
			destination.RT4 = source.RT4;
			destination.RT5 = source.RT5;
			destination.RT6 = source.RT6;
			destination.RT7 = source.RT7;
			destination.RT8 = source.RT8;
			destination.RT9 = source.RT9;
			destination.RT10 = source.RT10;
			destination.Rdate1 = source.Rdate1;
			destination.RDate2 = source.RDate2;
			destination.RDate3 = source.RDate3;
			destination.Rdate4 = source.Rdate4;
			destination.RDate5 = source.RDate5;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Location = source.Location;
			destination.Special = source.Special;
			destination.CustType = source.CustType;
			destination.TafsilId = source.TafsilId;
			destination.TafsilLevel = source.TafsilLevel;
			destination.GroupGuid = source.GroupGuid;
			destination.TitleGuid = source.TitleGuid;
			destination.VisitorGuid = source.VisitorGuid;
			destination.BPercent = source.BPercent;
			destination.ServerStatus = source.ServerStatus;
			destination.ResetTime = source.ResetTime;
			destination.InPlacePayment = source.InPlacePayment;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.FullName = source.FullName;
			destination.IsDisposed = source.IsDisposed;
			destination.TaxType = source.TaxType;
			destination.Version = source.Version;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 841);
		}
		return destination;
	}

	public Task<List<WebCus>> MapListAsync(string contextGuid, IEnumerable<WebCus> source, List<WebCus> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> PasswordRecover(Guid tokenGuid, string username, string mobile)
	{
		ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> ret = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<CusTmpCmtRsp>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<CusTmpCmtRsp, PasswordRecoveryModel>(new PasswordRecoveryModel
			{
				Mobile = mobile,
				Username = username,
				Token = tokenGuid
			}, apiUrl + "PasswordRecoveryAsync/", null, strongDeserialize: false, 15000, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 859);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 865);
			ret.AddReturnedValue(ex, "", "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 866);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> PasswordRecover(ENSource source, string username, string depUserName, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> ret = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, depUserName, password).GetToken(token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 876);
			if (ret.HasError)
			{
				return ret;
			}
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<CusTmpCmtRsp>> returnedSaveFuncInfoWithValue2 = await HttpHelpers.PostToApi<CusTmpCmtRsp, object>(username, apiUrl + "AccPortal/PasswordRecoveryAsync/", headers, strongDeserialize: false, 15000, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 883);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue2.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 889);
			ret.AddReturnedValue(ex, "", "PasswordRecover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 890);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebCus>> LoginAsync(ENSource source, string userName, string password, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebCus> result = new ReturnedSaveFuncInfoWithValue<WebCus>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, userName, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 901);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> obj = new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "Password", password }
			};
			obj.Add("Authorization", (returnedSaveFuncInfoWithValue.value ?? "") ?? "");
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebCus>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "Get").GetFromApi<WebCus>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 910);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 916);
			result.AddReturnedValue(ex, "", "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 917);
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> Send2ServerAsync(ENSource source, string userName, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> result = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, userName, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 927);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<CusTmpCmtRsp>> returnedSaveFuncInfoWithValue2 = await HttpHelpers.PostToApi<CusTmpCmtRsp, WebCus>(this, apiUrl + "UpdateCustomerAsync", headers, strongDeserialize: false, 15000, 3);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 934);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCus.cs", 940);
		}
		return result;
	}
}
