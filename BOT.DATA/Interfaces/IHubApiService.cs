using BOT.DATA.Models;

namespace BOT.DATA.Interfaces
{
    public interface IHubApiService
    {
        EmpleadoModel GetDataEmployee(string ficha);
        DatosAreaEmpleadoModelo GetDataEmployeeArea(string ficha);
    }
}
