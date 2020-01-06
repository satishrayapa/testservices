using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalParty.Repository.Models
{
  [Table( "GRMEvent" )]
  public class GrmEvent
  {
    public int Id { get; set; }

    public int EventType { get; set; }
  }
}