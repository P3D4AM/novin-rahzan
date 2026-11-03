using System;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel.RequestModel;

public class TafsilRequestModel : BaseRequestModel
{
	public HesabType TypeHesab;

	public Guid? Tafsillevel { get; set; }
}
