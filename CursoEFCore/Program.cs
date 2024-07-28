using CursoEFCore.Data;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;

//InserirDados();
//InserirDadosEmMassa();
InserirListaDeClientes();

static void InserirDados()
{
    var produto = new Produto
    {
        Descricao = "Produto teste",
        CodigoBarras = "1234567891231",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
    };

    using var db = new ApplicationContext();

    db.Produtos.Add(produto);
    //db.Set<Produto>().Add(produto);               // Opções para adiconar no entity framework core
    //db.Entry(produto).State = EntityState.Added;
    //db.Add(produto);

    var registros = db.SaveChanges();
    Console.WriteLine($"Registros afetados: {registros}");
}

static void InserirDadosEmMassa() // Inserir registros de tabelas diferentes
{
    var produto = new Produto
    {
        Descricao = "Xbox Gamer",
        CodigoBarras = "2338597491233",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
    };

    var cliente = new Cliente
    {
        Nome = "Chico",
        CEP = "99999000",
        Cidade = "Itatipa",
        Telefone = "99000001111",
        Estado = "SP"
    };

    using var db = new ApplicationContext();
    db.AddRange(produto, cliente);

    var registros = db.SaveChanges();
    Console.WriteLine($"Registros afetados {registros}");
}

static void InserirListaDeClientes() // Inserir uma lista 
{
    var listaDeClitentes = new List<Cliente>
    {
        new Cliente
        {
            Nome = "Benedito",
            CEP = "98989000",
            Cidade = "Askinlandia",
            Telefone = "99000001111",
            Estado = "RJ"
        },
        new Cliente
        {
            Nome = "Chico",
            CEP = "74239000",
            Cidade = "Itabotipa",
            Telefone = "99000001111",
            Estado = "MT"
        }
    };

    using var db = new ApplicationContext();
    db.AddRange(listaDeClitentes);
    
    var registros = db.SaveChanges();
    Console.WriteLine($"Registros afetados {registros}");
}