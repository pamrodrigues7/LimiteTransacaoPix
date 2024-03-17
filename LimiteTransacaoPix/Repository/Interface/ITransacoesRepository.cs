using LimiteTransacaoPix.Models;

namespace LimiteTransacaoPix.Repository.Interface
{
    public interface ITransacoesRepository
    {
        Task<(bool transacaoCriada, decimal valorLimiteDisponivel)> CreateAsync(Transacoes transacoes);
    }
}
