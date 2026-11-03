using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class CustomerSettings
{
	[DefaultValue("تلفن منزل")]
	public string HomeTell { get; set; } = "تلفن منزل";


	[DefaultValue("تلفن محل کار")]
	public string WorkTell { get; set; } = "تلفن محل کار";


	[DefaultValue("همراه 1")]
	public string Mob1 { get; set; } = "همراه 1";


	[DefaultValue("همراه 2")]
	public string Mob2 { get; set; } = "همراه 2";


	[DefaultValue("آدرس منزل")]
	public string HomeAddress { get; set; } = "آدرس منزل";


	[DefaultValue("محل کار")]
	public string WorkAddress { get; set; } = "محل کار";


	[DefaultValue("فکس")]
	public string Fax { get; set; } = "فکس";


	[DefaultValue("تلفن 1")]
	public string Tell1 { get; set; } = "تلفن 1";


	[DefaultValue("تلفن 2")]
	public string Tell2 { get; set; } = "تلفن 2";


	[DefaultValue("تلفن 3")]
	public string Tell3 { get; set; } = "تلفن 3";


	[DefaultValue("تلفن 4")]
	public string Tell4 { get; set; } = "تلفن 4";


	[DefaultValue("تلفن 5")]
	public string Tell5 { get; set; } = "تلفن 5";


	[DefaultValue("تلفن 6")]
	public string Tell6 { get; set; } = "تلفن 6";


	[DefaultValue("تلفن 7")]
	public string Tell7 { get; set; } = "تلفن 7";


	[DefaultValue("تلفن 8")]
	public string Tell8 { get; set; } = "تلفن 8";


	[DefaultValue("تلفن 9")]
	public string Tell9 { get; set; } = "تلفن 9";


	[DefaultValue("تلفن 10")]
	public string Tell10 { get; set; } = "تلفن 10";


	[DefaultValue("کد پستی")]
	public string Zip { get; set; } = "کد پستی";


	[DefaultValue("توضیحات")]
	public string Description { get; set; } = "توضیحات";


	[DefaultValue("توضیحات")]
	public string Desc
	{
		get
		{
			return Description;
		}
		set
		{
			Description = value;
		}
	}

	[DefaultValue("")]
	public string CustomerMotefaregheGuidList { get; set; } = "";


	public List<Guid> CustomerMotefaregheList
	{
		get
		{
			try
			{
				List<string> list = CustomerMotefaregheGuidList?.Split(new char[1] { ',' })?.ToList();
				if (list == null || list.Count <= 0)
				{
					return null;
				}
				string[] array = new string[7] { "@", "[", "]", "\\", "\\ ", " \\", "\"" };
				List<Guid> list2 = new List<Guid>();
				foreach (string item in list)
				{
					string text = item;
					string[] array2 = array;
					foreach (string oldValue in array2)
					{
						text = text.Replace(oldValue, string.Empty);
					}
					if (Guid.TryParse(text, out var result))
					{
						list2.Add(result);
					}
				}
				return list2?.Where((Guid p) => p != Guid.Empty)?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CustomerMotefaregheList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\CustomerSettings.cs", 56);
				return null;
			}
		}
	}

	[DefaultValue(false)]
	public bool DisplayTell1InGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayTell2InGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayMob1InGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayMob2InGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayAddress1InGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayAddress2InGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayFaxInGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayZipInGrid { get; set; }

	[DefaultValue(false)]
	public bool DisplayDescriptionInGrid { get; set; }

	[DefaultValue(true)]
	public bool IsFullScreen { get; set; } = true;


	public int DisplayWidth { get; set; } = 800;


	public int DisplayHeight { get; set; } = 600;


	[DefaultValue("عنوان فیلد 8")]
	public string Customer8 { get; set; } = "کدپستی";


	[DefaultValue("عنوان فیلد 9")]
	public string Customer9 { get; set; } = "توضیحات";


	[Obsolete("", true)]
	[DefaultValue(false)]
	public bool DisplaySanad { get; set; }

	[DefaultValue(false)]
	public bool DisplayPic { get; set; }

	[DefaultValue(false)]
	public bool SuggestNextCode { get; set; }

	[Obsolete("", true)]
	public int MaxDisplay { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutNumber { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutName { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutAddress { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutMethodOfIntroduction { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutEmail { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutUserName { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutArea { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutNationalCode { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutZip { get; set; }

	[DefaultValue(ReturnedState.None)]
	public ReturnedState Customer_WithoutCodeEghtesadi { get; set; }

	public Guid? DefaultGroupGuid { get; set; }

	public Guid? DefaultTitleGuid { get; set; }

	[DefaultValue(EnSuggestionType.FillGaps)]
	public EnSuggestionType SuggestionType { get; set; } = EnSuggestionType.FillGaps;


	[DefaultValue(true)]
	public bool SuggestFamilyWhenNotFound { get; set; } = true;

}
