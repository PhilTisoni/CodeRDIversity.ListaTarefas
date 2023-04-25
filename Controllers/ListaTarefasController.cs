using ListaTarefas.Models;
using ListaTarefas.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ListaTarefas.Controllers
{
    [Route("[controller]")]
    public class ListaTarefasController : Controller
    {
        private readonly ITarefaRepository _tarefa;

        public ListaTarefasController(ITarefaRepository tarefa)
        {
            _tarefa = tarefa;
        }

        [HttpGet("[action]")]
        public List<Tarefa> ConsultarTodasTarefas()
        {
            return _tarefa.ConsultarTodasTarefas();
        }

        [HttpGet("[action]")]
        public List<Tarefa> ConsultarTarefasConcluidas()
        {
            return _tarefa.ConsultarTarefasConcluidas();
        }

        [HttpGet("[action]")]
        public List<Tarefa> ConsultarTarefasEmAberto()
        {
            return _tarefa.ConsultarTarefasEmAberto();
        }

        [HttpPost("[action]/{tarefa}")]
        public Tarefa IncluirTarefa([FromQuery] Tarefa tarefa)
        {
            return _tarefa.IncluirTarefa(tarefa);
        }

        [HttpPut("[action]")]
        public Tarefa AtualizarDescricao([FromQuery] Tarefa tarefa)
        {
            return _tarefa.AtualizarDescricao(tarefa);
        }

        [HttpDelete("[action]/{id}")]
        public void ExcluirTarefa(int id)
        {
            _tarefa.ExcluirTarefa(id);
        }
    }
}
