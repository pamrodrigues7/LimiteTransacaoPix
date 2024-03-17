using LimiteTransacaoPix.Models;

namespace LimiteTransacaoPix.Repository.Interface
{
    public interface IGestaoLimiteRepository
    {
        Task<bool> CreateAsync(GestaoLimite gestaoLimite);
        Task<bool> UpdateAsync(GestaoLimite gestaoLimite);
        Task<List<GestaoLimite>> GetListGestaoLimite();
        Task<GestaoLimite> GetByCpfAsync(string cpf);
        Task<bool> DeleteAsync(string cpf);

    }
}
