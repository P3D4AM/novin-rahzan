using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.Mapping;
using PacketParser.TransferLayer;
using Server.Core.Assistance;
using WebSmsStandard;

namespace Server.Core.Connection.Dal;

public class ClsCustomer : IEntity<WebCus>
{
	private class NestedCus
	{
		internal static readonly ClsCustomer CusInstance;

		static NestedCus()
		{
			CusInstance = new ClsCustomer();
		}
	}

	private static Random random = new Random();

	private static TimingCache<short, MapperItemContainer<bool>> _dic_IsNewCustomerExistsFromClient;

	private static TimingCache<short, MapperItemContainer<DateTime>> _dic_isModifiedDepartmentData;

	private static TimingCache<Guid, WebCus> _dic_TempRequests;

	private static TimingCache<Guid, CustomerRegister> _dic_TempRegCus;

	private static TimingCache<string, RegisterRequest> _dic_RegRequests;

	private static TimingCache<short, MapperItemContainer<bool>> dic_IsNewCustomerExistsFromClient
	{
		get
		{
			try
			{
				if (_dic_IsNewCustomerExistsFromClient == null)
				{
					_dic_IsNewCustomerExistsFromClient = new TimingCache<short, MapperItemContainer<bool>>("empty", new MapperItemContainer<bool>(), 300);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "dic_IsNewCustomerExistsFromClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 37);
			}
			return _dic_IsNewCustomerExistsFromClient;
		}
	}

	private static TimingCache<short, MapperItemContainer<DateTime>> dic_isModifiedDepartmentData
	{
		get
		{
			try
			{
				if (_dic_isModifiedDepartmentData == null)
				{
					_dic_isModifiedDepartmentData = new TimingCache<short, MapperItemContainer<DateTime>>("empty", new MapperItemContainer<DateTime>(), 300);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "dic_isModifiedDepartmentData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 54);
			}
			return _dic_isModifiedDepartmentData;
		}
	}

	private static TimingCache<Guid, WebCus> dic_TempRequests
	{
		get
		{
			try
			{
				if (_dic_TempRequests == null)
				{
					_dic_TempRequests = new TimingCache<Guid, WebCus>("empty", new WebCus(), 300);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "dic_TempRequests", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 70);
			}
			return _dic_TempRequests;
		}
	}

	private static TimingCache<Guid, CustomerRegister> dic_TempRegCus
	{
		get
		{
			try
			{
				if (_dic_TempRegCus == null)
				{
					_dic_TempRegCus = new TimingCache<Guid, CustomerRegister>("empty", new CustomerRegister(), 300);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "dic_TempRegCus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 85);
			}
			return _dic_TempRegCus;
		}
	}

	private static TimingCache<string, RegisterRequest> dic_RegRequests
	{
		get
		{
			try
			{
				if (_dic_RegRequests == null)
				{
					_dic_RegRequests = new TimingCache<string, RegisterRequest>("empty", new RegisterRequest(), 300);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "dic_RegRequests", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 100);
			}
			return _dic_RegRequests;
		}
	}

	public static ClsCustomer CusInstance => NestedCus.CusInstance;

	private ClsCustomer()
	{
	}

	public static async Task ProgressSms(long mobile, string panel, string massage)
	{
		int line = 0;
		using (new JaegerMonitor("ProgressSms", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			try
			{
				massage = massage.FixString();
				line = 2;
				CustomerRegister temp = dic_TempRegCus.GetAll()?.FirstOrDefault((CustomerRegister p) => p.Mobile == mobile && p.ReciveVerificationCode == massage);
				line = 10;
				if (temp != null)
				{
					line = 11;
					if (!dic_TempRequests.TryGetValue(temp.Guid, out var ret))
					{
						return;
					}
					line = 15;
					dic_TempRegCus.TryGetValue(temp.Guid, out var request);
					line = 16;
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(request.ConnectionGuid);
					line = 17;
					List<SaveResponse> source = await Server.Core.Assistance.Initializer.Dal.Cus.SaveAsync("serverHubStandar.ClsCustomer.ProgressSms", connection, new List<WebCus>(1) { ret });
					line = 18;
					if (source.FirstOrDefault()?.SaveResultStatus ?? false)
					{
						line = 19;
						Packet packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.RegisterSuccess, temp.Guid);
						line = 20;
						ConnectionManager.Instance.GetConnection(request.ConnectionGuid)?.SendPacketAsync(packet, Priority.High);
						line = 21;
						dic_TempRequests.TryRemove(temp.Guid, out var _);
						line = 22;
						dic_TempRegCus.TryRemove(temp.Guid, out var _);
						line = 23;
					}
				}
				line = 30;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ProgressSms", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 150);
			}
		}
	}

	public static async Task UpdateDictionaryAsync(short depId, string tranName = "")
	{
		try
		{
			dic_isModifiedDepartmentData.TryGetValue(depId, out var _);
			await dic_isModifiedDepartmentData.UpdateTimeAsync(depId, new MapperItemContainer<DateTime>(DateTime.Now), tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 161);
		}
	}

	public static async Task CustomerInstance_OnTempSaveRequest(object sender, CustomerTempSaveRequestEventArgs e)
	{
		using (new JaegerMonitor("CustomerInstance_OnTempSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			Packet packet = null;
			int line = 0;
			short depId = 0;
			string UseFullData = "";
			try
			{
				_ = 7;
				try
				{
					ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Customer, 10, isReceivedPacket: true);
					string tranName = "";
					line = 1;
					string ip = e?.Connection?.Ip ?? "";
					line = 2;
					if (e.Connection.Source == ENSource.WebSite)
					{
						ip = e?.ClientIp ?? "";
					}
					line = 3;
					WebCus tmpCus = e.TempCustomer;
					if (e?.Connection == null)
					{
						throw new NullReferenceException("e?.Connection==null");
					}
					if (e.Connection.DepId <= 0)
					{
						throw new NullReferenceException("e.Connection.depId<=0");
					}
					if (Server.Core.Assistance.Initializer.Dal?.Cus == null)
					{
						throw new NullReferenceException("Server.Core.Assistance.Initializer.Dal?.Cus==null");
					}
					depId = e.Connection.DepId;
					UseFullData = string.Format("TafsilGuid:{0}//tempCus.User:{1}//tmpCus.Mob1:{2}//tmpCus.Email:{3}", e.Connection?.CustomerTafsilGuid ?? Guid.Empty, tmpCus?.User ?? "null", tmpCus?.Mob1 ?? "Null", tmpCus?.Email ?? "Null");
					line = 4;
					CusTmpCmtRsp cusTmpCmtRsp = await Server.Core.Assistance.Initializer.Dal.Cus.IsUniqueAsync(tmpCus.User, tmpCus.Mob1, tmpCus.Email, e.Connection.DepId);
					line = 5;
					if (cusTmpCmtRsp != 0)
					{
						line = 5;
						packet = WebCus.GenerateTempSaveResponse(cusTmpCmtRsp, e.TempCustomer.Guid);
						line = 6;
						return;
					}
					line = 7;
					CustomerRegister Cus2 = null;
					line = 8;
					if (!dic_RegRequests.TryGetValue(ip, out var request))
					{
						line = 9;
						request = new RegisterRequest
						{
							LastRequest = DateTime.Now,
							RequestCount = 1
						};
						line = 10;
						await dic_RegRequests.UpdateTimeAsync(ip, request, tranName);
						line = 11;
						goto IL_0420;
					}
					if (request.RequestCount <= 5)
					{
						goto IL_0420;
					}
					packet = WebCus.GenerateTempSaveResponse(CusTmpCmtRsp.TooMuchRequest, e.TempCustomer.Guid);
					goto end_IL_008b;
					IL_0420:
					line = 12;
					WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(e.Connection.UserId);
					if (!dic_TempRegCus.TryGetValue(e.TempCustomer.Guid, out Cus2))
					{
						line = 13;
						Cus2 = new CustomerRegister
						{
							Email = e.TempCustomer.Email,
							Mobile = e.TempCustomer.Mob1.ParseToLong(0L),
							UserName = e.TempCustomer.User,
							ReciveVerificationCode = GetVerificationCode(),
							SentVerificationCode = GetVerificationCode(),
							Ip = ip,
							Guid = e.TempCustomer.Guid,
							ConnectionGuid = e.ConnectionUniqueId,
							Source = e.Connection.Source,
							DepId = e.Connection.DepId
						};
						line = 14;
						await dic_TempRegCus.UpdateTimeAsync(e.TempCustomer.Guid, Cus2, tranName);
						line = 15;
						if (!e.Connection.cancellationToken.IsCancellationRequested)
						{
							line = 16;
							CusTmpCmtRsp status = await SentMessage(Cus2, tmpCus.Guid, e.Connection.DepId, e.Connection.UserId, e.TempCustomer.Mob1);
							line = 17;
							packet = WebCus.GenerateTempSaveResponse(status, tmpCus.Guid, Cus2.ReciveVerificationCode, user.SMSLine);
							await dic_TempRequests.UpdateTimeAsync(e.TempCustomer.Guid, e.TempCustomer, tranName);
							line = 18;
						}
						goto end_IL_008b;
					}
					line = 19;
					line = 20;
					request.LastRequest = DateTime.Now;
					line = 21;
					request.RequestCount++;
					line = 22;
					CusTmpCmtRsp status2 = await SentMessage(Cus2, tmpCus.Guid, e.Connection.DepId, e.Connection.UserId, e.TempCustomer.Mob1);
					line = 23;
					packet = WebCus.GenerateTempSaveResponse(status2, tmpCus.Guid, Cus2.ReciveVerificationCode, user.SMSLine);
					await dic_TempRequests.UpdateTimeAsync(e.TempCustomer.Guid, e.TempCustomer, tranName);
					line = 24;
					goto end_IL_0060;
					end_IL_008b:;
				}
				catch (OperationCanceledException)
				{
					goto end_IL_0060;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//DepId:{depId}//Data:{UseFullData}", "CustomerInstance_OnTempSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 284);
					packet = WebCus.GenerateTempSaveResponse(CusTmpCmtRsp.ErrorInServer, e.TempCustomer.Guid);
					goto end_IL_0060;
				}
				return;
				end_IL_0060:;
			}
			finally
			{
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if (connection != null)
				{
					await connection.SendPacketAsync(packet, Priority.High);
				}
			}
		}
	}

	public static async Task<CusTmpCmtRsp> SentMessage(CustomerRegister tmpCus, Guid CusGuid, short DepId, int UserId, string Mobile)
	{
		using (new JaegerMonitor("SentMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			_ = 1;
			try
			{
				bool resEmail = await SendEmailVerificationCode(tmpCus.SentVerificationCode, Mobile, DepId, UserId);
				if (await SendVerificationCode(tmpCus.SentVerificationCode, Mobile, DepId, UserId))
				{
					return CusTmpCmtRsp.SmsCodeSent;
				}
				if (resEmail)
				{
					return CusTmpCmtRsp.EmailSent;
				}
				return CusTmpCmtRsp.ErrorInSendSms;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SentMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 313);
				return CusTmpCmtRsp.ErrorInServer;
			}
		}
	}

	private static async Task<bool> SendEmailVerificationCode(string verificationCode, string mobile, short depId, int userId)
	{
		return false;
	}

	private static string GetVerificationCode()
	{
		return random.Next(10000, 99999).ToString();
	}

	public static async Task CustomerInstance_OnTempCommitRequest(object sender, CustomerCommitRequestEventArgs e)
	{
		using (new JaegerMonitor("CustomerInstance_OnTempCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			Packet packet = null;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Customer, 8, isReceivedPacket: true);
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if (!dic_TempRegCus.TryRemove(e.Guid, out var user))
				{
					packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.UserNotFound, e.Guid);
					return;
				}
				dic_RegRequests.TryGetValue(user.Ip, out var ret);
				if (!dic_TempRequests.TryRemove(e.Guid, out var cus))
				{
					packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.UserNotFound, e.Guid);
					return;
				}
				ret.RequestCount--;
				if (user.SentVerificationCode == e.SmsCode)
				{
					if (dic_TempRegCus.GetAll().Count((CustomerRegister w) => w.Mobile == user.Mobile) > 0)
					{
						packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.DuplicateUserMobile, e.Guid);
						return;
					}
					if (dic_TempRegCus.GetAll().Count((CustomerRegister w) => w.UserName == user.UserName) > 0)
					{
						packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.DuplicateUserName, e.Guid);
						return;
					}
					if (dic_TempRegCus.GetAll().Count((CustomerRegister w) => !string.IsNullOrEmpty(w.Email) && w.Email == user.Email) > 0)
					{
						packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.DuplicateUserEmail, e.Guid);
						return;
					}
					await Server.Core.Assistance.Initializer.Dal.Cus.SaveAsync("serverHubStandar.ClsCustomer.OnTempCommitRequest", connection, new List<WebCus>(1) { cus });
					packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.RegisterSuccess, cus.Guid);
				}
				else
				{
					packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.InvalidCode, e.Guid);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "CustomerInstance_OnTempCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 372);
				packet = WebCus.GenerateCommitResponse(CusTmpCmtRsp.ErrorInServer, e.Guid);
			}
			finally
			{
				ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if (connection2 != null)
				{
					await connection2.SendPacketAsync(packet, Priority.High);
				}
			}
		}
	}

	public static async Task CustomerInstance_OnPasswordRecoveryRequest(object sender, CustomerPasswordRecoveryRequestEventArgs e)
	{
		using (new JaegerMonitor("CustomerInstance_OnPasswordRecoveryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			Packet packet = null;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Customer, 11, isReceivedPacket: true);
				WebCus webCus = await Server.Core.Assistance.Initializer.Dal.Cus.LoadDataAsync(e.Connection.DepId, e.UserName, e.MobileNumber, e.Email, true);
				packet = ((webCus != null) ? ((await SendPasswordRecovery(webCus.Password, webCus.User, webCus.Mob1, e.Connection.DepId, e.Connection.UserId)) ? WebCus.GeneratePasswordRecoveryResponse(success: true, "پیامک حاوی رمز عبور ارسال شد") : WebCus.GeneratePasswordRecoveryResponse(success: false, "سیستم قادر به ارسال پیامک نیست")) : WebCus.GeneratePasswordRecoveryResponse(success: false, "کاربری با این مشخصات یافت نشد"));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CustomerInstance_OnPasswordRecoveryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 404);
			}
			finally
			{
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(packet, Priority.High);
			}
		}
	}

	private static async Task<bool> SendVerificationCode(string code, string Mobile, short DepId, int UserId, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendVerificationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			_ = 2;
			try
			{
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(UserId);
				WebDepartment webDepartment = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(DepId);
				if (!string.IsNullOrEmpty(webDepartment.SMSTemplate_Singup))
				{
					string template = webDepartment.SMSTemplate_Singup;
					template = template.Replace("[کد]", code);
					if (!new WebTafsilNumbers(Mobile).Mobile)
					{
						WebErrorLog.Instance.StartLog(new Exception(string.Format("خطا در ارسال پیامک کد فعال سازی شماره موبایل نامعتبر است . شماره موبایل:{0} کد:{1} شناسه دپارتمان:{2} شناسه کاربر:{3} متن:{4}", Mobile, code, DepId, UserId, template ?? "Null!!")), "", "SendVerificationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 421);
						return false;
					}
					if (string.IsNullOrEmpty(template))
					{
						WebErrorLog.Instance.StartLog(new Exception(string.Format("خطا در ارسال پیامک کد فعال سازی متن پیام خالی است . شماره موبایل:{0} کد:{1} شناسه دپارتمان:{2} شناسه کاربر:{3} متن:{4}", Mobile, code, DepId, UserId, template ?? "Null!!")), "", "SendVerificationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 427);
						return false;
					}
					long[] array = await SmsSender.SendSmsAsync(user.PanelUName, user.PanelPass, user.PanelDomain, template, Mobile, user.SMSLine, token);
					if (array.Length < 1)
					{
						WebErrorLog.Instance.StartLog(new Exception(string.Format("خطا در ارسال پیامک کد فعال سازی .کد خطا:{0}\r\n شماره موبایل:{1}\r\n کد:{2}\r\n شناسه دپارتمان:{3}\r\n شناسه کاربر:{4}\r\n متن:{5}\r\n Username:{6}\r\n password:{7}\r\n domain:{8}\r\n line:{9}", array[0], Mobile, code, DepId, UserId, template ?? "Null!!", user.PanelUName, user.PanelPass, user.PanelDomain, user.SMSLine)), "", "SendVerificationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 436);
						return false;
					}
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendVerificationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 446);
				return false;
			}
		}
	}

	public static async Task<bool> SendPasswordRecovery(string Password, string UserName, string Mobile, short DepId, int UserId, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendPasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			_ = 2;
			try
			{
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(UserId);
				WebDepartment webDepartment = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(DepId);
				if (!string.IsNullOrEmpty(webDepartment.SMSTemplate_Singup))
				{
					string sMSTemplate_RecoveryPass = webDepartment.SMSTemplate_RecoveryPass;
					sMSTemplate_RecoveryPass = sMSTemplate_RecoveryPass.Replace("[کلمه عبور]", Password);
					sMSTemplate_RecoveryPass = sMSTemplate_RecoveryPass.Replace("[نام کاربری]", UserName);
					long[] array = await SmsSender.SendSmsAsync(user.PanelUName, user.PanelPass, user.PanelDomain, sMSTemplate_RecoveryPass, Mobile, user.SMSLine, token);
					if (array.Single() < 1)
					{
						WebErrorLog.Instance.StartLog(new Exception($"خطا در ارسال پیامک بازیابی کلمه عبور .کد خطا:{array} شماره موبایل:{Mobile} pass:{Password} User:{UserName} شناسه دپارتمان:{DepId} شناسه کاربر:{UserId}"), "", "SendPasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 467);
						return false;
					}
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendPasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 478);
				return false;
			}
		}
	}

	public async Task<List<WebCus>> ApplyAccessAsync(IEnumerable<WebCus> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			_ = 1;
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return null;
				}
				if (connection.Source != ENSource.Accounting)
				{
					lst?.Count();
					lst = lst?.Where((WebCus p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				if (lst != null)
				{
					IEnumerable<WebCus> enumerable = lst;
					if (enumerable == null || enumerable.Count() > 0)
					{
						WebUser user = await connection.GetUserAsync();
						WebUserAcc userInfo = await connection.GetUserAccInfoAsync();
						StringBuilder stringBuilder = new StringBuilder(100000);
						_ = user?.Id;
						stringBuilder.AppendLine("#CustomerApplyAccess");
						stringBuilder.AppendLine($"#DepId_{connection?.DepId ?? 0}");
						stringBuilder.AppendLine($"#UserId_{user?.Id ?? 0}");
						stringBuilder.AppendLine("#UserName_" + user?.Name);
						stringBuilder.AppendLine($"CountBefore:{lst?.Count() ?? 0}");
						WebUserAcc webUserAcc = userInfo;
						if (webUserAcc != null && webUserAcc.UserAccess?.TafsilGroupList?.Count > 0)
						{
							stringBuilder.AppendLine($"TafsilLevelFilter countTafsil:{(userInfo?.UserAccess?.TafsilGroupList?.Count).GetValueOrDefault()}");
							lst = lst?.Where((WebCus e) => userInfo.UserAccess.TafsilGroupList.Any((short e2) => e2.Equals(e.TafsilLevel))).ToList();
							stringBuilder.AppendLine($"CountAfterTafsilLevelFilter:{lst?.Count() ?? 0}////Enables:{lst?.Count((WebCus p) => p.Status) ?? 0}");
						}
						WebUserAcc webUserAcc2 = userInfo;
						if (webUserAcc2 != null && webUserAcc2.UserAccess?.Customers?.Customer_DastrasiBeAshkhaseKhas == false)
						{
							lst = lst.Where((WebCus e) => !e.Special).ToList();
							stringBuilder.AppendLine($"CountAfterSpecialFilter:{lst?.Count() ?? 0}");
						}
						WebUserAcc webUserAcc3 = userInfo;
						if (webUserAcc3 != null && !webUserAcc3.AllVisitorAccess)
						{
							lst = lst.Where(delegate(WebCus e)
							{
								Guid visitorGuid = e.VisitorGuid;
								Guid? visitorGuid2 = userInfo.VisitorGuid;
								return visitorGuid == visitorGuid2;
							}).ToList();
							stringBuilder.AppendLine($"CountAfterVisitor:{lst?.Count() ?? 0}");
						}
						lst = lst?.Where((WebCus p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered)?.ToList();
						stringBuilder.AppendLine($"تعداد اشخاص ارسال شده:{lst?.Count() ?? 0}//اشخاص فعال:{lst?.Count((WebCus p) => p.Status) ?? 0}");
						WebTelegramMessage.GetWebServiceMonitoring_bot().Sent2web(stringBuilder.ToString(), null, "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 532);
						return lst?.ToList();
					}
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 537);
				return null;
			}
		}
	}

	public Task WebCus_OnReadResponse(object sender, ConnectionEventArgs<WebCus> e)
	{
		try
		{
			ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Customer, 6, isReceivedPacket: true);
			if (e.Connection.Source != ENSource.Accounting)
			{
				throw new Exception($"Source({e.Connection.Source}) not supported!");
			}
			IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p.Source != ENSource.Accounting);
			if (enumerable != null && enumerable.Any())
			{
				foreach (ClsConnectionData connection in enumerable)
				{
					Task.Run(() => connection.SendPacketAsync(WebCus.GenerateReadResponse(e.ConnectionData, 0), Priority.High));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebCus_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 554);
		}
		return Task.CompletedTask;
	}

	public async Task CustomerInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCus> e)
	{
		StringBuilder sb = new StringBuilder(100000);
		try
		{
			(e?.Connection?.DepId).GetValueOrDefault(-1);
			ENSource source = (e?.Connection?.Source).GetValueOrDefault();
			int user = (e?.Connection?.UserId).GetValueOrDefault(-1);
			Task timeout = Task.Delay(30000, e.Connection.cancellationToken);
			if (await Task.WhenAny(new Task[2]
			{
				timeout,
				CustomerInstance_OnSaveRequest_(sender, e, sb)
			}) == timeout)
			{
				WebErrorLog.Instance.StartLog("TimedOut!\r\n" + $"depId:{(e?.Connection?.DepId).GetValueOrDefault(-1)}\r\n" + $"source:{source}\r\n" + $"user:{user}\r\n" + $"sb:{sb}\r\n" + "Data:" + Json.ToStringJson(e), "", "CustomerInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 570);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CustomerInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 577);
		}
	}

	private async Task CustomerInstance_OnSaveRequest_(object sender, ConnectionEventArgs<WebCus> e, StringBuilder sb)
	{
		CancellationToken token = e.Connection.cancellationToken;
		if (e?.ConnectionData == null)
		{
			return;
		}
		ConnectionEventArgs<WebCus> connectionEventArgs = e;
		if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
		{
			return;
		}
		IEnumerable<SaveResponse> ret = new List<SaveResponse>();
		int line = 1;
		using (new JaegerMonitor("CustomerInstance_OnSaveRequest_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			short depId = (e?.Connection?.DepId).GetValueOrDefault(-1);
			try
			{
				sb.AppendLine("CustomerInstance_OnSaveRequest_...");
				await FixDuplicateErrorForOldClientsAsync(e, sb);
				sb.AppendLine("FixDuplicateErrorForOldClientsAsync has been finished!");
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Customer, 1, isReceivedPacket: true);
				sb.AppendLine($"line:{line}");
				if (!(e.ConnectionData?.Any() ?? false))
				{
					throw new Exception("Connection contains no element");
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				line = 2;
				sb.AppendLine($"line:{line}");
				e?.Connection?.cancellationToken.ThrowIfCancellationRequested();
				line = 3;
				sb.AppendLine($"line:{line}");
				if ((e?.Connection?.DepId).GetValueOrDefault() <= 0)
				{
					throw new OperationCanceledException();
				}
				line = 4;
				sb.AppendLine($"line:{line}");
				if (e.Connection.Source != ENSource.Accounting)
				{
					List<WebCus> connectionData = e.ConnectionData;
					if (connectionData != null && connectionData.Count > 0)
					{
						foreach (WebCus connectionDatum in e.ConnectionData)
						{
							await Fix(e.Connection.DepId, connectionDatum);
						}
					}
				}
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 50;
				sb.AppendLine($"line:{line}");
				token.ThrowIfCancellationRequested();
				line = 51;
				ret = await Server.Core.Assistance.Initializer.Dal.Cus.SaveAsync("serverHubStandar.ClsCustomer.OnSaveRequest", connection, e.ConnectionData);
				line = 52;
				sb.AppendLine("✅✅#Save_Successfully");
				line = 53;
				if (ret == null)
				{
					line = 54;
					throw new NullReferenceException("ret");
				}
				line = 55;
				foreach (SaveResponse item in ret)
				{
					line = 56;
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					line = 57;
					item.PartitionId = e.PartitionId;
				}
				line = 6;
				sb.AppendLine($"line:{line}");
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					line = 7;
					sb.AppendLine($"line:{line}");
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 8;
					sb.AppendLine($"line:{line}");
					if (connection2 != null)
					{
						await connection2.SendPacketAsync(WebCus.GenerateSaveResponse(ret), Priority.High, 0, e.Connection.cancellationToken);
					}
					line = 9;
					sb.AppendLine($"line:{line}");
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					line = 10;
					sb.AppendLine($"line:{line}");
					ClsConnectionData cn3 = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					line = 110;
					sb.AppendLine($"line:{line}");
					if (!(cn3?.IsDisposed ?? true))
					{
						line = 113;
						sb.AppendLine($"line:{line}");
						if (e.Connection.cancellationToken.IsCancellationRequested)
						{
							return;
						}
						line = 114;
						sb.AppendLine($"line:{line}");
						Task.Run(() => cn3.SendPacketAsync(WebCus.GenerateSaveRequest(e.ConnectionData), Priority.High, 0, e.Connection.cancellationToken));
					}
					else
					{
						line = 1150;
						sb.AppendLine($"line:{line}");
						if ((connection?.DepId ?? (-1)) == 3)
						{
							line = 1151;
							sb.AppendLine($"line:{line}");
							WebTelegramMessage.GetNp_ServerMessaging_bot().Sent2web("accounting connection could not found!", null, "CustomerInstance_OnSaveRequest_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 680);
							line = 1152;
							sb.AppendLine($"line:{line}");
						}
						line = 1153;
						sb.AppendLine($"line:{line}");
					}
					line = 12;
					sb.AppendLine($"line:{line}");
				}
				line = 15;
				sb.AppendLine($"line:{line}");
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData cn2 in enumerable)
				{
					if (cn2?.IsDisposed ?? true)
					{
						continue;
					}
					line = 17;
					sb.AppendLine($"line:{line}");
					IEnumerable<WebCus> SendLst = e.ConnectionData;
					if (connection.ClientVersion < 10 && connection.Source != ENSource.WebsiteWordPress)
					{
						SendLst = await CusInstance.ApplyAccessAsync(SendLst, cn2);
					}
					if (SendLst?.Any() ?? false)
					{
						Task.Run(() => cn2.SendPacketAsync(WebCus.GenerateReadResponse(SendLst, 0), Priority.Low, 0, cn2.cancellationToken));
					}
					line = 18;
					sb.AppendLine($"line:{line}");
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				sb.Append($"DepID:{(e?.Connection?.DepId).GetValueOrDefault(-1)}");
				sb.AppendLine("❌❌❌Exception=>" + ex2.Message);
				if (ex2.InnerException != null)
				{
					sb.Append("❌❌❌InnerException=>" + ex2.InnerException.Message);
				}
				if (ex2.InnerException?.InnerException != null)
				{
					sb.Append("❌❌❌InnerInnerException=>" + ex2.InnerException.InnerException.Message);
				}
				ret = ret.Append(new SaveResponse
				{
					SaveResultStatus = false
				});
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ClsConnectionData cn = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if (cn != null)
				{
					Task.Run(() => cn.SendPacketAsync(WebCus.GenerateSaveResponse(ret), Priority.High));
				}
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}\r\n" + $"DepIdOnStart:{depId}\r\n" + $"CurrentDepId:{(e?.Connection?.DepId).GetValueOrDefault(-1)}\r\n" + $"IsNull:{e?.Connection == null}\r\n" + $"IsDisposed:{(e?.Connection?.IsDisposed).GetValueOrDefault(true)}\r\n" + $"TokenCancell:{(e?.Connection?.cancellationToken.IsCancellationRequested).GetValueOrDefault(true)}\r\n" + $"sb:{sb}", "CustomerInstance_OnSaveRequest_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 736);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> FixDuplicateErrorForOldClientsAsync(ConnectionEventArgs<WebCus> data, StringBuilder sbTotal)
	{
		using (new JaegerMonitor("FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			StringBuilder sb = new StringBuilder(100000);
			try
			{
				sb.AppendLine($"#DepId_{data.Connection.DepId}");
				if (!(data?.ConnectionData?.Any()).GetValueOrDefault())
				{
					return ret;
				}
				if (data.Connection.Source == ENSource.WordPressDebug || data.Connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				if (data.Connection.ClientVersion >= 12)
				{
					return ret;
				}
				short depId = data.Connection.DepId;
				foreach (WebCus cus in data.ConnectionData)
				{
					if (cus == null)
					{
						continue;
					}
					if (string.IsNullOrEmpty(cus.Email) && !string.IsNullOrEmpty(cus.RSTR9))
					{
						cus.Email = cus.RSTR9;
						WebErrorLog.Instance.StartLog("❌❌❌❌❌❌❌❌❌❌❌❌\r\n❌❌❌❌❌❌❌❌❌❌❌❌\r\n❌❌❌❌❌❌❌❌❌❌❌❌\r\ncus.Email : " + cus.Email + "\r\ncus.Rstr9 : " + cus.RSTR9 + "\r\nفیلد ایمیل برای این شخص درست پر نشده است !!!!", "", "FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 775);
					}
					sb.AppendLine($"#Guid: #{cus.Guid}\r\n" + "#Email: #" + cus.Email);
					if ((data?.Connection?.cancellationToken.IsCancellationRequested).GetValueOrDefault(true))
					{
						return ret.AddErrorTokenIsCancelled("", "", "FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 785);
					}
					WebCus existsCus = await Server.Core.Assistance.Initializer.Dal.Cus.GetAsync(data.Connection.DepId, cus.Guid);
					if (!string.IsNullOrEmpty(cus.User) && (existsCus?.User?.ToLower() ?? "") != (cus.User?.ToLower() ?? ""))
					{
						WebCus webCus = await Server.Core.Assistance.Initializer.Dal.Cus.LoadDataAsync(depId, cus.User);
						if (webCus != null && webCus.Guid != cus.Guid)
						{
							sb.AppendLine("UserConfilict\r\n" + $"Guid:{cus.Guid}\r\n" + $"ConfilictedGuid:{webCus.Guid}\r\n" + "User:" + cus.User);
							cus.User = "";
						}
					}
					sb.AppendLine($"x100/ #ExistsEmail_{existsCus?.Email} / ExistsIsNull:{existsCus == null}");
					sb.AppendLine($"x200/ #CusEmail_{cus?.Email} / CusIsNull:{cus == null} / CusEmailIsNull:{string.IsNullOrEmpty(cus.Email)}");
					if ((data?.Connection?.cancellationToken.IsCancellationRequested).GetValueOrDefault(true))
					{
						return ret.AddErrorTokenIsCancelled("", "", "FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 810);
					}
					if (!string.IsNullOrEmpty(cus.Email) && (existsCus?.Email?.ToLower() ?? "") != (cus?.Email?.ToLower() ?? ""))
					{
						WebCus webCus2 = await Server.Core.Assistance.Initializer.Dal.Cus.LoadDataAsync(depId, "", "", cus.Email);
						sb.AppendLine("x5/ #SearchByEmail_" + webCus2?.Email);
						if (webCus2 != null && webCus2.Guid != cus.Guid)
						{
							sb.AppendLine("EMailConfilict\r\n" + $"Guid:{cus.Guid}\r\n" + $"ConfilictedGuid:{webCus2.Guid}\r\n" + "EMail:" + cus.Email);
							cus.Email = string.Format("R_{0}_{1}", new Random().Next(), cus?.Email ?? "x@NovinP.com");
							cus.RSTR9 = cus.Email;
						}
					}
				}
				sb.AppendLine("FixDuplicate/Line:1");
				var enumerable = from p in data.ConnectionData
					group p by p.User.ToLower() into @group
					select new
					{
						UserName = @group.Key,
						Count = @group.Count()
					} into p
					where p.Count > 1
					select p;
				sb.AppendLine("FixDuplicate/Line:2");
				int num = 0;
				foreach (var duplicateItem2 in enumerable)
				{
					while (data.ConnectionData.Count((WebCus p) => !string.IsNullOrEmpty(p.User) && p.User.ToLower() == duplicateItem2.UserName) > 1)
					{
						num++;
						if (num > 100)
						{
							throw new Exception($"index({num}) out of range!");
						}
						data.ConnectionData.First((WebCus p) => !string.IsNullOrEmpty(p.User) && p.User == duplicateItem2.UserName).User = "";
					}
				}
				sb.AppendLine("FixDuplicate/Line:5");
				var enumerable2 = from p in data.ConnectionData
					group p by p.Email?.ToLower() ?? "" into @group
					select new
					{
						EMail = @group.Key,
						Count = @group.Count()
					} into p
					where p.Count > 1
					select p;
				int num2 = 0;
				foreach (var duplicateItem in enumerable2)
				{
					while (data.ConnectionData.Count((WebCus p) => !string.IsNullOrEmpty(p.Email) && (p?.Email?.ToLower() ?? "") == duplicateItem.EMail) > 1)
					{
						num2++;
						if (num2 > 100)
						{
							throw new Exception($"index1({num2}) out of range!");
						}
						WebCus webCus3 = data.ConnectionData.First((WebCus p) => !string.IsNullOrEmpty(p.Email) && p.Email == duplicateItem.EMail);
						webCus3.Email = "";
						webCus3.RSTR9 = "";
					}
				}
			}
			catch (Exception ex)
			{
				sb.AppendLine("❌Exception:" + ex.Message);
				WebErrorLog.Instance.StartLog(ex, "", "FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 885);
				ret.AddReturnedValue(ex, "", "FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 886);
			}
			finally
			{
				sb.AppendLine("ClsCus.FixDuplicate ha been finished!");
				WebTelegramMessage.GetHotelAsa_bot().Sent2web(sb.ToString(), null, "FixDuplicateErrorForOldClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 892);
				sbTotal.AppendLine(sb.ToString());
			}
			return ret;
		}
	}

	private async Task Fix(short depId, WebCus webCus)
	{
		using (new JaegerMonitor("Fix", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			_ = 1;
			try
			{
				if (webCus.TitleGuid != Guid.Empty)
				{
					webCus.Title = (await Server.Core.Assistance.Initializer.Dal.CusTitle.GetAsync(depId, webCus.TitleGuid))?.Id ?? 0;
				}
				if (webCus.GroupGuid != Guid.Empty)
				{
					webCus.IdGroup = ((WebCusGroup)(await Server.Core.Assistance.Initializer.Dal.CusGroup.GetAsync(depId, webCus.GroupGuid)))?.Id ?? 0;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"{webCus}", "Fix", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 912);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel, StringBuilder sb)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 300000))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1;
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 925);
				}
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				if (connection.ClientVersion < 10 && (object)syncInformation != null && syncInformation.CustomerReset && connection.Source != ENSource.WebsiteWordPress)
				{
					line = 2;
					IEnumerable<WebCus> lst = await Server.Core.Assistance.Initializer.Dal.Cus.GetAllAsync(connection.DepId, new DateTime(1900, 1, 1), connection.cancellationToken);
					line = 3;
					lst = await ApplyAccessAsync(lst, connection);
					line = 4;
					List<SyncResetItem> ResetItems = new List<SyncResetItem>();
					line = 5;
					if ((lst?.Count() ?? 0) > 0)
					{
						foreach (WebCus item in lst)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item.Guid,
								Id = item.Id
							});
						}
					}
					line = 10;
					SyncDoneWaiter syncDoneWaiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Customer, 300000, connection.cancellationToken);
					line = 11;
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.Customer, ResetItems), Priority.High));
					line = 12;
					if (await syncDoneWaiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.Customer, connection.cancellationToken);
					}
					line = 15;
				}
				line = 16;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 956);
				}
				line = 17;
				List<int> totalPartitions = new List<int>();
				line = 18;
				ReturnedSaveFuncInfoWithValue<List<int>> returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.Cus.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 961);
				if (ret.HasError)
				{
					return ret;
				}
				line = 19;
				if (returnedSaveFuncInfoWithValue.value?.Any() ?? false)
				{
					totalPartitions.AddRange(returnedSaveFuncInfoWithValue.value);
					line = 20;
					while (returnedSaveFuncInfoWithValue.value?.Any() ?? false)
					{
						if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 971);
						}
						line = 21;
						await SyncPartitionsAsync(returnedSaveFuncInfoWithValue.value, connection, maxParalel);
						line = 22;
						returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.Cus.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
						line = 23;
						if (returnedSaveFuncInfoWithValue != null && returnedSaveFuncInfoWithValue.value.Any())
						{
							returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue.value?.Where((int x) => !totalPartitions.Contains(x)).ToList();
						}
						line = 24;
					}
				}
				line = 25;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 984);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 987);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 988);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"CustomerSyncResult:{!ret.HasError}");
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionsAsync(IEnumerable<int> partitions, ClsConnectionData connection, short maxParalel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				maxParalel = 1;
			}
			if ((partitions?.Count() ?? 0) <= 0)
			{
				return ret.AddInformation("partitions is null or empty!", "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1005);
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (int partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1013);
					tasks.Remove(t);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1017);
			if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("connection is disposed or token is canceled!", "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1019);
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1023);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1027);
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1028);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(int partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			line = 1;
			DateTime syncing = DateTime.Now;
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddError("disposed", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1041);
			}
			SyncInformation syncInfo = await connection.GetSyncInfoAsync();
			IEnumerable<WebCus> lst = await Server.Core.Assistance.Initializer.Dal.Cus.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			line = 10;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1046);
			}
			line = 15;
			line = 250;
			IEnumerable<WebCus> deletedLst = lst;
			line = 251;
			if (connection.ClientVersion < 10 && connection.Source != ENSource.WebsiteWordPress)
			{
				lst = await ApplyAccessAsync(lst, connection);
			}
			else
			{
				lst = lst?.Where((WebCus p) => p.ServerStatus == ServerStatusForClient.Delivered || p.ServerStatus == ServerStatusForClient.DirectDelivery);
			}
			line = 254;
			if (lst != null && connection.ClientVersion < 10)
			{
				deletedLst = deletedLst?.Except(lst).ToList();
			}
			line = 255;
			if (connection.ClientVersion < 10 && (deletedLst?.Count() ?? 0) > 0)
			{
				line = 31;
				if (connection?.IsDisposed ?? true)
				{
					line = 32;
					ret.AddReturnedValue(ReturnedState.Error, "disposed", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1073);
					return ret;
				}
				line = 35;
				if ((connection?.IsDisposed ?? true) || (connection?.cancellationToken.IsCancellationRequested ?? true))
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1077);
				}
				deletedLst = deletedLst?.Where((WebCus p) => p.ResetTime > syncInfo.Customer && (p.ServerStatus == ServerStatusForClient.Delivered || p.ServerStatus == ServerStatusForClient.DirectDelivery)).ToList();
				line = 40;
				IEnumerable<WebCus> enumerable = deletedLst;
				if (enumerable != null && enumerable.Count() > 0)
				{
					line = 41;
					GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, partition, (connection.Source == ENSource.WebsiteWordPress) ? 1800000 : 300000, connection.cancellationToken, EntityPart.Customer, connection.ContextGuid);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1089);
					}
					if (connection.Source == ENSource.WebsiteWordPress)
					{
						Task.Run(() => connection.SendPacketAsync(WebCus.GenerateDeleteRequest(deletedLst, partition), Priority.Low, partition, connection.cancellationToken));
					}
					else
					{
						Task.Run(() => connection.SendPacketAsync(WebCus.GenerateDeleteRequest(deletedLst.Select((WebCus p) => p.Guid), partition), Priority.Low, partition, connection.cancellationToken));
					}
					bool flag = await getDeleteResponsePacket.WaitForDeleteDonePacket();
					line = 50;
					if (!flag && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
					{
						return ret.AddError($"waiterForDeleteResult:{flag}" + $"\r\nResetTimeFlag:{Server.Core.Assistance.Initializer.Dal.ResetTimeFlag}" + $"\r\nSource:{connection.Source}", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1102);
					}
					line = 55;
				}
				line = 60;
			}
			line = 65;
			line = 75;
			if (lst?.Any() ?? false)
			{
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Customer, (connection.Source == ENSource.WebsiteWordPress) ? 1800000 : 300000, connection.cancellationToken);
				line = 80;
				Task.Run(() => connection.SendPacketAsync(WebCus.GenerateReadResponse(lst, partition, isSync: true), Priority.Low, partition, connection.cancellationToken));
				line = 85;
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					return ret.AddError("WaitForSyncDonePacket failed!", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1129);
				}
				line = 90;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1131);
				}
				line = 91;
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Customer, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1134);
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1139);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"Source:{connection?.Source}\r\n" + Json.ToStringJson(connection), "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1143);
			ret.AddReturnedValue(ex, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1146);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDelivered(StringBuilder sb, ClsConnectionData connection, string ConnectionUniqueId, short maxParalelTask = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			_ = 4;
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Customer, connection.cancellationToken);
				sb.AppendLine($"تعداد اشخاص جهت ارسال به کانکشن حسابداری: {list?.Count ?? (-1)}");
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1184);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("شخصی برای سینک با حسابداری وجود ندارد.", "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1185);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1189);
					}
					WebCus webCus = await Server.Core.Assistance.Initializer.Dal.Cus.LoadDataAsync(connection.DepId, item);
					if (webCus != null)
					{
						tasks.Add(SendCustomerAsync(sb, webCus, connection));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1196);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1200);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1202);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1205);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1206);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendCustomerAsync(StringBuilder sb, WebCus cus, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using CustomerSaveWaiter waiter = new CustomerSaveWaiter(connection.Context, cus.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebCus.GenerateSaveRequest(new List<WebCus> { cus }), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("CustomerNotSent=>GenerateSaveRequest\r\n" + $"Guid:{cus?.Guid}\r\n" + "Name:" + cus?.Name + " Family:" + cus?.Family + "\r\n" + $"#DepId{connection?.DepId ?? (-1)} {connection?.DepName?.FixForTelegram()}\r\n" + $"#HashId{connection?.HashId}\r\n" + "#Source:" + connection?.Source.GetTelegramDisplay()), ReturnedState.None, "SendCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1220);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1227);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1230);
				ret.AddReturnedValue(ex, "", "SendCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1231);
			}
			finally
			{
				if (ret.HasError)
				{
					sb.AppendLine($"✅✅ارسال موفق ارسال شخص به حسابداری ({cus?.Name}//{cus?.Family}) Guid({cus?.Guid})");
				}
				else
				{
					sb.AppendLine($"❌❌ارسال ناموفق شخص به حسابداری ({cus?.Name}//{cus?.Family}) Guid({cus?.Guid})");
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendCustomerIfNotDeliveredToAccAsync(ClsConnectionData accountingConnection, short maxParalelTasks, List<Guid?> guids, CancellationToken timeOutToken)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if ((accountingConnection?.IsDisposed ?? true) || accountingConnection.ClientVersion < 10)
			{
				return ret;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			line = 1;
			if (!(guids?.Any() ?? false))
			{
				return ret;
			}
			line = 2;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (Guid? guid in guids)
			{
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				if ((guid ?? Guid.Empty) == Guid.Empty)
				{
					return ret;
				}
				line = 50;
				if (accountingConnection?.IsDisposed ?? true)
				{
					throw new Exception("AccountingConnection is disposed!");
				}
				line = 51;
				WebCus cus = await Server.Core.Assistance.Initializer.Dal.Cus.GetAsync(accountingConnection.DepId, guid.Value);
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				if (cus == null)
				{
					continue;
				}
				line = 7;
				WebCusGroup webCusGroup = (WebCusGroup)(await Server.Core.Assistance.Initializer.Dal.CusGroup.GetAsync(accountingConnection.DepId, cus.GroupGuid));
				if (webCusGroup.ServerStatus != ServerStatusForClient.Delivered && webCusGroup.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					if (timeOutToken.IsCancellationRequested)
					{
						return ret;
					}
					line = 10;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCusGroup.CusGroupInstance.SendGroupAsync(webCusGroup, accountingConnection), ReturnedState.None, "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1284);
				}
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				line = 11;
				WebCusTitle webCusTitle = await Server.Core.Assistance.Initializer.Dal.CusTitle.GetAsync(accountingConnection.DepId, cus.TitleGuid);
				line = 12;
				if (webCusTitle.ServerStatus != ServerStatusForClient.Delivered && webCusTitle.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					if (timeOutToken.IsCancellationRequested)
					{
						return ret;
					}
					line = 15;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCusTitle.CusTitleInstance.SendTitleAsync(webCusTitle, accountingConnection), ReturnedState.None, "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1297);
				}
				WebCustomerRole webCustomerRole = await Server.Core.Assistance.Initializer.Dal.CustomerRole.GetAsync(accountingConnection.DepId, cus.RoleGuid ?? Guid.Empty);
				line = 12;
				if (webCustomerRole != null && webCustomerRole.ServerStatus != ServerStatusForClient.Delivered && webCustomerRole.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					if (timeOutToken.IsCancellationRequested)
					{
						return ret;
					}
					line = 15;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCustomerRole.CustomerRoleInstance.SendAsync(webCustomerRole, accountingConnection), ReturnedState.None, "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1308);
				}
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				line = 16;
				ret.throwExceptionIfError();
				line = 17;
				if (cus.ServerStatus == ServerStatusForClient.Delivered || cus.ServerStatus == ServerStatusForClient.DirectDelivery)
				{
					continue;
				}
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				line = 20;
				tasks.Add(CusInstance.SendCustomerAsync(new StringBuilder(100000), cus, accountingConnection));
				line = 21;
				if (tasks.Count >= maxParalelTasks)
				{
					line = 22;
					Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
					if (timeOutToken.IsCancellationRequested)
					{
						return ret;
					}
					line = 23;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1327);
					line = 24;
					tasks.Remove(t);
					line = 25;
				}
			}
			if (timeOutToken.IsCancellationRequested)
			{
				return ret;
			}
			line = 30;
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1336);
		}
		catch (Exception ex)
		{
			if (!(accountingConnection?.IsDisposed ?? true))
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//DepId:{accountingConnection?.DepId ?? (-1)}//Version:{accountingConnection?.ClientVersion ?? (-1)}", "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1341);
			}
			ret.AddReturnedValue(ex, "", "SendCustomerIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomer.cs", 1342);
		}
		return ret;
	}
}
