using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.InterFaces;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities.Attendance;

public class WebAttPerson : WebAttPersonCompact, IPerson, IHasJustGuid
{
	private string groupName;

	private string strEnrollDatas;

	private readonly int defPersonNo;

	private readonly int defDeviceNo;

	public List<WebAttContract> WebAttContracts { get; set; }

	public List<WebAttTaradodFull> TaradodFulls { get; set; }

	public WebAttMorakhasi Morakhasi { get; set; }

	public List<WebAttTempVacations> TempVacationList { get; set; }

	public string DepName { get; set; }

	public bool Checked { get; set; }

	public int Id { get; set; }

	public short GroupId { get; set; }

	public short TitleId { get; set; }

	public string FullCode { get; set; }

	public short Type { get; set; }

	public string DateShBirthday => base.DateMBirthday.MiladiToCalture();

	public long CodeTafsil { get; set; }

	public string Mob2 { get; set; }

	public string Tel2 { get; set; }

	public bool St { get; set; }

	public int Hid { get; set; }

	public short HType { get; set; }

	public string PicPath { get; set; }

	public long CodeMoein { get; set; }

	public string MTavalod { get; set; }

	public DateTime DateMFinishW { get; set; }

	public string DateShFinishW => DateMFinishW.MiladiToCalture();

	public short EisarId { get; set; }

	public short Jensiat { get; set; }

	public bool Check { get; set; }

	public byte[] HashPassword { get; set; }

	public string HashedQueryString { get; set; }

	public Guid? DepartmentGuid { get; set; }

	public string LatinFullName { get; set; }

	public EnUserRole? UserRole { get; set; } = EnUserRole.RegularUser;


	public string GetGroupName()
	{
		return groupName;
	}

	public void SetGroupName(string value)
	{
		groupName = value;
	}

	public string GetStrEnrollDatas()
	{
		return strEnrollDatas;
	}

	public void SetStrEnrollDatas(string value)
	{
		strEnrollDatas = value;
	}

	public int GetDefPersonNo()
	{
		return defPersonNo;
	}

	public int GetDefDeviceNo()
	{
		return defDeviceNo;
	}

	public static async Task<ReturnedSaveFuncInfo> PostToApiAsync(string userName, string password, WebAttPerson personInfo, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			WebAttPersonForPostToApi dataForPost = new WebAttPersonForPostToApi
			{
				UserName = personInfo.UserName,
				Guid = personInfo.Guid,
				FullName = personInfo.FullName,
				HashPassword = personInfo.HashPassword,
				HashedQueryString = personInfo.HashedQueryString,
				DepartmentGuid = personInfo.DepartmentGuid
			};
			byte[] data = Json.ToBytesJson(personInfo);
			WebAttPersonForPostToApi webAttPersonForPostToApi = dataForPost;
			webAttPersonForPostToApi.Data = await GZip.Compress(data);
			LoginInfo loginInfo = new LoginInfo
			{
				UserName = userName,
				Password = password,
				Source = ENSource.TimeAttendance
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await dataForPost.PostToApiAsync(loginInfo, token), ReturnedState.None, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 312);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 316);
			res.AddReturnedValue(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 317);
		}
		return res;
	}

	public static WebAttPerson DeserializaData(string serializedData)
	{
		return serializedData.FromJson<WebAttPerson>("DeserializaData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPerson.cs", 321);
	}
}
