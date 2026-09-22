namespace SecureLab.Api.Presentation.Contracts;

public sealed record IncidentListItemResponse(
    Guid Id,
    string Title,
    string Severity,
    string Status,
    DateTimeOffset OccurredAtUtc,
    DateTimeOffset CreatedAtUtc);

public sealed record IncidentDetailsResponse(
    Guid Id,
    string Title,
    string Description,
    string Severity,
    string Status,
    DateTimeOffset OccurredAtUtc,
    DateTimeOffset CreatedAtUtc,
    string OwnerDisplayName,
    IReadOnlyList<IncidentCommentResponse> Comments);

public sealed record IncidentCommentResponse(
    Guid Id,
    string AuthorDisplayName,
    string Text,
    DateTimeOffset CreatedAtUtc);

/// <summary>
/// Контракт «нормативного» рівня для підсумку інцидентів за severity.
/// Повертає лише пару severity + count; жодних полів entity (owner, description тощо).
/// </summary>
public sealed record IncidentSeveritySummaryResponse(string Severity, int Count);
