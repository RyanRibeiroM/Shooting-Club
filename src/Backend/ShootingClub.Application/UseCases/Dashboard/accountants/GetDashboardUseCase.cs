
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;

namespace ShootingClub.Application.UseCases.Dashboard.accountants
{
    public class GetDashboardUseCase : IGetDashboardUseCase
    {
        private readonly IArmaReadOnlyRepository _armaRepository;
        private readonly IUsuarioReadOnlyRepository _usuarioRepository;
        private readonly ILoggedUsuario _loggedUsuario;

        public GetDashboardUseCase(
            IArmaReadOnlyRepository armaRepository,
            IUsuarioReadOnlyRepository usuarioRepository,
            ILoggedUsuario loggedUsuario)
        {
            _armaRepository = armaRepository;
            _usuarioRepository = usuarioRepository;
            _loggedUsuario = loggedUsuario;
        }

        public async Task<ResponseAccountantsDashboard> Execute()
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            var armasAtrasadasTask = await _armaRepository.CountExpiredByClub(loggedUsuario.ClubeId);
            var usuariosNoClubeTask = await _usuarioRepository.CountTotalByClub(loggedUsuario.ClubeId);
            var totalArmasTask = await _armaRepository.CountTotalByClub(loggedUsuario.ClubeId);

            return new ResponseAccountantsDashboard
            {
                QuantArmasAtrasadas = armasAtrasadasTask,
                QuantUsuariosNoClube = usuariosNoClubeTask,
                QuantTotalArmas = totalArmasTask
            };
        }
    }
}
