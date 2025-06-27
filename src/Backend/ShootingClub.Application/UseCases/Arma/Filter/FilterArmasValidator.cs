using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingClub.Application.UseCases.Arma.Filter
{
    public class FilterArmasValidator : AbstractValidator<RequestFilterArmaJson>
    {
        public FilterArmasValidator()
        {
            RuleFor(a => a.TipoPosse).IsInEnum().WithMessage(ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO);
        }
    }
}
