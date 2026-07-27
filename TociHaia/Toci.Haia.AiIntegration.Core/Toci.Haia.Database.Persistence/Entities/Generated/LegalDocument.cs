using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

public partial class LegalDocument
{
    public Guid LegalDocumentId { get; set; }

    public string DocumentKey { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public bool IsRequired { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<LegalDocumentVersion> LegalDocumentVersions { get; set; } = new List<LegalDocumentVersion>();
}
