namespace ShootingClub.Communication.Requests
{
    public class RequestRegisterUsuarioJson
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string EnderecoPais { get; set; } = string.Empty;
        public string EnderecoEstado { get; set; } = string.Empty;
        public string EnderecoCidade { get; set; } = string.Empty;
        public string EnderecoBairro { get; set; } = string.Empty;
        public string EnderecoRua { get; set; } = string.Empty;
        public string EnderecoNumero { get; set; } = string.Empty;
        public string CR { get; set; } = string.Empty;
        public string NumeroFiliacao { get; set; } = string.Empty;
        public DateOnly DataFiliacao { get; set; }
        public DateOnly DataRenovacaoFiliacao { get; set; }


    }
}
