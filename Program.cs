using CnpjCpfForNet8;
using System.Diagnostics;

//Gero uma massa de dados
var arrayChars = GenerateDocuments.FillArray();

var validosTeste01 = 0;
var validosTeste02 = 0;
var paradas = 0;

var sw = new Stopwatch();
var before2 = GC.CollectionCount(2);
var before1 = GC.CollectionCount(1);
var before0 = GC.CollectionCount(0);

//Teste 01
sw.Start();
//O resultado depende de vários fatores, por exemplo, este é um caso rápido.
//É esperado que o primeiro If da validação já resolva
for (var k = 0; k < 1_000_000; k++)
    if (CpfCnpjDotNET8.IsCpf("694.505.215-87".AsSpan())) validosTeste01++;              
sw.Stop();
var time01 = sw.ElapsedMilliseconds;

//Teste 02
sw.Start();
//O resultado depende de vários fatores, por exemplo, aqui estou percorrendo um array para tentar encontrar CPFs válidos
//É esperado que este método seja um pouco mais lento, pois existe maior processamento.
for (var k = 0; k < 1_000_000; k++)
    if (CpfCnpjDotNET8.IsCpf(GenerateDocuments.GenerateCPFs(arrayChars, k))) validosTeste02++;
sw.Stop();
var time02 = sw.ElapsedMilliseconds;

//Teste Exception
sw.Start();
//O resultado depende de vários fatores, por exemplo:
//Vamos supor que existam apenas 100 lançamentos de exceções, aqui nem tento fazer a validação
//Vejam o impacto de lançar exceção. Procurem utilizar um ValidationResult!
for (var k = 0; k < 100; k++)
{
    try { throw new Exception("Ops vai interromper o fluxo do programa!"); }
    catch (Exception) { paradas++; }
}
sw.Stop();
var time03 = sw.ElapsedMilliseconds;

Console.WriteLine($"Tempo total - Teste 01: {time01}ms. Validos {validosTeste01}");
Console.WriteLine($"Tempo total - Teste 02: {time02}ms. Validos {validosTeste02}");
Console.WriteLine($"Tempo total - Exceções: {time03}ms. Exceções: {paradas}");
Console.WriteLine($"GC Gen #2  : {GC.CollectionCount(2) - before2}");
Console.WriteLine($"GC Gen #1  : {GC.CollectionCount(1) - before1}");
Console.WriteLine($"GC Gen #0  : {GC.CollectionCount(0) - before0}");
Console.WriteLine("Done!");