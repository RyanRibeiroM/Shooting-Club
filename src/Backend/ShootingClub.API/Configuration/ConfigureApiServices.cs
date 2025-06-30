using ShootingClub.API.Converters;
using ShootingClub.Communication.Enums;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ShootingClub.API.Configuration
{
    public static class ConfigureApiServices
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new StringConverter());

                options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers =
                    {
                        static typeInfo =>
                        {
                            // Configuração para DESERIALIZAÇÃO (Request)
                            // Garante que a API saiba como ler o JSON de uma nova arma
                            if (typeInfo.Type == typeof(RequestArmaBaseJson))
                            {
                                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "tipoPosse",
                                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(RequestArmaExercitoJson), (int)TipoPosseArma.Exercito),
                                        new JsonDerivedType(typeof(RequestArmaPFJson), (int)TipoPosseArma.PoliciaFederal),
                                        new JsonDerivedType(typeof(RequestArmaPorteJson), (int)TipoPosseArma.PortePessoal)
                                    }
                                };
                            }
                            // Configuração para SERIALIZAÇÃO (Response Curta)
                            // Garante que a API saiba como montar o JSON para listas de armas
                            else if (typeInfo.Type == typeof(ResponseArmaShortJson))
                            {
                                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "tipoPosse",
                                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(ResponseArmaExercitoShortJson), TipoPosseArma.Exercito.ToString()),
                                        new JsonDerivedType(typeof(ResponseArmaPFShortJson), TipoPosseArma.PoliciaFederal.ToString()),
                                        new JsonDerivedType(typeof(ResponseArmaPortePessoalShortJson), TipoPosseArma.PortePessoal.ToString())
                                    }
                                };
                            }
                            // Configuração para SERIALIZAÇÃO (Response Completa)
                            // Garante que a API saiba como montar o JSON para detalhes de uma arma
                            else if (typeInfo.Type == typeof(ResponseArmaBaseJson))
                            {
                                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "tipoPosse",
                                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(ResponseArmaExercitoJson), TipoPosseArma.Exercito.ToString()),
                                        new JsonDerivedType(typeof(ResponseArmaPFJson), TipoPosseArma.PoliciaFederal.ToString()),
                                        new JsonDerivedType(typeof(ResponseArmaPortePessoalJson), TipoPosseArma.PortePessoal.ToString())
                                    }
                                };
                            }
                        }
                    }
                };
            });

            return services;
        }
    }
}