using LimiteTransacaoPix.Models;
using LimiteTransacaoPix.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LimiteTransacaoPix.Controllers
{
    public class GestaoLimiteController : Controller
    {
        private const string index = "Index";

        private readonly IGestaoLimiteRepository _repo;
        public GestaoLimiteController(IGestaoLimiteRepository gestaoLimiteRepository)
        {
            _repo = gestaoLimiteRepository;
        }

        public async Task<IActionResult> Index()
        {
            var limitesPix = await _repo.GetListGestaoLimite();
            return View(limitesPix);
        }
        public IActionResult Create()
        {
            return View();
        }

		public async Task<IActionResult> Edit(string cpf)
		{
			var gestaoLimite = await _repo.GetByCpfAsync(cpf);
			return View(gestaoLimite);
		}

        [HttpGet]
		public async Task<IActionResult> Delete(string cpf)
		{
			if (cpf is null)
				return NotFound();

			var gestaoLimite = await _repo.GetByCpfAsync(cpf);
			
            if (gestaoLimite is null)
				return NotFound();

			return View(gestaoLimite);
        }

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(string cpf)
		{
			var gestaoLimite = await _repo.GetByCpfAsync(cpf);

            if (gestaoLimite is not null)
                await _repo.DeleteAsync(cpf);

            return RedirectToAction(index);
		}

        [HttpPost]
        public async Task<IActionResult> Create(GestaoLimite gestaoLimite)
        {
            var gestaoLimiteCriada = await _repo.CreateAsync(gestaoLimite);

            if (!gestaoLimiteCriada)
                throw new Exception("Não foi possível realizar o cadastro.");

            return RedirectToAction(index);
        }       

        [HttpPost]
        public async Task<IActionResult> Edit(GestaoLimite gestaoLimite)
        {
            var gestaoLimiteAtualizada = await _repo.UpdateAsync(gestaoLimite);

            if (!gestaoLimiteAtualizada)
                throw new Exception("Não foi possível atualizar o cadastro.");

            return RedirectToAction(index);
        }

        [HttpGet]
        public IActionResult GetListGestaoLimite()
        {
            _repo.GetListGestaoLimite();
            return RedirectToAction(index);
        }
    }
}
