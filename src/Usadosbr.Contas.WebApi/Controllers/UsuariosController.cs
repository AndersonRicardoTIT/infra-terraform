using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usadosbr.Contas.Core.Common.Result;
using Usadosbr.Contas.Core.Features.Usuarios.Commands.CriaUsuarioCommand;
using Usadosbr.Contas.WebApi.Shared.Constants;

namespace Usadosbr.Contas.WebApi.Controllers
{
    [ApiVersion(ApiVersions.V1)]
    public class UsuariosController : ApiControllerBase
    {
        private readonly ISender _sender;

        public UsuariosController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [MapToApiVersion(ApiVersions.V1)]
        [HttpPost(Name = nameof(CadastraUsuario))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Result<Guid>> CadastraUsuario([FromBody] CriaUsuarioCommand command)
        {
            var response = await _sender.Send(command);
            
            return response;
        }
    }
}