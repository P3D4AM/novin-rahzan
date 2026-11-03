using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Assistance.Net;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Scale;

namespace PacketParser.Assistance;

public class BytePacketManager
{
	public static short PacketHeaderSize => 23;

	public EnServerMessageInstruction MessageType { get; set; }

	public int CurrentPart { get; set; } = 1;


	public int TotalPart { get; set; } = 1;


	public Guid RefrenceGuid { get; set; } = Guid.Empty;


	public byte[] Data { get; set; }

	public int BufferSize { get; set; }

	public int DataLenght { get; private set; }

	public byte[] GeneratePacket()
	{
		byte[] data = Data;
		checked
		{
			if (((data != null) ? data.Length : 0) + PacketHeaderSize > BufferSize)
			{
				byte[] data2 = Data;
				throw new ArgumentException($"Data Lenght is {((data2 != null) ? data2.Length : 0) + PacketHeaderSize} is Bigger than {BufferSize}");
			}
			try
			{
				byte[] array = new byte[BufferSize];
				array[0] = (byte)((short)MessageType & 0xFF);
				array[1] = (byte)(CurrentPart & 0xFF);
				array[2] = (byte)((CurrentPart >> 8) & 0xFF);
				array[3] = (byte)(TotalPart & 0xFF);
				array[4] = (byte)((TotalPart >> 8) & 0xFF);
				byte[] array2 = RefrenceGuid.ToByteArray();
				Array.Copy(array2, 0, array, 5, array2.Length);
				byte[] data3 = Data;
				DataLenght = ((data3 != null) ? data3.Length : 0);
				array[21] = (byte)(DataLenght & 0xFF);
				array[22] = (byte)((DataLenght >> 8) & 0xFF);
				if (Data != null)
				{
					byte[] data4 = Data;
					int length = Math.Min((data4 != null) ? data4.Length : 0, array.Length - 23);
					Array.Copy(Data, 0, array, 23, length);
				}
				return array;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GeneratePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BytePacketManager.cs", 58);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> GenerateAndSavePacketAsync(BaseTcpClientConnection connection, byte[] data)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				List<List<byte>> chunked = data.ChunkListBySpecialLenght(connection.BufferSize - PacketHeaderSize);
				if (chunked == null)
				{
					return res;
				}
				short part = 0;
				Guid guid = Guid.NewGuid();
				foreach (List<byte> item in chunked)
				{
					if (connection == null || connection.Status != ConnectionStatus.Connected)
					{
						res.AddReturnedValue(ReturnedState.Error, "connetionError", "", "GenerateAndSavePacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BytePacketManager.cs", 76);
						return res;
					}
					byte[] data2 = new BytePacketManager
					{
						Data = item.ToArray(),
						MessageType = EnServerMessageInstruction.ByteData,
						BufferSize = connection.BufferSize,
						CurrentPart = part,
						TotalPart = chunked.Count,
						RefrenceGuid = guid
					}.GeneratePacket();
					await connection.SendAsync(data2);
					part++;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateAndSavePacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BytePacketManager.cs", 96);
				res.AddReturnedValue(ex, "", "GenerateAndSavePacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BytePacketManager.cs", 97);
			}
			return res;
		}
	}

	public static BytePacketManager ParsePacket(byte[] data)
	{
		try
		{
			if (data == null || !data.Any())
			{
				return null;
			}
			BytePacketManager bytePacketManager = new BytePacketManager
			{
				MessageType = (EnServerMessageInstruction)data[0],
				CurrentPart = ((data[2] << 8) | data[1]),
				TotalPart = ((data[4] << 8) | data[3]),
				DataLenght = ((data[22] << 8) | data[21])
			};
			byte[] array = new byte[16];
			Array.Copy(data, 5, array, 0, 16);
			bytePacketManager.RefrenceGuid = new Guid(array);
			bytePacketManager.Data = new byte[bytePacketManager.DataLenght];
			Array.Copy(data, 23, bytePacketManager.Data, 0, bytePacketManager.DataLenght);
			return bytePacketManager;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParsePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\BytePacketManager.cs", 125);
			throw;
		}
	}
}
