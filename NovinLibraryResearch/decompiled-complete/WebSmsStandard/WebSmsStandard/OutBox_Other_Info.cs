using System;
using System.IO;
using System.Xml.Serialization;
using PacketParser.Entities;

namespace WebSmsStandard;

[Serializable]
public class OutBox_Other_Info
{
	private string _ProfileTxt = "";

	private string _ProfilePic = "";

	private string _NameChanel = "";

	private string _ChanelPic = "";

	private string _pic1B = "";

	private string _pic2B = "";

	private string _pic1A = "";

	private string _pic2A = "";

	private string _pic3A = "";

	private string _Movie1 = "";

	private string _Voice = "";

	private string _PicText = "";

	public string pic1A
	{
		get
		{
			return _pic1A;
		}
		set
		{
			_pic1A = value;
		}
	}

	public string pic2A
	{
		get
		{
			return _pic2A;
		}
		set
		{
			_pic2A = value;
		}
	}

	public string pic3A
	{
		get
		{
			return _pic3A;
		}
		set
		{
			_pic3A = value;
		}
	}

	public string pic1B
	{
		get
		{
			return _pic1B;
		}
		set
		{
			_pic1B = value;
		}
	}

	public string pic2B
	{
		get
		{
			return _pic2B;
		}
		set
		{
			_pic2B = value;
		}
	}

	public string ChanelName
	{
		get
		{
			return _NameChanel;
		}
		set
		{
			_NameChanel = value;
		}
	}

	public string Profile
	{
		get
		{
			return _ProfileTxt;
		}
		set
		{
			_ProfileTxt = value;
		}
	}

	public string Movie1
	{
		get
		{
			return _Movie1;
		}
		set
		{
			_Movie1 = value;
		}
	}

	public string Voice
	{
		get
		{
			return _Voice;
		}
		set
		{
			_Voice = value;
		}
	}

	public string ProfilePic
	{
		get
		{
			return _ProfilePic;
		}
		set
		{
			_ProfilePic = value;
		}
	}

	public string ChanelPic
	{
		get
		{
			return _ChanelPic;
		}
		set
		{
			_ChanelPic = value;
		}
	}

	public string PicText
	{
		get
		{
			return _PicText;
		}
		set
		{
			_PicText = value;
		}
	}

	public OutBox_Other_Info(string pic1A, string pic2A, string pic3A, string pic1B, string Pic2B, string ChanelName, string PicChanel, string ProfileTXT, string ProfilePic, string Movie1, string Voice1, string PicText)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		JaegerMonitor val = new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\OutBox_Other_Info.cs", 60000);
		try
		{
			Profile = ProfileTXT;
			this.ChanelName = ChanelName;
			this.ProfilePic = ProfilePic;
			ChanelPic = PicChanel;
			this.Movie1 = Movie1;
			Voice = Voice1;
			this.pic1B = pic1B;
			pic2B = Pic2B;
			this.pic1A = pic1A;
			this.pic2A = pic2A;
			this.pic3A = pic3A;
			this.PicText = PicText;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\OutBox_Other_Info.cs", 47);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public OutBox_Other_Info()
	{
	}

	public static string Serialize(OutBox_Other_Info info)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		JaegerMonitor val = new JaegerMonitor("Serialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\OutBox_Other_Info.cs", 60000);
		try
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(info.GetType());
				StringWriter stringWriter = new StringWriter();
				xmlSerializer.Serialize(stringWriter, info);
				return stringWriter.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Serialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\OutBox_Other_Info.cs", 208);
			}
			return "";
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static OutBox_Other_Info Deserialize(string data)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		JaegerMonitor val = new JaegerMonitor("Deserialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\OutBox_Other_Info.cs", 60000);
		try
		{
			if (data.Trim() == "")
			{
				return new OutBox_Other_Info();
			}
			XmlSerializer xmlSerializer = new XmlSerializer(new OutBox_Other_Info().GetType());
			StringReader textReader = new StringReader(data);
			return (OutBox_Other_Info)xmlSerializer.Deserialize(textReader);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
