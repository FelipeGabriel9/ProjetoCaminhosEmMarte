using System;
using System.Collections.Generic;
using Projeto2Ed;

public class HashDuplo<T> : IHashing<T> where T : IRegistro<T>, new()
{
    private const int tamanhoPadrao = 10007; // Número primo
    private T[] tabelaDeHash; // Vetor para armazenar os dados
    private int quantidade;

    public HashDuplo(int tamanhoDesejado) // Construtor que recebe o tamanho desejado para a tabela de hash
    {
        if (tamanhoDesejado <= 0)
            throw new Exception("Tamanho da tabela de dados deve ser maior que zero!");

        tabelaDeHash = new T[tamanhoDesejado];
        quantidade = 0;
    }

    public HashDuplo() : this(tamanhoPadrao) { }

    private int Hash1(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];

        tot = tot % tabelaDeHash.Length;
        if (tot < 0) tot += tabelaDeHash.Length;
        return (int)tot;
    }

    private int Hash2(string chave)
    {
        int R = 9973;
        if (tabelaDeHash.Length <= 9973)
            R = 7;

        int valorChave = Math.Abs(chave.GetHashCode());
        int resultado = R - (valorChave % R);
        return resultado;
    }

    public bool Incluiu(T novoDado)
    {
        int indice = Hash1(novoDado.Chave);
        int salto = Hash2(novoDado.Chave);
        int tentativa = 0;

        while (tabelaDeHash[indice] != null && tentativa < tabelaDeHash.Length)
        {

            if (tabelaDeHash[indice].Chave == novoDado.Chave)
                return false;

            indice = (indice + salto) % tabelaDeHash.Length;
            tentativa++;
        }

        if (tabelaDeHash[indice] == null)
        {
            tabelaDeHash[indice] = novoDado;
            quantidade++;
            return true;
        }

        return false;
    }

    public bool Existe(T dado, out int posicao)
    {
        int indiceOriginal = Hash1(dado.Chave);
        int salto = Hash2(dado.Chave);
        int indiceAtual = indiceOriginal;
        int tentativa = 0;

        while (tentativa < tabelaDeHash.Length)
        {
            if (tabelaDeHash[indiceAtual] != null)
            {
                if (tabelaDeHash[indiceAtual].Chave == dado.Chave)
                {
                    posicao = indiceAtual;
                    return true;
                }
            }

            indiceAtual = (indiceAtual + salto) % tabelaDeHash.Length;
            tentativa++;
        }

        posicao = -1;
        return false;
    }



    public bool Excluiu(T dado) // Mesma lógica do HashSimples
    {
        if (Existe(dado, out int ondeAchou))
        {
            tabelaDeHash[ondeAchou] = default(T);
            quantidade--;
            return true;
        }
        return false;
    }

    public List<string> LocaisDosDados()
    {
        var dados = new List<string>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null)
                dados.Add($"{i,5} : {tabelaDeHash[i]}");
        return dados;
    }

    public List<T> Conteudo()
    {
        var dados = new List<T>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null)
                dados.Add(tabelaDeHash[i]);
        return dados;
    }

}