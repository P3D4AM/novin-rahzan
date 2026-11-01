using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business;
using PacketParser.Entities;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.FLRSanad;

[Serializable]
[DoNotPruneType]
public class Sanad
{
	public List<SanadDet> List { get; set; } = new List<SanadDet>();


	public string Description { get; set; } = "";


	public DateTime DateM { get; set; } = DateTime.Now;


	public static string DirectoryName
	{
		get
		{
			try
			{
				string text = Application.StartupPath + "\\Sanad_Hoghogh";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DirectoryName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 127);
				return "";
			}
		}
	}

	public string NextFileName
	{
		get
		{
			try
			{
				string text = "Sanad_";
				for (int i = 1; i <= 1000; i++)
				{
					if (!File.Exists(DirectoryName + "\\" + text + i + ".xml"))
					{
						return DirectoryName + "\\" + text + i + ".xml";
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextFileName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 149);
			}
			return "";
		}
	}

	public Sanad()
	{
	}

	public Sanad(string path, bool isRemoveFileAfterRead)
	{
		try
		{
			Load(path, isRemoveFileAfterRead);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 28);
		}
	}

	private void Load(string path, bool isRemoveFileAfterRead)
	{
		try
		{
			string text = File.ReadAllText(path);
			if (isRemoveFileAfterRead)
			{
				File.Delete(path);
			}
			List = new List<SanadDet>();
			List<string> list = text.Split(new char[1] { '\n' }).ToList();
			if (list.Count <= 1)
			{
				LoadFromFile(list[0]);
				return;
			}
			for (int i = 1; i < list.Count; i++)
			{
				List.Add(SanadDet.FromString(list[i]));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 53);
		}
	}

	private void LoadFromFile(string fileContents)
	{
		try
		{
			List<string> list = fileContents.Split(new char[1] { '\t' }).ToList();
			if (list.Count > 1)
			{
				Description = list[1];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadFromFile", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 67);
		}
	}

	public bool Save()
	{
		try
		{
			string text = DateM.ToString("yyyy/MMMM/dd HH:mm") + "\t" + Description;
			foreach (SanadDet item in List)
			{
				text = text + "\r\n" + item;
			}
			File.WriteAllText(NextFileName, text);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 85);
			return false;
		}
	}

	public static List<Sanad> LoadData(bool isRemoveFileAfterRead)
	{
		List<Sanad> list = new List<Sanad>();
		try
		{
			string[] files = Directory.GetFiles(DirectoryName);
			if (!files.Any())
			{
				return list;
			}
			for (int i = 0; i < files.Count(); i++)
			{
				list.Add(new Sanad(files[i], isRemoveFileAfterRead));
				File.Delete(files[i]);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 107);
		}
		return list;
	}

	public async Task<SanadBusiness> ToBusiness(string contextGuid)
	{
		_ = 1;
		try
		{
			SanadBusiness sanad = new SanadBusiness(contextGuid, 0)
			{
				DateM = DateM,
				Description = Description,
				Guid = Guid.NewGuid(),
				Id = 0,
				Modified = DateTime.Now,
				IsTemp = true,
				ContextGuid = contextGuid,
				SanadSt = (SanadSt)1,
				Status = true,
				SanadType = (SanadType)2
			};
			SanadBusiness val = sanad;
			val.SanadNo = await SanadBusiness.NextIdAsync(contextGuid, default(CancellationToken), "");
			List<SanadDet> list = List;
			if (list != null && list.Count > 0)
			{
				foreach (SanadDet item in List)
				{
					_ = item;
					await sanad.AddToListSanadAsync(sanad.Guid, (Document)50, (EnSystem)0, default(CancellationToken));
				}
			}
			return sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToBusiness", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\Sanad.cs", 182);
			return null;
		}
	}
}
