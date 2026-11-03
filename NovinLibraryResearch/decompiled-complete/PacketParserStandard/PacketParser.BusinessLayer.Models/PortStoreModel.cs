using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class PortStoreModel
{
	public string Name { get; private set; }

	public int Port { get; private set; }

	public EnPortType Type { get; private set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	private PortStoreModel(string name, int port, EnPortType type)
	{
		Name = name;
		Port = port;
		Type = type;
	}

	public static List<PortStoreModel> GetAll()
	{
		try
		{
			return new List<PortStoreModel>
			{
				new PortStoreModel("کالر آیدی", 7599, EnPortType.Service),
				new PortStoreModel("ایزابل", 7598, EnPortType.Service),
				new PortStoreModel("پیامک", 7595, EnPortType.Service),
				new PortStoreModel("ترازو", 7401, EnPortType.Service),
				new PortStoreModel("سرویس قفل", 7596, EnPortType.Service),
				new PortStoreModel("تجارت الکترونیک پارسیان", 1362, EnPortType.PcPoss),
				new PortStoreModel("فناپ", 7000, EnPortType.PcPoss),
				new PortStoreModel("پرداخت نوین", 1362, EnPortType.PcPoss),
				new PortStoreModel("سامان کیش TCP", 1197, EnPortType.PcPoss),
				new PortStoreModel("مبنا کارت", 1, EnPortType.PcPoss),
				new PortStoreModel("آسان پرداخت", 17000, EnPortType.PcPoss),
				new PortStoreModel("به پرداخت TCP Version3", 1024, EnPortType.PcPoss),
				new PortStoreModel("سداد (BlueBird)", 8888, EnPortType.PcPoss),
				new PortStoreModel("رادین", 8080, EnPortType.Scale),
				new PortStoreModel("کارین Wifi دو فلش", 6005, EnPortType.Scale),
				new PortStoreModel("تکین Serial", 115200, EnPortType.Scale),
				new PortStoreModel("رادین Serial", 9600, EnPortType.Scale)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\PortStoreModel.cs", 50);
			throw;
		}
	}
}
