using SampleAPI.Enums;

namespace SampleAPI.Services.DataProviders.Models
{
    public record Choice(int Id, string Name)
    {
        public Choice(GameChoice enumElement)
        : this((int)enumElement, enumElement.ToString())
        { }
    }
}
