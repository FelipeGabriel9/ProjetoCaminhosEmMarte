using System;
using System.Collections.Generic;

public class HashLinear<T> : IHashing<T>
  where T : IComparable<T>, IRegistro<T>, new()
{
    private const int tamanhoPadrao = 10007; 
    private T[] tabelaDeHash;
    private bool[] foiExcluido; 


    public HashLinear(int tamanhoDesejado)
    {
        tabelaDeHash = new T[tamanhoDesejado];
        foiExcluido = new bool[tamanhoDesejado];
    }

    public HashLinear() : this(tamanhoPadrao) { }

    private int Hash(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];
        tot = tot % tabelaDeHash.Length;
        if (tot < 0)
            tot += tabelaDeHash.Length;
        return (int)tot;
    }

    public bool Incluiu(T item)
    {
        int indiceAtual = Hash(item.Chave);
        int posicao = indiceAtual;

        for (int salto = 0; salto < tabelaDeHash.Length; salto++)
        {
            posicao = (indiceAtual + salto) % tabelaDeHash.Length;

            if (tabelaDeHash[posicao] == null || foiExcluido[posicao])
            {
                tabelaDeHash[posicao] = item;
                foiExcluido[posicao] = false;
                return true;
            }

            if (tabelaDeHash[posicao].Equals(item))
                return false;
        }

        return false; 
    }

    public bool Existe(T item, out int onde)
    {
        int indice = Hash(item.Chave);
        onde = -1;

        for (int i = 0; i < tabelaDeHash.Length; i++)
        {
            int pos = (indice + i) % tabelaDeHash.Length;

            if (tabelaDeHash[pos] == null && !foiExcluido[pos])
                return false;

            if (tabelaDeHash[pos] != null && tabelaDeHash[pos].Equals(item))
            {
                onde = pos;
                return true;
            }
        }

       
        return false;
    }

    public bool Excluiu(T item)
    {
        int onde;
        if (!Existe(item, out onde))
            return false;

        tabelaDeHash[onde] = default;
        foiExcluido[onde] = true;
        return true;
    }

    public List<string> LocaisDosDados()
    {
        var dados = new List<string>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null && !foiExcluido[i])
                dados.Add($"{i,5} : {tabelaDeHash[i]}");
        return dados;
    }

    public List<T> Conteudo()
    {
        var dados = new List<T>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null && !foiExcluido[i])
                dados.Add(tabelaDeHash[i]);
        return dados;
    }
}
