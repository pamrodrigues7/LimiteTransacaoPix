using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using LimiteTransacaoPix.Models;
using Amazon.DynamoDBv2.DocumentModel;
using LimiteTransacaoPix.Repository.Interface;

namespace LimiteTransacaoPix.Repository
{
    public class GestaoLimiteRepository : IGestaoLimiteRepository
    {
        private readonly DynamoDBContext _context;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public GestaoLimiteRepository(AmazonDynamoDBClient dynamoDbClient, IContaBancariaRepository contaBancariaRepository)
        {
            _context = new DynamoDBContext(dynamoDbClient);
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<bool> CreateAsync(GestaoLimite gestaoLimite)
        {
            try
            {
                if(gestaoLimite.CPF is null || 
                    gestaoLimite.Agencia is null || 
                    gestaoLimite.Conta is null || 
                    gestaoLimite.LimiteParaTransacao == 0)
                    return false;

                var gestaoLimiteJaCadastrado = await GetByCpfAsync(gestaoLimite.CPF);

                if (gestaoLimiteJaCadastrado is not null)
                    return false;
                else
                {
                    //Este método só é utilizado pra ter uma conta bancária criada pra poder testar
                    //o método que verifica e atualiza o saldo na transação 
                    await _contaBancariaRepository.CreateByGestaoLimiteAsync(gestaoLimite); 
                    await _context.SaveAsync(gestaoLimite);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return true;
        }

        public async Task<bool> UpdateAsync(GestaoLimite gestaoLimite)
        {
            try
            {
                await _context.SaveAsync(gestaoLimite);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return true;
        }

        public async Task<List<GestaoLimite>> GetListGestaoLimite()
        {
            try
            {
                var scanConditions = new List<ScanCondition>(); // Pode adicionar condições aqui, se necessário
                var config = new DynamoDBOperationConfig();
                config.OverrideTableName = "gestaoLimite"; // Nome da sua tabela DynamoDB

                var result = await _context.ScanAsync<GestaoLimite>(scanConditions, config).GetNextSetAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<GestaoLimite> GetByCpfAsync(string cpf)
        {
            try
            {
                return await _context.LoadAsync<GestaoLimite>(cpf);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string cpf)
        {
            try
            {
                var objToDelete = await _context.LoadAsync<GestaoLimite>(cpf);

                if (objToDelete is not null)
                    await _context.DeleteAsync<GestaoLimite>(cpf);
                else
                    throw new Exception("Houve um erro na exclusão do limite de transação!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return true;
        }
    }
}
