using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Compression;
using PacketParser.Connection;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.EventArgs.Connection;
using PacketParser.TaxService;
using Server.Core.Assistance;

namespace Server.Core.Connection;

public class ServerFactory
{
	private static HashSet<IServer> _Servers = new HashSet<IServer>();

	private static ConcurrentDictionary<string, ClientConnectionSettings> dic_ConnectionSettings = new ConcurrentDictionary<string, ClientConnectionSettings>();

	public static ConcurrentDictionary<string, ConnectionKey> keys;

	private static RSACryptoServiceProvider encryptionProvider { get; set; }

	public static ServerFactory Instance { get; set; }

	public bool IsSelfHost { get; set; }

	public event Func<object, ConnectionDataEventArgs, Task> Disconnected;

	public event Func<object, ConnectionDataEventArgs, Task> Connected;

	public event Func<object, ConnectionDataEventArgs, Task> DataReceived;

	public ServerFactory()
	{
		keys = new ConcurrentDictionary<string, ConnectionKey>();
		encryptionProvider = new RSACryptoServiceProvider(2048);
	}

	private async Task SendPublicKey(IServer server, string connectionId, ClientConnectionSettings sett)
	{
		try
		{
			string data = Convert.ToBase64String(encryptionProvider.ExportCspBlob(includePrivateParameters: false));
			await server.SendAsync(connectionId, EnServerMessageInstruction.publicKey, data, sett);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendPublicKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 38);
		}
	}

	public async Task RecieveSystemMessagesAsync(IServer server, string connectionUniqueId, EnServerMessageInstruction instruction, string data)
	{
		int line = 0;
		try
		{
			line = 1;
			dic_ConnectionSettings.TryGetValue(connectionUniqueId, out var value);
			line = 2;
			switch (instruction)
			{
			case EnServerMessageInstruction.none:
				line = 3;
				break;
			case EnServerMessageInstruction.ping:
				line = 5;
				await Pong(server, connectionUniqueId);
				break;
			case EnServerMessageInstruction.pongExists:
				line = 10;
				break;
			case EnServerMessageInstruction.pongNotExists:
				line = 15;
				break;
			case EnServerMessageInstruction.publicKey:
				line = 20;
				break;
			case EnServerMessageInstruction.symmetricKey:
			{
				line = 25;
				ConnectionKey connectionKey = data.FromJson<ConnectionKey>("RecieveSystemMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 69);
				ConnectionKey connectionKey2 = new ConnectionKey
				{
					SymmetricKey = encryptionProvider.Decrypt(connectionKey.SymmetricKey, fOAEP: true),
					SymmetricIv = encryptionProvider.Decrypt(connectionKey.SymmetricIv, fOAEP: true)
				};
				Aes aes = Aes.Create();
				aes.Key = connectionKey2.SymmetricKey;
				aes.IV = connectionKey2.SymmetricIv;
				keys.TryRemove(connectionUniqueId, out var _);
				keys.TryAdd(connectionUniqueId, connectionKey2);
				WebErrorLog.Instance.Trace("Symmetric received", "", "RecieveSystemMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 80);
				await server.SendAsync(connectionUniqueId, EnServerMessageInstruction.IsSecured, "", value);
				break;
			}
			case EnServerMessageInstruction.setting:
			{
				line = 40;
				ClientConnectionSettings clientConnectionSettings = data.FromJson<ClientConnectionSettings>("RecieveSystemMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 85);
				dic_ConnectionSettings.TryRemove(connectionUniqueId, out var _);
				dic_ConnectionSettings.TryAdd(connectionUniqueId, clientConnectionSettings);
				await SendPublicKey(server, connectionUniqueId, clientConnectionSettings);
				break;
			}
			case EnServerMessageInstruction.IsSecured:
				line = 45;
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "RecieveSystemMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 99);
		}
	}

	public async Task handleRecievedBytes(WebContextHolder ctxtHolder, string ip, string connectionId, byte[] data, short current, short total, Guid groupingGuid, StringBuilder sb, ITaxConnectionManager taxManager)
	{
		try
		{
			sb.AppendLine("2-handleRecievedBytes...");
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(connectionId);
			if (connection == null)
			{
				sb.AppendLine("❌connection is null!!=>Return");
				return;
			}
			sb.AppendLine($"HandledConnectionInfo:HashIs:{connection?.HashId}\r\n" + "UserName:" + connection?.Username + "\r\n" + $"UserId:{connection?.UserId}\r\n" + $"DepId:{connection?.DepId}\r\n" + $"Source:{connection?.Source}");
			byte[] array = connection.ProgressReceived(data, current, total, groupingGuid.ToString(), sb);
			if (array != null && array.Any())
			{
				await handleRecievedBytes(ctxtHolder, ip, connectionId, array, sb, taxManager, 10);
			}
		}
		catch (Exception ex)
		{
			sb.AppendLine("❌Exception:" + ex.Message);
			WebErrorLog.Instance.StartLog(ex, "", "handleRecievedBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 126);
		}
	}

	private async Task Pong(IServer server, string connectionUniqueId)
	{
		_ = 2;
		try
		{
			dic_ConnectionSettings.TryGetValue(connectionUniqueId, out var value);
			if (string.IsNullOrEmpty(connectionUniqueId))
			{
				throw new NullReferenceException("connectionUniqueId");
			}
			keys.TryGetValue(connectionUniqueId, out var value2);
			if (value2 == null)
			{
				await server.SendAsync(connectionUniqueId, EnServerMessageInstruction.pongNotExists, null, value);
			}
			else if (ConnectionManager.Instance.GetConnection(connectionUniqueId)?.IsDisposed ?? true)
			{
				keys.TryRemove(connectionUniqueId, out var _);
				await server.SendAsync(connectionUniqueId, EnServerMessageInstruction.pongNotExists, null, value);
			}
			else
			{
				await server.SendAsync(connectionUniqueId, EnServerMessageInstruction.pongExists, null, value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Pong", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 154);
		}
	}

	public static ClientConnectionSettings GetSetting(string connectionId)
	{
		dic_ConnectionSettings.TryGetValue(connectionId, out var value);
		return value;
	}

	public async Task<bool> SendAsync(Packet packetData, WebContextHolder context, string ConnectionUniqueId, ENCompressedMode CompressedMode, string sender)
	{
		bool result;
		try
		{
			if (!keys.TryGetValue(ConnectionUniqueId, out var key) || key == null)
			{
				return false;
			}
			byte[] SendBytes = packetData.ToBytes();
			switch (CompressedMode)
			{
			case ENCompressedMode.GZip:
				SendBytes = await GZip.Compress(SendBytes);
				break;
			case ENCompressedMode.Brotli:
				SendBytes = await Brotli.Compress(SendBytes);
				break;
			}
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
			if (connection != null && connection.Encryption == EncryptionType.Custom)
			{
				SendBytes = Instance.Encrypt(SendBytes, key.SymmetricKey, key.SymmetricIv);
			}
			List<IServer> list = _Servers.Where((IServer p) => p.WebContextHolder.Guid == context.Guid && p.ExistsConnection(ConnectionUniqueId)).ToList();
			if (list != null && list.Any())
			{
				foreach (IServer item in list)
				{
					if (item != null)
					{
						await item.SendBytesAsync(ConnectionUniqueId, SendBytes, sender);
					}
				}
			}
			result = true;
		}
		catch (Exception ex)
		{
			result = false;
			WebErrorLog.Instance.StartLog(ex, $"{CompressedMode}{ConnectionUniqueId},{packetData?.PacketType ?? PacketType.none},{packetData?.PacketActions ?? (-1)}", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 192);
		}
		return result;
	}

	public async Task handleRecievedBytes(WebContextHolder ctxtHolder, string Ip, string connectionId, byte[] data, StringBuilder sb, ITaxConnectionManager taxManager, short tryCount = 10)
	{
		int line = 0;
		try
		{
			sb.AppendLine("1-handleRecievedBytes");
			ClientConnectionSettings connectionSettings = GetSetting(connectionId);
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(connectionId);
			if (Initializer.Dal?.User != null)
			{
				WebUser user = await Initializer.Dal.User.LoadDataAsync(connection?.UserId ?? 0);
				if (connection == null)
				{
					connection = new ClsConnectionData(user, taxManager)
					{
						Guid = connectionId,
						Context = ctxtHolder,
						Ip = Ip
					};
				}
			}
			if (string.IsNullOrEmpty(connection.Ip) || Ip != connection.Ip)
			{
				connection.Ip = Ip;
			}
			sb.AppendLine("1-handleRecievedBytes/Connection:" + Json.ToStringJson(connection));
			if (connectionSettings != null)
			{
				if (!keys.TryGetValue(connectionId, out var value) || value == null)
				{
					return;
				}
				if (connection.DepId <= 0 || connection.Encryption == EncryptionType.Custom)
				{
					data = Instance.Decrypt(data, value.SymmetricKey, value.SymmetricIv);
				}
				if (data == null || !data.Any())
				{
					return;
				}
				line = 12;
				if (data == null)
				{
					return;
				}
			}
			line = 3;
			ClsConnectionData clsConnectionData = connection;
			if (clsConnectionData != null && clsConnectionData.CompressedMode == ENCompressedMode.GZip)
			{
				data = await GZip.Decompress(data);
			}
			else
			{
				ClsConnectionData clsConnectionData2 = connection;
				if (clsConnectionData2 != null && clsConnectionData2.CompressedMode == ENCompressedMode.Brotli)
				{
					data = await Brotli.Decompress(data);
				}
			}
			if (connection.DepId <= 0)
			{
				await Packet.ParseLoginPacket(ctxtHolder, data, connectionId, connection, sb);
				return;
			}
			Packet packet = Packet.GetPacket(data, sb);
			await connection.RecievePacketAndParse(packet, ctxtHolder, connectionId, Ip, sb);
		}
		catch (CryptographicException ex)
		{
			sb.AppendLine("❌CryptographicException:" + ex.Message);
			WebErrorLog.Instance.StartLog(ex, "", "handleRecievedBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 250);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			sb.AppendLine("❌Exception:" + ex3.Message);
			WebErrorLog.Instance.StartLog(ex3, $"Line:{line}", "handleRecievedBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 256);
		}
	}

	public static void AddServer(IServer server)
	{
		try
		{
			_Servers.Add(server);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 361);
		}
	}

	public async Task OnDataReceived(IServer server, ConnectionDataEventArgs args)
	{
		using (new JaegerMonitor("OnDataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs"))
		{
			try
			{
				if (this.DataReceived != null)
				{
					await Utilities.RaiseEventOnNewThread(this.DataReceived, this, args);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OnDataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 385);
			}
		}
	}

	public async Task OnConnected(object sender, ConnectionDataEventArgs args)
	{
		try
		{
			if (this.Connected != null)
			{
				await Utilities.RaiseEventOnNewThread(this.Connected, this, args);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnConnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 396);
		}
	}

	public async Task OnDisconnected(object sender, ConnectionDataEventArgs e)
	{
		try
		{
			dic_ConnectionSettings.TryRemove(e.ConnectionUniqueId, out var _);
			keys.TryRemove(e.ConnectionUniqueId, out var _);
			if (this.Disconnected != null)
			{
				await Utilities.RaiseEventOnNewThread(this.Disconnected, this, e);
			}
			ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.Dispose("client Disconnected!");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnDisconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 409);
		}
	}

	public async Task<bool> SendToClient(Packet packetData, WebContextHolder context, string ConnectionUniqueId, ENCompressedMode CompressedMode, string sender)
	{
		if (packetData == null)
		{
			return false;
		}
		try
		{
			List<IServer> list = _Servers.Where((IServer p) => p.WebContextHolder.Guid == context.Guid && p.ExistsConnection(ConnectionUniqueId)).ToList();
			if (list != null && list.Any())
			{
				foreach (IServer item in list)
				{
					if (item != null)
					{
						return await SendAsync(packetData, context, ConnectionUniqueId, CompressedMode, sender);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 528);
		}
		return false;
	}

	private byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
	{
		byte[] result = new byte[0];
		try
		{
			using Aes aes = Aes.Create();
			result = aes.CreateEncryptor(key, iv).TransformFinalBlock(data, 0, data.Length);
		}
		catch (TargetInvocationException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Encrypt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ServerFactory.cs", 612);
		}
		return result;
	}

	private byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
	{
		if (data == null)
		{
			return null;
		}
		byte[] array = new byte[0];
		try
		{
			using Aes aes = Aes.Create();
			return aes.CreateDecryptor(key, iv).TransformFinalBlock(data, 0, data.Length);
		}
		catch (CryptographicException)
		{
			return data;
		}
		catch
		{
			throw;
		}
	}
}
