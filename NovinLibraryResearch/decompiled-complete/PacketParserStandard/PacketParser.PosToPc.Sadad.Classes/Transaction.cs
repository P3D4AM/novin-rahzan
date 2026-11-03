using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using PacketParser.Entities;
using PacketParser.PosToPc.Sadad.Interface;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class Transaction : PcPosActivity<PosResult>
{
	private string AdvertisementText { get; set; }

	private decimal Amount { get; set; }

	private string BillInfo { get; set; }

	private string CardInfo { get; set; }

	private int Count { get; set; }

	private string MultiAccountInfo { get; set; }

	private EnSadadProcessingCode ProcessingCode { get; set; }

	private int ProviderId { get; set; }

	private string SaleId { get; set; }

	private int ServiceCode { get; set; }

	internal Transaction(IModem modem, string dllVersion)
		: base(modem, dllVersion)
	{
		base.IsCanceled = false;
	}

	internal override PosResult Deserialize(byte[] buffer, string orderId)
	{
		checked
		{
			try
			{
				PosResult posResult = new PosResult();
				List<char> list = Encoding.Unicode.GetChars(buffer).ToList();
				int num = 0;
				posResult.ResponseCode = new string(list.GetRange(num, 4).ToArray()).TrimEnd(new char[1]);
				num += 4;
				posResult.Amount = new string(list.GetRange(num, 13).ToArray()).TrimEnd(new char[1]);
				num += 13;
				posResult.CardNo = new string(list.GetRange(num, 17).ToArray()).TrimEnd(new char[1]);
				num += 17;
				posResult.ProcessingCode = new string(list.GetRange(num, 7).ToArray()).TrimEnd(new char[1]);
				num += 7;
				posResult.TransactionNo = new string(list.GetRange(num, 7).ToArray()).TrimEnd(new char[1]);
				num += 7;
				_ = new byte[16];
				num += 6;
				posResult.TransactionDate = Utility.GetPersianPcDate();
				posResult.TransactionTime = Utility.GetPersianPcTime();
				num += 8;
				posResult.Rrn = new string(list.GetRange(num, 13).ToArray()).TrimEnd(new char[1]);
				num += 13;
				posResult.ApprovalCode = new string(list.GetRange(num, 7).ToArray()).TrimEnd(new char[1]);
				num += 7;
				posResult.TerminalId = new string(list.GetRange(num, 9).ToArray()).TrimEnd(new char[1]);
				num += 9;
				posResult.MerchantId = new string(list.GetRange(num, 16).ToArray()).TrimEnd(new char[1]);
				num += 16;
				num += 30;
				posResult.PcPosStatusCode = 4;
				posResult.OrderId = orderId;
				return posResult;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Deserialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\Transaction.cs", 66);
				throw;
			}
		}
	}

	internal Transaction SetAmount(decimal amount)
	{
		Amount = amount;
		return this;
	}

	internal override PosResult Start(string serialNo, string orderId)
	{
		TransactionMessage transactionMessage = new TransactionMessage();
		PosResult result;
		try
		{
			try
			{
				base.Modem.Connect();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\Transaction.cs", 89);
				result = new PosResult
				{
					OrderId = orderId,
					PcPosStatusCode = 0,
					ResponseCode = 223.ToString()
				};
				return result;
			}
			transactionMessage.SetAmount(Amount);
			transactionMessage.SetPacketType(EnSadadPacketType.Inm);
			transactionMessage.SetVersion(EnSadadFrameVersion.Sadad);
			int pcPosStatusCode = 0;
			byte[] array = Transmitter(transactionMessage, orderId, out pcPosStatusCode);
			if (base.State != EnSadadPacketType.Ack)
			{
				result = new PosResult
				{
					Amount = Amount.ToString(),
					TransactionDate = Utility.GetPersianPcDate(),
					TransactionTime = Utility.GetPersianPcTime(),
					ApprovalCode = "",
					CardNo = "",
					MerchantId = "",
					SaleId = SaleId,
					OrderId = orderId,
					PcPosStatusCode = pcPosStatusCode
				};
			}
			else
			{
				transactionMessage = new TransactionMessage();
				transactionMessage.SetPacketType(EnSadadPacketType.Request);
				transactionMessage.SetVersion(EnSadadFrameVersion.Sadad);
				transactionMessage.SetMessageType("0200");
				transactionMessage.SetProcessingCode(((int)ProcessingCode).ToString().PadLeft(6, '0'));
				transactionMessage.SetSerialNo(serialNo);
				transactionMessage.SetDllVersion(base.DllVersion);
				transactionMessage.SetAmount(Amount);
				transactionMessage.SetSaleId(SaleId);
				if (ProviderId > 0 && ServiceCode > 0 && Count > 0 && ProviderId <= 999 && ServiceCode <= 9999 && Count <= 999)
				{
					transactionMessage.SetFixedDuty(ProviderId, ServiceCode, Count);
				}
				transactionMessage.SetOrderId(orderId);
				transactionMessage.SetMultiAccount(MultiAccountInfo);
				transactionMessage.SetCardInfo(CardInfo);
				transactionMessage.SetBillInfo(BillInfo);
				transactionMessage.SetAdvertisement(AdvertisementText);
				array = Transmitter(transactionMessage, orderId, out pcPosStatusCode);
				if (base.State != EnSadadPacketType.Ack)
				{
					result = new PosResult
					{
						Amount = Amount.ToString(),
						TransactionDate = Utility.GetPersianPcDate(),
						TransactionTime = Utility.GetPersianPcTime(),
						ApprovalCode = "",
						CardNo = "",
						MerchantId = "",
						SaleId = SaleId,
						OrderId = orderId,
						PcPosStatusCode = pcPosStatusCode
					};
				}
				else
				{
					try
					{
						array = base.Modem.Receive();
						PosResult posResult = Analyse(array, orderId, out pcPosStatusCode);
						if (base.State != EnSadadPacketType.Response)
						{
							result = new PosResult
							{
								Amount = Amount.ToString(),
								TransactionDate = Utility.GetPersianPcDate(),
								TransactionTime = Utility.GetPersianPcTime(),
								ApprovalCode = "",
								CardNo = "",
								MerchantId = "",
								SaleId = SaleId,
								OrderId = orderId,
								PcPosStatusCode = pcPosStatusCode
							};
						}
						else
						{
							posResult.SaleId = SaleId;
							posResult.PcPosStatusCode = pcPosStatusCode;
							result = posResult;
						}
					}
					finally
					{
						if (!base.IsCanceled)
						{
							transactionMessage = new TransactionMessage();
							if (base.State == EnSadadPacketType.Response || base.State == EnSadadPacketType.Eot)
							{
								transactionMessage.SetPacketType(EnSadadPacketType.Ack);
							}
							else if (!base.Modem.HasTimeoutError)
							{
								transactionMessage.SetPacketType(EnSadadPacketType.Nack);
							}
							else
							{
								transactionMessage.SetPacketType(EnSadadPacketType.Eot);
							}
							transactionMessage.SetVersion(EnSadadFrameVersion.Sadad);
							base.Modem.Send(transactionMessage);
						}
					}
				}
			}
		}
		catch (SocketException ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\Transaction.cs", 206);
			result = new PosResult
			{
				OrderId = orderId,
				PcPosStatusCode = 0,
				ResponseCode = 223.ToString()
			};
		}
		catch (IOException ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\Transaction.cs", 216);
			result = new PosResult
			{
				OrderId = orderId,
				PcPosStatusCode = 0,
				ResponseCode = 223.ToString()
			};
		}
		catch (Exception ex4)
		{
			WebErrorLog.Instance.StartLog(ex4, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\Transaction.cs", 226);
			result = new PosResult
			{
				OrderId = orderId,
				PcPosStatusCode = 5
			};
		}
		finally
		{
			try
			{
				base.Modem.Disconnect();
			}
			catch (Exception ex5)
			{
				WebErrorLog.Instance.StartLog(ex5, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\Transaction.cs", 237);
			}
		}
		return result;
	}
}
