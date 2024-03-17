using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using LimiteTransacaoPix.Repository.Interface;
using LimiteTransacaoPix.Models;

namespace LimiteTransacaoPix.Repository
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly DynamoDBContext _context;


        public ContaBancariaRepository(AmazonDynamoDBClient dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<bool> CreateByGestaoLimiteAsync(GestaoLimite gestaoLimite)
        {
            try
            {
                ContaBancaria contaBancaria = new()
                {
                    CPF = gestaoLimite.CPF,
                    Agencia = gestaoLimite.Agencia,
                    Conta = gestaoLimite.Conta,
                    Saldo = 5000
                };

                await _context.SaveAsync(contaBancaria);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return true;
        }

        public async Task<bool> UpdateByTransacaoAsync(string cpf, decimal valorTransacao)
        {
            try
            {
                var contaBancaria = await GetContaBancariaAsync(cpf);

                if (contaBancaria is not null)
                {
                    contaBancaria.Saldo -=valorTransacao;
                    await _context.SaveAsync(contaBancaria);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return true;
        }

        public async Task<decimal> GetSaldoAsync(string cpf)
        {
            try
            {
                var contaBancariaObj = await _context.LoadAsync<ContaBancaria>(cpf);

                return contaBancariaObj?.Saldo ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }


        }

        public async Task<ContaBancaria> GetContaBancariaAsync(string cpf)
        {
            try
            {
                var contaBancariaObj = await _context.LoadAsync<ContaBancaria>(cpf);

                return contaBancariaObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }


        }
    }
}

