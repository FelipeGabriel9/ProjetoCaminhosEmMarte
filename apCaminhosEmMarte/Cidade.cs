using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace apCaminhosEmMarte
{
  public class Cidade : IRegistro<Cidade>, 
                        IComparable<Cidade>
  {
    // atributos que formam uma linha do arquivo de cidades
    private string nome;
    private double x;
    private double y;

    public Cidade() { }  // construtor default

    public Cidade(string nome, double x, double y)
        {
            this.nome = nome;
            this.x = x;
            this.y = y;
        }
    public Cidade LerRegistro(StreamReader arquivo)
    {
      if (arquivo != null)  // está aberto
      {
        string linha = arquivo.ReadLine(); // lê uma linha
        string[] dados = linha.Split(';');
        nome = dados[0];  // (inicio, quantos)
        x = double.Parse(dados[1]);
        y = double.Parse(dados[2]);
                return new Cidade(nome, x, y);
            }
      return default(Cidade);  // para arquivo não aberto
    }
    public void EscreverRegistro(StreamWriter arquivo)
    {
      if (arquivo != null)
      {
        arquivo.WriteLine($"{nome};{x:0.00000};{y:0.00000}");
      }
    }
    public int CompareTo(Cidade outra)  // <0, ==0, >0
    {
      return this.nome.CompareTo(outra.nome);
    }
    public string Chave => this.nome;

    public double X => this.x;
    public double Y => this.y;

    public bool Equals(Cidade outra)
    {
      return this.nome.Equals(outra.nome);
    }
  }
}
