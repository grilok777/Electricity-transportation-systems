

namespace Domain.Enum
{
    public enum DayStatus
    {
        Pending,  // Власник подав, чекає рішення мережі
        Approved, // Мережа прийняла
        Rejected, // Мережа відхилила
        Canceled  // Власник сам скасував
    }
}
