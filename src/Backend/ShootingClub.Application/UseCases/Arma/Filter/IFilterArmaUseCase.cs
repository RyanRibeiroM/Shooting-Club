using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingClub.Application.UseCases.Arma.Filter
{
    public interface IFilterArmaUseCase
    {
        public Task<ResponseArmasJson> Execute(RequestFilterArmaJson request);
    }
}
