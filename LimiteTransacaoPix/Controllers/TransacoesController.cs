using LimiteTransacaoPix.Models;
using LimiteTransacaoPix.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LimiteTransacaoPix.Controllers
{
    public class TransacoesController : Controller
    {
        private const string index = "Index";
        private readonly ITransacoesRepository _repo;
        public TransacoesController(ITransacoesRepository transacoesRepository)
        {
            _repo = transacoesRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transacoes transacao)
        {
            var transacaoCriadaESaldoLimite = await _repo.CreateAsync(transacao);
            
            if (!transacaoCriadaESaldoLimite.transacaoCriada)
                throw new Exception("Não foi possível realizar a transação.");
            
            return RedirectToAction(index);
        }

    }
}
