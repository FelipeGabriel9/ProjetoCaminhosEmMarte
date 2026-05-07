using System;
using System.Collections.Generic;
using System.IO;

namespace apCaminhosEmMarte
{
  public class Cidade : IRegistro<Cidade>, 
                        IComparable<Cidade>
  {
    // atributos que formam uma linha do arquivo de cidades
    string nome;
    double x, y;

    public Cidade() { }  // construtor default
    public Cidade LerRegistro(StreamReader arquivo)
    {
      if (arquivo != null)  // está aberto
      {
        string linha = arquivo.ReadLine(); // lê uma linha
        nome = linha.Substring(0, 15);  // (inicio, quantos)
        x = double.Parse(linha.Substring(15, 7));
        y = double.Parse(linha.Substring(22, 7));
        return this;
      }
      return default(Cidade);  // para arquivo não aberto
    }
    public void EscreverRegistro(StreamWriter arquivo)
    {
      if (arquivo != null)
      {
        arquivo.WriteLine($"{nome}{x:0.00000}{y:0.00000}");
      }
    }
    public int CompareTo(Cidade outra)  // <0, ==0, >0
    {
      return this.nome.CompareTo(outra.nome);
    }
    public string Chave => this.nome;

    public bool Equals(Cidade outra)
    {
      return this.nome.Equals(outra.nome);
    }
  }
}
