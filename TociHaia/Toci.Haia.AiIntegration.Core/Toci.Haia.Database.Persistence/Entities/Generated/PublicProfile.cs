using System;
using System.Collections.Generic;

namespace Toci.Haia.Database.Persistence.Entities;

/// <summary>
/// Publiczna tożsamość użytkownika. Pseudonim rezerwowany podczas rejestracji.
/// </summary>
public partial class PublicProfile
{
    public Guid ProfileId { get; set; }

    public Guid AccountId { get; set; }

    public string Nickname { get; set; } = null!;

    public string NicknameNormalized { get; set; } = null!;

    public string ProfileVisibility { get; set; } = null!;

    public string? AvatarAssetRef { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
