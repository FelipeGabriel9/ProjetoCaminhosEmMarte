using System;
using System.Collections.Generic;

public class HashQuadratico<T> : IHashing<T> where T : IComparable<T>, IRegistro<T>, new()
{

    private T[] tabelaDeHash;
    private int tamanhoPadrao = 10007;
    private int quantidade;
<<<<<<< HEAD
=======
    private bool[] FoiExcluido;
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794

    public HashQuadratico()
    {
        tabelaDeHash = new T[tamanhoPadrao];
        quantidade = 0;
<<<<<<< HEAD
=======
        FoiExcluido = new bool[tamanhoPadrao];
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
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

<<<<<<< HEAD
        while (tabelaDeHash[posicaoAtual] != null)
=======
        while (tabelaDeHash[posicaoAtual] != null && !FoiExcluido[posicaoAtual])
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
        {
            if (tabelaDeHash[posicaoAtual].Chave == novoDado.Chave)
                return false;

            tentativa++;
<<<<<<< HEAD
            posicaoAtual = (indice + (tentativa * tentativa)) % tamanhoPadrao;

            if (tentativa >= tamanhoPadrao)
                return false;
=======
            if (tentativa >= tamanhoPadrao)
                return false;
            posicaoAtual = (indice + (tentativa * tentativa)) % tamanhoPadrao;
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
        }

        tabelaDeHash[posicaoAtual] = novoDado;
        quantidade++;
        return true;
    }

    public bool Existe(T item, out int onde)
    {
        int indice = Hash(item.Chave);
        int tentativa = 0;
        int posicaoAtual = indice;

        while (tabelaDeHash[posicaoAtual] != null && tentativa < tamanhoPadrao)
        {
<<<<<<< HEAD
            if (tabelaDeHash[posicaoAtual].Chave == item.Chave)
=======
            if (!FoiExcluido[posicaoAtual] && tabelaDeHash[posicaoAtual].Chave == item.Chave)
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
            {
                onde = posicaoAtual;
                return true;
            }
            tentativa++;
<<<<<<< HEAD
            posicaoAtual = (indice + (tentativa * tentativa)) % tamanhoPadrao;
        }

=======
            posicaoAtual = (indice + tentativa * tentativa) % tamanhoPadrao;
        }
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
        onde = -1;
        return false;
    }

    public bool Excluiu(T dado)
    {
        if (Existe(dado, out int onde))
        {
<<<<<<< HEAD
            tabelaDeHash[onde] = default(T);
=======
            FoiExcluido[onde] = true;
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
            quantidade--;
            return true;
        }
        return false;
    }

    public List<string> LocaisDosDados()
    {
        var dados = new List<string>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
<<<<<<< HEAD
            if (tabelaDeHash[i] != null)
=======
            if (tabelaDeHash[i] != null && !FoiExcluido[i])
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
                dados.Add($"{i,5} : {tabelaDeHash[i]}");
        return dados;
    }

    public List<T> Conteudo()
    {
        var dados = new List<T>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
<<<<<<< HEAD
            if (tabelaDeHash[i] != null)
=======
            if (tabelaDeHash[i] != null && !FoiExcluido[i])
>>>>>>> d9cfae4ff53903d1139d809777805231d8ac0794
                dados.Add(tabelaDeHash[i]);
        return dados;
    }
}