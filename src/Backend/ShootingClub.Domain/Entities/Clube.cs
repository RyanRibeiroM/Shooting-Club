﻿namespace ShootingClub.Domain.Entities
{
    public class Clube : EntityBase
    {
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string CertificadoRegistro { get; set; } = string.Empty;
        public string EnderecoPais { get; set; } = string.Empty;
        public string EnderecoEstado { get; set; } = string.Empty;
        public string EnderecoCidade { get; set; } = string.Empty;
        public string EnderecoBairro { get; set; } = string.Empty;
        public string EnderecoRua { get; set; } = string.Empty;
        public string EnderecoNumero { get; set; } = string.Empty;
        public int ResponsavelId { get; set; }

    }
}
