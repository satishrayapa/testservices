﻿using System;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  public class LegalPartyDocument
  {
    public int LegalPartyRoleId { get; set; }

    public int? GrmEventId { get; set; }

    public int? RightTransferId { get; set; }

    public string LegalPartyDisplayName { get; set; }

    public DateTime? DocDate { get; set; }

    public string DocNumber { get; set; }

    public string DocType { get; set; }

    public decimal PctGain { get; set; }

  }

}