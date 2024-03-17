using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;

namespace LimiteTransacaoPix.DataBase
{
    public class DataBase
    {
        private const string gestaoLimite = "gestaoLimite";
        private const string transacoes = "transacoes";
        private const string contaBancaria = "contaBancaria";

        private readonly AmazonDynamoDBClient _dynamoDbClient;

        public DataBase(AmazonDynamoDBClient dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task CreateDynamoTableAsync()
        {
            var gestaoLimiteExists = await TableExistsAsync(gestaoLimite);
            var transacoesExists = await TableExistsAsync(transacoes);
            var contaBancariaExists = await TableExistsAsync(contaBancaria);

            if (gestaoLimiteExists && transacoesExists && contaBancariaExists)
            {
                return;
            }
            if (!gestaoLimiteExists)
            {
                var request = new CreateTableRequest
                {
                    TableName = gestaoLimite,
                    AttributeDefinitions = new List<AttributeDefinition>
        {
            new AttributeDefinition
            {
                AttributeName = "CPF",
                AttributeType = "S"
            }
        },
                    KeySchema = new List<KeySchemaElement>
        {
            new KeySchemaElement
            {
                AttributeName = "CPF",
                KeyType = "HASH"
            }
        },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                };

                var response = await _dynamoDbClient.CreateTableAsync(request);

            }           
            
            if (!contaBancariaExists)
            {
                var request = new CreateTableRequest
                {
                    TableName = contaBancaria,
                    AttributeDefinitions = new List<AttributeDefinition>
        {
            new AttributeDefinition
            {
                AttributeName = "CPF",
                AttributeType = "S"
            }
        },
                    KeySchema = new List<KeySchemaElement>
        {
            new KeySchemaElement
            {
                AttributeName = "CPF",
                KeyType = "HASH"
            }
        },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                };

                var response = await _dynamoDbClient.CreateTableAsync(request);

            }

            if (!transacoesExists)
            {
                var request = new CreateTableRequest
                {
                    TableName = transacoes,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "CPF",
                            AttributeType = "S"
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "Data",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "CPF",
                            KeyType = KeyType.HASH
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "Data",
                            KeyType = KeyType.RANGE
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                };

                var response = await _dynamoDbClient.CreateTableAsync(request);
            }
        }

        async Task<bool> TableExistsAsync(string tableName)
        {
            try
            {
                await _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                {
                    TableName = tableName
                });
                return true;
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }
        }
    }
}