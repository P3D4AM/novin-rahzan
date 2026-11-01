using System;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class FishDetJsonForHoghoghReport
{
	private short _typeAccount;

	public int HesabId { get; set; }

	public int FishNo { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public decimal TmpValue { get; set; }

	public string Description { get; set; } = "";


	public short TypeAccount
	{
		get
		{
			if (_typeAccount == 0 && Type > 0)
			{
				_typeAccount = Type;
			}
			return _typeAccount;
		}
		set
		{
			_typeAccount = value;
		}
	}

	public short Type { get; set; }

	public string Name { get; set; } = "";


	public bool IsDecrease { get; set; }

	public string Formula { get; set; } = "";


	public int Privilege { get; set; }

	public decimal TotalPayable { get; set; }

	public decimal RemainingBeforeBill { get; set; }

	public DateTime StartMounthPayM { get; set; }

	public bool Taxable { get; set; }

	public bool Insurance { get; set; }

	public bool DisableCompute { get; set; }

	public FishDetJsonForHoghoghReport(FishDetJson fishDetJson, int fishNo)
	{
		TypeAccount = fishDetJson.TypeAccount;
		Credit = fishDetJson.Credit;
		Debit = fishDetJson.Debit;
		Description = fishDetJson.Description;
		DisableCompute = fishDetJson.DisableCompute;
		Formula = fishDetJson.Formula;
		Insurance = fishDetJson.Insurance;
		HesabId = fishDetJson.HesabId;
		IsDecrease = fishDetJson.IsDecrease;
		Name = fishDetJson.Name;
		Privilege = fishDetJson.Privilege;
		RemainingBeforeBill = fishDetJson.RemainingBeforeBill;
		StartMounthPayM = fishDetJson.StartMounthPayM;
		Taxable = fishDetJson.Taxable;
		TmpValue = fishDetJson.TmpValue;
		FishNo = fishNo;
	}
}
