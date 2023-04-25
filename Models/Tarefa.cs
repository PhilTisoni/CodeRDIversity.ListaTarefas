using System.ComponentModel.DataAnnotations.Schema;

namespace ListaTarefas.Models
{
    [Table("Tarefa")]
    public class Tarefa
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("descricao")]
        public string Descricao { get; set; }
        [Column("dataCriacao")]
        public DateTime DataCriacao { get; set; }
        [Column("responsavel")]
        public string Responsavel { get; set; }
        [Column("isConcluido")]
        public bool IsConcluido { get; set; }
    }
}
