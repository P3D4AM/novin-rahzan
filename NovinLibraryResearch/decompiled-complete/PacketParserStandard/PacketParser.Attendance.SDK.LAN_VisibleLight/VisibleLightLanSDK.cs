using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using Serilog;

namespace PacketParser.Attendance.SDK.LAN_VisibleLight;

public class VisibleLightLanSDK
{
	public class DeviceConfigRequest
	{
		public string pass { get; set; }

		public int alarmDelay { get; set; }

		public int firealarmSwitch { get; set; }

		public int isCheckMask { get; set; }

		public int wgOutputEnable { get; set; }

		public int wgOutputModel { get; set; }

		public int wgOutputFormat { get; set; }

		public int repeatOpenTime { get; set; }

		public int faceRepeatEnable { get; set; }

		public int faceThreshold { get; set; }

		public int faceLiving { get; set; }

		public int faceDistance { get; set; }

		public int Volume { get; set; }

		public int Language { get; set; }

		public int displayStandbyTime { get; set; }

		public int lightEnable { get; set; }

		public List<LightTimePeriod> lightTime { get; set; }

		public List<DoorParam> doorParam { get; set; }

		public int ioModeType { get; set; }

		public List<IoModeParam> ioModeParam { get; set; }

		public int dstEnable { get; set; }

		public string dstStart { get; set; }

		public string dstEnd { get; set; }
	}

	public class LightTimePeriod
	{
		public int beginTime { get; set; }

		public int endTime { get; set; }
	}

	public class DoorParam
	{
		public int doorWorkWay { get; set; }

		public int appKeyButton { get; set; }

		public int timeGroup { get; set; }

		public int doorMultiUsers { get; set; }

		public int closeDelay { get; set; }
	}

	public class IoModeParam
	{
		public string ioModeName { get; set; }

		public int ioModeTime { get; set; }
	}

	public class UserData
	{
		public string pass { get; set; }

		public int accNo { get; set; }

		public int opType { get; set; }

		public int isManager { get; set; }

		public string password { get; set; }

		public string cardSN { get; set; }

		public string endExpDate { get; set; }

		public string beginExpDate { get; set; }

		public int timeGroup { get; set; }

		public string name { get; set; }

		public int faceId1 { get; set; }

		public string faceImage1 { get; set; }

		public int dept { get; set; }

		public int fpId1 { get; set; }

		public int fpId2 { get; set; }

		public string fp1 { get; set; }

		public string fp2 { get; set; }

		public int palmId { get; set; }

		public string palmInfo { get; set; }
	}

	public class Logdata
	{
		public string deviceKey { get; set; }

		public string reqSeqNo { get; set; }

		public int logType { get; set; }

		public int accNo { get; set; }

		public string passTime { get; set; }

		public int operation { get; set; }

		public int alarmCode { get; set; }

		public int passStatus { get; set; }

		public int doorId { get; set; }

		public int readId { get; set; }

		public int temperature { get; set; }
	}

	public class LogResponse
	{
		public bool result { get; set; }

		public string message { get; set; }

		public string data { get; set; }
	}

	private readonly HttpClient _httpClient;

	private readonly string _baseUrl;

	public string Password { get; private set; }

	public VisibleLightLanSDK(string deviceIp, string password = "")
	{
		Password = password;
		_baseUrl = "http://" + deviceIp + ":14460";
		_httpClient = new HttpClient();
	}

	public async Task CheckMethods()
	{
		_ = 1;
		try
		{
			await SetLogCallBackAsync("http://192.168.1.101:800");
			await GetDeviceTimeAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task<string> PostAsync(string endpointAction, params string[] args)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		checked
		{
			for (int i = 0; i < args.Length - 1; i += 2)
			{
				dictionary.Add(args[i], args[i + 1]);
			}
			StringContent content = new StringContent(JsonSerializer.Serialize(dictionary), Encoding.UTF8, "application/json");
			HttpResponseMessage obj = await _httpClient.PostAsync(_baseUrl + endpointAction, content);
			obj.EnsureSuccessStatusCode();
			return await obj.Content.ReadAsStringAsync();
		}
	}

	private async Task<string> PostAsync(string endpointAction, object data)
	{
		_ = 1;
		try
		{
			StringContent content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
			HttpResponseMessage obj = await _httpClient.PostAsync(_baseUrl + endpointAction, content);
			obj.EnsureSuccessStatusCode();
			return await obj.Content.ReadAsStringAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<ReturnedSaveFuncInfo> SetPasswordAsync(string oldPass, string newPass)
	{
		new ReturnedSaveFuncInfo();
		try
		{
			var data = new { oldPass, newPass };
			await PostAsync("/setPassword", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<bool> IsConnectAsync()
	{
		try
		{
			_ = new
			{
				pass = ""
			};
			string text = await GetDeviceKeyAsync();
			return text != null && !text.ToLower().Contains("false");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public async Task<string> GetDeviceKeyAsync()
	{
		try
		{
			var data = new
			{
				pass = ""
			};
			return await PostAsync("/getDeviceKey", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetOEMAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/getOEM", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetDeviceTimeAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/getDeviceTime", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetTimeAsync(DateTime dateTime)
	{
		try
		{
			string serverTime = dateTime.MiladiToShamsiDatePlusTime().Replace("/", "-");
			var data = new
			{
				pass = Password,
				serverTime = serverTime
			};
			return await PostAsync("/setTime", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetDeviceInfoAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/getDeviceInfo", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetConfigAsync(object config)
	{
		try
		{
			new DeviceConfigRequest
			{
				pass = Password,
				alarmDelay = 1,
				firealarmSwitch = 0,
				isCheckMask = 0,
				wgOutputEnable = 1,
				wgOutputModel = 1,
				wgOutputFormat = 0,
				repeatOpenTime = 1,
				faceRepeatEnable = 1,
				faceThreshold = 50,
				faceLiving = 45,
				faceDistance = 0,
				Volume = 8,
				Language = 1,
				displayStandbyTime = 60,
				lightEnable = 1,
				lightTime = new List<LightTimePeriod>(1)
				{
					new LightTimePeriod
					{
						beginTime = 1730,
						endTime = 730
					}
				},
				doorParam = new List<DoorParam>(1)
				{
					new DoorParam
					{
						doorWorkWay = 0,
						appKeyButton = 1,
						timeGroup = 0,
						doorMultiUsers = 1,
						closeDelay = 5
					}
				},
				ioModeType = 1,
				ioModeParam = new List<IoModeParam>(10)
				{
					new IoModeParam
					{
						ioModeName = "On Duty",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Off Duty",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Lunch In",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Lunch Out",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "OT In",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "OT Out",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Tea In",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Tea Out",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Pray In",
						ioModeTime = 0
					},
					new IoModeParam
					{
						ioModeName = "Pray Out",
						ioModeTime = 0
					}
				},
				dstEnable = 1,
				dstStart = "05/22 08:01",
				dstEnd = "06/22 08:01"
			};
			return await PostAsync("/setConfig", config);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetConfigAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/getConfig", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> RestartDeviceAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/restartDevice", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> ResetDeviceAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/resetDevice", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> OpenDoorControlAsync(int doorId)
	{
		try
		{
			var data = new
			{
				pass = Password,
				doorId = doorId
			};
			return await PostAsync("/openDoorControl", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetEmergencyControlAsync(int doorId, int controlWay)
	{
		try
		{
			var data = new
			{
				pass = Password,
				doorId = doorId,
				controlWay = controlWay
			};
			return await PostAsync("/setEmergencyControl", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetTimePartAsync(object timePartCollection)
	{
		try
		{
			var data = new
			{
				pass = Password,
				timePart = timePartCollection
			};
			return await PostAsync("/setTimePart", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetTimePartAsync(int index)
	{
		try
		{
			var data = new
			{
				pass = Password,
				index = index
			};
			return await PostAsync("/getTimePart", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetTimeGroupAsync(object timeGroupCollection)
	{
		try
		{
			var data = new
			{
				pass = Password,
				timeGroup = timeGroupCollection
			};
			return await PostAsync("/setTimeGroup", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetTimeGroupAsync(int index)
	{
		try
		{
			var data = new
			{
				pass = Password,
				index = index
			};
			return await PostAsync("/getTimeGroup", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetUserDataAsync(UserData userData)
	{
		try
		{
			UserData data = new UserData
			{
				pass = "",
				opType = 1,
				accNo = 1,
				name = "shafiei reza2",
				dept = 1,
				password = "111111",
				beginExpDate = "1404-06-20 10:00:00",
				endExpDate = "1405-03-23 18:00:00",
				isManager = 0,
				cardSN = "123456789",
				timeGroup = 0,
				faceId1 = 0,
				faceImage1 = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAoHBwgHBgoICAgLCgoLDhgQDg0NDh0VFhEYIx8lJCIfIiEmKzcvJik0KSEiMEExNDk7Pj4+JS5ESUM8SDc9Pjv/2wBDAQoLCw4NDhwQEBw7KCIoOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozv/wAARCAKAAeADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2aiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACikpCcUALmjNM3Um73oAkpC2Kj3Um7FADy1IWqPdQW5oEP3Cm5x3pm7imk+tICQtg03d70wtzTS2aAHlqbuphbmmlvegBxamFsd6aWzTWYUCFZqaWx6UwvTGfJzQApbH1qvqFnaazYPp+oIGjflHH3oz6ink96jYtjPbNAHmGs6VeeH9R+y3vzwv/qJwPlkH+NVNwI+U5Ar1W/tLPWdOew1BN0Z5Ru6H1FeX6xpV14ev/sl2NyN/qZezirTGQFhg96QuOM5x7UyRtr/MR7YqIyKM5Yc+tUImLYPIPNAfqKhEgJwDk9aeu58bUY/QUASg5BB6mgyAAZ+lEdvcsuEgJ9zxUq6ZeuuTGqA9CWFMRGH7HinBs8elWV0eViA0wVj14zU8elxBSryljnqBjFMDOdQ53nGQMDPSmIAGwcY9hWymmWqBhtaRiP4jViO3jXgwqAOgFArmFHED1JOOyjJNTRW8h+7FM56j5Dmt6NBzhV9iQKeA5YAnj2poTZlw6ZLKQ3llcdS/GPwqwmmy4K71JBweOBWj36mnqvGemeaLk3PU6KKKwNwooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACikozQAtJSFsGk3ZoAdQTTd1NJoAcWphYUE1GzUAKWpN1MLUhagB+4+tIDx1pm6k3YoAkzTS2KYX9qbu45oEPLYpC1M3UhbP1pAPZsDpTS2fWmbs03cOaAHFvemk+1NJppbH1oAUt1HemMaGPy5puT1IoACaaSPWgn04phOe9AhDTSefYdqCwwc0wknBzTADyCc9ahv7G01fTnsdQj82Nh8jfxRn1BqUn0qMsc9ccUAef8A/CLpayNBLK0gQnDdyKE0+1i+9ag9lLd/wrq7yHe27YCzdD71kyW/UsOR2J6GmguZ4jiUDZAijsNop+SG3Z6dsYFWTGS3AGcdqYU9emOwpoCHnd0xnsKdt7/lT9q7R8uOetO8slu2D0qiSNep7EDj3pSp7HqamCDnOcelKEIByKYiNUwRzmpVX3yD09c0oHc81JGgLcLjdzmmIZtJBFTBcgHtigRjcOcAelShMLnJIzTExFUEc45FOVRjnpnAp6RMMDA3Ec1MsXAXp7UhHo1FFFYnQFFFFABRRSUALRSUZoAWikzRmgBaKTIpCaAFopu6jdQA6kzTd3FJuoAfuo3UzdzSFqYD91JmmbqC3FIBxOaTIxxTc+9ITQA7NBNMzSFuMUAKWIpjHNBqNjmgALYNIW5phPrSbqBjt3vRmmbu47UmaBD88U0txTS3HWmluKQCsfTtSEnPXim7u1Nz7UAP3YpCaYWOSKTcaAHFvem5pM0maBCluKYTQWHWmls0ALnPXpTegpC3cc47U0nv+lAgPFM5PT8qXccYI60xmI4FMALAniozgk56U4nA45phI2kmgCrOCXAIyKpTRDBZfmX09K0XIJ4qs8ec+9MRnNGV+9x9KiER5P3j7dqvumc4zg96YYgGPPBpoCmBgjI49+lAjAU471aMQUY2g/SjyAq4JIpklfysqG3FfbGc09YmK5VcD3qfyvlB9u1OEfy8nk807gV1jyDtXj1xTxGMjnAA/OrAjcKDj8KesQPG3oe9MkiSMjCgEr14qQQ7iOSKm8sL908/Wrtnpk9ztAUAZ5c0wKKx/NtIyT0A61oWWh3N0Vdw0ceeSRya3LDRYLX5ioL+p5rVVdoxUuVhqNwozUe6jdWZqPzRupm6kzzQMk3Um7mmZpN3pQBJu5oJ96Zu7UmRQA/PekzTS3tSZ9KYDycUm6mE0Z4oAduzRuz3pmaPpQA4sKM03PNFADyaQ03OKM0ABopM0hOBQAuaTNNyaTIoGPzmkJpufWkJoEITzTGbjg0rHjrUTEA0hiFvakzzzTdxzzSZ54pAOLYyPWk3d+lNJ55pM0CHE96aT1oz+lMJx9KBik8ZzSFu9NLUmR3oEPzTSaaWAOcU0kDnrSAeWx36UwNn2prHmmk54pgPPHfim7h+FMzRnmgQopM5PrSbuvrSZFAATnnNIfSjOfakyec0CE6dKaQehP5UZwPrQTTAjcDoRz2NRFRjrUx+Y4phGDwKYiBk7g4Peo2QZyRmrBX5jznPamFec4oAh2Dr0pNoNSgZOMUbR2/SmIYqc8AY9KcEBPzAYpxUjsakjt3l+VBz+gpiZEFxjbxnoKtW9rNO4WNSWzitOw0Zx80zZ9Bitu3tI4gAqD609hbmXY6EiDMwBY9a2YoFjUBQAB2qQKBTqlyKUO4gGKWiipNCsWozz1pufekzQIfnvmkLehppbkU3ce9AyTJ+tISabu9OKQsaYEmaM81GGoxzQIfmkJ/Cm0p57UDHD1o70najrQAuRRTSfzoNADqKZupQTQAueaDz3puTQTkUAKTSZNJnimkkHFAx2Tjmm5pDmkyaAFzQcdaTNIaBCMeM1Exp7VE54pDGe9JupM8009etIBxJoJPrTC1N3etAh5I9aYTknnikZuM00tg4zzQMUk49aTdmm5wRSZxQA4mm7j6Zo59etJkYpCFz2zTck8UhNIccUwF9qTPNISM9KOtAhetNJ+bjn3oOcUY49KAE70mcjApdp6gUbTjOOKBDc4xSHNOEeRz0pwTklsjApiIjwM00jjOam8nP8WKPJJB5piK2DzkUm0nGOv8AKrnkADg59vWnLAHb5RzjHSgZQCsOvWnCCSThFP8AvVrR6X5mN7E49K0oLFEIJXPFUkK5j2ujs4DSPx6Ctq1sIoUCqgyO9WkhCgACpgoFNuwrNjUjwOafS0VDdzRKwUUUUhhRRRQBQzmlzxzTKM4oEOzSUmaTNMY/NJTRmloELmnZ/GmjnqaO/FADhSk44poNGcigY73pM00Z/ClpgKfakzRSZ5oAWjtSE0ZoAKM8YoH1pKADOaCaKaecn0oAO1Npfemk0gFpDmjPamk54oAax4NRSNxTz82cDNRujsRtH4Y5FIZEW+Yg9B3pAH342n8qjuri0sIpZ72YIsS7iueW9hXmmreOPEl3f5sbv7JaucJEig8e5NAz090kQcrzUU9za2sXmXcwgXvurzG28aa5ZXHz37SKOGVgMH6VHrOsX+syxyzXKyfLxGMD86QrHdf8Jp4badkF1JkfxmM7fzpJ/GOgRfP9oLJ/ejGc/hXljLIXIIYY7GpBtAB2DzAMc0Dset6Xrek62n/EvvVMgPKP8rD8DWh9lkOSCGI9DmvESzRZeL5X6ketIs92JPPt724SVfmDLIRj8Kdgse1lGHBBA+lM5xzXn2n/ABH1y2slguVgvJBwsssZ3D64NdB4f8eW2qXCWGrxR2dw5yky/wCrc9gfSiwmmjodpIzilEZK5GKtNbyLj5dwPcdKa8ZU8/jSJuV/L9c5pwjyOlThDjjNKI/UEUwK4iDEj9TSrFzyOe+as7AygdKd5WenJosK5V8r0+UfSnCHAyT+A71ZEWBnk+tOEPOSAAO9AFUREHnjNHlc8gGrghLH196cIMHgU7ElNYiOcYz7U4Qknp19KvJbEnn8jViO1C9VHNOwjOS0Zsdj6VciswByozVxIgvGKeFouGrIkhA7CpQuBTgMUtJstR7iYpaKKRQUUUUAFFFFABRRRQBmZxSA5zSZ4oBpiHZHOaM+9NH50uaAHcUUmaTIpgOBpQaYeOaXIzQA4mkpPeloAcOeKO9MzSg0AOOQM9aTOaQEY6GigYvajtSZ4ozxQAZxRmk7UmaAFzSdj2pCeKQn1oACabmgkA0mwsQeApP3icUgF7+lDqEQu8qRr6swFZuu+JdP8ORgSE3FyykpGnNeUalq13qu9rtiGkfccE8DsKAWp6V4s119ItobTTsPqFzja/VYwe/vXHX3jHVpbWa2bUEWaHrJGNpcjtXO6xeXNxqME32l1EUYCBe3GKo8Yzkbic5NItIuXWtX1+ivcTtMzHB3cCqkokRN3mMcHpnioipj6Rkg+lTHaYRghlcfrQBH5qSAb0V8HkGoWVHiIhQAk8iklYxtyAccE01XHBxx6UAWvtMs/lLgh0Tbk98etOMjlQHALdARUDMyKXTk4qOO9kidW2AjHII7+tAFwYdec5HQ+lNOY/mRuO+KI7jzDgph+5HSmy5Emd49iKBkzorIXRsD9RUYHmMqTEnB/SoCxL7snPep1YSnH8Q/i9aYGpaeIdd0xvMsNVm8oHmKUl0OOxBru/D/AMRdO1RWi1wR6fMqgq65KP6/SvNoCqhlY53c807MVuVZgJEYYwvUUEOJ7Npms6Jq0xt9P1KKaYfwcg49s1pmAq2w14NlrO6S5sZCrp8yuDgj2rv9A+KESwpb69bynBwLqIBsD/aHX8RTIcWjuxHyMqOlOEXOSKfA8N3bR3NrIs0MgyrqeCKkKnv1pk3IhGMY7U5Y16AVME9aeEB96AIVi56DFSrEBwBgU/aB/hTgKVxDQg7U8ClopXKSACloopFhRRRQAUUUUAFFFFABRRRQAUUUUAZGfSlzTPpTgaokUmgGmk80UDH545oyc5puaM4oAd160U3NLnmgBQfU0uabmlzQAuaORSUUALnPWlLU2gHHNAC5FGfSkzSUAKTz1oJpKTNAxc+9JzikNHyBS8jhETlmY4AoAXaBG8kjBI0GWZugFef+PPENvqUtvpFjMZLVP3sskTffPYcdqd4/8ZhoW0fTJMxuQssmPvew9q8+l8yyUyOWUv8AJx6UrjSHvdNd3CmMv8jYQFieKctxF/a++MFo+ASfWmyMtk8MgTdGw5Ye9Vo4pFfzI3V41PX1FK5Qt2We4Z2Y4zxzxUSkljtzu9KdJIjMNx2FuRjoaQXbxx7ExyepFBRLukVSu7CYIqGN2XOMAE5qNG3AtI4B7VIFiP8Ae/CgQSNHLwxO71FMCt1xlelOMKBMgbT25zQ7FFBGWHU4oASNxwucc0yWP58dCefpSnY2ZgvB68dKG+bkknPegQ2N/LxuAz6jtVlSPuucDqGqsUzwBSo5GFAyewBpjLEy+WwIyykdcdKBtwGPTsR1FOEwnjIPBHYDpSbTGcZ+tAiUsQQ3UY6ipUKzMAQFNVlBVNwPy96VZVR8OdpPRu1AFiNiuYZACV4B9RS27Rq7K+CDxSbfNTK/M38OKjaBgSfuv6HvTEdH4b8T3nhC+COHnsW5MQb7oPcV7Jpmo2WsWKXthKJYpOQR1B9D6V4FbyC5QQzf61RhfcVqeGfEF54X1AyQB5Y2bE1uTwy+o96CXFHugA6Yp3Ss/RNd0/xDp63unzCRDw6dGjb0YdjWhikRYAKWilpFJBRRRQMKKKKACiiigAooooAKKKKACiiigAooooAxc+lGaQGlzVEi0A5pM96M0AO9qOR3zSZwRQPXrQMXPNLTc8UvUUALxS5/Gm4pc4oAWlyM9abmge9AC0UUlAAaPYUUc0AH1pKWlVGdgqjNADDnHHJ7CuZ+IOsWVnocemvOEurh1Yx5wdo9av694v07w6XiIFxdqvyop4B9DXkes30viG8+3XUi/aiSMH07AUmUkM1SWOKbzSWbLAHjnGO1RahIbmFFi5xzzTLiQzIFcc9/YionuXQ7Ci7wPve1SWhY7qRICu1XVgPkPaoWcA7l+UtxjNOaJtglMq9OlRonmuQXVcdzxQMa5JPzH6e1NJbPtT3iKkgsrEenQ1G25FyRx6UAPCgqT0py7QMZ2+lRorbcjBDDPHcUiOCWUths8A0ATyQyxqHK/Jjk+lM3Mp254H60glk27XZivcHvUhjyQ6txjoBTENyqjjjPWoxkZbPTqKmChupwR1qHzMM23gUAP+XYJEJ5OCKYDtkDqoGDk00HGBTyobBRhnHKnvQIlBAIkHfqO1Ojc/dOPYmq6NtyD36inHjkGmMso3OB36insBINp69s1VVycE9Qak8xm2kgEZ7dqBEkMslsxwvGMEH+lTeakkgRmOcZU+lCTJcBlkCn0z1/OpfLO0LsDIOlMQNFKHDRnDKAQexq/bTw3Me1kEU4GN3bPvVVFIQKWznoO4p4tnCbwcDOCaBF+wvr/wANan9ssHCykfvIz92QV6/4Z8S2viXThPFhJ04mhJ5Q/wCFeOQRC7JWafaEXCn1PpU1jfX3h7VFv9PO2VeJISfllX0P+NO1xHu1LWfomsWuu6VDf2rArIo3LnlG7qfcVoVABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQBh/Sj8RTQaXAqiRSfWlzTc+lGaAHZ/Okz0ptKKYx+aA3bFNzS57UgHEmlzxmmUuaAFzSk0zkUucmgB+aTrzSZpc0ALRSUm4g8igB3U4HNZHi/W4/D/hu7mW4Ed46hIlHLAnvU2v+IIPDOhS6nKN0pOyCM/xueleNaxrF3q0Uvnyl3kkEkrds9hQOKuRgves8lyS8jLyT61T8sKAMYI6+1KqvJELm3kJeNwsoP6HFOM8MpKyMVkHRcdahmtiByxMnUDPPv70tz88SMVzwMHuRSBxvbOVDcEelL88sSxEDKcDFIZEnBwoyfQ80u0Z4XrVmKPaDlRnHrS+aRxtVh6Y60XHYq4Yfw89qkVA52ruVu4xwaR48NtQNz1GelIolRsxswxxRcVh5EUTgxcevFIwQkkqAW6E96iLMWIz83rTWIb7wzigCVojkAH5sZx6imKx2sDwR+lIT05zSqdpxwQR0piHRyLKuMbX7ehpkgHAx17igAcbDgg8D1pTG3DDBOeaYDMrnpjtmmkK6g9CDSuOckEZpvUcdKBDzk8k5NCtjAJ4pRzgEd6k8rKklTgeh5oGMXkkgAZpQ2CR3p6QZGBlcdN1KIJMbuoB5yaYrDQ4bAKjPrU0bYIKkqfSodjIfnIHsKdjdjnvTEaMLAIG3nzB3NWIZpCpVn3lm4ArMRiQMHr19q0bGVYXaROXxtVj0X3pkk+6Ms3lg7sjdjsavXqefYNcQyAmA8kd19KxxOILsOoG08N7+9X7GSTE4LgrIeU7EUxM2/Bmu/2LrsC79lrfMI5gT8qt2b2r2OvnyVMxNGBh+q+1e3eGbw3vh+zleQyOIwrsepI60pLqI1aKKKgYUUUUAFFFFABRRRQAUUUUAFFFFABRRRQBz4NLnFMBxS5qiR2e+aM03NLnNAC+9KDzTaM0wJMjtQeaZ2pd3YUhju1AP50gNA4oAdRx9KTP40uc0ALmlzTQaO9ADs0dOp4oqC+1G20qwm1C9bZb26lnf1x2HvTA4T4x3Ev2bRbVVxF5zOxPXOOK4BwJCQAdtX9S1m88V6jJe3TMqE/uI3bO1R/Wq0n7uV4iRsC8c96hmqVkMiaOG1nYcHeCeevFMhgN1KJSBtzSwQtcStEvKY5z61t29uscIXpgc4rKUrGsY3MqSyRd2OSx6moWs545AVkUsB09a6VY1frgD09adFCiE/IMdhio5zVUzk3gugSWtyPQjoaWNJVP+qbjqa64RJk7lU56cUwxoBhY1X1wOtLnD2Zzywgxhm5YjgUhVlUZBGT0NbU9srgMF2r0OKoyQ7G6EgDr60cw+Uz5bUNwQA2M1C1uD3xxjAFabW5ZgducDrmoWQ7cFT1wQRVKRLijLdDGMkcdM0m3jcCeecYq/wCWhzEw6etQyW7KQUIMZOCAOlWpGbiVSvTBye9KJG28Hn1p0gHmEqpXgfSm7SWPQEDp61VybCGYnr831FJjA6flU6BZYUUgbs804Rjnah+XqB1ouKxXD4bGPm64qaC4KuA68etOkiy2ApPHfqKjMBI6cEcU7hYtx7ZVLpg4pZGEeFbHIDYqgIZQxwGA9m6VNDaSzNnDKMY59KdwsTqFcl8cHgKRQ0CKoLJznqO1WorLYA+Tx3HNSmJiNpO4HnPpS5h8pnmJgNoXGO/agAoCQ+fb0q0yZIXJz9OtRPHsbtg1aZDQ2M7hhhVu3nH2fBAEsZ65+8KppwCGbvSoC8yrgnccADvVEM23RmXdld7DO0GvRfhjqcM+iyaczn7TbOWZD/dPQivPpriNo1jiwAoAKr1B9Km0LVX0fxLZaghbDSCKeNejI3H6daLXRJ7lRSKwZQynIIyDS1mMKKKKACiiigAooooAKKKKACiiigAooooA5vgc0ZNNznvTu1USOBHWkyfSkBxR+FMB2eaXvmm57UA9qBj6UGm9qXPakAv4Uv8AWm5pR1oAcPSjvSZ5pe9MBe1KKaOtLnmkAoPoCT6CuA+KGqFbqHRt/wAskYYxZ6Nnv+lego6QJJcO2EhUu5xyAOTXhdzqR8S+IrvXJyVV5MxIx+6q/dFJsuKuyO8WKK4S2hGHRAWPvVHcZZtuRg56c1Iry3MjTDb5shIOeABV60s4lbarCRgPmx2rNs1Suy5ptosEQZyS2OnpVuMZJB5A/Q0IqogUH6n1qZF2/eOQa5pO7OmKsLkAcjNGScds0cZ6cUpPIAXIqTQOdxzx6UhAx196dy3tjtTCGIwzZFAxrHcMiq7qXONvGec9qsttA4BwKhbDyBQeSPyppisVRFkE5I544qGVCsmDnDDjNaO3kAduOagmi3L82Q4JIqrkNFIxENnPbjPemGMGM78kk89qvBN4znt3qJo92FORincVik8e0hMZU+ozim/Zkbgx5HUMD0q7tIfb0z1xR5RA+UDB61SZLVzKe1dHZ4xlc4FLHJKo2mNtw9B1rU8kse2AetRyW5BJK5A6YPNHMTylPcXcjYV/3utPEa7DuOCOoqcJGSCR83Y1OqSsPuKw/lVcwcpDFDGqgoSR6VaCKVXIwBzim+Qu4MRyOmDUvlg4GcGlcLCbBt4GB6UBO/4VLjHvSgZ9qdyWivJDkEjnPUmq81sjHKpnjgmtNFEmQoUY656VItoJcDJ5/u9qtSIsc46bF4U9cUsQA/i+YHIPcVs32ltEcq4dSOe1YbjYWB/hPFaxlczkjSgdpkPAyOc+tWNyxxR5Uhics6nOKoqjeQkkEhIx8wq1C8YaMyZCFvm9hVozZ6r8P9cbVNJktp7jzri1cgnGPkP3a6yvMfCF1b2PiWFYpFWG4Ux4A+8T939R+tenVElqCCiiipGFFFFABRRRQAUUUUAFFFFABRRRQBzAHtThgVHnmndDVkjs0Ckz+VGfSkA76UvU00Z/CjOKBj8kdaAabmgHsaAHg0vamA0tADs06mD2pwNADqQnjqcUgobp9aAMbxfqf9k+Fb0wj99LEQCey9DXjNqGSzjhCkELlvc16Z8UppYvCUG1OLify39SOtedRvHFEGb7uMcdamRrDYmgiUQAsoO7naeMH3NaVqiLFvVNrEc8Vl2aPeSlz/q0I4PStpV+XI/KsJs6YIfGDkE9DVjqOuahjU54HJ6+1TAEYGcVgboNmO9C/Mfl7daGjRzgk/UGgQREEfNz70hikgcb1z6A803dTxbQ7gSmcdD3p/kx4Csox7UDIeGOOlIQcADsak8iPeANyD1FHlMCQH6diKVwsRt2H6+9QuAchiRnrU7ErgunHU4qNcSMCVyvemIqkFSf7meDSEgyFqsGPknpz0pnkFjv6dsDvVXIsRrnrShOeMDFWEtjkhueOmOlOFrGCDs2+/rRcOUqqkaDKgDPWnBRuJ7HrgVcMSBSFG3JyTS+XkdcfSi4cpU8lDEVXgk5NILcKCAcH2q6yRBuOoFMkVCCd2M+tNMLFTYQOfzpVQnjirCrG2AD+tSm2yAT932qrkNFRULA8HIp6RAAlgMn1q2LRhl85B7ZpfKOQdoJHr2qrktDEiVkBUY9RirVsFMoCqoyOSahRwhy4O0jqO1X4MMhYgYHT0xTJZDNbANvI3Bz0Nc5qNgkVwxAO3PA7V1bSHaAOMdyaxL+IkYZ+pwDVweplIybMCObBcFSMZ9DVh0hEgMgLKB/D/OquCkwDYxu6irccjoAVIK7txBHauhGLNKwl8m7so45shbmJgSMEDcO9e2V4VMPPia6tmX/AEdgx9xmvcLZ/MtYn/vID+lKQkS0UUVAwooooAKKKKACiiigAooooAKjmlWCF5XztRSxx1xUlIQGUqwBB4IPegDl+c8U6o+tOzxVkjgaB9abkdzS5pAOoBpvt+VLn2oGKOtO+tN9qXgUAKM4pQaaSMYpR9KAHZpR0xTRSg0AOBHpSPkgDt3pM0vJYY70Acn8VV8zw3aOcCK3mViPVjxXlZaSeTe4GCcYHevQPjFqRistK0lCd9wTNIccbQcD9a4TT42mkOFJ2jgGolob01oadnGy4UAADsK1UjUJw3GMYqC2gSLBGQTx1qwE4IzXJJ3Z1xVhwIUYHXHanYPQmmyyQwIpllSMMOtVDqluj4Mykjpk0rNlcyRohB1GeKOV5HXvVFNWgMeDOitnn6VE+sKm4xuWXPRcEmjlYudGqn3iOc9TS+ZGF5Jzn0rH/t2ybkTMsvoUINO/tRZCHWQhgcAEDn8KHBjU0axlwoBGMmmysQ2F4x+tUPtRY5Pr1Bo+0bm27hzU8rL5ky2S5JPX2phTng4Peo0lXuSfQ5604SDPXmgB+44AzSA4kDdvSoxKQSMA470pl9sUCLaMu0kYAPrTWJLjGAoH61BG5II61KpABpAOwX4CnNSqiquDgkjmoVlPfp0FKHUNnPNMCY4DAnGPcVMfLYkSRoR3qk02OV+YnrmojKzMePxqkJmibTT3wCjIe5U0LpcR+aC6lj7YI3VS8zJwpIq2s4ZTyeep6ZqiGO/s6/ifMMsUw68nbQRqCZabS5Md2XBH6VZhnRlwH6Doe5qYSMCGViDj1pkGbGUkO0oVHUqRTzI0bHZggj1qdyrYaUZbpkdaY1qWU/MCo5yDzTJYOyvtdenT8ao3fORIMx5znuKuwqI/3chGMZBqOcQ7PMChuMMoq0Zs5u6tpLc7gRJHngn0p0ErRyrnDK2QRVyZYZFaIAgYyB6e1Z4VYR1OwEZPpXSjBkkUTjzFjOwsjHHbjmvc9Auftnh/T7nIPm26Nx/uivD7y3eNDhiVdMhumRXq3wzmM/w+0pmzlUZDn2dhRLYDqqKKKgQUUUUAFFFFABRRSMdqk4Jx2HU0ALRTUcOoOCPZhginUAFFFFAHJg0uc0zPOAKdVki5FLmmjvk5NLmgBwb1pcg00HApcikMcKWmfSlzQA7NKDTaUHigB1LTc4ozQA7NKOtN/GnL94c0AeX/ABdnR/Fum2oG77NZ5J+pzWRoMQe2e6bIfeVAx2rV+KkDP4/iMbbCbFCx/E1HaQrDYwxKeNufesaj0OqktB8anfjOT7VFdXgtfl25bHIourkWsTEct7HpXPSSTPIxMh+bndWCjc3vYsXN2WfMinPYHoKqSBJnGIwWH8VHlOMl3JJPVu9OFo0iYJJPtxW1kjJ3bKhCo2W5b370wu2Rnjd0ArQ/s5W7MMc4Bpv2BlUhhlOw9Kd0KzKZZwwySfr1p6zSAgMxeP1I6VYNkgz5eQO+eaBZgNlCyt3B6UrodmMiuXR9yuy4P4fiKvR3UoX5uQeQQetU2t33DOePTmpk3BV647DHapZauXobpWZdpZc9jV6NwQ7YywrFjUvk5wQcrWlauFyT0JzispI1iy4euRTGc57e9IXIJz09KYDzxUFFiIn+E8nnmpn4OO5Haq0JK5yeKfLNhOOnrQA5pMfLwdvoeag+0YPJwe2TVKaQhSe/ds1TkuSzAIWYjrx0q1EzcrG0LkcnH45qWKdAVO7kc4Fc4buU5AcHPaka8nPVzjtwKvkIczr0ulI2sowR1Ap6yL8sYwABxuPWuPF4/wAvzHI7ZxzUi3rgYY57n0FVyEuZ16y8leDx0Axg1YjvYYyFkUlmGAorl4dRfIEpxnkEGrAv2YZ4YHnd3FLlYuY6Z/KulXaSrD25qvG3kyOrBgynBU96q2GoCUHawMiHPPetOQDUYhsbZOvI3DrSasO5DOgGJfXsDmqxCq2T909RU6Fkl8uRNjr1FQT/ACybQOn8VUiWZ86kSs0fAUcr61RDCSMqFOHGDmrjZEgYHBPJPvVO5nDXoKReUpwCnqe5roic8tyzEwbS5YJw29f9UepHtXpPwq3jwFaK5yVklA9vnNeXCULkkDjOD6V6z8Nk2+BrF/8AnqZH/N2q5bEnU0UUVmAUUUUAFFFFABRWXfa/baZLtvLe6jjzjzxCWj/EjOKtWepWWoRiSyuobhe/luCR9R2p2YWLVFJWVrkusW0BuNKSKcr9+GQckeo96QGo5ZVJRdx9M4qodRQvJCkcgnT+FkIB/HoRWJpvim5miJuoE3Dn5MjI7j603XdfsrzStttcMk3mL8hUq3vVcrRn7SLWhGDnmlzTRS5zTLFz1oB4pPwoB5pAOzS803OKDyaAHZ96cDTOc0ooAfRmmg8Uc4oAfnilzjpTQaN3rQA4kYqSAK0ygnHNRVYsQGu03Dgc0gPHvFxm1Dx7dzM5YFljAz0QHpWgWRSQowqDAAHSsaFzqHiu5lU5QSyFWz2B4rVum8qCQ56dcd656m52wVkZN9IZmZQo5PJquq4wD83HTFSyN5jFgNo9KFkCnofekixGCxrul2gDoGpglmmP+iWjuq/xngfrTbKFtRnknmJW3jbamf4j3xW0pSFS0USr7nk0nIaiZgstXkxh7WMHrt3Mf5VE1hqCSfvJVwP4ShFbXnzFMpcMnckAVHL50igteOd3fAzUcxVrGOy3CffmCgf3l4pm9s/OFz6qetW7nzRuG9JR2DDGKoqAh2hSuf4f8KtMRMOe+aUqOetMjYHJHSpN3y5HftQIbtyoU8Vft1IUZ6k1UGCy8dav2oIzxgmoky4kxTsTTGGDnj8Kl7Z/ComBD7T1xmsyhVJCjA69qhnlHllerUF2U7Q4/KoJGIztxwcZq0SyvIAcZ428VXx8rLHwD69zU7jHBOT1NNIGQfzFaoyZXEDNgfLnu2aabd8Hkccc96s7VHJbtTC/IRF3E1aZDRWMMhYAKfY00xSK5HI9RV1XmX5sBexHpR9oKuFkjB98U7kuJUWSSNsPk98VMlyU4X5gTnmpwIbhRwM9B6imm3G4Y+UDg8dapMm1ixb3hST5xjP3WBrodM1R5Pkf7y/dc9a5MwuEwRuI6Yqa2uZYXBU/OP1pONxpneS/6TGHHLAYLCqZ+YZ65qrp+qjCuQVVxiSPr+VWm2K5VCceorO1iihdRnyC64yDWfNO5twrfMqNnB61rzKpVlJ61g3fD7QcKw2mtoMymhLsbYGYH+DNez/D6MReA9IUHINuG/Mk/wBa8R1A+XpshBJCrjPrXv3hu0+weGtNtCMGG1jQj3CitZbGbNOiiisxBRRRQAUUUUAIRkYPSsHVPB2mX8jXNuHsL3+G4tm2EH3A4NXdYi1c27y6RcxrOB8sUyZRvxHINcTF4w8URzOlwLLzIjiSF4WQg/XNXFPoJy5TWMXjfRZEm+0Q65boMNEFEcmPX3NU7jxhqcl3ugQQrjDWk6cqe/NPj+I9zGoW50CRnxy0E6lT+eDVW81z+3ruOSWzECgbEjJy5JPc09b6oxnNNaMVNWS4vIftFrBa+a4TzYs4UnjJFa0lvJoV/FcXMRu9uSrRrz6dPWs+68K3X2V5dqps+bYXyce2B1rSOsJb21uL6QT3kQ/dKq9fdqsxi113IwaM5FNHFLk4xmsjsF5HelB44FN+lO7UAL2ozikB5o96AHZ44pQaaCM46UoNAxwo5zSfSjJ7UCH0Cm5p3akAoNOLtDZXkqk7kt3II7fKaYDxVbW5Xt/CWsyxnDi0cL+VAzyjwrHutmmB+7Hgn3J5q5fysqBGGVJ7fpUHhmI2/hzcW/1rAgn071ZlXII4weATXLN+8d0VoZbPgkbce9RSMzIwVsMRjNWZIguVPLdc1XkBRGzhcjrmhDLsWFihC8LgAVaM0cK5mbDgZOa5+XVJGkWO1X94OAD0oTT7q5kZ7q6bJblBzRy3HzWNs+IrKOPCZZCeRwTVWXXLKdiUdUwMcqRXOzwGC5kjVd6g8HFOihQtJ5j7BsyOM81SpE+0NmSUE70IlVhwymoHBZQC1Y6l4troSGzzV/zJEAaZSM9DQ42KUrkiHaxwcY6j1qwJkYD+HsB1qqDuGeM0HK8E7aQy3kgHHB+ta9i0UjiMnc4HQVhBmDoB83OOa3tIB24CYZ8knFZT0LjqaS2ccilQhA9Sajexi3Fl3ZUYxmr42gAAHpzQqB3wV3ZGKwubcqMOeFET5TnHfvVGZUU8EdPyrUv8q5yu0gdPSsGec7yCM8ZJJreLMZKwjN+dBcKV3c57Cq5fJyuQD600M5PbH0rZGLLDvnp0HvSxgj/ZJGc+1QIQQcj+lW0lj3q5U8Hke1AiaOLjmQH8Kkyu3BjUr0wRUf2i1jUs8gLE8KDzUwFs3+suY+Rwu8ZoAglt7dxwgjPtTGilhQnPmIO/cVoTWtoUUQSl8jkE52/jVbYQ+McjiqTJZXHzrlTuFAR1YZAPOMUYFvPnAAc4I9Ksc4wTn3qiCazlMZAJ6nGCK3pHDGNgCuF5rAgZ94+XcAeTWvCTNA2DyDyKljROF3vhxnI+U46Vz2oIq3BUdRXUwNyoBOMd65/U7YrO5zkr1x/OqhuTPYj0/TJtU1XT7CJVbzbhC3oADk/oDX0D0rw7wnpl3q2oSJZXiWzRRlhKxIwfYivQ/Dk/iPR1NvrjxX9mD8t5E+5o/Zh3Hv2rdq5zOVnY6+ikBBAIOQayPEN/dW1mbXTtv2+4RvKLfdQDqx+mfzqErjbsrmxRWN4Skmk8LWL3Ll5TH87k8scnmuUl8e6jq3jSz0nQkj+wmcxyzMu5pQv3mHoox1p8rvYXMkrnolFFJuXdtyM+makoWsXXfC1hrwEkpkguFGFnhOG/H1/GteSSOJC8jqijqzHAFcxrXjSGEfZ9GaO7nP3pOTHGPXI6n6U1foS7dTAm8Fa5pRMo1Czu4E53SAxv+XI/Wn2M8dpI7z2yO/QHP3T7VRury+uTvnvpLhzzhz8o+gHFQl55EzIVc59MVsl3OaUU3obE+qzMMLI6IRjCyGo7e4iZxKF+Ze7DqapRRvGyiaNgR/Ca0ERJ3VXZViBG446CtCeVI0B096XtSAnilrmOwM80uSB1pAM0uRQAtBNNpaBjh+tHvSe9KOnWgB2c0uabn0pRSEGSKcDTaUcmgB3asvxjdiy8D6lIT80wWFB6liP6ZrTHFcv8Spymg2EGPlnudv8AX+lJ7FR1ZztvEsGm2sAX5VjBNBiGGIc5PQEcVK4MUaKRwqheKZgE98jtXE3qeiloVmsUbB87n6VWlsFkfy/vcck9K1RblvmLKF7A9apahdQ2M0SPIIjIOCelJMdjOezt0JiYeTjo6jpTvJuYnVlXeh/iTkGrM6CV9zk8cqRWe6zRNkysCTxsYj860TJaKerxSw3a3cf+oIAkUdQazTcRE8Pnca3EvplJSQqee6jmmPdxglRbRHIznaMitVMzcDMtoDezLGCUQHMjY6CtW6uIjAYvJJB9qYLoHjG3PtR5hLbDzk1LlcpRsVYk3A7ZFQAfxDNTQFJpFjfr3arAhG35wvXoBU4USJhIlVgRggYqHIuxF9kDTqmep+X3rodOCog2fKCQvPtVGGBYAsmeWHG6tGx2cMFDKBx9awm7msI2NAnJ4GKfbMftSIP4jj6VC7lo/erWmHy7lJmAb5TjPSoRq9EZOtpi4kCHJzgmuWukZOBwM10t9N9omklUHDuWAI7Vh3kAf5sHPXArWGhhPUoBWY4Vc8UhKQjDOC3qOamXJHlt8ue4pBaOm/YqMh6ZPNbpmJA0ztNtQbnPqKfLBcLaPPP8gQZ4qaNZAyuyAfSrbvHdWckDZG9MYPrVIiVzBBVwW+bI75qaWLyZAvmoWAGdoyKhyY5DCzYZODgU8S+U5fIK+jdxWqMmT2d5cRP5UZVgwJ2sOtaNvqEbgxzr5cg5yTwRVfTrWKYm8IaNei7u/wBKlu4bYoArZlHvUuw1cfd+XJyh3DHBFLC+VGfoarKkqgLuwB3xVpEGMkg0gJkcrx2xwa07R/m4JO5e1ZkQJxg/XNaengMGBfaMYzSYGnCrbwoOBnJqhq/m22pmdUDRlMNz2PGKuQnBUsTkfrVXXAGlhbnDgg/0pw3FLYt+EPDVv4hs763t7hreSE/JIV3A57EVPaaP408F6ibu10wXdoOLhIJwyTJ/uHkEeoH5itr4baYbnwZdvFMYbi4vJCkyHlduAv4cdPenaz4p1VdKm0SSMwaw8iQebGPl2swBf24zW7ZxySvdna6bcpdadDPFDJEroGEbjDLkdCPauM1PUZZfEk7Sr5ciL5McJOcr1OPrmrei+Lbg6zNpeotFstsp56jAfBxuNN1bSJ7jxX5qphJSskUw52kAc/pU3sJvmiUPE3ioeHvB1pplsCuoXkRRFA/1SdC317D3qT4ceFZdGtzq+pQ+VcTRiO3hb70UfXn3PFc9c65bzeJbvUgtt9tGI1klQukRUY+Uf55Na9l8Tr5bTybvRGuLtQQssThY3PYkHkfhmq5tCE9deh6J56hWdvlVerNwK4zxpq2g3EkFvJdk3KZIe3YkqD2JWsGZvEGs6dNq+r3ZNpGwUQR/JGM+38X1NQS28duIt9og3gMCAM4pWHzt6EqxPPCh/tGeeIHKpK5YD8DVe/1BkPkwqjHoxUYFU7i9dJXSBTGoOCvrUSuZCA6sueeBWiEo21JY555GCiJFz3Ga0Uaa2CE7dy+ozmqsYtXRd/nK3qozirE1sbaFZPNE0JGQR1X8KsGW/thlgIHDk9D2+lMju/KfKN83fmsuW6IXKsMkcUxLlmkGeQO9NMOU7g8nmlB54pobNKK5zoHZ5FHem5556Uvfr+FADs8UuaYCDS0DHUopopehpAO78igUmc0oOOtAC0vvSCgHNAh2Oxrkvighex8PlT8v2wow9yOP611ox1rE8b6eNQ8Nxz5IayuopB+eP60nsVH4kc1dgiY8gD0qNOQQKS4kEkxODmljb5B8w3elcMj0ok2cjgc1V1KyS9t9s8QdG7d19xVxQTgmkCgsT68VFy7GW1rthCwMCqgDYf8AGqTo6Ejy2Ue9bU0EZJaNChPXbVOS0OCyS7x3zVKRNjKkTcMtH24JHWoPIdztSMYAHLHjNarRSDO0ZHYZqHyJZZNw7jnJrRSFYoC1IP70jOe3apkjVRhec9z1qZogcjdjFSQW7ykCPJC8k+lNsLEMULyOB0H860oodi9efapIbdEi3Bfm7FutSFcY46d6xlI0jHuNbAUqR8vSrNmoGQdxCjoBVfG9wMZHoKvW8JRC2SGb9KzZoid2QIvXkccVpWRxZFDjGPmJ9KzZNpwVcE4q/GIpNP8ALB+YJtJ9/pTiEtjnLjcsjFhxgYqiy/Jx19DWtqURQoEyOmR9Kz9uSSRVmbMySE7t3UZpoO1SvbNaEijd6j0qq8JT5kOUY8g9q0TMmis4bqpH0NCy7MZXk1IUxwrZ579qCB91lBrS5DRFLJBNHiZMkHqBzUaWtgP3mHbHPzVKYVL/ACAAe9SLbqTlmxxgbe9O5FkIZVfaq8gD7uelPQHOEHPrRFaRQtu3Ek+tSouDtQHPrTuKw8rv4JJx3pyQlm4wB6nmrMFtmPnOT2x1qyto46IAR2ouKxRjt3B24yOpNXrMN5nzKArcEU1VGSuOe4qRAA/vSuFi5vZNvy+2e1Q6oIjCMswk/hB6Gnq5eMoDhlHFUb8mbyldjtxgN6GqjuTLY3fBPiQ+HLMWk8W+Ga5Zmx1UEDkflWx8RreOfS7TxBZMHRGEcrp/cbofwbA/GuK03zo5XmEiK8YMYLDO4kY7+1bOn62LbTLrSdRLLb3UTRhyuU3HofY1pc5ajSdjMhuFOmySs4DcxkA8kEVPb+Kb2y8E/wBiRzSG9DmOOY8lYcdQfXqKoW1m6acrTuiN5u0v1AX1qG6a1F48dpcrcwoMecFKg/QGkjJIhto1h8tUBbZ82T3NdDHbaumjHUINJeeEtjcB8x9wOpFYQkENsWALuQcMBworqvCVlr9/pyagPEN5b2KDalv649PQVWhD7lXSvEZgtJtM1TTb7y2cPskTaMjnoccVWuLufVLw3DFYy3CpuwAPStXVZ7y8xFcXclxEhIDNjj3rOjstNjs0nl1MzyI3zWiRlTj/AHqpNMcSv9guWBkEQVe53d6jacJtV3Zf1p1/rEsimKBVgi7Rp2+p9aymlOVIfc59R0rRGhqvqAA2xEZx1Iqs93K4278A/eHrVAybVxycdakDgkAjrTuKxN85OeTg96du7A4HrUL3RDbBgD1p6zIYwW4JpXKSPRc9qUc03PpS9eKzLFzQDk0goHAoGPB7Uo9KZnjJpeSaAHkknIJx6UUgPHSlBFIY7JNFNB707NAhQacPemg0ZNAh4NZniouPD2FkKRvMqsv9/vj9K0Qe1YfjRm+wadGGwDM0hHrgY/rUS2Lh8SOXYgyFh6U6Mrj7oyT19Ka2WABHucCkDAr8oOa4mejEsrIQNo6UoJJzmq4cdfSl3ZGS3FSaIkfeh+U7h6VSlOGOY5FP04qUzBB06HtQ8u7jkr6nrSHYplwWI+YY74qGR8D5SQc9e1aJAdcKOD61JDaQqf3i7m6gHpVKVhcpRt9OaUiSfcUPOMYFaCBEjCxJtUcDFTsQdoY9eMU0sIIzgYBOKlybKskQuwDYb5c01m+TA61FJKCw56npmnx/O230qRosW0SswkyQOwq8mdmcc96itLfKLkbQOtWiFWMseAOgpMtELY645PGK0YEwrNtRCoyQeprMPK5Ofc1etbiOQMk7cY+8RVRFLYy73l95425z7VRcgDKgH6Vq6qiyTsUbcjAcgYrCRyrMpGMEjNWZMWRCNzcjJ4B9KjUgPyAV9DT2lByp5OKYE3MCSfwpkkEttsl3oAVPXnpSbFKYIOfWr20NH865Hoe9QSW527oweD0qlIloqfZTklGH0NAglXoxbI6DtUx3K5UqRUkTFfx74qrkWIkgkdsfLkVdtrCRpAGmSP6cmnIhzgKpJ/WrcNnG5BkhVio9TxRzC5Se2tBGQfNfcv8AEQMVbxtXbIQP9r1qCLyUGzayD1zUoJUYxx2ouKxUmRXfeRg9iB1qIjD5I98VdmOVIPP9KqsASOOcVSJYhHz57+tZ+pyhykQbBdwhx2OavMSF3NyBzWesT3niKGKOMuP9YSOi4q4mctjTt/LdmsFVfNHK7uN59B71DFqbwMbTUrOSLLYXcuR+dZ93NePfZIyUfYsaLyDnr9a6iyuoNas2s71A8qj94jdTj+Ie9M4ZvUyY4FjV2RvMt2+/G3b3qvpmm/bvE0FjZxFoSu9vTrU91aT6PMSrmW0Y+udn1rQ8IXOPH+ntGAqTQSxkL0JAz/SmiSnrdkYfF9ppMjrBA675ih2jbz1/75/WtFdWe4vItO0tZJi/yJHBnbj3bpiszxHGmp+M9UlmJ2RyCJfoo6VZj1FNLWMW48soPkWM/doWxFro1Z/CGpFJb3X9dh0yxtxnyLEFhj/aZhyT6Yrm7/UraZithbtDAPulzl2Hqfc1Jq+sSam4e/llIUZWEvlc+pHTNYryb3ycKG6LnkVpFG0VoE07N91dv160hYdjzjtUcrpuEe4+5FIxQfdDcfxHvWty7D921cZ2/WmB23MwIJA6VEW3OeSRnjNBmWIgnscEDk0XFYtKR5WXzuYdKSOaPBAGMdz3qk1w0khYL2wNxoc7hzSuOx68OeaXqc03NAP/AOuoKHA+lLnjnimhvajmgB45wQfwpeetMBH/ANelGcHmgB4570bqaOwo6c0DHg07rUYzjNOBwOaQh+QBml6UzORilzgCgB4PNc141eBr2wUzgvFE26MD7uTXSKeRXL+MWWXWFUjBSIA4HU4qJ/CaU/iMCWVt25W2qBjio1Yp91j9M0M2IcsQcnFRFtp+7+NcjO9FjexAHH1pM993SoizAjjH0oB7etTYtEpwWyTjPU06NVyW2scenOaijO+Mg8sDjGO1WUBUqQMYHAFSy0TxxqPmxj2qXGR6Go1ZQCMnPU0hkjdSAxNIoWV1HzNwO1ULi4VpCpLDpjJ4p00zsNuQEPpWdczLvAI56U0hMsRgyS56c8A1fhAilySARxzTLS1EUCk/NIR37UyaYB/lYbgccilYWxofacKojcjucdKRrgzAbpQSOxrPE7gjO0/hSPMyr8w4J6ijlHzaF/zWKBGb5c06OUxsMOSO1Z0dwPMw33cYxmmtcOGYnLDPGO1PlFzGuJzKGDZJx1NUJIMAqRuYHIpsN0Y87s4xnOMkVbTZOQQQyt3q9iXqY94rLKGxjpjPenI+5gMcNxWje6ctzbsiMxdTuQ+mKxbKbzVGSd2eg7U9xGkApjABzzUiDHQjNQQEbNuck9KsIox05qQGtCkh3MoJNOMEeR346E0qrlsAhR6mjy/LXAff3zTJY5RGgABwfSpBIckK5B781VZ2ODuxUZY5BJJP61RLLxuDnbuAPrQkpjYgng+2apE54yc46U9W56mqIZpCUsm9Tj6VGwyeWy3YZ6VWVz5RDOQQfug0/Kou8Hd9PWmQPwDwSAD1BpBLFp87ADbOVBHPGKfBA1yY4pSUEjAZHUCq3iW3bTvEklrISdkKEEe+aroStXYi1ORoNWjnjdk89Q+5f4WqD7a0dw7hmNxENxbGN1WpbZb6whMl19n2Meibi3pVIf2el6kJmllkbKluAoHvTjsclaNpGwt3FdW3mnABH71fT/6xrObbZXlvc6fI0NxbyCWM5ztP+Bqjb3ccMzRswIbK9at77SfSIIY7aRdSjndXkByJUP3fxFWjDYsSXtxeXk9/OySXFw+5ti4X8qgWawktJCzzi8VsqcfK3tSfZpvIOSsCd2PUVSm2xIWc/L0jGeW96pFxjcWaQsd7bVPX6VWBRFdt3bOTUV1ckDy41AYj5jnOPaopJGaIqR1A6CtDSw95A5+XH496VWYgDAUDqc1Au0D7vSjzSSFA47mi4yVyOiuDkU0KccZAHeoRknlsAHtU0UGVO4M248KW/nRcLAojiOWBY+5qRiijPJz1PpUyRKp5G0Dr3pY4YpJCy8A9KVx2PXhpd9/z7v8AlS/2dej/AJdn/KvExp8KHKtIPpIRT1tSuSt1cj2Ezf407BY9pOn3gz/oz/lTfsF2P+XeT8q8eSS9j4j1XUEHot04/rUovtXUYTXdWX/t9k/xpWCx659kul6wPn/dpBazjrC/5V5J/aGtkf8AIxauPpfSf409dV12Nvk8SauPdrt2/maLMLHrHkTA8xNj6UGKU9I3H4V5UuveJE5XxJqOfeXP86lHibxUvK+JbwHtnaf6UWY7HqGyQYyrE/SjbJkkoQPpXmX/AAlni5Rg+Jrn/v3H/wDE0HxZ4wB/5GSYj3hjP9KLBZnp21+uD+VKAcZIrzT/AITDxanI8QF/QNAn+FOHjjxguMapCfrbJ/hSsFmelp94DFcd4wQprzTOxAkTgDstZJ8f+MFTIvrM49bRc1O99PrOnw6prE6TXTggiJdoCgnGAKzqbF00+YqMyRr5gJIUccVFE0hCyHJLDkAUx1WZnZDhSMBT2FSyiTdEsR6jn6VzHahryPk5baFHT1NJE7NGGxgkc8d6WUK0b9OMU2HLDAGAnWkWi2u7IOOe2KnDLgHJB71AjjJOeMZqRMsuWHNQy0PLqinAyarzTchsYJH4UsrkEgDtVN5SBy2T3NCRVxks5AKqMjtmo7RFa6R5eVDZOabEhmn64VTyfarxgUwkqAMHg1bEPlu1ydjDI5qsJRJNjhS3QZ61n3DSZYByCeM46Vny2W5cqSHByGzg04xIkzpGcqAM4GOSahZyOmWHuaq211LcwATg+cowxxw3vSlxkAAr61SiTclExByowSOvWlSQLnaeT2NVg/b0pzSIg3yOowM7e5p2E2WUlMbZ+Zv9k81bgupAflUqF56Vzd1qN3KTHbo0CZ4Ycsfx7UkRuxhzNKxI5LMTTcNBKR2keo7sA8HsOgNZd8n2bUfMQBFuBuwOmc1StrqQABwD6nuKnmk89Y85zHk5zWfLY0uXEwQORmpvMC4YgkD0NULeTIYN1FW43TCgKQQOvalYC0G3DPXIzg0hPGRgCo8sgyePrTSwIIB7dqCWDMMcnPpURYHPTikLEewqCSRIn9c9cU0SyUkgdTkU8S5HAxkdqgY5Ax3penI/KrMmWI8xQrICCDwQfWrkTqIm7knI9qoQvtfBXI64PQGrluzXU0QiQLJEScY4cdTmmSX9NVbu5ggun2IGLGQdQPeuZLNJe3BeSRyrsoLtnAB6V0+n3BC3cq2yySyE7dvG3PQD2rM0vwrqshk8/wAmGWMkuskmCCeelDHEalxb2mnF7mN5PMBVAOzY61BptpBFaeZKqFmUl5H5x9Kknls4Gktb6B7nYrBFjbA3+ufSkF2lnYeY0W/aAFj65ParjsctfWRJNe204UJZ2kUQ6yMgDH3qBbzTVnjitpC8rN8u3gZrGuG2ussuT5mQ47DvWgljpKwrKrFZ1G8BH449qZhYm1zUGVo4SU3vyygdKxJZRNPvLMWHqKS8kQ3BlQsZH6knNQLkcAVojVKyJHPPA+YnOaaSchec/oKg3ccNg/WkGVy3f1FUMnJGctlj6CkzI5A2hMnFIHBwQDg+tSRJLcTlU6Jzk9KQEltEzsQ2GVeSfWrBaIL8r5b+7jpTFiZGVy4BXqF70qRpGpBBZ26n+lK47DgAmC0pcN/CRVmKNto2RSsR2jQtVq2itNOtheXpzk/LEfWkfxXqcbsumxRWq54+QMTWbn2No0r7mMl3DOCI5kJB6d6mzjvWtaa1pviBxpHiy3t0uJm2Wmr2sSxPE56bwuARn/Pesq7srvSNVudHv9purRsM69HUjKsPYgg1uYJ33E4oB4x6Unc96TNBQ6gNTc8+1GaAFB/Cl4xgU3OfrRu5wTzSGL09CaduPpTKMigY7FLkk9eKbmkyRzQA5vmVsda2bG7lfSWCkIsZ8pFUY46n+dYuTj3q/pUwFpcxsCwVh5aj1PWs6iui4bk6jcMKoGO4pJQs6xjkDqwqC2mk3MVBPONtT71jyz5xngD1rmsdJGf3cTKqgEjAzRHvSErn75zxUkpjkUEE7QelIXU9QSO3HNSykWVwMA8AjpUuflJweBTAoK8kj39KhaTavUn2HNQzRCyyhcYBJJ6CqEhwmDgljjFSSyszEFRkHkU6xtXumaRgFQ/dz6+tPYotW1qYYlVgM9c1JIpK7QOM5NWCpOFxyOtNAU/X60rgY99blwCgzjJz6VnEkPsYYNdHKgMpOcqe2KoyxQmSQlSMcE1rFmcjJLLIAOeKkDEr7jg1K0S4KKqk9zUQRwTvAIHGRWhmIHPKt17GkOWfJPf0pSMZzTliZsA5FAC7k6YGTxx2pDGqEBc/jTZLchwzE8njFOET8j5ffntQNADg5XJOatqCBjr702GFUbhi34VYUHHIwahlkcb4bbyR3FWEkA6KQB2qs67Wz37EU9ZWwc1IFpWz/CB9DSONykBtvvTEfIBHGaHO1c5oJbBnOfvbj61ExAbcc9PSmsTvGSOaSUFWIBBHr61SRm2SK/y4K+4psTOY23rjByCKQy5Hy8EDuKBkOVYj5l7VRmyxDLuDiR8kdKux3j2pVo1VSp6kdj2zWdGOm0jj9atxwy3VutvGFwSWPrxTQmbLsNOjRJwVRyG49PSszxFr5v8AXGeEAMY1UkcdBj86XWrxpnggS5EgWPdISOmK58TLBOJZQSpPXHShonm5VcluJtzwjJG7IP1qK9unSbYpYrFFvb3OeKdfN5MyTr9xhz3/ABqC7wr/AD5BdeCPSqRzXu7sW4hnjmt3kZCJoRIFBztz6+9NmmSAbkILvxxUtvA7zW4Ubsgsx9ABVCWVRK7MB8pwq+vvVoqxE7rGQBksT1PrSSMQh5II7etInzTAsMnrxSSH5sDOB1PrVjEwwRc7FLdOakP3lUduvvUQy7D+eO1T7QuVU5LHAJoAkRQBl87j0XFX3/cRRwquN4+Y03YqoF6uo64704SMzFSfmx3qGxpDGBKlc4PbFTaXam8vwpOIo+WPqe1SQWU0mGYhSfxq9Ev2OErFjzWPJ9azlPSxtCGtyDV0a81RbdE3CMDOOeane0t7ZSHJPH41LAsOnRvMwJnl6sTWfeTPcs0SSLn+L6VkjZnS3vhXwaNDv7jR7W1vrOHTriVb97xjIk4DbV25A9McVgeMfMOv6S8wAmbQ7cy46lst198YpNH8I2emP/b3jGL+zbG3+aCwcgTXjDkDb1xWbfahPretXes3KCOS6YbIh0jjAwq/gAK7ThVhmaSm+tGeKZQ7POc0Zx9KaDxzRkZpDHHg/WjdjJwD603JpKBjywz0zR0PNMzSg8UAO3Zoz9KYTx7CjdjvmgB+Tgd8davafcmCwnS3UvNO45x9wD096ztxArQ09T9lLGYHk7VXt9aiexcdwg3srLGdkuep7VN5e+LLEtjgue/0qOVz522RRkgYI70CRo3RSMpGSdo6ZrmZuibC20XPdu5yRUgKiFRzudt2aiaISLyxAY5anGRGYA5AHCgHmpZaJEJbKmTv0qKVirbEYjHU0shDOWC7ARj3NQ9NzMCFHJz3qSxux7i8WFQRu5dq18xRDy1bGOOKpWgSG3Z3w0kvT5sbRUcgCqTkDfwCGyaLXDmsXlm8td28uufTmgTKQ77sEjjIxWfEFU4XgCpkmBP3TtwQQT1o5Q5x6SSMhMsinHpTGT7SS6Pjd8pyOtMdQChUnchzjtikjdo1fYTyOg7VSRLZBMsqsNoHyn5x7VDln6YBPtVuYNNCvlq/zH061SZW3/dPyn6c1pFksa8ayZUsRu4PtTgruAFJO3gZ71HJJ+8Klh9amQvwCSBtyCO1USEe5mbeMAdc1KiqQHGOvTrUQMjFS79G4wOoqWFYxOxJC7xk+lSNFtWAAAA/CkIDcr0qJHBLZYD0Bp0ZIywIZR6dahl3ElGFyVORUSMCcY59Kskhwe5I4qnIrRXG3HDc0Bcspu5zwR2qTqnBBqCNscHkds1JnI6ZNBLI5TtXAH3v0piruJXndjinBixw3Ht6U1XVdx3ZB6GrM2O+eNWHVSOR6U8bXVP7yjn3pm5WU/NjPFMBzlR1WmQWEcA7ipZCpGB1FXrOzuJLKK+8ssgYxhlbnI5PH0NZ8A851zJsx1rVtrqSGKS1syTHBIJd45Kkj09KaE2R3cStaOWZUIGA59PSsieFldBbFJ4HX5lJ5U/4VN4i1CH91bqyAs+59nTp+lZBkUDqw9CWyD+VUomUzQMCvb/ZnVQF6ZfoPaq8tnczW8aeZG7Iu3cW6gdKhNxwEzj3PGKjMw3lNxz654P41SiZ2Nu0e2tMiSVElFm0SdwXIxWImnXDQkNcW5kPOTn/AApGnwANxcjtn+tN85tzZBDHpk9PxqkikTDTJ03L9stmLDBG1hj9KaNHd1KfbrcbeSNrUzzVTJcujHrg/wCFAmK4ViT3y3INMCRNMKcfbl44GIz/AI1LHZwRyK0l4Xx2EeKqidicnJHoaDKNpG47SO9AjRcQMwP2hmDf7OCKlimtImG9ZZPXgVkk7VychSOo5pTONqDlscUnG5Sdjpm1iyI+UTR8YAKDj9aRL2zLr++5AzhlNc153PTOetP8wrj+Ld0Gaj2US1VaNTVH1C8nAghEkJXqhz09awt7CVkeV0kHBwSDWgk7qoKlox2w1akN/HOVW6iSRgOJNoDD8afs0loL2re5gtG0somuppbqYDAkmcuQPbNSZ47DjrTN3J5AoLZ7/hitSB4IxQWpgPQijPB9e9IY/NGaZu/Cjvz6YoGPzzxRuBNMycGjd0oAfuPqKCe+eaZnnFLmgY7ORRnNN6g4FG49O3tQA7PFX9KUiCeJQNzuCP8AZ+tZxNXtFMkl1KiJuQJlj2B7VEti47luRyL12DKSqYBpFMmCIxuI5d26Cq83Exz1Z8Z9vWn27SzSzI+PKjGQfc9q52bIme63ho9gGcAEDBap4Yh5pkdOSOM9hVSM7SJCMFTxU7TPIqgHy1Jyx6moZaHrDmVm2nA6BjQYGndVVBsXliTgVMCjRLyx9M+lIw2g85A5wKksgliDAnYq9icdqqSNHCq+UORyxNSTXSyMY8gIOq+tU5eVOCAQcgH0rSKIZZjYSRbiRn9KljkAi2k7nJ4FUo5V8vBIUd/SnC8ht2XfIDtPbpTaGjQcGRQfun0qVohgO21SOODmsZ9bdjsgiyWPBHOage7uHB3h1JPODihRY7G+hEahN+Mng9Kr3cZfcwIwTyBWL9oPZ2XPueasw30iBVYbwDT5Wh2HJbt5jqTleClWVikVQCAR6ion1OAMSsJ3dKjbUZN24R5A4A3cCnqTYmRd7lWIUdue9PQiOQF0GApGfQ1DHeQyZMqiPB4Oc1K2CA2Qwboc8UEsSTK9SDlcgipUCiCNj8sjD5TnrVSR2Nw/92mynMeznOeDnpSsK5dU+TKgfoDycdPSnaggUAg5IAYH1FVlCsdu/hhwSe9WIpHFo8MoDKBx/smpaGmRK4KK351OrAZyc1WiACYA4HvUiluSTnPFAMVzhy5XaCfvA5pImTLb8Y9DSSEBdrE4/rTtsYIOeMY5FUZsbPtGBF90jv2ppJ37k4yMfjQ2Y5ArsMDriozIySMNoKnoSKolk1m5F1E+0Ejk/XpWs1qEN1eRzmDaNkrA52k9OPQ1iq+1QTETjnjqa1Zrz7LoF1cLErq0Pzq35imiGcfFOxkZ3f8AeE4Oe9KXK5jVcDuG/pVaNiULkcPzj0p+7APAOffpWxDJi38JJOOoJyKbu+XGPwqPzDjsaGbv1HagRMZCCMEL2IPIpC55LEIT09DUROAOcg0hI6E8UCJt24fMxHt2NKW9+aiz69PSm5J+g7UwJwxPOaN7Zyx9qhDZzjp6UA5AHp2oAl4BODgnutLuGRxtP94VHuwPX2o3ZOV49jQIn3tjtx3pSSDlSASetQA88H8KcGG3B59QaYFoSk8YCt6HpVqFzxlM4PcVng4UA8r29qlQksM8t2Jbg+1MkacHrzSE/X86TbJDNJBMyeZFIYyVPBI9KUk9KRoGffFIGO/lzj3oNJnLHrxSGSZx7+lAPY8Uz6UbuPXtQBJmkyM03kHOKCT2oAfnvilz/wDqqPdzSg++c0hj+O9GaQHrz0HPtRkdqAFq3pTbLiU7tuV6ZwGNU+tS2rRR3cby5Kqc4Hek9ikaEoaNmuGcEZGQakjSJo2khzg+tVmk86WSTarrIeQOmO1Pgl/0ho+dxBOR29qwNkPX53DO4Kqc1aCtKNygbT1J6YqmzLhA3Bc4wKuJjaIv4cdKzaLRKpS1QiQ5yPlNNuLoxxbUi+8OWPamxv5pKsoAUcVHcXYdNoTnpipSKuUpQF2qqnrwfWoJLa4kkwi5LHHHYVqW9ukoMjnO04wKvRQQjChQuPSr5rAo3MaPQoty/aZWIH3lTjP41aXR9KjQFYCx9GYmr00bFs52r2GOtVJ3WIlX64zgHrU8zZSVhVsLGEArAoPsKVoYm5Cj06VAZnhjZQo2t0z2qFb5CqoyspPXaapXK5ojrm3hA2hOe5xVGS0w3mJkD61bW6iaQh3K/rinieFgQswNVdj0M0QyjJdxzTPIlUk8nPpzWhceWCPmG49fpTPLBO3dkgfpVXJsUWRwcAg47UiXctrlSMg9Bt4q+I45cAkDPp3pGtYS2PmwPWnczZGtzHOB5Z5Ay2e1IXO3OOaiNhlS6sVbt71FvuEIVsmghl3IZuARjnirAuSECTAkdjVOF96MCCp9KngcGXa7Z6Fc/wAqhgWUwznFPKZyy8gdqWSNVbfCcBxyvoaiSZjkHgN1AqRjZAJl+bjbxwaIMPEwky2OBUbfKpwSMnmlUOpQ7cqferIZGjmQMApJyQQaVZGWJIyN7BsjNJ5g+0nJIGcA/wBDTf8AWOQOOcCmQXbbyWfzZ5WWAkBwpwyj1qfxQJYNBcadcRvavgOHwJGX2qOyt0hurWG9Rjbu2GkXkg+uKr+PpoZFsreNMFXYpIP416f0qo7kM5kHgAAg0bs/UU0dSrHggUEk5yeexrUkfuHbrSBiPyxTc/mDR2yenrQIcPypQevTOOlM3Z60UASBsgHvQWwc0zPFGeKYh+TRkk9cGm843ZOD+lGcDk/Q0APLHB9aUE9D+YpmSOpBBpwPHsaBDgTnnrT8gdTzUW7kdMHvTwCMg9OxpgSLnkjGR29alEiDBYEqeOOxqAMA3HXsadu3FhjBPUGgRJdZGqXqkZxOT+JFIemaZcyFtUvW/vTmgdeCKGUth+fwoBOT9OabmgcZ9KQx2eegH0pQaTOevajI9x9KBjs5xx0ozmkHSjr70AOHJ6UbVI/zxSZpRnscZpDFwwz83GOlKKTHrS96AAZOfYUDYrJv+5uG76d6XkDAocZBGPpQM13NpFaYhlwpb5Rg5NRCWOJ9kbAySDlsdPao1mM8Kq0YXAySOmar+a5YHoF6LWNjVFq3mAkzty6DAyMirpmEciK0bEMuTg8is61DvKu7ADEnPpU9ww+0qI3D7sBjnjFQ0UmTvIwfaqkn+FR3NQRwyCfDkjBztI6Ukb/6QMy+Sinr3rUSFHjADMQeR3J/GoehaFhTYCoKlc5GKn3KmM8ZqGYNGFVABj0qGRi23Cnd1JJrNmqLbuJFHORULhDlgfnHTimLKeCSOmKUHqKEMq3MLuGdm5I4JNUo4WQhjkHHfvWlKvRiM4GOaqy434Azx0NaxZDRRCFZSWAO3PB71GsRC8cNjJFW2QIm59pJ7d6QImQ7F8kcDPatLiK1vCXdgpwW7HtR8ys6nqpxn1qVA0RJHB74FPSBMtl8k8ggUCIGkPCgcjAqwAyAkcj3NTJErHa4BJH3hT440XgjjtUtjI2jzwOahktmJDbc46n0q4qnJwwYfypejH6UriaMx4imcZIJzuqRVEiDAxz2PSrLRLggKcHoKrAiOYDkA9vWnuQXIpFZRkYI4+tMSNjE7oudp55pARj0A9KcWP2eV4ycDAJHbNICLK5yD2IIpoyFwp2/WjaykF+SRmmO7RxLwGwatEMjfdzKcAgjpUzS+bLynllvmbHTPqKguAVA7pIMrj9RVqAM8ux8YI+RjxmmSacVst28R81XUckbsb//AK9c74s8yPWksizFIEyuewNb1rELUGeWELEBklGyPqfSuMurqS8v5biSV5NzEIXOcLngVUUQxp4pDx1oznOAcCkyOo79RWpI7kfSkySNpYlfSm9uM0vb0oELn8qXNNziigB2SKMnOKTOTxikBBx70CHDvzmlBPGcYpuQM9M0uc+1ADgeMHpSg8cHg03jtxQOelAiTrjuO/FLuxngFfSmZ7g804Eg5FMBynt1FBYc55NNzwRSZPQD8aBEt0qJqt6sTmSMTNgnuKcCAMDPHSo5hnVb454Fw+B+Jp/p0wKGUh+cigNycemKTdk+oFCk8g8jtSGP78HFAyT0zmm9uaUHJ44oGO6HHUUufQYpBknk8mnD09KBAB7U4Y/OkAz+FPtYL+/nkg07Trq+ljUF1tojIVHqcDikMaARn5iaUgZxTHkMEksF3FLbTQ8SxyoVZD6EGt3RvA3ivxDam7sbBba3bBSS7fy/MHqoxnHvRsFzHPqOoGTQAOPWu90f4M6pdtu13VoraIHBisxudh/vHgfkav3PwOgErNY+I7qOPsk8KyEfiCP5VPNHuHMed2Y8wSRCRVYDK7jgUzYHcguzuv611rfCDxLBcNLbajYXKoTszlS39Aa5d/Pt55o5YWgnVjHKjjDKe4qG09jSLuNDKx8v7i45amoyKm0MNxPHsKcPlwNpKnrU0Rjhk82ONXfHCYyE96RZYiVJQF2AlQOT6VZ2iFRErEhPVuTVO0gBllk37mUbiXbCpVq2uRE8kawfaXkGS5OSKyaLTJbgElPI3AYzmgWbLGGDnJ7DnFQyJdSOQJMADoTgA1akRokQDOABkjvUGqZH5JRMHIxx60uwlPlGce1TF98W4Hp0OKWJt8IUSEsW5yMAe1SWMjhdgN2OetSC2y+ScAei81JKMIwIwSPyqNHkaPY0qkgcEGmNFe5tlLZIHTq1VZbbfgjC+hq2xWQFGJZwcYJpEiZSQ7Agd6pNoTsUVgZSwJLDuPWn7ADkYX6VaKEgtyfpSNbBlDjhj3qrk2KwGRSEgjqQfWnPAcjcu5Ac5xkZo8kbcJkH3oEMRwMgAAn2p4YsCSpyO4qMo8RIYjPqRSeaqxmTJIHXHaiwmSEhY2JPtVNncpyAVHerSSYAYOMv13Dg1XnAZy0YXae3TmqRDGRTKS4Y7uwoJkWUxIvEi84PpVcKUY44yeQKUXDQFiOQQPeqsQ2TyTJJhSxBUcHpSFjPOUOEJ+6c8NUSSLID+7PzDA+tRM4YPEQV6AH0NUkQ2TyxzwIpYgpnsehp8cm1BkB167agikO9RKxKgYc9/rVho4AsghuCzHBVguPz9KGIdeM8Og313G/2dZFEawyfeYHg4rl1GI1HoK6HxncTNLY2dxMJGEfmNgg49AcfSufPStIrQhu4Hmg/Xn+dGeelITVCFzSD/Ioz+FHOR60CFpOgxRnmjPrigQvH5UCkyehpffvQAd8dKXrTcg84OaXPSgB2SenBpeT1pm7jn86Xqec5NAh6gdu1KDjkUwZ6dDSj0zzTAeT0P6Ugz13YHftSAg85pj42kkZ9B6UhFhv+P68P/Td/50/gj19qa4C392g6iZv50ox+NMpDhgcClHOaQe9APoOc0hkn0pVHNNHGfWl7EnOAM8UDLGm2Goa7q0elaTbefctyRnCoO5J7Ct64+Gvjm3u3gj0uG5VFDeZFOoRgewLEEnjpiuu+DVvHZ+G9Qv7x1txfXBEUp4YqowefrXoWhayNU02OeWN4pMfMGH6/Q9anmSZLufNK3KAvFMTFJGxV42HKkHGD+Nen/BTUtGt/tmnNmPV7iQuWccSoOgU+3Jx713R8I6DeW+p20dvb/Z9TJa48lQHLHvuH0zj1zXAax8KI9DMdzoniJ7e5F0n2ZbgDCMfVhz0z2ocklcFrobPxS+Hc3iaSDVdLZFu49sU6EhRImfvZ9R/Kujvb92vEjimMcUeApQ4H0rkNQ+J6eQmkt+81W0cxXjIP3TleCUPfJGelHhzxKmum/jnjMbW7rtViMlSOvH0rgxMpS0WxrCNtzthcCe8jiM5jYjIxwDWlIbiN8qy+WMfePWuburrSL/w9e3sB8x7JSn7tskP2HHfJFYNhNqBSSK6sL5WC5j3RMcn0rnipQQ7JnV3mqXllN5qQKsLZwNuckdcntXA/FHwpd+f/AMJdZ5eCWJPtcIHMfAAceo9a6/xP4hh8OeEI0w0l7cx+XbxsPmL45Y+w61jeGrrxB4j8F6hp891YSfabd4bd2kO/5hjJAzxya2hNU1zSehPmjy9ZGdFXIOfu/Sp4ikaby5EefmUDlsVHfaVqHh7UTperwGFiCUkByrgdwaZ9oR40UIyqoOP9quuLUldGhci3XVvJcSIkUKfcVjjcf61oW80sNkI7eMRSOPnOMHHrWVaxQkqJk3Sr8y7jxgdqnFxucyZOW9Gzmk0UmXNPtXcSJtLID98nvVuVWijJhlIkHTK5qtA5uCpt5JIreM7SFPLHvUkzIl4sSOzSMcYLZKismapk0T/aEEjfO68H5cCkncRhi45A5Aprp9jvBGpLFsg88Z7VEzzWk5+0ESMnBFKxdwL3E0ZVWA3jrSSiJF27uqjPHeoDPKNvlqFHUAd6RpRJIcnnGCD2qlEOYfBvKtsxvU5BPpVmIs0WZcFielUclHXnHbg1LbuizFmJJORxzTaBMtowcNsXp1BqJnKtkkEf3QOlRShxIShJA5Y9Pwo+0JPKwRCF/vZpWBssoMBg3Ck8VCNhQzF/lB5qVJd6lJBllA2uPT3prRxxxqJAFRjgE9CaYinPIh4dgQ54+lRSARKoEJZCcFc81cnjXzVCrt4yQBkH3FI9mzhXgmG3HHmcc+lUkQ2UpLhmf7OqIuRgIwy34Gq7AndG0ZjYetTSyTW255FGd31xU0/kvbfanJLIRuCjJNUiGZrFUwN3Xke4pqq4n27QVI7VKZUupMtjaOmBgr+FMUgtmF8kcDIqiGRuWgYhB8yngGmMZZboyMqxk9QBxSyMzuzSkeYTk4706RhJsKg7uATmqJZNFbm5cIkiFychD/EPrV+1traCK7eSKWLyk3SRMR84H901mRwIjq0UxDD+ED5lNN8TahPLb21jNIkswy0jjhgPQ0JEtmF5gmmkl7ucgHnA9Kd7Ypv0HbijIxWhIueuaO1JmjjPSgA9qCenX60nfFGeD9aBCnGaQnvxR+PFGcAigQvrRn8aQ9aPagBRS5puaUc0AL04PWlBGefwpueOaM8+1Ah4OF5PTvS0znGOcU7rTAX25pshITAGS3AOadnHGakt133lvvXK7+/ekJisc3lyT1MzZ/On45681WD/AL6Ys3JcnB+tSblPAagtbFgDGOfrQo46de9QhhjJbJNAkUdXAA7k0AWcEHHQ04A5/nmq4faN3mAg9Km02CXWNYtdLt7qKOS5kCLJIflBPrik3bVjNfRvGOseGo0sYEivrN5NwtJVJyT2UjkfhXpMupePdQubEf2fFottdJ+9mbbI0QI6YPOfwrO8LeE9I8LXr3c9wNU1eLd5TkFYoz7A9/esnTfFvinX9Su4AwkuGyFO3C2y5wf8muJ1qcm+XoVZno+gro+m6QNEtr4yXUMRjaRDlznvx35ovZNI0vS7rTIZi10qCV/PBLMN2cliMVyGn67p/gC0g0uK3l1HVp5GBY4DSOx4/DpXX+I/D+qeJfDcMUjwWd8HSRxyVIHVCR2yf0ol70WkCVndmPfaZo3iHTfJu7SKH7QuY7uCFVkVuxBHWsqy+GsmkpLPoPiBprhwN0dzENr46DI6H861L2z1fT3t9Lit1urpot8cUDYVVB5JLcDmkXUtT0uSJdU097eaT7q5Uh/xUkZr5nnx2HUuqv1O7kpz1i9Tg/DGoLpHjDyvENw1lic/aIG4UkAkE+ozjBr1dtZ+1aE3iOykkvltS7xxwD/WDoVxWFqfiG13+c2n2/mHhnliR2b8SKdo3iOGS5CPcwWjsdsFtCMZ9SQK9XD432zS5WjCpRcVdmlrGh6V8R9JtLpLyWzu4o90UkZ+aLcBlWXvXnuq+EJPDFvPp9xbXM17cMiafqVluAJzyjKDwfavT2stOW9Go3UBt72X5GnjkKhiOmR0zVVtRs7/AFuWzl1yFXtlBWNHG4Me9dNepJK0Vcino7nJ+K9C1WD4cXH9q6it9cWbReQzxASKC6g/NknvXnCt5wbf+78sYKnrXf8AiDWJDp2oaJI/nkyBWmYHoGDAj8q5LUtNF7EstvgTIuDj+Kpy+dTlftF1NakexnQSN56IqsWkcKD1wKvW9rNJfPHGUQ5+Y5+6PWsnzpklViSrxjb8vGKs2jSPMEid03f609ynevUaMToEuVtvLhhbMIfYjY5Zu5qf7TbWVwYHKmVVD7jyMn196zbxo/OhuI1ZkTlUXsBSXF0N7TmIDz8PhvTtWThctSsWZLl47lYx8xlbczdCoqIoGuXfJO71qm0lw8n2qRiGfgAdqn89liWRowDJxnPNPlsVzXJHUC1ZuhQgg/U1HEyIhn3ZYNjZ6ikHlvKYlnxgbjnvVeVxGu1Tlif0p2Hct71lYygbCnODTWLNG2xN38XHeo9v7g8jPVqeBJJCpiz+6XL++TRYLlqNhHArzN15OT2pttGzXMiQsCzZBHoKq28pieaGUrh4+N3JFPXKTrJBuiATlvWlYdyazlRp/JMgAbjnuatfaI/KEEoXZExXaetUBcqoVPKUOJM7gOGFLu+1TSM8ZjYPuyOjUuUVzQRBEiqjMxJ4zVC/IjmMLu2JgSgBx8w7VNG581zIGw6EAg8A9qo6gskaJcnJkiTdk855qkiWxXZ/7OkRdoD/AMJ6gjuKqxzuXJhnXBGSG7069m/0mC8iVSjxkttPAJrOba/zgc55HSrsQ2W3dCPNByjnOO4NJIGilGG2gjOfSopZVkCoiYDdfY01yc7snI459KLCuSuRuUnBI9O9KjR7tj5BP3RUcskEaiPzCxHO4CnfaJiqkFQMfIQOTTEX9GeE3rPNui8vlmK5BHeuYldZr2eZTkPKzAnuCa65RLdW6WsxWKadCGYD8q4/Y0TyQt96Nih/CnEhijOOmKBzSdP6UfWqEB9qO3J4pCaKAFwaD09qSigQfhRR19fekoEOzgcUmaTI6UeoB7UwFz+NH0pM0ue2KQhRx9aXJ+tNz+JoWgB4pR0pvvR75/GmAv8Au9at6WvmanGgGep+hHNVMg4ByMdxWroKZmuJSvbGT2oEzNZALu4HpKwz+NOC9SQM+9Ncr9ruAOP3zY/OpI/m4z24pFLYa+EVj1IFen+DPh14Zl0ixvdW8zVbrUIxJHBFIVRB6HBByO/NefaZpw1bWrHTXlEKXU4jaU/wg9a9PhGo+HtUtfDHhmLbp5YyPezod6An5huH04FROVh2C0+EXhi11KU6hdXdwGctFZp8gjXsGPU/mKPG2j6Lo2naXeabYw2ljpl6sl2tvjzWBwByeSc+prqZ5YC2YdYhF9JGVCTMAX44rgraPUNR1RI3snubVPlmjKb1dj3btiuGrUnfyLhFdRE1hPEgkbSVuTGHOd4+bP19Kk8J6q1zb6hD9ieC7hdfPcDHmdcc+2D+daqz3PmNZafYou0f6qJAkYH4YFOmvr2ztDDenTrUEHdHEp+Uf3iSa86VSKuoo6VFuxjX1nHLfrqzxj7SkkcgnPJXYRjH5Vo+LviTaXvg+W0sruWPU2lXBKFMBSCTz9MVhtq0Ot6dcQ6ZO7TK4WILEeTkcn2rftdM0nQAjCxt9Vv3wZbu7QNtYc/Ip+6K0hiHRX7z7hSp83wmBffFDUrnStKayjRNVhjZbt5EOGXtj69a6238UXHiXwjHNf6O0UvHkTGQbd47nPIH51h+M3/4SWxin+zx299aEtGyD/Wx91P8xWBd6hJf6LDpdtMsa8bYQ3JJ4LH2ArX2qr0/cW+5HK4P3jbtrXUfEGoT2kFuFW04lupH/dbvQcZP4VJb6O+i+JLbV9QeG7is8jy7XJYEjGTkDgZ7Va07V1srO3tZG2pFGOF6k46mtDSdQtJbtzaxhTI/LNzlu/JFeROtOhUfJHRHao+0jqzB8VvP45e6j0e6lf7Mv2mCNMgTgfeH+8Ku65FpPh6ysNREItnliDKhHztwM9eevFZPie8m8P8Ai5prRUtWVfOWSIfMzN1Hpj1rjNY1W91zUHvtSuWnnOMBvuqPQDoK9mlSniIRk9FuccpRpuyNDUfE15r/AIgs2lVbe3DKiQx9CB6nuTV/d5b4XI5NctFJs1C2YYyJkI9ua6qZtk7njGc49K75QjBJRJi29zK1DT1eMyw5Vs5YZrPjdoZg4bchGG5xmuiuIwYyxIGeuKxZ7QI2VxhhkDNVCQSRNY38iuWVSI87SvXNTO638hXJTbjPHFZJaWIERyMoPXFIJ5QhXzZAfVTj/wDXWliTVAaHzHI3nd8gJ4AqUSnc0rKSVj+6exPSqTXySlWA2ZYfgO9SPcxSSTeW+9GIU/QHNFhplpAHk+0TAKFXJxUDy7UjRyu13JVx1IqG6nPmrFkiJgWxUTS7pVTIcRp8oPp3pco+YmWTyiHzuG4E5PUVYW7knm8mLMYYnGB1HXmqBcTYZcqijnJpWla3uNyHoMj6U7BcvtLCXDM251+U08tLLbmMZTByPf61QZvObdGQAw+apBNIke8S52naBn9aXKPmLKSgmGU5YIdzj09RToZmliExXb82AM8MuelRC4TzGygK7OSPXvSSzfuYREpBRSSO3WjlC5envo4nIjTcqkE54wPSsy+maVjGjkx5YKPRT2pZ7kSrIN+E3ggke1UpGMoDL8pTPI7imo2E2OBmjBSMKATyD0IqPcMkFeD2701zJsILbhnPNNJJDq2cjoe4qiSRM7d0bAketO3bgZRjI4I7iqwBkGcnHqKswwKo3bWO4cn/ABpCEji3EtLnaRgGr1nbLJIXLqscQBw3eoIgZHCDOO+Owqa4kwy20Q3OepPRRStcNi1HL50xk7KcKfYVkeIYlh1jcqBUnjVxjuehP51qopjUKGOAKg1u1S7itX84RuhKAkZBzzW3LaNjn5ryuc/wTzyO1JyQB0FWm0q9XJRVmA/uN/SqkjNEcSRtG3owxUGlxcijtSZBHBoI+XqMUAH9aMik7Yz0oxSELz60fzpKTOR/WgBc0UlH8qYC56UA4ozSfzFIQo6YA+tO7Cm/zpeOO9MBfrS5pvelFAhcgdeldFoiBNJZiP8AWyn5vUVzrYC8ttNdVYIE0q3Xp3NAmUtHsYJ7e8upbU3Ev2jYBzgZ57VHrFolnqMKR2/2bfbh3jHTO4jP6VSt5Lm3eb7Ldywq0hyEbGeakd5JX824uHnkxgF2yQPSkUiCZWJiWNiJGlUKw6g165q8d5J4gsrK3unt44CBK6nO8Ac8V5poiQP4p0lblSYTdJuA7813Ot69FpOt3EzCeSaNiwCRFgc9s9KxqdCg8aeApFuk1Gzs5rqGRNsskswZoz6+wqdfFV14R8JWOjaOkUd27hFkm5LFjy2PbNcbq3j7XtTLrFO9la5/1UOF3e7HvVnwn4V1DxjO2pXl+beztZAPtEh3yM3Xav8AjWfK2tRo777VPHFDYOpnkhHzSk4Lt3P50yN7eS4P222iEpQhY51DLJx93NUNQuLGy1Xz2120mETENA7gN+NZ2vXP78hQbXP/ACyLbo5PQq3Yj0rwnh5qfMz0FJNWRdS5s9N0xl06w+xwyTb5I4yeWP15xVV7+O5LJCx+U4fnJU1Ttru7mgkuNiJDGPmmdhjI74NUU1mF5I5hsdJsiR4gFcfUVo6LnrLVlQ7I1Z9btLLytPt4m1K/nDAon/LIY71xFpMJdTiupz5bplcDjB6Yran1sabqK3NrCgymxnPU45rBnU3buLW2Z5JXJyik8k+telhqKhHRbnLWlrqdp4Q3Ra1fSSol5BHZhjFIcsyFsZX3HWtDUdYg8PacsR/eXTAmCEH7uSTuaqttfad4J0ZobGGO+1a6hx9olOTESPm/4CPTviuLmmlklaaeUzTyHLu3c1DwarVeaWy/EUa3JGy3Fvr67v7tru7maSUjGCflUdgKqsQUIY4PUGlYkc8D0FRM2c5716kYqKsjC93diwfNf2vr5q8/jXWTnE74/i61ydspbUrRF6+aprrJsByT3OeKyq7mtPqNLurHKhkb9Kz7xf365U9OK0DtxkHIHOKguQs0THo6jI96zTLZQeIGTgsAfUdagkt9zFRwV7Y61oZJwQQCBwagKyMN3VgefWtUybGcVdGxt3BeqkULLhHzGMNwRV50OCoBJc8+1RS2+1HkJwQRxmquTYgQqcIeB1z6UhULJkNyvGae9uThxnC/eqN1bvTANgYbT8w9O1P3OjLubKrxik2FQFJ4ZetMAKqBnkCmA8F0KqCSMc+hojlYBgwOF6U1mIjVpDntmlO9yMAdKAHrKUGeS/Y0PO8nOSgxgDPSoUE+SpJyp64oZdxG5vcGgBXYtgNyBzg0hk+bIG0EYJoKEqOzVMkEe3D5Y+npQBCzNnbgnjr2qRYXb5nxjHHqalWFV5Ax7U4njgfU0gGgBl+WIYHfpSqfMcBRt9h0qWNCysM7eM+tIjrChVTnHXjFADmdbdGCdfT1NPtlYfORg1WgV5ZNzDbg9+1W/MPmmPkletawjbUwnK+iJhznJqtrg/4lKEZyJl/kak3g9+OxqS7UXGj3SkY2puGPUc1bMluZVtIcYPFW3uT5f7wI6js4zWbZncoJPbJNX5Ygw3Y59BSKZDImnzfNLbbT/eiO2oX0pThrS6WTPISQYNIyZJHdaZ9znnOfWpsh3KkqSQOY54mjb0I600np3roUkiuLfybmMSxn16j6HtVG50KVR5lg/wBoQ/8ALM/fX/Gk4sFNdTMPB9qOcZ7UhyGKsCrDgqw5FB+mMVJYp9ulJn16UAe/Wj2oEL096T+VAweQfqKPegBe2etL9Kbnil4z/jQAvSnDHfr2poPPSnDpQIci73UEfLnmurtGUxRLxz2rmIl5ViMdO9dJasRfwqABvU8YoJZgqTvlHAHmNwPrUqsoXDAYP8QHIqFcedNj/noxz+NSoONx/AUjRFmwh/0+3ePU4rW4jlBjkdTgHtXS65rEMfmpa3sU1xDjdtO4O3c59K4/YH4YA5ORVmzijEUnyfOTtGKymrlpak919m1KMX9vAtvJH/roRyrNjrj0r1TwzAmjeArNtKtmurrUG8942YKATweT0AxXldmtk03kS5UZwxjOd57A11eja/f6YssDwzSQCMLbbiAqEf0rL3ti58vLob3iXTdJ0jRoNbl8M2cd61wqyPu3KGbPJ7fpWdNrOjXaQ2EdgZPtMyo8k7qVU+q4J/pVPWvEs2uaY2lX7g27EFliHORyOaxo5razi8u1s4oB2LfMx980pYV1NWRGsoqx0N7a6XEVNnZ+eYxgpM3yDHoDxWDf2ttdzedLPBYesVvFk/piq8l3NMdzSEeoJzUJbHKglie9aUsIobu4/rMrWSJkg0yFcLbtdPniSfkf989KkfUXghwCFGMLGnAFV3cW4DEZc9FPAFZzTtK7M56nt0rqUVHRGDbm7sdcXTTSZcjOMZqsMnLOSSOgpc7mJ5HoKRm3HuPSkUkMLZOTxULZzipGPze1RsfmHGM80i0WNKGdbtuMAEn9DXSM2x9rdeSM1zekZOrbv7sZNdK48yMI5yw5U9xXPU3N4bBHtBDkbtw/Co5M49jzmnRMdnlsMFf4f60SoWiODjB6Z61miio2FOAeeq8VGZGZwCDuPHyipyA5298ZpoIXqa0TJEkCs6oBl8fe9aiaAuAsi/Ln5gKs+UGVWYHvg+opjKQoXp35p3GQAZUhQQpyMHvUMXMjBlyNuMYq75RKAk4PoDxUWxN5l5yO+atMkr7OBnAx0ppdY+WX8hUxXzGjLHADnOB2qMxnkZyM4waoQxgBHjggtmm7QRlTjFTsm5eUOB2ximspYEBeT6UAQKHfhT+JNDRHaEdsk9MVOIlPQAH24oIVV6fXJoAYI1iUYHzGnKQrZwTkdqRjnaEPfvSiN2OSCuf1oAYxzzznFSLCSCCRgj6VMiRxLlupHTFQyTkjKn5SOtABv8uP5fTrUHzzPy2AeSBSFndhkAYq5AhjB4A3e1aRiZTlYcuFRdowD2NQzs32z5WwWUDipuXgdgOQcVTlLtKNu7gYBrUwLG75R6itC2/eWxTGQ+QRWcg9xzVu0kZFDAfdbnPpQSzC0zk+X3yRWoc4IY8+1ZjqLLV5Yifk3blx6HpWuNrQhiQ2O/rUoqRRlBXnYD24qpuy2Ofxq5dAkYHQcjmqJbPPWkxotQswG4AEjtV+JmVVMZ256kVmxYVmODV6EnjJOMVSIkWbm3tdTUC7XbIvAmT7w+vrWLf6LeWAMgxcQDpJH2HuO1bCHJBPQ9cVaglMWCp+ue9DimQqjiccCOxycc+1API7iunudEsb6R3jk+yzPyCoyh/CsC+02806QrcREpnCyKMq341k4tG0ZqRXzRSA5GRzSjOOePekWH06GnA96bjnFKB60AKOlOBIzxk03r3FOXjtz3oEWrFPNnSMKDg54+tbNrchvEEUWOY2JBz7VQ0qFgHunXIC/Lj1pLS6A1GKaZD5vmctjBGT0NOxLIIQz3E7ttWGORsn15qKS83twQqg/Ko610I8LjDLd6kqxM5YpEuSefWtK2t9L04hrCwjVgP9bIS7fXnp+FHKx86RzVnpWr6iC9vaGNMcyS/Iv6/0rVi0C3tkH9o6g87DkxwfIv4t1NXrm+mmOZZ2Yn34qi7Ak5BJPGM1XIupLm+hYW5hto/LtbaOFD02rz+dQtMz8O5bPY1HuJ+7SZ75qrE3Y/eBgdh6UxiMnbwD60Hr1phIzgAk56UAGSx2rkk9hStIlug3EGU8AdhSsTb8cbyOT6e1Z1xKWIOeM8A0mUkLNKZCWLls9eKhHJPUenpigkkH9KXoMD8aRaGnnhen86aT296c4wOO9NAJ5pFDG6jNRyZ3eoxT36Z7VG5+bpxUlIu6GD9vkfH8GK6FeIxmsLQlO13z95sA+2K3CFVFx6YNc9Tc3hsIwON6H5h0HqKdH8ysG+pz2qMP5bg4O2p5YsAlBg9x6isyyo652sOWHcUkihoQzYHfp+lSvs7cEd6EYBcYJ/lVIRESw2/McDp7UNjdkEtmpJFDHecEnimMuAMZ5qgBldV3BgR05FQfLt5wR3pxZAxVywxyM96Rnjzz0PTAqkSRn5V5weeDTSc8g803cWO2Pkf7VGHzgEDPerEOZiUBJ4qMMMljn2p3lEAqzce3elEIGd2fYZoAi5IICkgGgq7MFOVqZI0UhwCO3Jp5AzljmgRGkMKkOzMwx096dvUnIJB9xS/KFyentUDucbueB2pgEsoPJzhRzjvVUOZecHaD27UrvuyoNTQrkgeX83GD2q4xM5SsSwQbFWTIcE8VOCWYDigghTnqKIwSM8c9K3sc7dwi5LgZA3VJ9mjliyvEnoepp6jHNITzleo5pklELtbaVwc1at8qrYyOeKS7UNGLgE8feH9aW2ybeUg5ANITM7XYSJ0ulUDgBh/KrVq2+03ZyNvAHXNSTwrdW0kToSWHy88g1V0hnELhvvA4we1SVe6EucE5A7DrWfyGbKgEHpV2Qhkxn8T2qkw5Y4I5qWUtiWLJIXIx7mraDjBPTpiqcZAYccdcVaUtuwOrU0TIuxsS3UZPUYqwg4YZwR09KrQgAhejHvVnkfeOcdatGEh6j5eeOcZp4kaM7QcrjoeQaTjsMUmPlwehpkmdeaLZ3Z32rfZJj1Un5G/wrDu7K5sJClzEVHZhyrfQ11WD34waf5peE286CaFusb/54rNwXQ1jUa3OMBzyDTgOTkda37vw1DcZl0ubYe9vKe/s3+NYU8M9pKYLqJonHZhWbTW5upJ7CD6U9FZ5EjQfMxwBUedo681oabayMBdNG7RjOApGQPXFCV2NuxaKqYorUv5Dx4yccE+9RyBk4uAHKfdkH6fWrHDL1FzEPfDLSeTujIiYNHydjcEVo0Z3N5nUEgZOKidzz9ODURlZhtA2g9TUb4ORmmSK3bPbim4GPSmFWzkNnBqQJgZNACcZ60FwowADn1oxk8UxlOeMZ9aAELE8AjFSInlIZWwGbgE9qrXF0mnweawDuWwo7fjUB1VLn5Zmy56Bfu0ikh88u52GckDr61UJ3kkng8/SpJlAjBXnHSogefXH86llodg8juRSHG3ik+blieT3ph60DF60mCDjH40DqKcff8qQyNxjPNVpeM49KsNjJJ4z2qvOT0xjNSykbWjxbbZCDgtyRWxjccdKzdPULGqgHGBjnpWiMEYwfauae50R2IGJZsHgA4q9EwkDF+CpxVJ2AO0jqani2tjacHrj1rMsZcRtFh04z96o1POcde1WmO8MHHsR6VU/1Zw3THXtVIQpJPK/iKQtngcGlPHIBx60nf61YhHAZCrdT0PoagKbuCAcVNz1Ymg88Y59aoRCPkAUADdSFVzwBn+dSEfNn8qjYgAMmWOefaqENU5LYPAOM0EADJyaFyFODjnoKUsF6n2oAB8y4/Him855OR600cZCgnjtSMjMME7eKoQkkih9pYA44qqzMxznjpipHVW4HzHpkUoUE4C/Keoq4q5EpWGxx5bIAwKvRIxO70qONFPbA7VL/D046VslY55O4rEYxn6mnqBtHHIpIxtOWGd3AFOClWIz8ueBVEA4xwKac88fSnNnHFJkepoECv8AMFf5kb5SCPWhbcWttMVyQCCB14qNmIBNTwyB1MTZG49RQIgjmCsC6OvpUG0W5l28l23Kf6VMVZDtcE44zVW9bYybeQw9elSxohY5T1z3qnn5ju6ircjELnHAHSqp56/rUM0Q5MNx3zVuNASBluDgc1WQ4Xj8KtQLwrEhick4NNEstQepOQeM+lXAQSMjOOlQKy7OmMVKnHbqOatGEiXHHNKPfilR1K4KnBpzRlgMDNMghkAIAPPPUU3oeRU209+tNaPP40AJtZWyowRzipWmiuofIv4FuI+24cr9D1FMAbkYOR+tNDFj8x/ChgmUn8NW8cxntP8ASYuogkOGH0Peo94L7MFJEOMrwyexFakbeWBg8g8HNSyTwXcflXyF1HAdeGA9jS5exoqj6mJPNa2gLXMgaQjK+V1J96yL3UZ75F8zagXpsGM/Wta78KOMy6ZdpdL18qQbZPp6GsORXjd4pVaJxwUcYIrGV+pvFp7HVsLjZ8sSdOu6oh5yRkyISPUGlaUsQSTx71G5c85OPSrJHx3MQchg+fpTPtKO/O5V6ZqM98dfWk3AdAaAJ5rpQoQE5z6UxJ4wxaTe2emFqBizn696fnHA5FAFfVJVlgTepX5iQuKzeCc4Ax0q/qCkwxluobg1RHNSzSOxZtXM42sSZFB49fepWTEhQcKvX3NGnJ/oMswOHaTbz7AdPzpCc5Gee/agAYnOMHApCuQCOBTl6YzxmnMcKOnNAyLBHNJyKfjJz29KYePX2pDGHrjt/KodvmXCrkfMep7VOehJ61DEgkvM9lHNSy0dDaDjC59avAEVTs8swG3Kgcj+VXQMLjBIHvXNLc3RWmxuyPvDp7VahBZFIXtmqtwPmXjGfvVbt/lC4OO2Khlj2Cs2W4BGDUJRcFHAIzwanZeNuAR71VLbGZcnC9c00Jke1kGAMe3Wk5HOMVIsgY4JAPY0hUHJ3Yq0IjySOvNI3A4OalWAEZ80j8Kd5IAxvz6UxFbbxkn6GmcjPTPtVn5AoB5fnI9KC2OwwadxFVY5WI+XaD60oiGC7YIB6EVLk78bc8Ux/nUDJ680wI2KopVFAz+tV5WHQYJ9c1NMyxpkge1VmOyMuRyei1pFXIk7Cww/NjIGep9KmFsu3IO/HoeKzJbmUsQp+vPSq7Sy5+TAHfHetU7GLVzomjG3aq7SOhzxmmcidYnABI6mqEOsyI4jmCtHjAbHIrUGZo45lwwU7gVPWtE7mUlYUIyAg9VPB9aUd8809zknHNNUYxkimQJ1yO3SmgbevU0rLjlR3pD1oAY4wD6UxWC5/Qg1McHAIxxzULxhyfL4I6Z70AWGV2IKsB8veqeoRsUjdl5Q8kdOasQSNsKsfwPY027LGyZTjg5Df0qWJbmXIPUnkVXByTkdKnlwHAGSSOfTNV3dRk4APcVBsS/KqgseM5xViOW3hbe4dB1G5SKfZ2qjbLNkufujqBWrCdzKQikHPJFUkZykkVI5baXhbhCSPu561IjAELgg1O9pazqVkgUDP8PB/OofsctvzYP5iE5MUp5/BqozumSo5wcZ9alRmHzAkEVHBNDOzRDdHKo5ikGD+HrUqyBGIkQFfQ9aZDQoYtk479u9LnjmpAImX5c4pCgHckUySNWHfpTgg3Y28U4Iv3jnFBRlBABI9RzQBCECHDHilaLgkD8Ksr86KNvTgZFHl7uy49M0Bcrj5SNuRjuKdcm3v4jBfW6TAjAkAw6/Q1P5I2naMY96f9lTZ8p5PU0mrgpW2Of3ZHXH0NLuHXnNR+npS7sVmdQ8jnI5ppUZBpRIuaQkZwCMe/WgAUYOf0pcbiAo6joKQZJ4HPYVLuFspGQZG6kdqBlLWFEUMUecnJJ/SswDA4IH1q5qTEtGh6gEnPuaqgE8Y4FSy1sW9Kk/eS27EYOHX696tyrwQwB5rLTMDiSI/MD+laQlEsYdQCSKEDG7VYBQBx6dqjcKTjn5f4qeI5QCwHBHSmRW5kwqv8vUgmgBNpDcfNmkyHGcYNSbPKyM5w1MbkbsfSkNEPIOCMU6yVhcO2PvNzTSFA4B6+tTWuROBg/NgcVLNIm1aqDlc7R1P+FWxjdgHk+9RWi/uNpx1yD61OjFWPTjpXM9zdFa9O5FG3J/UVagwygg/MOtQXoLRrjGSe9WbQbowGGMcg1LLHMOpqvIAScHknnNWJhtBQ5A65qo+QMd+tCERuFAA79qaMYpxw7AsOV6ZppzViHKcAgdDSFmHfAHSm5Yc9KaXOPm5PtTELwSTnnqaaOSR796ZuA5A+oqWJS5Jx8vT8aADGMknPvTJFyuFGcZJp88scCnecsf4R3rNmuSSQ0nJ/gXoK0jG5EpWHMm4h5+PQZ6VSvLneQkYIQHrmms5wCzcCoCx3njittjHW92BIxwTTaCTmigYjDd2q/pWqNZusUo3QnggdvcVQ9qQgHpjI6UJ2E1c67+BJlIZJBlSvemvwcZ4z6Vi6brD2sa20qgwgk89Qa3ZoyUjnTLxzAFa1TuYSViPOG2kcUyQFTTlIkVgRhhwc0KBgKSCKZAnDJxTJFYfMjHPpjtTdzJI2OAOPwqUNkD0NAiNW4zjp1FRXhWSxmRchmGR7VM42AMoGMc1BeN/o542k9D61LGtzHbzEwd/Qc5HWmxqxkUtyS2SankOVGeRjkVFCrfaQduQOc1Bsa1ueSCfu4xirStkcAjnp6VWgJUFm5BGMCrqnsDz3960Rzy3HglhjOPWlIAQ7RzRgDjp9adsx9786ZBBcwx3K7Jow3v3pqT3Fm68C7gGAQ4+cD61O0ZzkHNMPGRkjHU0DuP/tO325+zTxkHgHkfpViK4tpxj7TAGHVXYL/OqStscncPepStvOcyxB2zyxUZIoFoaCvEcETwgZ/hcEUojG47SGPXg5qBbDT5RnyIyrdPlqNNBs4pC4Zlz1Abb/KgnQtiPdwWUH1JpBCSco6tzzzT0t9Kix5ixkjozE8VKYrCVSYpTbv/AAvEcj8R3piIvLxnnI9qkRQcDrUoimRQR5cue68ZpigGQjZhl4I9KZJyGcdP0oycUnoc80nTisDuAHFAk3NtT5mPYUz5mkCIAxPAFXI41tlHzAyHqR29qYCr/o6kn756H0quzKXy56kZpZH7nJ5xgnpVeQ5xzzQBXnR5L2XfnaGJHNIAAu0fWpG+U80wjPJ6+tSWhuBuI/OnJK0J4yRnpRgYyKaemcfnQMvLOZI1K856CpN6cFGBPcVk5ZJN4Y9OR2qxFdhd2VCHHU80CsWyAc8dKZIATuVQPYU4OkqZRtrAZbd0qHexJDEAD070DQxsFs54PX60+Bttwh6DNNJUELnHvQwyCVPKkEVLLTOlgGAEx+PpUvU+9VbObzLVWV+ccj3q2h3EhfzrkludEdiG4Ubkz9RVyzUiHPrxj2qtc5TacfjVu1IMA2n3xUlj5V7tyvQ5FQGKMjaV5zwR6VZlBIIyORUeAqZzn+lMRSeHg89ehx1qJ1G3JyD0Oa0GTcQWzjHaoWjBIxkk9s8U0xFFkzgAnnoTSmMlcsBkVZaMq2CAfSni3BOXwnHRu1VcRSWLHOee5pl1erAhEfzSHv2FR32oIAY4V2hDy3rWNLMz7vmOD69xWsIdWZyn0RJJcM5LFic9TUDuVUnFQvIXIJ6DoKQ52DnkHpWxkIGkcfM59hS0nakzSAXFFJSZoAU0nWj60AZ9fb3oATaTyOtaNlqtxEq2zkFd2QD/AE9KogYFIy5wSTTQmrnVRmO5iFxA3zYIZf8AGo0Zl+VsH0rAsr+axkOBvQ8EGtmw1C1uVJkG1geCOAa0UrmLjYsFopTh2HTGRSKMEIRg8gc0fZopIjMhYANllB60xYmDqdxK5zk9qZmx8mQuQPu1UvnJhTgA5/OrrgtnHc9Kz9TUho0+p60mOO5Sfpj0pLZSZ+ucAmhzyeRk0+zTMmeoLZ/KoRqzTgBA3KB0yferaLlFJHPeq0ZDncnANWosEhQBjvWiOdkg7cEkUuGYgNzjjjtSAjJIPGeBUoyBxTJEVcHPXFNKKDxjB7U7PPIoKgkAUAV2jU5zyTT4YTnIye3Wn4Bb0HSleUBFjQYFAiYSCBNgAbbn5s9KqSXcq5J5XtnvUc8jMqlug64qk9wruV55OBk8Ck2NRJJZd5OQNw5xTYbuaJ1dGKjP1H5VX3EEknNSoNq4xjJzSKsadteyxSK8c4+b+E1pJPHfpiZmhmU/LLHwy/4/jXPJuUZB6HNaEU5b5h8pYZ+tUiGjHxkcDpRsLNtHX0qRY2dgMEc8kdqkbaiEKpz0yetZnSIiCAMBy54LAdqiL5Oc/jTmdskc1EegpAI/3j3qJuOxqQjFM6sT1GcUFIiamHgetWAOT75NMKHPAoGREHtzTGByQcZHrU/lscHANI0YUDCgnFIZWYY7fhUZB/Op3B/EComH50hjWkYL24H51ddyeSMEgZHvVJhx/OrkmJEDk7eBQAqHIzxjHNP28cdqbGAFBI61IV2MBigZc0WYJcG3c4D8p9a3YlZNyY4xx71yyyGCeO5AOYmDYrroD5sUdwrAhl4x3zXPVjrc3pvoRXETHYCfl6mrcSDyA2QB7CmSLkhcZ9qmgQpEQ424J4rBmw08AY59KjRgwGRznkVMWQoQeD2wKhXy2wwBJPvQAbscnpT9iAZCUyMbT6+lSlWC5JwPU9KYhhQrGzouSP0rnNTvds8ke/ftOAfetbV/ENpp9rLDCRPcOhUbDkLnua4hS2wbmJLHJ5renHqzGcuhPJKxJLtuyc4qIsxBxwtAGQTQTj2rcxGnABPpTSaN3HNKFyQc8ntigYgOfwpcUuOlGMUAJ7YpCB6/pT+q4waacCgBoGT1xUoU7Mk8CnwRZwccnpUix5AHc9qoVyDZk4yOfWkxg46n1xVvb8o9B0A7U0pu69+nFArlTnFMZcjHI+lWWjfHHT071Ey55Xn+lIAhu5rYsEyQeOtXIdRnV/XP3l7n8ao45wRk1NGAuee/WmiWkaX9rIqgeXLkeo4qG9vFvDFIkbIYwQc1GANh4OfWoyTz+VO5KSE6c9TVqzB8vjjI3Z9DVU4A5q5aqRGo6ZHzUIJF6LmIHAUe1WlUAbQOetQQ7BGAB0GalTdkZ5yc57irOdk4XnJAPvT/AOHjmkxkdOCaTgZx0piHEAHk0mQT3AFNzkio3kxwDkdzQBJvDfN1qNm468k0wkICBxkfgKq3NyI1DFiGHGBSHa5JJMuQBg/XvWc6skpyc57elRvOWfcrfd6Z61YYF9rFQGI5qTRKwsY+Ug4zUqnLk/gRTV57YwKk4I6YpkjkGQM9+oqVAQAVI68Z9KYkYDHk9KeoK985qiGRlggIA4qFiMHk7T607kDnt3pp/nUG405z/jTORyTzTvrzTTjsSM+tIoacZ4JIP6UmMHGAB7UuM/hRtO72oAacA8mnbcHmlAPfHNPwDjjgUBcbsG7JB4zkVC+OcE4IxzVhjtAJP5VXdc7hyeaRSIHXHHAAFRFasNycdvWoipBHpmkMj2Erz17irVvgqEcAf3cnrUYX14o2g5z+FAywI8NtOeTwBUg+VRuB21Wju3hG1gXUnseRVxGjflJFYY7HNADDGrJkZ5Hc1r+HbpWjNlI3zx5ZfcVm4xx0NMikNteQ3AONjYPuDUTjdFwdmdWRzv59frT2lJkCAfLtyait5DNEdgzg457U5cIxZ+gyOK42jqRKvzHA4qORdq/IOQOABUkSsy4C5J5GO1Z2s6/b6Svlw7J7phjHUL9aEm3ZA2luS6lqttpECvMBJO65WPP865O+13U9RBEsgjj7KgwMVVmnmurhrm5kMsrdyeAPQVGTiuqNNLc55TbG7flOO/UmnqvGWGPahE6Ejjvmld1X1ya0MxGPFMOW96BljnPanAYzj/8AVTAFGB0z9aULg5I5FLjI4PNGCMiiwCUh64Jpc9vSjkHr19aAGfxccHPFSQx723N931pI0Mp9B/OtCKMrgCL5T1oSE2RhSeO3Y1L5ZBzwAOPpUscZGOBjt7VKEHIZQc9M96uxk5FcR4QkjJ7UhiIG0ggdhVkoxIJ4B6mlK4IPYdDRYXMUJUwCNpyOpx0qCSMAkr06/WtJkIbGcr71UkU84UYNJopMplT6YNTRoA205+XmlCZOD+VSIrEZHTPSkNsRsbG9ccVFxn+lTspA4HNMKHGe/emIiI4PpVy0HKDdjK1WK8Vbt8BgWGAFwfrQhPY0EZSo+U5HbFTR5LY9RmoYlBUHOADxUw45BFaGDJQcgD0p2MdajUgHOPwpSzFcHGTQIR229OoNQryfTNDs24gkAjsKQZxjtSGMaT90eck8Cs2+djsUOCRywxV2aVFJLcEdcVmuxlm3kjLECky4oYqktnjaO1XEAOOCM8mo0QBBtJHOMirSx7mHc0hsFXgk1MijduFNVWGAq5z15qVVONowVHeqM2xwA9SPekxjg/nS546dKGbLYUY/rTEf/9k=",
				fpId1 = 1,
				fp1 = "8074400410361E535A5C8484000040001A00150A010C0E06131805030B17071409101211A28090EE77F00F4D9EE71A18191F13152322291BA58B92CA64E2634888120815020C000B1304060215010719020F0503081119140C1C13081A160D0600100409031317080A16051A011B1D070D000A070D04010003021108050C14150B120B160F05081300000000000000000000000000000000000000007282635B5D78647382A0B4559A75BB4B5D63658DAA6B80BC4B7B88D2D4D8000000000000000000000000000000000000000000000000000000000000000000007761927E75A96355B154A2A5BA466BA142C6D0C8C89ACFC0D29694769F500000000000000000000000000000000000000000000000000000000000000000000080708080008080F0F87068907070E0917198A879D98089E1AB02826DED7500000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F5BFCFFFFFF3F000000F0FFFFFF00000000E0FFFF0300000000FEFF2B00000000A8FFBF0000000080FAFF3F00000000A8FFFF0200000080FEFF3F00000000EAFFFF01000000A0FEFF1F00000000EAFFFF0000000010FFFF0F00000000F0FFFF03000000A0FEFF3F00000000EAFFFF03000000A0FEFF3F00000000F4FFFF03000000C0FFFF3F00000000FCFFFF03000000C0FFFF7F05000000FEFFFF57000002FCFFFF7F05A0A2CAFFFFFFFFBAAEEAFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00001818000000EA9ED9E6EDCE3BF799C7E919F7EFF7F943F3C5F8490EFE1139291BCF1D64135D00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000041B8A00D40BF8A66468B1006468B1002CA3F1A67D007401E000B9017D007401E000B901DC1D8A009488C700CC13F8A6D40BF8A600000000DCA3F1A67D007401E000B901F4C4C3006096F2A6D40BF8A6E8FE6A00141C8A000CD1F1A65C1D8A00400A6700E001000040054E001825860018258600E80300001C258600",
				fpId2 = 1,
				fp2 = "80F59D03493D224E5C58847C8487CC03000318071A080D10010204050B0C0E14110C05079F0EC48F85F1C3F91A3C191A191A152F1222181ABB8317B6EB0EF04A40791201130705030C060008011B1C1209190F0D211311020401170012100A0D1113191209061B0315051C1E0E1D090A19181A0704020A1A011200030802071204010400120C13051224232D273C354B1E15A8BFE3CDDE47F15D3217766871827368875765B58DBC51AB9FC85ABC7F88918FB241A7A48D655B9C53564EC90000000000000000000000000000000000000000000000000000000000007B7D8F6394655B8B5391B97EA4ABB777C5B28C8C42373E7E8C98AD4B4FB046383797000000000000000000000000000000000000000000000000000000000000A8A8C101C19881B090C868D0B8B098D9D0B9C0C8F88170A3CA4AEA92127A959D1DD5000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBF3C200000FAFFFFFF03000000FCFFFF3F00000040FCFFFF0B000000C0FFFF3F2A000000FCFFFF02000000C0FFFF2F00000000FCFFBF02000000C0FFFF2B00000000F0FFFF1500000000FFFF5F01000000F0FFFF15000000C0FFFF2F00000080FAFFFF03000000A8FFFF3F00000480FAFFFF03000000D0FFFF3F00000000FEFFFF0F040000D0FFFFFF00000000FFFFFF0F000100F8FFFFFF00000180FFFFFFAF000000FAFFFFFF2B80E0E8FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF97A21818000000F0A4D6D4E5B520D5A7DFFEFF08F0FCF05AEAE2EE3AF8FB1B2E2F0BF3207C215E0000000000700000B87F0000F87F3800F87F3800F07738008073000080030000000000007000000070000000F3FD0F00DFFD0F00FFFF0F00FFFF0300777E0000041B8A00D40BF8A66468B1006468B1002CA3F1A67D007401E000B9017D007401E000B901DC1D8A009488C700CC13F8A6D40BF8A600000000DCA3F1A67D007401E000B901F4C4C3006096F2A6D40BF8A668636800041C8A000CD1F1A65C1D8A00400A6700E001000040054E0070041800F41B8A000000000000000000",
				palmId = 1,
				palmInfo = "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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="
			};
			return await PostAsync("/setUserPower", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetUserDataAsync(int index)
	{
		try
		{
			var data = new
			{
				pass = Password,
				index = index
			};
			return await PostAsync("/getUserPower", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> DeleteAllUsersAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/deleteAllUserPower", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<(int newLogCount, int oldLogCount)> GetLogCountAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			string text = await PostAsync("/getLogCount", data);
			if (string.IsNullOrEmpty(text))
			{
				return (newLogCount: -1, oldLogCount: -1);
			}
			using JsonDocument jsonDocument = JsonDocument.Parse(text);
			JsonElement rootElement = jsonDocument.RootElement;
			if (rootElement.TryGetProperty("result", out var value))
			{
				if (!value.GetBoolean())
				{
					return (newLogCount: -1, oldLogCount: -1);
				}
				if (!rootElement.TryGetProperty("data", out var value2))
				{
					return (newLogCount: -1, oldLogCount: -1);
				}
				string @string = value2.GetString();
				if (string.IsNullOrEmpty(@string))
				{
					return (newLogCount: -1, oldLogCount: -1);
				}
				using JsonDocument jsonDocument2 = JsonDocument.Parse(@string);
				JsonElement rootElement2 = jsonDocument2.RootElement;
				int @int = rootElement2.GetProperty("newLogCnt").GetInt32();
				int int2 = rootElement2.GetProperty("oldLogCnt").GetInt32();
				return (newLogCount: @int, oldLogCount: int2);
			}
			return (newLogCount: -1, oldLogCount: -1);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return (newLogCount: -1, oldLogCount: -1);
	}

	public async Task<string> GetLogInfoAsync(int flag, int length, int index, int? isPhoto = null)
	{
		try
		{
			var data = new
			{
				pass = Password,
				flag = flag,
				length = length,
				index = index,
				isphoto = isPhoto
			};
			return await PostAsync("/getLogInfo", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<List<Logdata>> GetLogInfoAsync(int countTaradod = 100, int indexOfFirstLog = 0, bool newTaradodOnly = false)
	{
		try
		{
			if (countTaradod > 100)
			{
				countTaradod = 100;
			}
			int flag = 0;
			if (newTaradodOnly)
			{
				flag = 1;
			}
			string text = await GetLogInfoAsync(flag, countTaradod, indexOfFirstLog, 0);
			return string.IsNullOrEmpty(text) ? null : ParseLogDataArray(text);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> DeleteNewLogAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/deleteNewLog", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> DeleteAllLogAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/deleteAllLog", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<ReturnedSaveFuncInfo> SetLogCallBackAsync(string logCallBackUrl, int readCard = 0)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			var data = new
			{
				pass = Password,
				logCallBackURL = logCallBackUrl + "/LogCallBack",
				readCard = readCard
			};
			string text = await PostAsync("/setLogCallBack", data);
			if (string.IsNullOrEmpty(text))
			{
				return ret.AddError("received result rom device is null or empty!!", "", "SetLogCallBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\SDK\\LAN_VisibleLight\\VisibleLightLanSDK.cs", 643);
			}
			if (text.ToLower() != "{\r\n    \"result\": true,\r\n    \"message\": \"\",\r\n    \"data\": \"\"\r\n}")
			{
				ret.AddError("خطا در تنظیم آدرس کال بک \r\n" + logCallBackUrl + "\r\n " + text, "", "SetLogCallBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\SDK\\LAN_VisibleLight\\VisibleLightLanSDK.cs", 645);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SetLogCallBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\SDK\\LAN_VisibleLight\\VisibleLightLanSDK.cs", 649);
			Log.Error(ex, "");
		}
		return ret;
	}

	public async Task<string> GetLogCallBackAsync()
	{
		try
		{
			var data = new
			{
				pass = Password
			};
			return await PostAsync("/getLogCallBack", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> SetDepartmentAsync(object deptList)
	{
		try
		{
			var data = new
			{
				pass = Password,
				deptlist = deptList
			};
			return await PostAsync("/setDepartment", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<string> GetDepartmentAsync(int index)
	{
		try
		{
			var data = new
			{
				pass = Password,
				index = index
			};
			return await PostAsync("/getDepartment", data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public static List<Logdata> ParseLogDataArray(string json)
	{
		try
		{
			LogResponse logResponse = JsonSerializer.Deserialize<LogResponse>(json);
			if (logResponse == null || string.IsNullOrEmpty(logResponse.data))
			{
				return null;
			}
			return JsonSerializer.Deserialize<List<Logdata>>(logResponse.data);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public static Logdata ParseLogDataSingle(string json)
	{
		try
		{
			return JsonSerializer.Deserialize<Logdata>(json);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}
}
