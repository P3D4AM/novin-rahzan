using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.DescriptionParser;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Global;

public class CardAttribute
{
	public string Text { get; set; }

	public string ForeColor { get; set; }

	public bool IsItalic { get; set; }

	public bool Bold { get; set; }

	public string FontName { get; set; }

	public float FontSize { get; set; }

	public ContentAlignment TextAlign { get; set; }

	public async Task<string> GetTextStringAsync(string contextGuid, EnSystem system, EnCardType type, List<Guid> targetGuidList, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		checked
		{
			try
			{
				string text = "";
				if (type != EnCardType.IncreaseProduct && type != EnCardType.DynamicGroups && type != 0)
				{
					return type.GetDisplay().GetTranslate();
				}
				if (!(targetGuidList?.Any() ?? false))
				{
					return text;
				}
				if (targetGuidList.Count <= 2)
				{
					foreach (Guid targetGuid in targetGuidList)
					{
						string text2 = text;
						text = text2 + await GetTextStringAsync(contextGuid, system, type, targetGuid, token) + ",";
					}
				}
				else
				{
					Guid target = targetGuidList[0];
					Guid secondGuid = targetGuidList[1];
					string text2 = text;
					text = text2 + await GetTextStringAsync(contextGuid, system, type, target, token) + ",";
					text2 = text;
					text = text2 + $"{await GetTextStringAsync(contextGuid, system, type, secondGuid, token)} و {targetGuidList.Count - 2} داده دیگر";
				}
				if (text.EndsWith(","))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetTextStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardAttribute.cs", 49);
				throw;
			}
		}
	}

	public async Task<string> GetTextStringAsync(string contextGuid, EnSystem system, EnCardType _type, Guid target, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			string text = "";
			switch (_type)
			{
			case EnCardType.GoToMenu:
				text = (string.IsNullOrEmpty(Text) ? ((await MenuBusiness.GetAsync(contextGuid, target, token))?.Title ?? "") : Text);
				break;
			case EnCardType.IncreaseProduct:
			{
				ProductBusiness productBusiness = await ProductBusiness.GetAsync(contextGuid, target, token);
				if (productBusiness != null && Text != null)
				{
					text = await productBusiness.GetStringAsync(Text, system, token);
				}
				break;
			}
			case EnCardType.DynamicGroups:
				text = (await PrdGroupBusiness.GetAsync(contextGuid, target, token))?.Name;
				break;
			default:
				text = (string.IsNullOrEmpty(Text) ? _type.GetDisplay().GetTranslate() : Text);
				break;
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTextStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Card\\CardAttribute.cs", 86);
			throw;
		}
	}
}
