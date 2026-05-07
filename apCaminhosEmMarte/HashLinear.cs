using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HashLinear<T> : IHashing<T>
  where T : IComparable<T>, IRegistro<T>, new()
{
  public List<T> Conteudo()
  {
    throw new NotImplementedException();
  }

  public List<string> LocaisDosDados()
  {
    throw new NotImplementedException();
  }

  public bool Existe(T item, out int onde)
  {
    throw new NotImplementedException();
  }

  private int Hash(string chave)
  {
    throw new NotImplementedException();
  }

  public bool Incluiu(T item)
  {
    throw new NotImplementedException();
  }

  public bool Excluiu(T item)
  {
    throw new NotImplementedException();
  }
}

