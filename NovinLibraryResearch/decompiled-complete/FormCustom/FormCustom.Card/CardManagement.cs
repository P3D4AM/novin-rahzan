using System;
using FormCustom.UC.Cards;
using PacketParser.Entities;

namespace FormCustom.Card;

public class CardManagement
{
	public static IMasterCard GetTheme(EnCardTheme theme)
	{
		try
		{
			return theme switch
			{
				EnCardTheme.Theme1 => new UcCardTemplate1(), 
				EnCardTheme.Theme2 => new UcCardTemplate2(), 
				EnCardTheme.Theme3 => new UcCardTemplate3(), 
				EnCardTheme.Theme4 => new UcCardTemplate4(), 
				EnCardTheme.Theme5 => new UcCardTemplate5(), 
				EnCardTheme.Theme6 => new UcCardTemplate6(), 
				EnCardTheme.Theme7 => new UcCardTemplate7(), 
				EnCardTheme.Theme8 => new UcCardTemplate8(), 
				EnCardTheme.Theme9 => new UcCardTemplate9(), 
				EnCardTheme.Theme10 => new UcCardTemplate10(), 
				_ => null, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTheme", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Card\\CardManagement.cs", 31);
			throw;
		}
	}
}
