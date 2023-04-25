# CodeRDIversity - API Lista de Tarefas

<img style = "width: 200px" src = "https://prosper.tech/wp-content/uploads/2023/02/stories_06-3-2-783x1024.png" alt = "CodeRDIversity"> <img style = "width: 150px" src = "https://media.licdn.com/dms/image/C4D0BAQESTAYGKhOdSQ/company-logo_200_200/0/1659620356007?e=2147483647&v=beta&t=9kHLR--f0dlKI6o6clQuGNllshlTOb96Mi51iU5idlg" alt = "Prosper Tech Talents"> <img style = "width: 200px" src = "https://www.rdisoftware.com/img/logo.png" alt = "RDI Softwares"> 

Exercício realizado durante a **CodeRDIversity** para o desenvolvimento de uma lista de tarefas.

## Sobre o Projeto
Realizado em Abril de 2023 durante o **CodeRDIversity** organizado pela **Prosper Tech Talents** em parceria com a **RDI Software** para o **treinamento** de **PCD's** na **linguagem C#**, o projeto foi proposto como exercício prático do curso, sendo orientado por **Rodrigo Grigoleto**, tendo como objetivo o desenvolvimento de uma API CRUD (Creat, Read, Update e Delete) de uma lista de tarefas.

## Tecnologias Utilizadas
- Linguagem C# / ASP Net
- .Net 6.0
- Sqlite
- Visual Stuio 2022 Community
- DBeaver
- Swagger / Postman

## Como executar o projeto
```bash
# clonar repositório
git clone https://github.com/PhilTisoni/CodeRDIversity-API_Lista_Tarefas.git
```
Após clonar o projeto, abra o executável na pasta bin.

# Índice

- <a href = "#Regra-de-Negócio">Regra de Negócio</a>
- <a href = "#Criação-do-Banco-de-Dados">Criação do Banco de Dados</a>
- <a href = "#Repositório">Repositório</a>
- <a href = "#Controller">Controller</a>
- <a href = "#Resultados">Resultados</a> 
- <a href = "#Autores">Autores</a>
- <a href = "#Agradecimentos">Agradecimentos</a>

# Regra de Negócio

Crie uma API para o gerenciamento de uma lista de tarefas pessoal, como por exemplo:

- Estudar para prova de programação
- Estudar para a prova de matemática
- Verificar quando devo entregar o trabalho

As tarefas serão representadas por uma classe chamada **Tarefa** e deve possuir os seguintes atributos:

- Id (int)
- Descricao (string)
- DataCriacao (DateTime)
- Responsavel (string)
- Concluida (bool)

Deverá ser criada uma classe **Controller**, para que a API receba as requisições de manipulação das tarefas. A controller deverá possuir métodos para:

- Consultar todas as tarefas
- Consultar todas as tarefas concluídas
- Consultar todas as tarefas em aberto
- Incluir uma nova tarefa
- Atualizar a descrição de uma tarefa
- Excluir uma tarefa

Requisitos extras:

- Todas as tarefas deverão ser armazenadas em um **banco de dados SQlite**. Consequentemente, todas as manipulações das tarefas serão feitas com o banco;
- Coloque **DataAnnotation** na classe **Model**, para representar de maneira correta o nome da tabela e suas colunas no banco de dados;
- Separe os métodos de manipulação do banco de dados em classes do sufixo **Repository**;
- Teste todos os métodos no **Postman** - A utilização do Swagger é opcional.

# Criação do Banco de Dados

Para a criação do banco de dados, foram instalados os pacotes:

- **Microsoft.EntityFrameworkCore:** Instalação do Entity Framework
- **Microsoft.EntityFrameworkCore.Design:** Utilização de Migrations
- **Microsoft.EntityFrameworkCore.Sqlite:** Utilização do Sqlite

Posteriormente, foi realizada a conexão com o banco através da classe **AppDbContext**:

```c#
  public class AppDbContext : DbContext
    {       
            public DbSet<Tarefa> Tarefas { get; set; }
        
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(connectionString: "DataSource=app.db;Cache=Shared");
    }
```

Como requisitado na regra de negócio, foi criada uma classa **Tarefa** contendo os atributos com a utilização de **DataAnnotations**:

```c#
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
```
A tabela e suas respectivas colunas, podem ser visualizadas no **DBeaver**, como a imagem abaixo:

<img style = "width: 300px" src="./assets/DBeaver - Tabela e Colunas.PNG" alt = "DBeaver - Tabela e Colunas">

# Repositório

Como forma de organização, foi criada uma pasta chamada **Repository** contendo uma classe com os métodos utilizados no CRUD, ela utiliza injeção
de dependências para evitar instâncias desnecessárias no código. Abaixo, está exemplificado o método **ConsultarTodasTarefas()** que retorna uma lista com todas as tarefas cadastradas no banco:

```c#
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Tarefa> ConsultarTodasTarefas()
        {
            return _context.Tarefas.ToList();
        }
```

Para inserir o método de **ExcluirTarefa()**, foi adicionada uma validação através do método **ObterTarefaById()**:
```c#
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
```

# Controller

Para os métodos da Controller, foi criada uma **interface** na pasta **Repository** para utilização de injeção de dependências:
```c#
   public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddTransient<ITarefaRepository, TarefaRepository>(); // Adicionando comando para injeção de dependências
```

Essa técnica facilitou a escrita e entendimento do código, simplificando os comandos na **Controller**:

```c#
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
```

Para os métodos **Incluir** e **Atualizar** tarefa, optou-se por utilizar a rota **FromQuery** como forma de praticar o conteúdo estudado, porém, também seria possível a utilização de outras rotas, como por exemplo a **FromRoute** ou **FromBody**, sendo necessários alguns ajustes na inserção desses comandos:
```c#  
        [HttpPost("[action]/{tarefa}")]
        public Tarefa IncluirTarefa([FromQuery] Tarefa tarefa)
        {
            return _tarefa.IncluirTarefa(tarefa);
        }
```

# Resultados

Utilizando o Swagger, executamos o método InserirTarefa():

<img style = "width: 400px" src="./assets/Swagger - Inserir Tarefa 1.PNG" alt = "Swagger - Inserir Tarefa">

Nota-se o **Status Code** igual a 200, demonstrando que a requisição foi executada com **sucesso**. Pode-se verificar através do DBeaver:

<img style = "width: 500px" src="./assets/DBeaver - Inserir Tarefa 1.PNG" alt = "DBeaver - Inserir Tarefa">

Também obtivemos um resultado positivo ao inserir dados através do **Postman**:

<img style = "width: 600px" src="./assets/Postman - FromQuery.png" alt = "Postman - FromQuery">
<img style = "width: 500px" src="./assets/Dbeaver - Inserir Tarefa com Postaman.PNG" alt = "Dbeaver - Inserir Tarefa com Postaman">

Os métodos de consulta exibem a **lista de todas as tarefas** (como observado na primeira figura), apenas as **tarefas concluídas**, ou seja, as que possuem **IsConcluida = true** (segunda figura), e apenas as **tarefas em aberto**, nesse caso, com **IsConcluida = false** (terceira figura):

<img style = "width: 400px" src="./assets/Swagger - Consultar Todas as Tarefas.PNG" alt = "Swagger - Consultar Todas as Tarefas">
<img style = "width: 400px" src="./assets/Swagger - Consultar Tarefas Concluidas.PNG" alt = "Swagger - Consultar Tarefas Concluidas"> 
<img style = "width: 400px" src="./assets/Swagger - Consultar Tarefas em Aberto.PNG" alt = "Swagger - Consultar Tarefas em Aberto">

Ao **atualizar os dados**, pode-se alterar qualquer atributo de uma tarefa já cadastrada. No exemplo abaixo, modificamos a **terceira tarefa** (Verificar quando devo entregar o trabalho) para **concluída** (IsConcluida = true):

<img style = "width: 400px" src="./assets/Swagger - Atualizar Tarefa.PNG" alt = "Swagger - Atualizar Tarefa">

Para **exlcuir uma tarefa** é necessário inserir o ID no **Swagger**, deste modo, deletamos a primeira e a terceira tarefa. Na imagem abaixo, nota-se que apenas a tarefa com **Id = 2** está presente no banco de dados:

<img style = "width: 500px" src="./assets/DBeaver - Excluir Tarefa.PNG" alt = "DBeaver - Excluir Tarefa">

Assim, as funções de criação, leitura, atualização e remoção de dados foram implementadas na API.

# Autores
- [Andreia Ribas](https://www.linkedin.com/in/andreiaribas/ "Andreia Linkedin")
- [Carolina Aizawa Moreira](https://www.linkedin.com/in/carolina-aizawa-moreira-pcd-9b0624179/ "Carolina Linkedin")
- Gabriel Vicente
- [Neto Trindade](https://www.linkedin.com/in/neto-trindade-09410287/ "Neto Linkedin")
- [Phelipe Augusto Tisoni](https://www.linkedin.com/in/phelipetisoni "Phelipe Linkedin")

# Agradecimentos
- [Rodrigo de Nadai Grigoleto](https://www.linkedin.com/in/rodrigo-de-nadai-grigoleto-58558133/ "Rodrigo Linkedin")
- [Prosper Tech Talents](https://www.linkedin.com/company/prosper-tech-talents/ "Prosper Linkedin")
- [RDI Software](https://www.linkedin.com/company/rdisoftware/ "RDI Linkedin")
