using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttContractDetail : IContractDet
{
	private bool insurance;

	private bool taxable;

	public decimal Account { get; set; }

	public string Desc { get; set; }

	public string Formula { get; set; }

	public int HesabId { get; set; }

	public bool IsDecrease { get; set; }

	public string Name { get; set; }

	public short Order { get; set; }

	public int Privilege { get; set; }

	public short TypeAccount { get; set; }

	public decimal nValue { get; set; }

	public bool GetInsurance()
	{
		return insurance;
	}

	public void SetInsurance(bool value)
	{
		insurance = value;
	}

	public bool GetTaxable()
	{
		return taxable;
	}

	public void SetTaxable(bool value)
	{
		taxable = value;
	}
}
