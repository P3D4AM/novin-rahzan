using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Setting;

namespace PayamakUi.UC.UCSMS;

public class UCMobiles : TextBox, IHasContext
{
	private List<object> _List = new List<object>();

	public List<object> List => _List;

	public string MSG_CheckAghsat
	{
		get
		{
			try
			{
				if (_List != null)
				{
					foreach (object item in _List)
					{
						if (item is CheckItem)
						{
							return ((CheckItem)item).MessageCalced;
						}
						if (item is GhestMItem)
						{
							return ((GhestMItem)item).MessageCalced;
						}
					}
				}
				return "";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MSG_CheckAghsat", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCMobiles.cs", 131);
				return "";
			}
		}
		set
		{
			try
			{
				int i = 0;
				for (int num = _List.Count - 1; i <= num; i++)
				{
					if (_List[i] is CheckItem)
					{
						CheckItem checkItem = (CheckItem)_List[i];
						checkItem.Message = value;
						_List[i] = checkItem;
					}
					else if (_List[i] is GhestMItem)
					{
						GhestMItem ghestMItem = (GhestMItem)_List[i];
						ghestMItem.Message = value;
						_List[i] = ghestMItem;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MSG_CheckAghsat", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCMobiles.cs", 156);
			}
		}
	}

	public string ContextGuid { get; set; }

	public UCMobiles()
	{
		try
		{
			((TextBoxBase)this).ReadOnly = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCMobiles.cs", 24);
		}
	}

	public async Task SetListAsync(List<object> value)
	{
		try
		{
			_List = value;
			string txt = "";
			if (value != null)
			{
				foreach (object item in _List)
				{
					if (item is CheckItem)
					{
						txt += ((CheckItem)item).Mobiles;
					}
					else if (item is GhestMItem)
					{
						txt += ((GhestMItem)item).Mobiles;
					}
					else if (item is CusCompactBusiness)
					{
						_ = (CusCompactBusiness)item;
						IEnumerable<string> values = (await ((CusCompactBusiness)item).TafsilNumbers())?.Where((TafsilNumberBusiness p) => p.Mobile)?.Select((TafsilNumberBusiness p) => $"0{p.CalcedNumber}");
						txt = txt + "<" + string.Join(",", values) + " - " + ((CusCompactBusiness)item).FullName + ">;";
					}
					else
					{
						if (!(item is string))
						{
							throw new NotImplementedException();
						}
						txt = txt + item.ToString() + Environment.NewLine + txt;
					}
				}
			}
			((Control)this).Text = txt;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCMobiles.cs", 103);
		}
	}
}
