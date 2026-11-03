using System;
using PacketParser.Entities;

namespace PacketParser.Activation;

public class Customer
{
	private Guid _guid;

	public int HID { get; set; }

	public string FullName { get; set; }

	public string Zip { get; set; }

	public string Address { get; set; }

	public string Tel1 { get; set; }

	public string Tel2 { get; set; }

	public string Fax { get; set; }

	public string Mob1 { get; set; }

	public string Mob2 { get; set; }

	public string Mob3 { get; set; }

	public string Mob4 { get; set; }

	public string E_Mail { get; set; }

	public string Description { get; set; }

	public bool Status { get; set; } = true;


	public bool JustCRM { get; set; }

	public long Code { get; set; }

	public Guid GUID
	{
		get
		{
			if (_guid == Guid.Empty)
			{
				_guid = Guid.NewGuid();
			}
			return _guid;
		}
		set
		{
			_guid = value;
		}
	}

	public Customer()
	{
	}

	public Customer(WebCus customer)
	{
		if (customer == null)
		{
			throw new ArgumentException("Customer is null!!");
		}
		int num = 0;
		try
		{
			num = 100;
			FullName = customer.FullName;
			num = 1;
			Zip = customer.Zip;
			num = 2;
			Address = customer.Address1;
			num = 3;
			Tel1 = customer.Tel1;
			num = 4;
			Tel2 = customer.Tel2;
			num = 5;
			Fax = customer.Fax;
			num = 6;
			Mob1 = customer.Mob1;
			num = 7;
			Mob2 = customer.Mob2;
			num = 8;
			E_Mail = customer.Email;
			num = 9;
			Description = customer.Description;
			num = 10;
			Status = customer.Status;
			num = 11;
			GUID = customer.Guid;
			num = 13;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//customer is null:{customer}//{Json.ToStringJson(customer)}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\Customer.cs", 49);
		}
	}
}
