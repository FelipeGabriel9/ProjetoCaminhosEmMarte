using System;
using System.IO;

public interface IRegistro<T> : IEquatable<T>, 
                                IComparable<T>
                                where T : IRegistro<T>, 
                                          new()
{
    string Chave { get; }
    T LerRegistro(StreamReader arquivo);
    void EscreverRegistro(StreamWriter arquivo);
    new bool Equals(T outroRegistro);
    new int CompareTo(T outroRegistro);
}
