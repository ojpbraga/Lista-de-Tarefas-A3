using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAssociadoApplication : IApplication<AssociadoDTO>
    {
        Task<AssociadoDTO> GetByPlaca(string cpf);
    }
}
