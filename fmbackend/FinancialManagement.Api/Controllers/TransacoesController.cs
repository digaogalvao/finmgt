using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;

        public TransacoesController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacao>>> Index()
        {
            var transacoes = await _transacaoService.GetTransacoes();
            return Ok(transacoes);
        }

        [HttpGet("{id:int}", Name = "GetTransacao")]
        public async Task<ActionResult<Transacao>> Details(int id)
        {
            var transacao = await _transacaoService.GetTransacao(id);

            if (transacao is null)
                return NotFound($"Transação com id= {id} não encontrada");

            return Ok(transacao);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Transacao transacao)
        {
            _transacaoService.CreateTransacao(transacao);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Transacao transacao)
        {
            try
            {
                if (transacao.Id != id)
                    return NotFound($"Transação com id= {id} não encontrado");

                await _transacaoService.UpdateTransacao(id, transacao);
                return CreatedAtRoute("GetTransacao", new { id = transacao.Id }, transacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var transacao = await _transacaoService.GetTransacao(id);

                if (transacao is null)
                    return NotFound($"Transação com id= {id} não encontrado");

                await _transacaoService.DeleteTransacao(id);
                return Ok(transacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerarRelatorioDiario")]
        public IActionResult GerarRelatorioDiario([FromQuery] DateTime data)
        {
            var relatorioDiario = _transacaoService.RelatorioDiario(data);
            return Ok(relatorioDiario);
        }
    }
}
