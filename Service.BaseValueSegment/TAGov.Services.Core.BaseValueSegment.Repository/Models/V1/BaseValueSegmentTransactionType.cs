using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "BVSTranType", Schema = "Service.BaseValueSegment" )]
  public class BaseValueSegmentTransactionType
  {
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Required]
    [Column( TypeName = "varchar(50)" )]
    public string Name { get; set; }

    [Required]
    [Column( TypeName = "varchar(100)" )]
    public string Description { get; set; }

    public virtual ICollection<BaseValueSegmentTransaction> BaseValueSegmentTransactions { get; set; }
      = new List<BaseValueSegmentTransaction>();
  }
}
