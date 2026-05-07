using apHashing1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class HashSimples<T> : IHashing<T>
                              where T : IRegistro<T>, new()
{
    const int tamanhoPadrao = 10007;    // número primo
    T[]? tabelaDeHash;

    public HashSimples(int tamanhoDesejado)
    {
        if (tamanhoDesejado <= 0)
            throw new Exception("Tamanho da tabela de dados deve maior que zero!");

        tabelaDeHash = new T[tamanhoDesejado];
    }
    public HashSimples() : this(tamanhoPadrao) { }

    private int HashAntigo(string chave)
    {
        int tot = 0;
        for (int i = 1; i < chave.Length; i++)
            tot += (int)chave[i];
        return tot % tabelaDeHash.Length;
    }

    private int Hash(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot += 37 * tot + (int)chave[i];
        tot = tot % tabelaDeHash.Length;
        if (tot < 0)
            tot += tabelaDeHash.Length;
        return (int)tot;
    }

    public bool Incluiu(T novoDado)
    {
        int indiceDeHash = Hash(novoDado.Chave);
        if (tabelaDeHash[indiceDeHash] != null)
            return false;

        tabelaDeHash[indiceDeHash] = novoDado;
        return true;
    }

    public bool Existe(T dado, out int posicao)
    {
        posicao = Hash(dado.Chave);
        var umDado = tabelaDeHash[posicao];
        if (umDado != null)
            return umDado.Equals(dado);
        return false;
    }

    public bool Excluiu(T dado)
    {
        int ondeAchou;
        if (Existe(dado, out ondeAchou))
        {
            tabelaDeHash[ondeAchou] = default(T);
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
    public void Limpar()
    {
        for (int i = 0; i < tabelaDeHash.Length; i++)
            tabelaDeHash[i] = default(T);
    }
}