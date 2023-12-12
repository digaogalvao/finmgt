using FinancialManagement.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialManagement.Domain.Entities
{
    public class Transacao
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }
        public EnumTipoTransacao Tipo { get; set; }
    }
}
