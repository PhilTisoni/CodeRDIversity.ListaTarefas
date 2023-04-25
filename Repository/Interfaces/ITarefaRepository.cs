using ListaTarefas.Models;

namespace ListaTarefas.Repository.Interfaces
{
    public interface ITarefaRepository
    {
        List<Tarefa> ConsultarTodasTarefas();
        List<Tarefa> ConsultarTarefasConcluidas();
        List<Tarefa> ConsultarTarefasEmAberto();
        Tarefa IncluirTarefa(Tarefa tarefa);
        Tarefa AtualizarDescricao(Tarefa tarefa);
        void ExcluirTarefa(int id);
    }
}
