using System.Collections.Generic;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class CustomerSaveResponse
{
	public CusTmpCmtRsp Status { get; set; }

	public string VerificationCode { get; set; }

	public string LineNumber { get; set; }

	public List<SaveResponse> SaveResponses { get; set; }
}
