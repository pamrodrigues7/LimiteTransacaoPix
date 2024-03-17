using LimiteTransacaoPix.Models;

namespace LimiteTransacaoPix.Repository.Interface
{
    public interface IContaBancariaRepository
    {
        Task<bool> CreateByGestaoLimiteAsync(GestaoLimite gestaoLimite);
        Task<decimal> GetSaldoAsync(string cpf);
        Task<bool> UpdateByTransacaoAsync(string cpf, decimal valorTransacao);
    }
}
