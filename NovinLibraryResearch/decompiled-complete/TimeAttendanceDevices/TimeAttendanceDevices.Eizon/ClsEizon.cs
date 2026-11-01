using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Riss.Devices;

namespace TimeAttendanceDevices.Eizon;

public class ClsEizon : IHasContext
{
	public struct UserInfo
	{
		public bool DeleteUser;

		public bool ChangeId;

		public bool Finger1Check;

		public bool Finger2Check;

		public bool Finger3Check;

		public bool Finger4Check;

		public bool Finger5Check;

		public bool Finger6Check;

		public bool Finger7Check;

		public bool Finger8Check;

		public bool Finger9Check;

		public bool Finger10Check;

		public bool PrivilegeCheck;

		public bool PasswordCheck;

		public bool CardIdCheck;

		public UserPrivilege Privilege;

		public long Id;

		public long NewId;

		public string UserName;

		public string Password;

		public string CardId;

		public string Finger1;

		public string Finger2;

		public string Finger3;

		public string Finger4;

		public string Finger5;

		public string Finger6;

		public string Finger7;

		public string Finger8;

		public string Finger9;

		public string Finger10;
	}

	public struct SystemInfo
	{
		public int MangerCount;

		public int UserCount;

		public int FingerPrintCount;

		public int PasswordCount;

		public int CardIdCount;

		public int NewManagmentCount;

		public int NewAttendanceCount;

		public int AllManagmentCount;

		public int AllAttendanceCount;

		public string FirmwareVersion;
	}

	public enum UserPrivilege
	{
		User,
		Manager,
		SuperManeger,
		Undefine
	}

	public enum UserSetType
	{
		ChangeUserId,
		DeleteUser,
		ChangeUserName,
		DeleteUserName,
		ChangeUserPrivilege,
		SetFingerPrint1,
		DeleteFingerPrint1,
		SetFingerPrint2,
		DeleteFingerPrint2,
		SetFingerPrint3,
		DeleteFingerPrint3,
		SetFingerPrint4,
		DeleteFingerPrint4,
		SetFingerPrint5,
		DeleteFingerPrint5,
		SetFingerPrint6,
		DeleteFingerPrint6,
		SetFingerPrint7,
		DeleteFingerPrint7,
		SetFingerPrint8,
		DeleteFingerPrint8,
		SetFingerPrint9,
		DeleteFingerPrint9,
		SetFingerPrint10,
		DeleteFingerPrint10,
		SetAllFingerPrint,
		DeleteAllFingerPrint,
		ChangePassword,
		DeletePassword,
		ChangeCardId,
		DeleteCardId,
		SetAllInformation,
		CreateNewUser
	}

	public enum CommunicatinTypeName
	{
		Serial,
		TcpIp,
		Usb,
		TypeError
	}

	public enum Systemcase
	{
		Ready,
		NotReady,
		NoData,
		DataRead,
		UserNotExist,
		UserExist,
		DataEdited,
		UserAdded,
		SpaceIsFull,
		NotAllUser
	}

	private enum ConnectionModel
	{
		Unknown,
		Zd100,
		Zd101,
		Zd2910,
		Zd3000
	}

	private enum DeviceZd100Status
	{
		Managers = 1,
		Users,
		Fingerprints,
		Passwords,
		NewlyManagement,
		NewlyAttendance,
		Cards,
		AllAttendance,
		AllManagement
	}

	private const CommunicationType DefaultCommunicationType = 2;

	private const int DefaultDeviceNumber = 1;

	private const string DefaultPassword = "0";

	private const string DefaultIp = "192.168.1.224";

	private const string DefaultPort = "5005";

	private const int DefaultSerialPort = 1;

	private const int DefaultBaudRate = 19200;

	protected const int DeviceBusy = 0;

	protected const int DeviceIdle = 1;

	private const int Arm9Size = 1404;

	private const int Arm7Size = 1416;

	private bool _sb100;

	private DeviceConnection _eDeviceConnection;

	private Device _currentDevice;

	private User _currentUser;

	private object _extraProperty;

	private object _extraData;

	private CommunicationType _communocationType;

	private int _deviceNumber;

	private string _password;

	private string _ip;

	private string _port;

	private int _serialPort;

	private int _baudRate;

	private Systemcase _systemStatus;

	private UserInfo _userInfo;

	public string ContextGuid { get; set; }

	public CommunicatinTypeName CommunicationTypeStting
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected I4, but got Unknown
			CommunicationType communocationType = _communocationType;
			return (int)communocationType switch
			{
				0 => CommunicatinTypeName.Serial, 
				1 => CommunicatinTypeName.TcpIp, 
				2 => CommunicatinTypeName.Usb, 
				_ => CommunicatinTypeName.TypeError, 
			};
		}
		set
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			switch (value)
			{
			case CommunicatinTypeName.Serial:
				_communocationType = (CommunicationType)0;
				break;
			case CommunicatinTypeName.TcpIp:
				_communocationType = (CommunicationType)1;
				break;
			case CommunicatinTypeName.Usb:
				_communocationType = (CommunicationType)2;
				break;
			default:
				_communocationType = (CommunicationType)2;
				break;
			}
		}
	}

	public int DeviceNumber
	{
		get
		{
			return _deviceNumber;
		}
		set
		{
			try
			{
				_deviceNumber = value;
			}
			catch
			{
				_deviceNumber = 1;
			}
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			try
			{
				_password = value;
			}
			catch
			{
				_password = "0";
			}
		}
	}

	public string Ip
	{
		get
		{
			return _ip;
		}
		set
		{
			try
			{
				_ip = value;
			}
			catch
			{
				_ip = "192.168.1.224";
			}
		}
	}

	public string Port
	{
		get
		{
			return _port;
		}
		set
		{
			try
			{
				if (Convert.ToInt32(value) > 0)
				{
					_port = value;
				}
				else
				{
					_port = "5005";
				}
			}
			catch
			{
				_port = "5005";
			}
		}
	}

	public int SerialPort
	{
		get
		{
			return _serialPort;
		}
		set
		{
			try
			{
				_serialPort = value;
			}
			catch
			{
				_serialPort = 1;
			}
		}
	}

	public int BaudRate
	{
		get
		{
			return _baudRate;
		}
		set
		{
			try
			{
				_baudRate = value;
			}
			catch
			{
				_baudRate = 19200;
			}
		}
	}

	public string Model => _currentDevice.Model;

	public Systemcase SystemStatus => _systemStatus;

	public ulong UserId { get; set; }

	public ClsEizon(string contextGuid)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		_sb100 = false;
		_eDeviceConnection = null;
		_currentDevice = null;
		_currentUser = null;
		_extraProperty = new object();
		_extraData = new object();
		_communocationType = (CommunicationType)2;
		_deviceNumber = 1;
		_password = "0";
		_ip = "192.168.1.224";
		_port = "5005";
		_serialPort = 1;
		_baudRate = 19200;
		_systemStatus = Systemcase.NotReady;
		_userInfo = default(UserInfo);
	}

	public ClsEizon(string contextGuid, int deviceNumber, string password)
	{
		ContextGuid = contextGuid;
		_sb100 = false;
		_eDeviceConnection = null;
		_currentDevice = null;
		_currentUser = null;
		_extraProperty = new object();
		_extraData = new object();
		UsbConnection(deviceNumber, password);
		_systemStatus = Systemcase.NotReady;
		_userInfo = default(UserInfo);
	}

	public ClsEizon(string contextGuid, int deviceNumber, string password, string ip, string port)
	{
		ContextGuid = contextGuid;
		_sb100 = false;
		_eDeviceConnection = null;
		_currentDevice = null;
		_currentUser = null;
		_extraProperty = new object();
		_extraData = new object();
		TcpIpConnection(deviceNumber, password, ip, port);
		_systemStatus = Systemcase.NotReady;
		_userInfo = default(UserInfo);
	}

	public ClsEizon(string contextGuid, int deviceNumber, string password, int comX, int baudRate)
	{
		ContextGuid = contextGuid;
		_sb100 = false;
		_eDeviceConnection = null;
		_currentDevice = null;
		_currentUser = null;
		_extraProperty = new object();
		_extraData = new object();
		SerialConnection(deviceNumber, password, comX, baudRate);
		_systemStatus = Systemcase.NotReady;
		_userInfo = default(UserInfo);
	}

	public async Task<ReturnedSaveFuncInfoWithValue<UserInfo>> GetUserDataAsync()
	{
		ReturnedSaveFuncInfoWithValue<UserInfo> ret = new ReturnedSaveFuncInfoWithValue<UserInfo>();
		int privilege = 3;
		_systemStatus = Systemcase.NotReady;
		_userInfo = default(UserInfo);
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 385);
			if (val.value)
			{
				_systemStatus = Systemcase.NoData;
				ReturnedSaveFuncInfoWithValue<bool> val2 = await UserCheckAsync((long)UserId);
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 391);
				if (val2.value)
				{
					Enroll item;
					for (int i = 0; i < 10; i++)
					{
						_currentUser = new User
						{
							DIN = UserId
						};
						item = new Enroll
						{
							DIN = UserId,
							EnrollType = (EnrollType)i,
							Fingerprint = null
						};
						_currentUser.Enrolls.Add(item);
						_extraData = null;
						ReturnedSaveFuncInfoWithValue<DeviceConnection> val3 = await GetEizonDeviceConnection();
						ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val3, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 407);
						if (val3.value.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
						{
							switch (i)
							{
							case 0:
								_userInfo.Finger1 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 1:
								_userInfo.Finger2 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 2:
								_userInfo.Finger3 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 3:
								_userInfo.Finger4 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 4:
								_userInfo.Finger5 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 5:
								_userInfo.Finger6 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 6:
								_userInfo.Finger7 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 7:
								_userInfo.Finger8 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 8:
								_userInfo.Finger9 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							case 9:
								_userInfo.Finger10 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
								break;
							}
							_systemStatus = Systemcase.UserExist;
						}
						else
						{
							switch (i)
							{
							case 0:
								_userInfo.Finger1 = string.Empty;
								break;
							case 1:
								_userInfo.Finger2 = string.Empty;
								break;
							case 2:
								_userInfo.Finger3 = string.Empty;
								break;
							case 3:
								_userInfo.Finger4 = string.Empty;
								break;
							case 4:
								_userInfo.Finger5 = string.Empty;
								break;
							case 5:
								_userInfo.Finger6 = string.Empty;
								break;
							case 6:
								_userInfo.Finger7 = string.Empty;
								break;
							case 7:
								_userInfo.Finger8 = string.Empty;
								break;
							case 8:
								_userInfo.Finger9 = string.Empty;
								break;
							case 9:
								_userInfo.Finger10 = string.Empty;
								break;
							}
						}
					}
					_currentUser = new User
					{
						DIN = UserId
					};
					item = new Enroll
					{
						DIN = UserId,
						EnrollType = (EnrollType)10,
						Fingerprint = null
					};
					_currentUser.Enrolls.Add(item);
					_extraData = null;
					ReturnedSaveFuncInfoWithValue<DeviceConnection> val4 = await GetEizonDeviceConnection();
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val4, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 497);
					if (val4.value.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
					{
						_userInfo.Password = _currentUser.Enrolls[0].Password;
						privilege = _currentUser.Privilege;
						_systemStatus = Systemcase.UserExist;
					}
					else
					{
						_userInfo.Password = null;
					}
					_currentUser = new User
					{
						DIN = UserId
					};
					item = new Enroll
					{
						DIN = UserId,
						EnrollType = (EnrollType)11,
						Fingerprint = null
					};
					_currentUser.Enrolls.Add(item);
					_extraData = null;
					ReturnedSaveFuncInfoWithValue<DeviceConnection> val5 = await GetEizonDeviceConnection();
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val5, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 524);
					if (val5.value.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
					{
						_userInfo.CardId = _currentUser.Enrolls[0].CardID;
						privilege = _currentUser.Privilege;
						_systemStatus = Systemcase.UserExist;
					}
					else
					{
						_userInfo.CardId = null;
					}
					_currentUser = new User
					{
						DIN = UserId
					};
					item = new Enroll
					{
						DIN = UserId,
						EnrollType = (EnrollType)31,
						Fingerprint = null
					};
					_currentUser.Enrolls.Add(item);
					_extraData = null;
					ReturnedSaveFuncInfoWithValue<DeviceConnection> val6 = await GetEizonDeviceConnection();
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val6, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 551);
					if (val6.value.GetProperty((UserProperty)0, (object)false, ref _currentUser, ref _extraData))
					{
						_userInfo.UserName = _currentUser.UserName;
						_systemStatus = Systemcase.UserExist;
					}
					else
					{
						_userInfo.UserName = null;
					}
					_currentUser = new User
					{
						DIN = UserId
					};
					item = new Enroll
					{
						DIN = UserId,
						EnrollType = (EnrollType)0,
						Fingerprint = null
					};
					_currentUser.Enrolls.Add(item);
					_extraData = null;
					ReturnedSaveFuncInfoWithValue<DeviceConnection> val7 = await GetEizonDeviceConnection();
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val7, (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 577);
					if (val7.value.GetProperty((UserProperty)5, (object)null, ref _currentUser, ref _extraData))
					{
						switch (_currentUser.Privilege)
						{
						case 0:
							_userInfo.Privilege = UserPrivilege.User;
							break;
						case 1:
							_userInfo.Privilege = UserPrivilege.Manager;
							break;
						case 2:
							_userInfo.Privilege = UserPrivilege.SuperManeger;
							break;
						default:
							_userInfo.Privilege = UserPrivilege.Undefine;
							break;
						}
						_systemStatus = Systemcase.UserExist;
					}
					else
					{
						switch (privilege)
						{
						case 0:
							_userInfo.Privilege = UserPrivilege.User;
							break;
						case 1:
							_userInfo.Privilege = UserPrivilege.Manager;
							break;
						case 2:
							_userInfo.Privilege = UserPrivilege.SuperManeger;
							break;
						default:
							_userInfo.Privilege = UserPrivilege.Undefine;
							break;
						}
					}
				}
				else
				{
					_systemStatus = Systemcase.UserNotExist;
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 626);
			}
			else
			{
				_systemStatus = Systemcase.NotReady;
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 633);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 634);
		}
		ret.value = _userInfo;
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetUserDataAsync(UserInfo userInfo)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		_systemStatus = Systemcase.NotReady;
		_userInfo = userInfo;
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 648);
			if (val.value)
			{
				_systemStatus = Systemcase.NoData;
				if (_userInfo.Id > 0)
				{
					ReturnedSaveFuncInfoWithValue<bool> val2 = await UserCheckAsync(_userInfo.Id);
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 656);
					if (val2.value)
					{
						if (_userInfo.ChangeId)
						{
							ReturnedSaveFuncInfoWithValue<bool> val3 = await UserCheckAsync(_userInfo.NewId);
							ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val3, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 663);
							if (!val3.value)
							{
								for (int i = 0; i < 12; i++)
								{
									_currentUser = new User
									{
										DIN = (ulong)_userInfo.Id
									};
									Enroll item = new Enroll
									{
										DIN = (ulong)_userInfo.Id,
										EnrollType = (EnrollType)i,
										Fingerprint = null
									};
									_currentUser.Enrolls.Add(item);
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val4 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val4, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 679);
									DeviceConnection value = val4.value;
									if (value.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
									{
										value.SetProperty((UserProperty)1, (object)false, _currentUser, (object)null);
										_currentUser.DIN = (ulong)_userInfo.NewId;
										_currentUser.Enrolls[0].DIN = (ulong)_userInfo.NewId;
										value.SetProperty((UserProperty)1, (object)true, _currentUser, (object)null);
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							else
							{
								_systemStatus = Systemcase.UserExist;
							}
						}
						else if (_userInfo.DeleteUser)
						{
							_currentUser = new User
							{
								DIN = (ulong)_userInfo.Id
							};
							Enroll item = new Enroll
							{
								DIN = (ulong)_userInfo.Id,
								EnrollType = (EnrollType)31,
								Fingerprint = null
							};
							_currentUser.Enrolls.Add(item);
							_extraProperty = false;
							_extraData = null;
							ReturnedSaveFuncInfoWithValue<DeviceConnection> val5 = await GetEizonDeviceConnection();
							ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val5, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 711);
							if (val5.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
							{
								_systemStatus = Systemcase.DataEdited;
							}
						}
						else
						{
							Enroll item;
							if (_userInfo.Finger1Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)0,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger1)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val6 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val6, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 754);
									DeviceConnection value2 = val6.value;
									if (value2.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value2.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val7 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val7, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 776);
									if (val7.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger2Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)1,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger2)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val8 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val8, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 821);
									DeviceConnection value3 = val8.value;
									if (value3.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value3.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val9 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val9, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 843);
									if (val9.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger3Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)2,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger3)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val10 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val10, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 888);
									DeviceConnection value4 = val10.value;
									if (value4.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value4.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val11 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val11, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 910);
									if (val11.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger4Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)3,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger4)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val12 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val12, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 955);
									DeviceConnection value5 = val12.value;
									if (value5.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value5.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val13 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val13, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 977);
									if (val13.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger5Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)4,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger5)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val14 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val14, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1022);
									DeviceConnection value6 = val14.value;
									if (value6.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value6.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val15 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val15, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1044);
									if (val15.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger6Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)5,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger6)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val16 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val16, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1089);
									DeviceConnection value7 = val16.value;
									if (value7.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value7.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val17 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val17, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1111);
									if (val17.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger7Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)6,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger7)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val18 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val18, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1156);
									DeviceConnection value8 = val18.value;
									if (value8.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value8.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val19 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val19, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1178);
									if (val19.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger8Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)7,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger8)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val20 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val20, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1223);
									DeviceConnection value9 = val20.value;
									if (value9.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value9.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val21 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val21, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1245);
									if (val21.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger9Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)8,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger9)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val22 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val22, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1290);
									DeviceConnection value10 = val22.value;
									if (value10.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value10.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val23 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val23, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1312);
									if (val23.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							if (_userInfo.Finger10Check)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)9,
									Fingerprint = GetByteOffFingerprint(_userInfo.Finger10)
								};
								_currentUser.Enrolls.Add(item);
								_extraData = null;
								_extraProperty = null;
								if (item.Fingerprint == null)
								{
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val24 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val24, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1357);
									DeviceConnection value11 = val24.value;
									if (value11.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
									{
										_extraData = null;
										_extraProperty = false;
										if (value11.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.DataEdited;
										}
									}
								}
								else
								{
									_extraProperty = true;
									_extraData = null;
									ReturnedSaveFuncInfoWithValue<DeviceConnection> val25 = await GetEizonDeviceConnection();
									ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val25, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1379);
									if (val25.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
									{
										_systemStatus = Systemcase.DataEdited;
									}
								}
							}
							_currentUser = new User
							{
								DIN = (ulong)_userInfo.Id,
								UserName = _userInfo.UserName
							};
							switch (_userInfo.Privilege)
							{
							case UserPrivilege.User:
								_currentUser.Privilege = 0;
								break;
							case UserPrivilege.Manager:
								_currentUser.Privilege = 1;
								break;
							case UserPrivilege.SuperManeger:
								_currentUser.Privilege = 2;
								break;
							default:
								_currentUser.Privilege = 0;
								break;
							}
							item = new Enroll
							{
								DIN = _currentUser.DIN,
								EnrollType = (EnrollType)31,
								Fingerprint = null
							};
							_currentUser.Enrolls.Add(item);
							_extraProperty = null;
							ReturnedSaveFuncInfoWithValue<DeviceConnection> val26 = await GetEizonDeviceConnection();
							ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val26, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1420);
							if (val26.value.SetProperty((UserProperty)0, (object)false, _currentUser, (object)null))
							{
								_systemStatus = Systemcase.DataEdited;
							}
							if (_userInfo.PrivilegeCheck)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)31,
									Fingerprint = null
								};
								_currentUser.Enrolls.Add(item);
								_extraProperty = null;
								ReturnedSaveFuncInfoWithValue<DeviceConnection> val27 = await GetEizonDeviceConnection();
								ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val27, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1461);
								if (val27.value.SetProperty((UserProperty)5, (object)null, _currentUser, (object)null))
								{
									_systemStatus = Systemcase.DataEdited;
								}
							}
							if (_userInfo.PasswordCheck)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)10,
									Password = _userInfo.Password,
									Fingerprint = null
								};
								_currentUser.Enrolls.Add(item);
								_extraProperty = !string.IsNullOrEmpty(_userInfo.Password);
								_extraData = null;
								ReturnedSaveFuncInfoWithValue<DeviceConnection> val28 = await GetEizonDeviceConnection();
								ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val28, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1504);
								val28.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData);
							}
							if (_userInfo.CardIdCheck)
							{
								_currentUser = new User
								{
									DIN = (ulong)_userInfo.Id,
									UserName = _userInfo.UserName
								};
								switch (_userInfo.Privilege)
								{
								case UserPrivilege.User:
									_currentUser.Privilege = 0;
									break;
								case UserPrivilege.Manager:
									_currentUser.Privilege = 1;
									break;
								case UserPrivilege.SuperManeger:
									_currentUser.Privilege = 2;
									break;
								default:
									_currentUser.Privilege = 0;
									break;
								}
								item = new Enroll
								{
									DIN = _currentUser.DIN,
									EnrollType = (EnrollType)11,
									CardID = _userInfo.CardId,
									Fingerprint = null
								};
								_currentUser.Enrolls.Add(item);
								_extraProperty = !string.IsNullOrEmpty(_userInfo.CardId);
								_extraData = null;
								ReturnedSaveFuncInfoWithValue<DeviceConnection> val29 = await GetEizonDeviceConnection();
								ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val29, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1545);
								val29.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData);
							}
						}
					}
					else
					{
						_systemStatus = Systemcase.UserNotExist;
					}
				}
				else
				{
					_systemStatus = Systemcase.NoData;
					ReturnedSaveFuncInfoWithValue<Tuple<bool, ulong>> val30 = await NewUserIdAsync();
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val30, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1564);
					bool item2 = val30.value.Item1;
					ulong item3 = val30.value.Item2;
					if (item2)
					{
						_userInfo.Id = (long)item3;
						ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1571);
						await Task.Delay(300);
						ReturnedSaveFuncInfoWithValue<bool> val31 = await OpenDeviceAsync();
						ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val31, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1577);
						_ = val31.value;
						Enroll item;
						for (int i = 0; i < 12; i++)
						{
							_currentUser = new User
							{
								DIN = (ulong)_userInfo.Id,
								UserName = _userInfo.UserName
							};
							switch (_userInfo.Privilege)
							{
							case UserPrivilege.User:
								_currentUser.Privilege = 0;
								break;
							case UserPrivilege.Manager:
								_currentUser.Privilege = 1;
								break;
							case UserPrivilege.SuperManeger:
								_currentUser.Privilege = 2;
								break;
							default:
								_currentUser.Privilege = 0;
								break;
							}
							item = new Enroll
							{
								DIN = _currentUser.DIN,
								EnrollType = (EnrollType)i
							};
							if (i < 10)
							{
								switch (i)
								{
								case 0:
									if (GetByteOffFingerprint(_userInfo.Finger1) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger1);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val33 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val33, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1621);
										if (val33.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 1:
									if (GetByteOffFingerprint(_userInfo.Finger2) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger2);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val39 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val39, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1640);
										if (val39.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 2:
									if (GetByteOffFingerprint(_userInfo.Finger3) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger3);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val34 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val34, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1659);
										if (val34.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 3:
									if (GetByteOffFingerprint(_userInfo.Finger4) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger4);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val37 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val37, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1678);
										if (val37.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 4:
									if (GetByteOffFingerprint(_userInfo.Finger5) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger5);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val40 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val40, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1697);
										if (val40.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 5:
									if (GetByteOffFingerprint(_userInfo.Finger6) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger6);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val36 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val36, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1716);
										if (val36.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 6:
									if (GetByteOffFingerprint(_userInfo.Finger7) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger7);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val41 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val41, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1735);
										if (val41.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 7:
									if (GetByteOffFingerprint(_userInfo.Finger8) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger8);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val38 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val38, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1754);
										if (val38.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 8:
									if (GetByteOffFingerprint(_userInfo.Finger9) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger9);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val35 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val35, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1773);
										if (val35.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								case 9:
									if (GetByteOffFingerprint(_userInfo.Finger10) != null)
									{
										item.Fingerprint = GetByteOffFingerprint(_userInfo.Finger10);
										_currentUser.Enrolls.Add(item);
										_extraProperty = true;
										_extraData = null;
										ReturnedSaveFuncInfoWithValue<DeviceConnection> val32 = await GetEizonDeviceConnection();
										ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val32, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1792);
										if (val32.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
										{
											_systemStatus = Systemcase.UserAdded;
										}
									}
									break;
								}
								continue;
							}
							if (i == 10)
							{
								if (string.IsNullOrEmpty(_userInfo.Password))
								{
									_userInfo.Password = "0";
								}
								item.Password = _userInfo.Password;
								item.Fingerprint = null;
								_currentUser.Enrolls.Add(item);
								_extraProperty = true;
								_extraData = null;
								ReturnedSaveFuncInfoWithValue<DeviceConnection> val42 = await GetEizonDeviceConnection();
								ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val42, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1819);
								if (val42.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
								{
									_systemStatus = Systemcase.UserAdded;
								}
								continue;
							}
							item.CardID = _userInfo.CardId;
							item.Fingerprint = null;
							_currentUser.Enrolls.Add(item);
							_extraProperty = true;
							_extraData = null;
							if (!string.IsNullOrEmpty(_userInfo.CardId))
							{
								ReturnedSaveFuncInfoWithValue<DeviceConnection> val43 = await GetEizonDeviceConnection();
								ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val43, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1837);
								if (val43.value.SetProperty((UserProperty)1, _extraProperty, _currentUser, _extraData))
								{
									_systemStatus = Systemcase.UserAdded;
								}
							}
						}
						_currentUser = new User
						{
							DIN = (ulong)_userInfo.Id,
							UserName = _userInfo.UserName
						};
						switch (_userInfo.Privilege)
						{
						case UserPrivilege.User:
							_currentUser.Privilege = 0;
							break;
						case UserPrivilege.Manager:
							_currentUser.Privilege = 1;
							break;
						case UserPrivilege.SuperManeger:
							_currentUser.Privilege = 2;
							break;
						default:
							_currentUser.Privilege = 0;
							break;
						}
						item = new Enroll
						{
							DIN = _currentUser.DIN,
							EnrollType = (EnrollType)31,
							Fingerprint = null
						};
						_currentUser.Enrolls.Add(item);
						_extraProperty = null;
						ReturnedSaveFuncInfoWithValue<DeviceConnection> val44 = await GetEizonDeviceConnection();
						ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val44, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1880);
						val44.value.SetProperty((UserProperty)0, (object)false, _currentUser, (object)null);
						_currentUser = new User
						{
							DIN = (ulong)_userInfo.Id,
							UserName = _userInfo.UserName
						};
						switch (_userInfo.Privilege)
						{
						case UserPrivilege.User:
							_currentUser.Privilege = 0;
							break;
						case UserPrivilege.Manager:
							_currentUser.Privilege = 1;
							break;
						case UserPrivilege.SuperManeger:
							_currentUser.Privilege = 2;
							break;
						default:
							_currentUser.Privilege = 0;
							break;
						}
						item = new Enroll
						{
							DIN = _currentUser.DIN,
							EnrollType = (EnrollType)31,
							Fingerprint = null
						};
						_currentUser.Enrolls.Add(item);
						_extraProperty = null;
						ReturnedSaveFuncInfoWithValue<DeviceConnection> val45 = await GetEizonDeviceConnection();
						ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val45, (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1918);
						val45.value.SetProperty((UserProperty)5, (object)null, _currentUser, (object)null);
					}
					else
					{
						_systemStatus = Systemcase.SpaceIsFull;
					}
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1931);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1936);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<DeviceConnection>> GetEizonDeviceConnection()
	{
		ReturnedSaveFuncInfoWithValue<DeviceConnection> ret = new ReturnedSaveFuncInfoWithValue<DeviceConnection>();
		try
		{
			if (_eDeviceConnection == null)
			{
				if (_currentDevice == null)
				{
					ReturnedSaveFuncInfoWithValue<Device> val = await InitEizonDeviceAsync(string.Empty);
					ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetEizonDeviceConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1952);
					_currentDevice = val.value;
				}
				if (!string.IsNullOrEmpty(_currentDevice.Model))
				{
					if (IsZd100(_currentDevice.Model))
					{
						_currentDevice.ConnectionModel = 1;
					}
					else if (IsZd101(_currentDevice.Model))
					{
						_currentDevice.ConnectionModel = 2;
					}
					else if (IsZd2910(_currentDevice.Model))
					{
						_currentDevice.ConnectionModel = 1;
					}
					else if (IsZd3000(_currentDevice.Model))
					{
						_currentDevice.ConnectionModel = 4;
					}
				}
				_eDeviceConnection = DeviceConnection.CreateConnection(ref _currentDevice);
				ret.value = _eDeviceConnection;
			}
			else
			{
				ret.value = _eDeviceConnection;
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetEizonDeviceConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1976);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetEizonDeviceConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 1977);
		}
		return ret;
	}

	private bool IsZd100(string model)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(model))
		{
			if (model.Contains("SB2880") || model.Contains("ZD2880") || model.Contains("ZD120SD") || model.Contains("ZD100SD") || model.Contains("ZD30SD") || model.Contains("ZD20SD") || model.Contains("ZD10XSD") || model.Contains("ZD10SD") || model.Contains("U260") || model.Contains("ZD2900") || model.Contains("SB2900") || model.Contains("ZD121"))
			{
				result = true;
			}
			else if (model.Contains("ZD2F2") || model.Contains("AC"))
			{
				result = true;
			}
			else if (model.Contains("ZD132") || model.Contains("ZD120"))
			{
				result = true;
			}
		}
		return result;
	}

	private bool IsZd101(string model)
	{
		if (!string.IsNullOrEmpty(model) && (model.Contains("ZD2F2") & model.Contains("V")))
		{
			try
			{
				return Convert.ToInt32(model.Substring(model.Length - 2, 2)) >= 3;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsZd101", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2022);
				return false;
			}
		}
		return false;
	}

	private bool IsZd2910(string model)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(model) && (model.Contains("ZDC20") || model.Contains("ZDC30") || model.Contains("ZDC50") || model.Contains("ZDC60") || model.Contains("ZDC80") || model.Contains("ZDC90") || model.Contains("2910")))
		{
			result = true;
		}
		return result;
	}

	private bool IsZd3000(string mode)
	{
		bool result = false;
		string text = mode.ToUpper();
		if (!string.IsNullOrEmpty(text) && (text.Contains("ZDC") || text.Contains("CMI800") || text.Contains("CMI820") || text.Contains("AC")))
		{
			result = true;
		}
		return result;
	}

	private void UsbConnection(int deviceNumber, string password)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_communocationType = (CommunicationType)2;
			_deviceNumber = deviceNumber;
			_password = password;
			_ip = "192.168.1.224";
			_port = "5005";
			_serialPort = 1;
			_baudRate = 19200;
		}
		catch
		{
			_communocationType = (CommunicationType)2;
			_deviceNumber = 1;
			_password = "0";
			_ip = "192.168.1.224";
			_port = "5005";
			_serialPort = 1;
			_baudRate = 19200;
		}
	}

	private void SerialConnection(int deviceNumber, string password, int comX, int baudRate)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_communocationType = (CommunicationType)0;
			_deviceNumber = deviceNumber;
			_password = password;
			_ip = "192.168.1.224";
			_port = "5005";
			_serialPort = comX;
			_baudRate = baudRate;
		}
		catch
		{
			_communocationType = (CommunicationType)2;
			_deviceNumber = 1;
			_password = "0";
			_ip = "192.168.1.224";
			_port = "5005";
			_serialPort = 1;
			_baudRate = 19200;
		}
	}

	private void TcpIpConnection(int deviceNumber, string password, string ip, string port)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_communocationType = (CommunicationType)1;
			_deviceNumber = deviceNumber;
			_password = password;
			_ip = ip;
			if (Convert.ToInt32(port) > 0)
			{
				_port = port;
			}
			_serialPort = 1;
			_baudRate = 19200;
		}
		catch
		{
			_communocationType = (CommunicationType)2;
			_deviceNumber = 1;
			_password = "0";
			_ip = "192.168.1.224";
			_port = "5005";
			_serialPort = 1;
			_baudRate = 19200;
		}
	}

	private byte[] GetByteOffFingerprint(string fingerPrint)
	{
		try
		{
			if (fingerPrint.Length < 2808)
			{
				return null;
			}
			_ = _sb100;
			List<byte> list = new List<byte>();
			for (int i = 0; i < 351; i++)
			{
				list.AddRange(BitConverter.GetBytes(Convert.ToInt32(fingerPrint.Substring(i * 8, 8), 16)));
			}
			return list.ToArray();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetByteOffFingerprint", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2152);
			return null;
		}
	}

	private string GetFingerprintString(byte[] data)
	{
		if (data == null || data.Length < 1404)
		{
			return string.Empty;
		}
		int num = data.Length;
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = ((num != 1404) ? 2 : 0);
		int num3 = ((num == 1404) ? (num / 4) : ((num - 4) / 4));
		for (int i = num2; i < num3; i++)
		{
			stringBuilder.Append(BitConverter.ToInt32(data, i * 4).ToString("X").PadLeft(8, '0'));
		}
		return stringBuilder.ToString();
	}

	private async Task<ReturnedSaveFuncInfoWithValue<Device>> InitEizonDeviceAsync(string mode)
	{
		ReturnedSaveFuncInfoWithValue<Device> val = new ReturnedSaveFuncInfoWithValue<Device>();
		Device val2 = new Device();
		try
		{
			val2.Model = mode;
			val2.DN = _deviceNumber;
			val2.IpAddress = _ip;
			val2.Password = _password;
			val2.IpPort = Convert.ToInt32(_port);
			val2.Baudrate = _baudRate;
			val2.SerialPort = _serialPort;
			val2.CommunicationType = _communocationType;
			val.value = val2;
		}
		catch (Exception ex)
		{
			val.value = val2;
			val.AddReturnedValue(ex, "", "InitEizonDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2190);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitEizonDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2191);
			MessageBox.Show("Device Initial Fail");
		}
		return val;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> GetProductCodeAsync()
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<DeviceConnection> val = await GetEizonDeviceConnection();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetProductCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2204);
			val.value.GetProperty((DeviceProperty)0, _extraProperty, ref _currentDevice, ref _extraData);
			ret.value = _extraData.ToString();
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetProductCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2213);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetProductCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2214);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<bool>> OpenDeviceAsync()
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		bool sw = false;
		try
		{
			ReturnedSaveFuncInfoWithValue<DeviceConnection> val = await GetEizonDeviceConnection();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "OpenDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2229);
			DeviceConnection value = val.value;
			if (value.Open() > 0)
			{
				sw = value.SetProperty((DeviceProperty)16, (object)null, _currentDevice, (object)0);
				ReturnedSaveFuncInfoWithValue<string> val2 = await GetProductCodeAsync();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "OpenDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2236);
				_currentDevice.Model = val2.value;
				_sb100 = IsZd100(_currentDevice.Model) || IsZd101(_currentDevice.Model) || IsZd2910(_currentDevice.Model);
			}
			ret.value = sw;
		}
		catch (SocketException ex)
		{
			ret.AddReturnedValue((Exception)ex, "", "OpenDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2244);
		}
		catch (Exception ex2)
		{
			ret.AddReturnedValue(ex2, "", "OpenDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2248);
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "OpenDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2249);
		}
		ret.value = sw;
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CloseDeviceAsync()
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<DeviceConnection> val = await GetEizonDeviceConnection();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "CloseDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2261);
			DeviceConnection value = val.value;
			value.SetProperty((DeviceProperty)16, (object)null, _currentDevice, (object)1);
			value.Close();
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "CloseDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2270);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CloseDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2271);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<Systemcase>> SystemCheckAsync()
	{
		ReturnedSaveFuncInfoWithValue<Systemcase> ret = new ReturnedSaveFuncInfoWithValue<Systemcase>();
		Systemcase result = Systemcase.NotReady;
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "SystemCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2282);
			if (val.value)
			{
				result = Systemcase.Ready;
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "SystemCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2289);
			}
			ret.value = result;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SystemCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2295);
			ret.value = result;
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SystemCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2297);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<bool>> UserCheckAsync(long userId)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		bool result = false;
		try
		{
			for (int i = 0; i < 12; i++)
			{
				_currentUser = new User
				{
					DIN = (ulong)userId
				};
				Enroll item = new Enroll
				{
					DIN = (ulong)userId,
					EnrollType = (EnrollType)i,
					Fingerprint = null
				};
				_currentUser.Enrolls.Add(item);
				_extraProperty = true;
				_extraData = null;
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "UserCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2321);
				if (val.value.GetProperty((UserProperty)1, _extraProperty, ref _currentUser, ref _extraData))
				{
					result = true;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			result = false;
			ret.AddReturnedValue(ex, "", "UserCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2335);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UserCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2336);
		}
		ret.value = result;
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<Tuple<bool, ulong>>> NewUserIdAsync()
	{
		ReturnedSaveFuncInfoWithValue<Tuple<bool, ulong>> ret = new ReturnedSaveFuncInfoWithValue<Tuple<bool, ulong>>();
		bool result = false;
		ulong newId = 0uL;
		try
		{
			for (long i = 1L; i < 65536; i++)
			{
				ReturnedSaveFuncInfoWithValue<bool> val = await UserCheckAsync(i);
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "NewUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2351);
				if (!val.value)
				{
					newId = (ulong)i;
					result = true;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "NewUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2363);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NewUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2364);
		}
		ret.value = new Tuple<bool, ulong>(result, newId);
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> SystemPowerOffAsync()
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		bool result = false;
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "SystemPowerOffAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2927);
			if (val.value)
			{
				_extraData = 0;
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "SystemPowerOffAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2933);
				val2.value.SetProperty((DeviceProperty)9, (object)null, _currentDevice, _extraData);
				result = true;
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SystemPowerOffAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2943);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SystemPowerOffAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2944);
		}
		ret.value = result;
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<SystemInfo>> GetSystemData()
	{
		SystemInfo systemData = default(SystemInfo);
		ReturnedSaveFuncInfoWithValue<SystemInfo> ret = new ReturnedSaveFuncInfoWithValue<SystemInfo>();
		try
		{
			_systemStatus = Systemcase.NotReady;
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetSystemData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2964);
			if (val.value)
			{
				_systemStatus = Systemcase.NoData;
				_extraProperty = DeviceZd100Status.Managers;
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetSystemData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 2972);
				DeviceConnection value = val2.value;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				byte[] array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.MangerCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.MangerCount = -1;
				}
				_extraProperty = DeviceZd100Status.Users;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.UserCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.UserCount = -1;
				}
				_extraProperty = DeviceZd100Status.Fingerprints;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.FingerPrintCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.FingerPrintCount = -1;
				}
				_extraProperty = DeviceZd100Status.Passwords;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.PasswordCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.PasswordCount = -1;
				}
				_extraProperty = DeviceZd100Status.Cards;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.CardIdCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.CardIdCount = -1;
				}
				_extraProperty = DeviceZd100Status.NewlyAttendance;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.NewAttendanceCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.NewAttendanceCount = -1;
				}
				_extraProperty = DeviceZd100Status.AllAttendance;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.AllAttendanceCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.AllAttendanceCount = -1;
				}
				_extraProperty = DeviceZd100Status.NewlyManagement;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.NewManagmentCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.NewManagmentCount = -1;
				}
				_extraProperty = DeviceZd100Status.AllManagement;
				value.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				array = (byte[])_extraData;
				if (array.Length == 4)
				{
					systemData.AllManagmentCount = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.DataRead;
				}
				else
				{
					systemData.AllManagmentCount = -1;
				}
				ReturnedSaveFuncInfoWithValue<string> val3 = await GetProductCodeAsync();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val3, (ReturnedState)0, "GetSystemData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3100);
				systemData.FirmwareVersion = val3.value;
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetSystemData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3103);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetSystemData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3108);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetSystemData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3109);
		}
		ret.value = systemData;
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<DateTime>> GetSystemTimeAsync()
	{
		ReturnedSaveFuncInfoWithValue<DateTime> ret = new ReturnedSaveFuncInfoWithValue<DateTime>();
		DateTime result = default(DateTime);
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3122);
			if (val.value)
			{
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3127);
				if (val2.value.GetProperty((DeviceProperty)10, _extraProperty, ref _currentDevice, ref _extraData))
				{
					try
					{
						result = (DateTime)_extraData;
					}
					catch
					{
						result = default(DateTime);
					}
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3139);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3144);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3145);
		}
		ret.value = result;
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetSystemTimeAsync(DateTime value)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "SetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3156);
			if (val.value)
			{
				_extraData = value;
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "SetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3162);
				val2.value.SetProperty((DeviceProperty)10, _extraProperty, _currentDevice, _extraData);
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "SetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3166);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3172);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetSystemTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3173);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>> GetNewAttendanceRecordAsync()
	{
		bool getNewRecord = true;
		List<Record> data = new List<Record>();
		ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> ret = new ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetNewAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3186);
			if (val.value)
			{
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetNewAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3191);
				if (val2.value.GetProperty((DeviceProperty)13, (object)getNewRecord, ref _currentDevice, ref _extraData))
				{
					data = (List<Record>)_extraData;
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetNewAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3196);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetNewAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3201);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetNewAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3202);
		}
		ret.value = RecordAllDevice.GetAllFromRiss(ContextGuid, data);
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>> GetAllAttendanceRecordAsync()
	{
		ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> ret = new ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>();
		bool getNewRecord = false;
		List<Record> data = new List<Record>();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetAllAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3215);
			if (val.value)
			{
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetAllAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3220);
				if (val2.value.GetProperty((DeviceProperty)13, (object)getNewRecord, ref _currentDevice, ref _extraData))
				{
					data = (List<Record>)_extraData;
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetAllAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3225);
			}
			ret.value = RecordAllDevice.GetAllFromRiss(ContextGuid, data);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetAllAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3232);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllAttendanceRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3233);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>> GetNewSuperLog()
	{
		ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> ret = new ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>();
		bool getNewRecord = true;
		List<Record> data = new List<Record>();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetNewSuperLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3246);
			if (val.value)
			{
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetNewSuperLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3251);
				if (val2.value.GetProperty((DeviceProperty)12, (object)getNewRecord, ref _currentDevice, ref _extraData))
				{
					data = (List<Record>)_extraData;
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetNewSuperLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3256);
			}
			ret.value = RecordAllDevice.GetAllFromRiss(ContextGuid, data);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetNewSuperLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3263);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetNewSuperLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3264);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>> GetAllSuperLogAsync()
	{
		ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>> ret = new ReturnedSaveFuncInfoWithValue<List<DeviceLogOnPerson>>();
		bool getNewRecord = false;
		List<Record> data = new List<Record>();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetAllSuperLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3277);
			if (val.value)
			{
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetAllSuperLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3282);
				if (val2.value.GetProperty((DeviceProperty)12, (object)getNewRecord, ref _currentDevice, ref _extraData))
				{
					data = (List<Record>)_extraData;
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetAllSuperLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3287);
			}
			ret.value = RecordAllDevice.GetAllFromRiss(ContextGuid, data);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetAllSuperLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3293);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllSuperLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3294);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<UserInfo>>> GetAllUserDataAsync()
	{
		ReturnedSaveFuncInfoWithValue<List<UserInfo>> ret = new ReturnedSaveFuncInfoWithValue<List<UserInfo>>();
		int privilege = 4;
		List<UserInfo> result = new List<UserInfo>();
		_systemStatus = Systemcase.NotReady;
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> val = await OpenDeviceAsync();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val, (ReturnedState)0, "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3313);
			if (val.value)
			{
				_extraProperty = DeviceZd100Status.Users;
				ReturnedSaveFuncInfoWithValue<DeviceConnection> val2 = await GetEizonDeviceConnection();
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3319);
				DeviceConnection eizonDeviceConnection = val2.value;
				eizonDeviceConnection.GetProperty((DeviceProperty)8, _extraProperty, ref _currentDevice, ref _extraData);
				byte[] array = (byte[])_extraData;
				if (array.Length == 4)
				{
					int countuser = BitConverter.ToInt32(array, 0);
					_systemStatus = Systemcase.NotAllUser;
					bool[] id = new bool[65535];
					int count2 = 1;
					for (long j = 1L; j < 65535; j++)
					{
						ReturnedSaveFuncInfoWithValue<bool> val3 = await UserCheckAsync(j);
						ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val3, (ReturnedState)0, "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3333);
						if (val3.value)
						{
							id[j] = true;
							if (count2++ >= countuser)
							{
								break;
							}
						}
						else
						{
							id[j] = false;
						}
					}
					count2 = 0;
					for (long j = 1L; j < 65535; j++)
					{
						if (!id[j])
						{
							continue;
						}
						UserId = (ulong)j;
						_userInfo = default(UserInfo);
						_userInfo.Id = j;
						Enroll val4;
						for (int k = 0; k < 10; k++)
						{
							_currentUser = new User();
							_currentUser.DIN = UserId;
							val4 = new Enroll();
							val4.DIN = UserId;
							val4.EnrollType = (EnrollType)k;
							val4.Fingerprint = null;
							_currentUser.Enrolls.Add(val4);
							_extraData = null;
							if (eizonDeviceConnection.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
							{
								switch (k)
								{
								case 0:
									_userInfo.Finger1 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 1:
									_userInfo.Finger2 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 2:
									_userInfo.Finger3 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 3:
									_userInfo.Finger4 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 4:
									_userInfo.Finger5 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 5:
									_userInfo.Finger6 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 6:
									_userInfo.Finger7 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 7:
									_userInfo.Finger8 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 8:
									_userInfo.Finger9 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								case 9:
									_userInfo.Finger10 = GetFingerprintString(_currentUser.Enrolls[0].Fingerprint);
									break;
								}
							}
							else
							{
								switch (k)
								{
								case 0:
									_userInfo.Finger1 = string.Empty;
									break;
								case 1:
									_userInfo.Finger2 = string.Empty;
									break;
								case 2:
									_userInfo.Finger3 = string.Empty;
									break;
								case 3:
									_userInfo.Finger4 = string.Empty;
									break;
								case 4:
									_userInfo.Finger5 = string.Empty;
									break;
								case 5:
									_userInfo.Finger6 = string.Empty;
									break;
								case 6:
									_userInfo.Finger7 = string.Empty;
									break;
								case 7:
									_userInfo.Finger8 = string.Empty;
									break;
								case 8:
									_userInfo.Finger9 = string.Empty;
									break;
								case 9:
									_userInfo.Finger10 = string.Empty;
									break;
								}
							}
						}
						_currentUser = new User();
						_currentUser.DIN = UserId;
						val4 = new Enroll();
						val4.DIN = UserId;
						val4.EnrollType = (EnrollType)10;
						val4.Fingerprint = null;
						_currentUser.Enrolls.Add(val4);
						_extraData = null;
						if (eizonDeviceConnection.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
						{
							_userInfo.Password = _currentUser.Enrolls[0].Password;
							privilege = _currentUser.Privilege;
						}
						else
						{
							_userInfo.Password = null;
						}
						_currentUser = new User();
						_currentUser.DIN = UserId;
						val4 = new Enroll();
						val4.DIN = UserId;
						val4.EnrollType = (EnrollType)11;
						val4.Fingerprint = null;
						_currentUser.Enrolls.Add(val4);
						_extraData = null;
						if (eizonDeviceConnection.GetProperty((UserProperty)1, (object)null, ref _currentUser, ref _extraData))
						{
							_userInfo.CardId = _currentUser.Enrolls[0].CardID;
							privilege = _currentUser.Privilege;
						}
						else
						{
							_userInfo.CardId = null;
						}
						_currentUser = new User();
						_currentUser.DIN = UserId;
						val4 = new Enroll();
						val4.DIN = UserId;
						val4.EnrollType = (EnrollType)31;
						val4.Fingerprint = null;
						_currentUser.Enrolls.Add(val4);
						_extraData = null;
						if (eizonDeviceConnection.GetProperty((UserProperty)0, (object)false, ref _currentUser, ref _extraData))
						{
							_userInfo.UserName = _currentUser.UserName;
						}
						else
						{
							_userInfo.UserName = null;
						}
						_currentUser = new User();
						_currentUser.DIN = UserId;
						val4 = new Enroll();
						val4.DIN = UserId;
						val4.EnrollType = (EnrollType)0;
						val4.Fingerprint = null;
						_currentUser.Enrolls.Add(val4);
						_extraData = null;
						if (eizonDeviceConnection.GetProperty((UserProperty)5, (object)null, ref _currentUser, ref _extraData))
						{
							switch (_currentUser.Privilege)
							{
							case 0:
								_userInfo.Privilege = UserPrivilege.User;
								break;
							case 1:
								_userInfo.Privilege = UserPrivilege.Manager;
								break;
							case 2:
								_userInfo.Privilege = UserPrivilege.SuperManeger;
								break;
							default:
								_userInfo.Privilege = UserPrivilege.Undefine;
								break;
							}
							_systemStatus = Systemcase.UserExist;
						}
						else
						{
							switch (privilege)
							{
							case 0:
								_userInfo.Privilege = UserPrivilege.User;
								break;
							case 1:
								_userInfo.Privilege = UserPrivilege.Manager;
								break;
							case 2:
								_userInfo.Privilege = UserPrivilege.SuperManeger;
								break;
							default:
								_userInfo.Privilege = UserPrivilege.Undefine;
								break;
							}
						}
						result.Insert(count2, _userInfo);
						count2++;
						if (count2 >= countuser)
						{
							_systemStatus = Systemcase.DataRead;
							break;
						}
						ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3531);
						await Task.Delay(50);
						ReturnedSaveFuncInfoWithValue<bool> val5 = await OpenDeviceAsync();
						ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val5, (ReturnedState)0, "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3534);
						if (!val5.value)
						{
							break;
						}
					}
				}
				else
				{
					_systemStatus = Systemcase.NoData;
				}
				ret.AddReturnedValue(await CloseDeviceAsync(), (ReturnedState)0, "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3546);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3551);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllUserDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\Eizon\\ClsEizon.cs", 3552);
		}
		ret.value = result;
		return ret;
	}
}
