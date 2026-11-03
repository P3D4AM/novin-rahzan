using System;
using Newtonsoft.Json;

namespace PacketParser.BusinessLayer.Models.Evaluation;

public class EvaluationDetailModel
{
	public Guid ProductGuid { get; set; }

	public string ProductName { get; set; }

	public string IdProduct { get; set; }

	public Guid? ProductGroupGuid { get; set; }

	public string ProductGroupName { get; set; }

	public string UnitName { get; set; }

	public DateTime? LastEvaluationDateM { get; set; }

	public string LastEvaluationDateSh => LastEvaluationDateM.MiladiToCalture();

	public decimal LastKardexPrice { get; set; }

	public decimal LastQuantity { get; set; }

	public decimal LastTotalPrice => Math.Round(LastKardexPrice * LastQuantity, DigitForRound);

	public decimal NewPrice { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public EvaluationDetailModel(short digitForRound)
	{
		DigitForRound = digitForRound;
	}
}
