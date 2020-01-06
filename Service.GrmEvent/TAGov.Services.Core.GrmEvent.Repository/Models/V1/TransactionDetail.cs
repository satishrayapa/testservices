using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  [Table( "TranDetail" )]
  public class TransactionDetail
  {
    [Key]
    [Column( "id" )]
    public long Id { get; set; }

    [Column( "sequenceNumber" )]
    public int SequenceNumber { get; set; }

    [Column( "objectType" )]
    public int ObjectType { get; set; }
  }
}