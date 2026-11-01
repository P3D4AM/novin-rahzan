using System;
using System.Text;
using Newtonsoft.Json;
using PacketParser.Attendance.Access;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Classes;

namespace TimeAttendanceDAL.Dal.User;

public class UserPermission : IHasContext
{
	protected short PermissionUserID;

	protected byte[] _userPermissionJson;

	private AccessLevelClass _accessLevelMerg;

	private AccessLevelClass _accessLevelSelf;

	private string _password;

	private readonly string _passPhrase = "[REDACTED]"; // [SANITIZED] کلید AES هاردکد حذف شد

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public AccessLevelClass AccessLevelMerg
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0017: Expected O, but got Unknown
			AccessLevelClass obj = _accessLevelMerg;
			if (obj == null)
			{
				AccessLevelClass val = new AccessLevelClass();
				AccessLevelClass val2 = val;
				_accessLevelMerg = val;
				obj = val2;
			}
			return obj;
		}
	}

	public AccessLevelClass AccessLevelSelf
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0017: Expected O, but got Unknown
			AccessLevelClass obj = _accessLevelSelf;
			if (obj == null)
			{
				AccessLevelClass val = new AccessLevelClass();
				AccessLevelClass val2 = val;
				_accessLevelSelf = val;
				obj = val2;
			}
			return obj;
		}
		set
		{
			_accessLevelSelf = value;
		}
	}

	protected UserPermission(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	protected void ConstructUserPermission(string oldPass, short userid)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			if (_userPermissionJson != null)
			{
				UserPermission userPermission = JsonConvert.DeserializeObject<UserAttendance>(Encryption.Decrypt(Encoding.UTF8.GetString(_userPermissionJson), _passPhrase), (JsonConverter[])(object)new JsonConverter[1]
				{
					new UserAttendanceJsonConvert(ContextGuid)
				});
				if (userPermission != null)
				{
					userPermission.ContextGuid = ContextGuid;
					PermissionUserID = userPermission.PermissionUserID;
					if (PermissionUserID == 0)
					{
						PermissionUserID = userid;
					}
					_password = userPermission._password;
					AccessLevelSelf = userPermission.AccessLevelSelf;
				}
			}
			else
			{
				AccessLevelSelf = new AccessLevelClass();
				PermissionUserID = userid;
				_password = oldPass;
			}
			AccessLevelMerg.ApllyAndTwoClass(AccessLevelSelf);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConstructUserPermission", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserPermission.cs", 63);
		}
	}

	protected void ConvertToJsonAndEncrypt()
	{
		try
		{
			string plainText = JsonConvert.SerializeObject((object)this, (JsonConverter[])(object)new JsonConverter[1]
			{
				new UserAttendanceJsonConvert(ContextGuid)
			});
			plainText = Encryption.Encrypt(plainText, _passPhrase);
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			_userPermissionJson = aSCIIEncoding.GetBytes(plainText);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertToJsonAndEncrypt", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserPermission.cs", 78);
		}
	}

	protected bool CheckPassWord(string pass, short userid)
	{
		if (!string.IsNullOrEmpty(_password) || !string.IsNullOrEmpty(pass))
		{
			return _password == pass;
		}
		return true;
	}

	protected void SetPassword(string pass, bool isForce)
	{
		try
		{
			if (isForce)
			{
				_password = pass;
			}
			else if (PermissionUserID == -1)
			{
				_password = pass;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetPassword", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserPermission.cs", 98);
		}
	}

	protected string GetPassword(short id)
	{
		if (PermissionUserID != id)
		{
			return "";
		}
		return _password;
	}

	public void ToJson(ref JsonWriter writer)
	{
		try
		{
			writer.WriteStartObject();
			writer.WritePropertyName("PermissionUserID");
			writer.WriteValue(PermissionUserID);
			if (!string.IsNullOrEmpty(_password))
			{
				writer.WritePropertyName("Password");
				writer.WriteValue(_password);
			}
			if (!string.IsNullOrEmpty(((object)AccessLevelSelf).ToString()))
			{
				writer.WritePropertyName("accessLevel");
				writer.WriteValue(((object)AccessLevelSelf).ToString());
			}
			writer.WriteEndObject();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToJson", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserPermission.cs", 128);
		}
	}

	public static void SetUserAttendanceProperty(JsonReader reader, UserAttendance model)
	{
		string text = (string)reader.Value;
		reader.Read();
		try
		{
			switch (text)
			{
			case "PermissionUserID":
				model.PermissionUserID = Convert.ToInt16(reader.Value);
				break;
			case "Password":
				model._password = (string)reader.Value;
				break;
			case "accessLevel":
				if (reader.Value != null && reader.Value.ToString() != "")
				{
					model.AccessLevelSelf = AccessLevelClass.FromString((string)reader.Value);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetUserAttendanceProperty", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserPermission.cs", 153);
		}
	}
}
