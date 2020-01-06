using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalParty.Domain.Models.V1
{
  /// <summary>
  /// Legal Party Search specific DTO.
  /// </summary>
  public class LegalPartySearchDto
  {
    /// <summary>
    /// public constructor for LegalPartySearchDto
    /// </summary>
    public LegalPartySearchDto()
    {
      this.LegalPartyRoleIdList = new List<int>();
    }

    /// <summary>
    /// Gets or sets a list of legal party role Ids that you want to retrieve.
    /// </summary>
    public List<int> LegalPartyRoleIdList { get; private set; }

    /// <summary>
    /// Gets or sets the effective date.
    /// </summary>
    public DateTime EffectiveDate { get; set; }
  }
}
