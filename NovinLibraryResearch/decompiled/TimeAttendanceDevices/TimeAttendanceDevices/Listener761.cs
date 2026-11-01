using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.InterFaces;

namespace TimeAttendanceDevices;

public class Listener761
{
	public static async Task ListenAsync(string contextGuid, IDalSaveAttLogOnPerson dalSaveTaradod, string url)
	{
		if (IsPortOpen(url))
		{
			return;
		}
		HttpListener listener = new HttpListener();
		listener.Prefixes.Add(url);
		listener.Start();
		Console.WriteLine("HTTP Server started at " + url);
		while (true)
		{
			HttpListenerContext context = listener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
			string text = "";
			using (StreamReader streamReader = new StreamReader(request.InputStream, request.ContentEncoding))
			{
				text = streamReader.ReadToEnd();
			}
			string s = ((await new NP761VisibleLightPacketParser(dalSaveTaradod).ParseAsync(contextGuid, text)).HasError ? "{\"result\":0}" : "{\"result\":1}");
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			response.ContentType = "application/json";
			response.ContentLength64 = bytes.Length;
			response.OutputStream.Write(bytes, 0, bytes.Length);
			response.OutputStream.Close();
		}
	}

	public static bool IsPortOpen(string url)
	{
		try
		{
			if (string.IsNullOrEmpty(url.Trim()))
			{
				return false;
			}
			if (!url.Contains(":"))
			{
				return false;
			}
			string[] array = url.Split(new char[1] { ':' });
			if (array.Length < 3)
			{
				return false;
			}
			string host = array[1].Substring(2, array[1].Length - 2);
			int port = int.Parse(Text.RemoveNoNumbers(array[2], ""));
			using TcpClient tcpClient = new TcpClient();
			IAsyncResult asyncResult = tcpClient.BeginConnect(host, port, null, null);
			if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(200.0)))
			{
				return false;
			}
			tcpClient.EndConnect(asyncResult);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
