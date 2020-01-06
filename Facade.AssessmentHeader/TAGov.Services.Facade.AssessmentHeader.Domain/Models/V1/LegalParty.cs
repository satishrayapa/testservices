namespace TAGov.Services.Facade.AssessmentHeader.Domain.Models.V1
{
  public class LegalParty
  {
    public int LegalPartyId { get; set; }
    public string DisplayName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string NameSfx { get; set; }
    public int RevenueAcct { get; set; }
    public int LegalPartyRoleObjectType { get; set; }
  }
}
