using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Access;

public class LockKeyGenerator
{
	public async Task<string> KeyGeneratorAsync(long lockSerial)
	{
		try
		{
			long num = new DateTimeOffset(DateTime.Now.AddMinutes(30.0)).ToUnixTimeMilliseconds();
			return lockSerial.ToString() + num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "KeyGeneratorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 18);
			return "Error";
		}
	}

	public async Task<ReturnedSaveFuncInfo> KeyValidatorAsync(string key, string lockSerial)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (key.Contains(" "))
			{
				returnedSaveFuncInfo.AddError("رمز عبور نمی تواند شامل فاصله باشد. فاصله ها را پاک کنید", "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 27);
				return returnedSaveFuncInfo;
			}
			if (key.Length < 12)
			{
				returnedSaveFuncInfo.AddError("رمز عبور باید حداقل 12 رقمی باشد", "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 32);
				return returnedSaveFuncInfo;
			}
			string text = key.Substring(0, 12);
			string value = key.Replace(text, "");
			if (text != lockSerial)
			{
				returnedSaveFuncInfo.AddError("سریال قفل با کلید منطبق نیست", "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 37);
			}
			if (DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(value)).LocalDateTime <= DateTime.Now)
			{
				returnedSaveFuncInfo.AddError("اعتبار رمز منقضی شده است", "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 38);
			}
		}
		catch (Exception ex)
		{
			if (ex is OverflowException)
			{
				returnedSaveFuncInfo.AddError("رمز عبور غیر معتبر است", "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 44);
				return returnedSaveFuncInfo;
			}
			if (ex is FormatException)
			{
				returnedSaveFuncInfo.AddError("فرمت رمز وارد شده معتبر نیست", "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 49);
				return returnedSaveFuncInfo;
			}
			WebErrorLog.Instance.StartLog(ex, "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 52);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "KeyValidatorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\LockKeyGenerator.cs", 53);
		}
		return returnedSaveFuncInfo;
	}
}
