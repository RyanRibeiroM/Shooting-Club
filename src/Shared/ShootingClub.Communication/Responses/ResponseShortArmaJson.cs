using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingClub.Communication.Responses
{
    public class ResponseShortArmaJson
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public DateOnly DataValidade { get; set; }
    }
}
