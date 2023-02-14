using ProyectoTienda.ADO.NET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoTienda;

namespace ProyectoTienda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("{usuario}/{contrasena}")]
        public Usuario Login(string usuario, string contrasena) {
            return UsuarioHandler.Login(usuario, contrasena);

        }
        [HttpPost]
        public void CrearUsuario(Usuario usuarioNuevo)
        {
            UsuarioHandler.InsertarUsuario(usuarioNuevo);
        }
        [HttpPut]
        public void ModificarUsuario(Usuario usuarioModificado)
        {
            UsuarioHandler.ModificarUsuario(usuarioModificado);
        }

    }
}
