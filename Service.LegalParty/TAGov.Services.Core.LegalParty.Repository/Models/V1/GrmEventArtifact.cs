using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  [Table( "GRMEventArtifact" )]
  public class GrmEventArtifact
  {
    public int Id { get; set; }

    public int GrmEventId { get; set; }

    public int ObjectType { get; set; }

    public int ObjectId { get; set; }
  }
}