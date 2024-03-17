using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LimiteTransacaoPix.Models.ConvertToString;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LimiteTransacaoPix.Models
{
    [DynamoDBTable("transacoes")]
    public class Transacoes
    {
        [DynamoDBHashKey]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O Limite para Transação PIX é obrigatório")]
        [DynamoDBProperty(typeof(DecimalConverterToDb))]
        public decimal Valor { get; set; }

        [Required]
        [DynamoDBProperty(typeof(DateTimeConverterToDb))]
        public DateTime Data { get; set; }

        #region Function

        public bool TransacaoLiberada(decimal valorTransacoesNoDia, decimal limite, decimal saldo)
        {
            return ((limite >= (valorTransacoesNoDia + Valor)) || saldo > Valor);
        }        
        public decimal LimiteDisponivel(decimal transacoesNoDia, decimal limite)
        {
            return (limite - transacoesNoDia - Valor);
        }

        #endregion

    }

    public class TransacoesAPI 
    {
        public const string Create = "Create";
    }
}
