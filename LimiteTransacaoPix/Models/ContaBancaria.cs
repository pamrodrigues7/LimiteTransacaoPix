using Amazon.DynamoDBv2.DataModel;
using LimiteTransacaoPix.Models.ConvertToString;
using System.ComponentModel.DataAnnotations;

namespace LimiteTransacaoPix.Models
{
    [DynamoDBTable("contaBancaria")]

    public class ContaBancaria
    {
        [Required]
        [DynamoDBHashKey]
        public string CPF { get; set; }
        [Required]
        public string Agencia { get; set; }
        [Required]
        public string Conta { get; set; }

        [Required]
        [DynamoDBProperty(typeof(DecimalConverterToDb))]
        public decimal Saldo { get; set; }

    }
}
