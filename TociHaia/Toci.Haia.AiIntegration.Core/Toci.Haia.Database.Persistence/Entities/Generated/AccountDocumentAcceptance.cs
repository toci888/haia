using System;
using System.Collections.Generic;
using System.Net;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class AccountDocumentAcceptance
{
    public Guid AccountDocumentAcceptanceId { get; set; }

    public Guid AccountId { get; set; }

    public Guid LegalDocumentVersionId { get; set; }

    public string AcceptanceType { get; set; } = null!;

    public DateTime AcceptedAt { get; set; }

    public string AcceptedVia { get; set; } = null!;

    public IPAddress? SourceIp { get; set; }

    public string? UserAgent { get; set; }

    public string? AcceptanceContext { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual LegalDocumentVersion LegalDocumentVersion { get; set; } = null!;
}
