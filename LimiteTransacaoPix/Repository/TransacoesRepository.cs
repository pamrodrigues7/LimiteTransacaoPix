using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using LimiteTransacaoPix.Repository.Interface;
using LimiteTransacaoPix.Models;
using Amazon.DynamoDBv2.DocumentModel;
using System.Globalization;

namespace LimiteTransacaoPix.Repository
{
    public class TransacoesRepository : ITransacoesRepository
    {
        private readonly DynamoDBContext _context;
        private readonly IGestaoLimiteRepository _gestaoLimiteRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public TransacoesRepository(AmazonDynamoDBClient dynamoDbClient, IGestaoLimiteRepository gestaoLimiteRepository, IContaBancariaRepository contaBancariaRepository)
        {
            _context = new DynamoDBContext(dynamoDbClient);
            _gestaoLimiteRepository = gestaoLimiteRepository;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<(bool transacaoCriada, decimal valorLimiteDisponivel)> CreateAsync(Transacoes transacoes)
        {
            decimal valorLimiteDisponivel;
            try
            {
                if (transacoes.CPF is null || transacoes.Valor == 0)
                    return (false, 0);

                transacoes.Data = DateTime.Now;

                var transacaoPodeSerFeitaESaldoLimite = await VerificaSeTransacaoPodeSerFeita(transacoes);
                valorLimiteDisponivel = transacaoPodeSerFeitaESaldoLimite.valorLimiteDisponivel;

                if (!transacaoPodeSerFeitaESaldoLimite.transacaoPodeSerFeita)
                    return (false, valorLimiteDisponivel);
                else 
                {
                    await _contaBancariaRepository.UpdateByTransacaoAsync(transacoes.CPF, transacoes.Valor);
                    await _context.SaveAsync(transacoes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return (true, valorLimiteDisponivel);

        }

        private async Task<List<Transacoes>> GetTransacoesNoMesmoDiaByCpfList(string cpf)
        {
            try
            {
                var startOfDay = DateTime.Today.Date; 
                var endOfDay = startOfDay.AddDays(1);


                var scanConditions = new List<ScanCondition>
        {
            new ScanCondition("CPF", ScanOperator.Equal, cpf),
            new ScanCondition("Data", ScanOperator.Between, startOfDay, endOfDay)
        };

                var config = new DynamoDBOperationConfig();
                config.OverrideTableName = "transacoes";

                var result = await _context.ScanAsync<Transacoes>(scanConditions, config).GetNextSetAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        private async Task<(bool transacaoPodeSerFeita, decimal valorLimiteDisponivel)> VerificaSeTransacaoPodeSerFeita(Transacoes transacoes)
        {
            var saldo = await _contaBancariaRepository.GetSaldoAsync(transacoes.CPF);

             var listTransacoesNoDia = await GetTransacoesNoMesmoDiaByCpfList(transacoes.CPF);

            var getaoLimite = await _gestaoLimiteRepository.GetByCpfAsync(transacoes.CPF);

            var totalTransacoesNoDia = listTransacoesNoDia?.Select(s => s.Valor).Sum() ?? 0;

            var transacaoLiberada = transacoes.TransacaoLiberada(totalTransacoesNoDia, getaoLimite.LimiteParaTransacao, saldo);

            var saldoLimiteDisponivel = transacoes.LimiteDisponivel(totalTransacoesNoDia, getaoLimite.LimiteParaTransacao);

            return (transacaoLiberada, saldoLimiteDisponivel);

        }

    }
}
