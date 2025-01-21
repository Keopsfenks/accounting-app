namespace Domain.Entities.CompanyEntities;

public sealed class Cheque {
	public string MaturityDate { get; set; }
	public string   BankName     { get; set; }
	public string   ChequeNumber { get; set; }

}