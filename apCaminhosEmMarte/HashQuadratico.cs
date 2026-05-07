using System;
using System.Collections.Generic;

public class HashQuadratico<T> : IHashing<T>
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

