using CursoEFCore.Data;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

//InserirDados();
//InserirDadosEmMassa();
//InserirListaDeClientes();
//InserirPedido();
//ConsultarDados();
//ConsultarParaExplicarAsNoTracking();
//ConsultarPedidoCarregamentoAdiantado();
//ConsultarPedidoCarregamentoTardio();
//ConsultarPedidoCarregamentoExplicito();
//AtualizarDados();
//RemoverRegistro();

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

static void ConsultarDados()
{
    using var db = new ApplicationContext();
    var consultaPorSintaxe = (from x in db.Clientes where x.Id > 0 select x).ToList();
    var consultaPorMetodo = db.Clientes.Where(x => x.Id > 0).ToList();
}

static void ConsultarParaExplicarAsNoTracking() // AsNoTracking
{
    using var db = new ApplicationContext();
    var consultaPorMetodo = db.Clientes.AsNoTracking().Where(x => x.Id > 0).ToList();
    
    foreach (var cliente in consultaPorMetodo)
    {
        Console.WriteLine($"Consultando Cliente: {cliente.Id}");
        var consulta = db.Clientes.FirstOrDefault(x => x.Id == cliente.Id);
    }

    /*
       O EntityFrameworkCore busca primeiro na memória e depois no banco de dados.
       Quando usamos o método AsNoTracking(), estamos informando ao EntityFrameworkCore para não criar
       cópias dos objetos na memória e também para não rastrear as alterações desses objetos.
       Isso significa que a consulta será feita diretamente no banco de dados, sem envolver a memória
       do contexto, resultando em uma operação mais rápida e leve, especialmente útil para consultas
       de leitura onde não precisamos modificar os dados.
    */
}

static void InserirPedido()
{
    using var db = new ApplicationContext();

    var cliente = db.Clientes.FirstOrDefault();
    var produto = db.Produtos.FirstOrDefault();

    var pedido = new Pedido
    {
        ClienteId = cliente.Id,
        IniciadoEm = DateTime.Now,
        FinalizadoEm = DateTime.Now,
        Observacao = "Pedido Teste",
        StatusPedido = StatusPedido.Analise,
        TipoFrete = TipoFrete.SemFrete,
        Itens = new List<PedidoItem>
        {
            new PedidoItem()
            {
                ProdutoId = produto.Id,
                Desconto = 0,
                Quantidade = 1,
                Valor = 10
            }
        }
    };

    db.Pedidos.Add(pedido);
    db.SaveChanges();
}

static void ConsultarPedidoCarregamentoAdiantado() // Eager Loading
{
    using var db = new ApplicationContext();
    var pedidos = db.Pedidos // carrega a entidade e entidade relacionadas, como por exemplo itens do pedido
        .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
        .ToList();

    Console.WriteLine($"numero de pedidos {pedidos.Count}");
}

static void ConsultarPedidoCarregamentoTardio() // Lazy Loading
{
    using var db = new ApplicationContext();
    var pedidos = db.Pedidos.ToList();

    foreach (var pedido in pedidos)
    {
        // Itens e Produtos serão carregados automaticamente quando acessados pela primeira vez
        foreach (var item in pedido.Itens)
        {
            Console.WriteLine($"Produto: {item.Produto.Descricao}");
        }
    }

    Console.WriteLine($"numero de pedidos {pedidos.Count}");
}

static void ConsultarPedidoCarregamentoExplicito() // Explicit Loading
{
    using var db = new ApplicationContext();
    var pedidos = db.Pedidos.ToList();
    
    // No carregamento explícito, você carrega os dados relacionados sob demanda,
    // depois de carregar a entidade principal.
    foreach (var pedido in pedidos)
    {
        db.Entry(pedido).Collection(p => p.Itens).Load();
        foreach (var item in pedido.Itens)
        {
            db.Entry(item).Reference(i => i.Produto).Load();
            Console.WriteLine(item.Produto.Descricao);
        }
    }

    Console.WriteLine($"numero de pedidos {pedidos.Count}");
} 

static void AtualizarDados()
{
    using var db = new ApplicationContext();
    
    var cliente = db.Clientes.FirstOrDefault(x => x.Id == 1);
    cliente.Nome = "Cliente alterado Passo 1";
    cliente.Estado = "SP";
    
    //db.Clientes.Update(cliente); // atualiza todos os campos 
    db.SaveChanges(); // atualizar apenas os campos necessarios e salvar as entidade rastreadas
}

static void RemoverRegistro()
{
    using var db = new ApplicationContext();
    var cliente = db.Clientes.FirstOrDefault(x => x.Id == 2);

    db.Clientes.Remove(cliente);
    //db.Remove(cliente);
    //db.Entry(cliente).State = EntityState.Deleted;

    db.SaveChanges();
}