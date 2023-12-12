using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Enum;
using FinancialManagement.Domain.Interfaces;

namespace FinancialManagement.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IEnumerable<Transacao>> GetTransacoes()
        {
            var transacoes = await _transacaoRepository.GetTransacoes();

            transacoes = transacoes.OrderByDescending(t => t.Data);

            return transacoes;
        }

        public async Task<Transacao> GetTransacao(int id)
        {
            return await _transacaoRepository.GetTransacao(id);
        }

        public async Task<int> CreateTransacao(Transacao transacao)
        {
            var createTransacao = new Transacao
            {
                Data = transacao.Data,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo
            };

            var transacaoId = await _transacaoRepository.CreateTransacao(createTransacao);

            return transacaoId;
        }

        public async Task UpdateTransacao(int id, Transacao transacao)
        {
            if (transacao is null)
                throw new ApplicationException("Dados inválidos");

            await _transacaoRepository.UpdateTransacao(transacao);
        }

        public async Task DeleteTransacao(int id)
        {
            var transacao = await _transacaoRepository.GetTransacao(id);

            if (transacao is null)
                throw new ApplicationException("Transação não encontrada");

            await _transacaoRepository.DeleteTransacao(transacao);
        }

        public async Task<RelatorioDiario> RelatorioDiario(DateTime data)
        {
            var transacoesDoDia = await _transacaoRepository.RelatorioDiario(data);

            decimal saldoDoDia = transacoesDoDia.Sum(t => t.Tipo == EnumTipoTransacao.Credito ? t.Valor : -t.Valor);

            var relatorioDiario = new RelatorioDiario
            {
                Data = data,
                SaldoDoDia = saldoDoDia
            };

            return relatorioDiario;
        }

    }
}
