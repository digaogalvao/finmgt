using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagement.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly AppDbContext _dbContext;

        public TransacaoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Transacao>> GetTransacoes()
        {
            return await _dbContext.Transacoes.ToListAsync();
        }

        public async Task<Transacao> GetTransacao(int id)
        {
            var transacao = await _dbContext.Transacoes.SingleOrDefaultAsync(c => c.Id == id);

            return transacao;
        }

        public async Task<int> CreateTransacao(Transacao transacao)
        {
            _dbContext.Transacoes.Add(transacao);
            _dbContext.SaveChanges();
            return transacao.Id;
        }

        public async Task UpdateTransacao(Transacao transacao)
        {
            _dbContext.Entry(transacao).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTransacao(Transacao transacao)
        {
            _dbContext.Transacoes.Remove(transacao);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transacao>> RelatorioDiario(DateTime data)
        {
            return _dbContext.Transacoes
                .Where(t => EF.Functions.DateDiffDay(t.Data, data) == 0)
                .ToList();
        }
    }
}
