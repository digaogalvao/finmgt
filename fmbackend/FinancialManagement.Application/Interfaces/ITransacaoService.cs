using FinancialManagement.Domain.Entities;

namespace FinancialManagement.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<IEnumerable<Transacao>> GetTransacoes();
        Task<Transacao> GetTransacao(int id);
        Task<int> CreateTransacao(Transacao transacao);
        Task UpdateTransacao(int id, Transacao transacao);
        Task DeleteTransacao(int id);
        Task<RelatorioDiario> RelatorioDiario(DateTime data);
    }
}
