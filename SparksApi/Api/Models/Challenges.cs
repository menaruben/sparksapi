namespace SparksApi.Api.Models;

public sealed record Challenges(
    int AbilityUses,
    float DamagePerMinute,
    int[] LegendaryItemUsed,
    float EffectiveHealingAndShielding,
    float GoldPerMinute,
    float Kda,
    float KillParticipation
);