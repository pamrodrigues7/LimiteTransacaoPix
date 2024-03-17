using LimiteTransacaoPix.Models;

namespace LimiteTransacaoPix.nUnitTests
{
    public class TransacoesTests
    {
        private Transacoes _transacoes { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            _transacoes = new Transacoes() { Valor = 200};

        }

        [Test]
        public void VerificaSeTransacaoPodeSerFeita_Test()
        {
            //Assign
            decimal valorTransacoesNoDia = 100;
            decimal limite = 1000;
            decimal saldo = 500;


            //Act

            var realizarTransacao = _transacoes.TransacaoLiberada(valorTransacoesNoDia, limite, saldo);

            //Assert

            Assert.AreEqual(true, realizarTransacao);
        }

        [Test]
        public void VerificaSeTransacaoPodeSerFeitaFalse_Test()
        {
            //Assign
            decimal valorTransacoesNoDia = 400;
            decimal limite = 500;
            decimal saldo = 100;


            //Act

            var realizarTransacao = _transacoes.TransacaoLiberada(valorTransacoesNoDia, limite, saldo);

            //Assert

            Assert.AreEqual(false, realizarTransacao);
        }

        [Test]
        public void LimiteDisponivel_Test()
        {
            //Assign
            decimal valorTransacoesNoDia = 100;
            decimal limite = 1000;

            //Act

            var saldoDisponivel = _transacoes.LimiteDisponivel(valorTransacoesNoDia, limite);

            //Assert

            Assert.AreEqual(700, saldoDisponivel);
        }
    }
}