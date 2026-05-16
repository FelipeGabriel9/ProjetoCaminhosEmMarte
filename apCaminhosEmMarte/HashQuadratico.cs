using System;
using System.Collections.Generic;

public class HashQuadratico<T> : IHashing<T> where T : IComparable<T>, IRegistro<T>, new()
{

    private T[] tabelaDeHash;
    private int tamanhoPadrao = 21;
    private int quantidade;
    private bool[] FoiExcluido;

    public HashQuadratico()
    {
        tabelaDeHash = new T[tamanhoPadrao];
        FoiExcluido = new bool[tamanhoPadrao];
        quantidade = 0;
    }

    private int Hash(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];

        tot = tot % tamanhoPadrao;
        if (tot < 0) tot += tamanhoPadrao;
        return (int)tot;
    }

    public bool Incluiu(T novoDado)
    {
        if (quantidade >= tamanhoPadrao / 2)
            return false;

        int indice = Hash(novoDado.Chave);
        int tentativa = 0;
        int posicaoAtual = indice;

        while (tabelaDeHash[posicaoAtual] != null && !FoiExcluido[posicaoAtual])
        {
            if (tabelaDeHash[posicaoAtual].Chave == novoDado.Chave)
                return false;

            tentativa++;
            if (tentativa >= tamanhoPadrao)
                return false;
            posicaoAtual = (indice + (tentativa * tentativa)) % tamanhoPadrao;
        }

        tabelaDeHash[posicaoAtual] = novoDado;
        quantidade++;
        return true;
    }

    public bool Existe(T item, out int onde)
    {
        int indice = Hash(item.Chave);
        onde = -1;


        for (int i = 0; i < tabelaDeHash.Length; i++)
        {
            int pos = (indice + (i * i)) % tamanhoPadrao;

            if (tabelaDeHash[pos] == null && !FoiExcluido[pos])
                 return false;
            

            if (tabelaDeHash[pos] != null && tabelaDeHash[pos].Chave.Equals(item.Chave))
            {
                onde = pos;
                return true;
            }
  
            
        }
        onde = -1;
        return false;
    }

    public bool Excluiu(T dadoParaExcluir)
    {
        if (Existe(dadoParaExcluir, out int onde))
        {
            tabelaDeHash[onde] = default;
            FoiExcluido[onde] = true;
            quantidade--;
            return true;
        }
        return false;
    }

    public List<string> LocaisDosDados()
    {
        var dados = new List<string>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null && !FoiExcluido[i])
                dados.Add($"{i,5} : {tabelaDeHash[i]}");
        return dados;
    }

    public List<T> Conteudo()
    {
        var dados = new List<T>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null && !FoiExcluido[i])
                dados.Add(tabelaDeHash[i]);
        return dados;
    }
}