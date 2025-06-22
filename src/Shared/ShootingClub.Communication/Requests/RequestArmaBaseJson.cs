using ShootingClub.Communication.Enums;
using System.Text.Json.Serialization;

namespace ShootingClub.Communication.Requests
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "tipoPosse")]
    [JsonDerivedType(typeof(RequestArmaExercitoJson), typeDiscriminator: (int)TipoPosseArma.Exercito)]
    [JsonDerivedType(typeof(RequestArmaPFJson), typeDiscriminator: (int)TipoPosseArma.PoliciaFederal)]
    [JsonDerivedType(typeof(RequestArmaPorteJson), typeDiscriminator: (int)TipoPosseArma.PortePessoal)]
    public abstract class RequestArmaBaseJson
    {
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("tipoPosse")]
        public virtual TipoPosseArma TipoPosse { get; protected set; }
        public string Cpf_proprietario = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public decimal? Calibre { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
    }
}
