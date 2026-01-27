using System.Text.Json.Serialization;

namespace SportManagementSystem.Entities.Enums;

/// <summary>
/// Категория спортивной услуги.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EServiceCategory
{
    Training,
    Massage,
    Sauna,
    Fitness,
    Other
}