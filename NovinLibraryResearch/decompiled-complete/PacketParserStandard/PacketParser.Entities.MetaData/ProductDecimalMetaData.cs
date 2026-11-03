using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.MetaData;

public class ProductDecimalMetaData
{
	public EnFeeType FeeType { get; set; }

	public decimal Fee { get; set; }

	public EnFeeType ProfitType { get; set; }

	public decimal Profit { get; set; }

	public int Carat { get; set; }

	public Guid? DecimalGuid { get; set; }

	public decimal DecimalQuantity { get; set; }

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((DecimalGuid ?? Guid.Empty) == Guid.Empty && DecimalQuantity != 0m)
			{
				await res.AddErrorAsync("لطفا ارز مبنا را وارد نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 23);
			}
			if (Fee != 0m)
			{
				switch (FeeType)
				{
				case EnFeeType.Percent:
					if (Fee > 100m)
					{
						await res.AddErrorAsync("درصد اجرت ساخت نمی تواند بزرگتر از 100 باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 28);
					}
					break;
				case EnFeeType.None:
					await res.AddErrorAsync("لطفا نحوه محاسبه اجرا ساخت را وارد نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 33);
					break;
				default:
					throw new NotImplementedException();
				case EnFeeType.FixedPrice:
				case EnFeeType.VariablePrice:
					break;
				}
			}
			if (Profit != 0m)
			{
				switch (ProfitType)
				{
				case EnFeeType.Percent:
					if (Profit > 100m)
					{
						await res.AddErrorAsync("درصد سود فروشنده نمی تواند بزرگتر از 100 باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 40);
					}
					break;
				case EnFeeType.None:
					await res.AddErrorAsync("لطفا نحوه محاسبه سود فروشنده را وارد نمایید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 45);
					break;
				default:
					throw new NotImplementedException();
				case EnFeeType.FixedPrice:
				case EnFeeType.VariablePrice:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 51);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\ProductDecimalMetaData.cs", 52);
		}
		return res;
	}
}
