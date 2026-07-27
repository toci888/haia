using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class LegalDocumentVersion
{
    public Guid LegalDocumentVersionId { get; set; }

    public Guid LegalDocumentId { get; set; }

    public string VersionLabel { get; set; } = null!;

    public string ContentHash { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AccountDocumentAcceptance> AccountDocumentAcceptances { get; set; } = new List<AccountDocumentAcceptance>();

    public virtual LegalDocument LegalDocument { get; set; } = null!;
}
