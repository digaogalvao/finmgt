using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<IEnumerable<Transacao>> GetTransacoes();
        Task<Transacao> GetTransacao(int id);
        Task<int> CreateTransacao(Transacao transacao);
        Task UpdateTransacao(Transacao transacao);
        Task DeleteTransacao(Transacao transacao);
        Task<IEnumerable<Transacao>> RelatorioDiario(DateTime data);
    }
}
