using System;
using System.Collections.Generic;
using System.Drawing;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class ColorHolder
{
	public List<ColorManager> ColorList { get; private set; }

	public ColorHolder()
	{
		try
		{
			ColorList = new List<ColorManager>
			{
				new ColorManager(Color.FromArgb(229, 225, 218), Color.Black, 1),
				new ColorManager(Color.FromArgb(255, 229, 229), Color.Black, 2),
				new ColorManager(Color.FromArgb(255, 207, 129), Color.Black, 3),
				new ColorManager(Color.FromArgb(217, 237, 191), Color.Black, 4),
				new ColorManager(Color.FromArgb(220, 242, 241), Color.Black, 5),
				new ColorManager(Color.FromArgb(241, 228, 195), Color.Black, 6),
				new ColorManager(Color.FromArgb(243, 204, 243), Color.Black, 7),
				new ColorManager(Color.FromArgb(134, 10, 53), Color.White, 8),
				new ColorManager(Color.FromArgb(22, 72, 99), Color.White, 9),
				new ColorManager(Color.FromArgb(79, 111, 82), Color.White, 10),
				new ColorManager(Color.FromArgb(199, 0, 57), Color.White, 11),
				new ColorManager(Color.FromArgb(102, 37, 73), Color.White, 12)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ColorHolder.cs", 32);
		}
	}
}
