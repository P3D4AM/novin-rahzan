using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal.FLRPerson;

public class PersonChanges
{
	public static async Task CheckPersonChangesAsync(string contextGuid, CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				await Task.Delay(1800000);
				TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
				string npUserName = timeAttendanceSettings?.WebServiceKey?.NPACCUserName;
				string npPassword = timeAttendanceSettings?.WebServiceKey?.NPACCPassword;
				ReturnedSaveFuncInfoWithValue<List<WebAttPersonChanges>> val = await WebAttPersonChanges.GetAllAsync(npUserName, npPassword, (ENSource)5, default(CancellationToken));
				if (!((ReturnedSaveFuncInfo)val).HasError && (val?.value?.Any()).GetValueOrDefault())
				{
					await CalculatePersonChangesAsync(contextGuid, val.value, npUserName, npPassword, token);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "");
				throw;
			}
		}
	}

	private static async Task CalculatePersonChangesAsync(string contextGuid, List<WebAttPersonChanges> list, string userName, string password, CancellationToken token)
	{
		_ = 3;
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			IEnumerable<IGrouping<Guid, WebAttPersonChanges>> enumerable = from q in list
				group q by q.PersonGuid;
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (IGrouping<Guid, WebAttPersonChanges> item in enumerable)
			{
				IEnumerable<WebAttPersonChanges> obj = item.Select((WebAttPersonChanges q) => q);
				if (!(obj?.Any() ?? false))
				{
					continue;
				}
				Person person = await Person.GetBypersonGuidAsync(contextGuid, item.Key, token);
				if (person == null)
				{
					continue;
				}
				foreach (WebAttPersonChanges item2 in obj)
				{
					if (!string.IsNullOrEmpty(item2.PropertyName) && !string.IsNullOrEmpty(item2.Value))
					{
						switch (item2.PropertyName)
						{
						case "HashPassword":
						{
							byte[] hashPassword = Convert.FromBase64String(item2.Value);
							person.HashPassword = hashPassword;
							break;
						}
						case "FatherName":
							((PersonBase)person).FatherName = item2.Value;
							break;
						case "DateMBirthday":
							((PersonBase)person).DateMBirthday = Utilities.ParseToDate(item2.Value, (DateTime?)null);
							break;
						case "CodeMeli":
							((PersonBase)person).CodeMeli = item2.Value;
							break;
						case "Address":
							((PersonBase)person).Address = item2.Value;
							break;
						case "ZipCode":
							((PersonBase)person).ZipCode = item2.Value;
							break;
						case "Mob1":
							((PersonBase)person).Mob1 = item2.Value;
							break;
						case "Tel1":
							((PersonBase)person).Tel1 = item2.Value;
							break;
						case "UserName":
							person.UserName = item2.Value;
							break;
						case "NoBimeh":
							((PersonBase)person).NoBimeh = item2.Value;
							break;
						case "Shsh":
							((PersonBase)person).Shsh = item2.Value;
							break;
						case "SerialSh":
							((PersonBase)person).SerialSh = item2.Value;
							break;
						case "MadrakTahsili":
							((PersonBase)person).MadrakTahsili = item2.Value;
							break;
						case "SodorSh":
							((PersonBase)person).SodorSh = item2.Value;
							break;
						default:
							throw new NotImplementedException("PropertyName is " + item2.PropertyName);
						}
					}
				}
				person.sentToServerStatus = (ServerStatusForClient)0;
				if (await person.SaveAsync(token))
				{
					await WebAttPersonChanges.RemoveAsync(userName, password, ((PersonBase)person).Guid, default(CancellationToken));
					await person.SendToServerAsync(contextGuid, token);
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}
}
