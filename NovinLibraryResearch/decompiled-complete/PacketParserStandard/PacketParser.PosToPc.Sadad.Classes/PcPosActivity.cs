using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using PacketParser.Entities;
using PacketParser.PosToPc.Sadad.Interface;

namespace PacketParser.PosToPc.Sadad.Classes;

internal abstract class PcPosActivity
{
	internal static readonly ResourceSet ResourceSet;

	internal bool IsCanceled { get; set; }

	internal IModem Modem { get; set; }

	internal EnSadadPacketType State { get; set; }

	static PcPosActivity()
	{
	}

	internal virtual void Cancel()
	{
		TransactionMessage transactionMessage = new TransactionMessage();
		try
		{
			try
			{
				Modem.Connect();
			}
			catch (Exception exception)
			{
				Utility.LogPcPos(exception);
			}
			transactionMessage.SetPacketType(EnSadadPacketType.Eot);
			transactionMessage.SetVersion(EnSadadFrameVersion.Sadad);
			Modem.Send(transactionMessage);
			IsCanceled = true;
			State = EnSadadPacketType.Eot;
		}
		catch (SocketException exception2)
		{
			Utility.LogPcPos(exception2);
		}
		catch (IOException exception3)
		{
			Utility.LogPcPos(exception3);
		}
		catch (Exception exception4)
		{
			Utility.LogPcPos(exception4);
		}
		finally
		{
			try
			{
				Modem.Disconnect();
			}
			catch (Exception exception5)
			{
				Utility.LogPcPos(exception5);
			}
		}
	}
}
internal abstract class PcPosActivity<T> : PcPosActivity
{
	public string DllVersion { get; private set; }

	internal PcPosActivity(IModem modem, string dllVersion)
	{
		base.Modem = modem;
		DllVersion = dllVersion;
	}

	internal virtual T Analyse(byte[] buffer, string orderId, out int pcPosStatusCode)
	{
		T result = (T)Activator.CreateInstance(typeof(T));
		try
		{
			int result2 = 0;
			List<byte> list = new List<byte>();
			list.AddRange(buffer.SkipWhile((byte q) => q != 2));
			byte[] buffer2;
			byte b;
			checked
			{
				if (list.Count != 0)
				{
					if (!int.TryParse(Encoding.ASCII.GetString(list.GetRange(FramePduLen.Header, FramePduLen.Length).ToArray()), out result2))
					{
						base.State = EnSadadPacketType.Unknown;
						pcPosStatusCode = 5;
					}
					else
					{
						if (!list.GetRange(FramePduLen.Header + FramePduLen.Length + result2 - FramePduLen.Footer - FramePduLen.Lrc, FramePduLen.Footer).ToArray().GetValue(0)
							.Equals((byte)3))
						{
							base.State = EnSadadPacketType.Unknown;
							pcPosStatusCode = 5;
							return result;
						}
						if (!list.GetRange(FramePduLen.Header + FramePduLen.Length + result2 - FramePduLen.Footer, FramePduLen.Lrc).ToArray().GetValue(0)
							.Equals(Utility.GetLRC(list.GetRange(FramePduLen.Header, result2 + FramePduLen.Length - FramePduLen.Lrc - FramePduLen.Footer).ToArray())))
						{
							base.State = EnSadadPacketType.Unknown;
							pcPosStatusCode = 5;
							return result;
						}
						Encoding.ASCII.GetString(list.GetRange(FramePduLen.Header + FramePduLen.Length, FramePduLen.Version).ToArray()).Equals(Encoding.ASCII.GetString(new byte[3] { 48, 48, 48 }));
						List<byte> range = list.GetRange(FramePduLen.Header + FramePduLen.Length + FramePduLen.Version, FramePduLen.PacketType);
						buffer2 = list.GetRange(FramePduLen.Header + FramePduLen.Length + FramePduLen.Version + FramePduLen.PacketType, result2 - FramePduLen.Lrc - FramePduLen.Footer - FramePduLen.Version - FramePduLen.PacketType).ToArray();
						b = (byte)range.ToArray().GetValue(0);
						if (b > 21)
						{
							if (b - 25 > 1)
							{
								_ = 241;
							}
							goto IL_0247;
						}
						switch (b)
						{
						case 4:
							break;
						default:
							goto IL_0247;
						}
						base.State = EnSadadPacketType.Eot;
						if (base.State != EnSadadPacketType.Eot)
						{
							pcPosStatusCode = 5;
						}
						else
						{
							pcPosStatusCode = 3;
						}
					}
					goto IL_027f;
				}
				base.State = EnSadadPacketType.Unknown;
				int num = (base.IsCanceled ? ((!base.Modem.HasTimeoutError) ? 2 : 0) : 0);
				pcPosStatusCode = num;
				return result;
			}
			IL_0247:
			base.State = (EnSadadPacketType)b;
			if (base.State != EnSadadPacketType.Response)
			{
				pcPosStatusCode = 5;
			}
			else
			{
				result = Deserialize(buffer2, orderId);
				pcPosStatusCode = 4;
			}
			goto IL_027f;
			IL_027f:
			return result;
		}
		catch (Exception exception)
		{
			Utility.LogPcPos(exception);
			throw;
		}
	}

	internal abstract T Deserialize(byte[] buffer, string orderId);

	internal T SetDllVersion(string version)
	{
		DllVersion = version;
		return default(T);
	}

	internal abstract T Start(string serialNo, string orderId);

	internal byte[] Transmitter(PcPosBaseMessage msg, string orderId, out int pcPosStatusCode)
	{
		int num = 0;
		byte[] array = new byte[0];
		do
		{
			base.Modem.Send(msg);
			array = base.Modem.Receive(2000);
			Analyse(array, orderId, out pcPosStatusCode);
			num = checked(num + 1);
		}
		while ((base.State != EnSadadPacketType.Ack || base.State == EnSadadPacketType.Nack) && (base.State == EnSadadPacketType.Ack || base.State != EnSadadPacketType.Nack) && num < 4);
		return array;
	}
}
