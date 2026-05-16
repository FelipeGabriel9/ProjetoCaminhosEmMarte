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
        if (string.IsNullOrWhiteSpace(linha))
              return null;
        string[] dados = linha.Split(';');
        if (dados.Length < 3)
                    return null;
                nome = dados[0];  // (inicio, quantos)
        x = double.Parse(dados[1]);
        y = double.Parse(dados[2]);
<<<<<<< HEAD

        return this;
      }
            return null;
=======
                return new Cidade(nome, x, y);
            }
      return default(Cidade);  // para arquivo não aberto
>>>>>>> 7f4fbe3a4feb140533a1c4d1b590c263c7ccca11
    }
    public void EscreverRegistro(StreamWriter arquivo)
    {
      if (arquivo != null)
      {
<<<<<<< HEAD
                arquivo.WriteLine($"{nome};{x:0.00000};{y:0.00000}");
            }
=======
        arquivo.WriteLine($"{nome};{x:0.00000};{y:0.00000}");
      }
>>>>>>> 7f4fbe3a4feb140533a1c4d1b590c263c7ccca11
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
