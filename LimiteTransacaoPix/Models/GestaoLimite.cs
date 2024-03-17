using Amazon.DynamoDBv2.DataModel;
using LimiteTransacaoPix.Models.ConvertToString;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LimiteTransacaoPix.Models
{
    [DynamoDBTable("gestaoLimite")]
    public class GestaoLimite
    {
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [DynamoDBHashKey]
        public string CPF { get; set; }

        [Required(ErrorMessage = "A Agência é obrigatório")]
        public string Agencia { get; set; }

        [Required(ErrorMessage = "A Conta é obrigatório")]
        public string Conta { get; set; }

        [Required(ErrorMessage = "O Limite para Transação PIX é obrigatório")]
        [DynamoDBProperty(typeof(DecimalConverterToDb))]
        public decimal LimiteParaTransacao { get; set; }
    }

    public class GestaoLimiteAPI 
    {
        public const string Create = "Create";
        public const string Edit = "Edit";
        public const string GetListGestaoLimite = "GetListGestaoLimite";
        public const string DeleteConfirmed = "DeleteConfirmed";
        public const string Delete = "Delete";
    }
}