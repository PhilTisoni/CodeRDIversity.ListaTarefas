using ListaTarefas.Data;
using ListaTarefas.Models;
using ListaTarefas.Repository.Interfaces;

namespace ListaTarefas.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Tarefa> ConsultarTodasTarefas()
        {
            return _context.Tarefas.ToList();
        }

        public List<Tarefa> ConsultarTarefasConcluidas()
        {
            return _context.Tarefas.Where(tarefa => tarefa.IsConcluido == true).ToList();
        }

        public List<Tarefa> ConsultarTarefasEmAberto()
        {
            return _context.Tarefas.Where(tarefa => tarefa.IsConcluido == false).ToList();
        }

        public Tarefa IncluirTarefa(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return tarefa;
        }

        public Tarefa AtualizarDescricao(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            _context.SaveChanges();
            return tarefa;
        }

        public void ExcluirTarefa(int id)
        {
            var tarefa = ObterTarefaById(id);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                _context.SaveChanges();
            }
        }

        public Tarefa ObterTarefaById(int id)
        {
            return _context.Tarefas.Find(id);
        }
    }
}
