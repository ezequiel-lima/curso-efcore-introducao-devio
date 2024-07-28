using CursoEFCore.Data;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;

InserirDados();
Console.WriteLine("Hello, World!");

void InserirDados()
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