using System;
using System.Collections.Generic;
public class HashDuplo<T> : IHashing<T> where T : IRegistro<T>, new()
{
    private const int tamanhoPadrao = 97; // Tamanho inicial padrão da tabela (um número primo ideal
    private T[] tabelaDeHash;             // Vetor que armazena os dados na memória
    private bool[] foiExcluido;           // Vetor booleano para marcar posições onde dados foram deletados, usado para evitar a quebra na busca
    private int quantidade;               //tamanho lógico

    // Construtor que define um tamanho personalizado para a tabela
    public HashDuplo(int tamanhoDesejado)
    {
        tabelaDeHash = new T[tamanhoDesejado];
        foiExcluido = new bool[tamanhoDesejado];
        quantidade = 0;
    }

    public HashDuplo() : this(tamanhoPadrao) { }

    // A primeira função hash define a posição inicial, que seria sua vaga ideal, do dado no vetor
    private int Hash1(string chave)
    {
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];

        tot = tot % tabelaDeHash.Length;
        if (tot < 0) tot += tabelaDeHash.Length;
        return (int)tot;
    }

    // Na segunda função hash, é calculado o quanto o dado vai andar caso ocorra uma colisão
    private int Hash2(string chave)
    {
        const int tot2 = 89; // Número primo menor que o tamanho padrão da tabela
        long tot = 0;
        for (int i = 0; i < chave.Length; i++)
            tot += (int)chave[i];

        int resultado = (int)(tot2 - (tot % tot2));
        return resultado == 0 ? 1 : resultado; // Garante que o salto seja de no mínimo 1 casa
    }

    // Insere um novo elemento na estrutura usando duas funções hash combinadas
    public bool Incluiu(T novoDado)
    {
        // Se atingir 80% da capacidade do vetor, aumenta o vetor
        if ((double)quantidade / tabelaDeHash.Length >= 0.80)
        {
            AmpliarTabela();
        }

        int indice = Hash1(novoDado.Chave);
        int salto = Hash2(novoDado.Chave);
        int tentativa = 0;

        // Um loop de busca por vaga livre. O incremento do índice é ditado pelo valor do quanto o dado vai andar
        while (tabelaDeHash[indice] != null && !foiExcluido[indice] && tentativa < tabelaDeHash.Length)
        {
            if (tabelaDeHash[indice].Chave == novoDado.Chave)
                return false;

            // Avança o índice somando o salto customizado do elemento
            indice = (indice + salto) % tabelaDeHash.Length;
            tentativa++;
        }

        // Se encontrou um espaço totalmente vazio ou um lugar de um índice excluído, armazena o dado
        if (tabelaDeHash[indice] == null || foiExcluido[indice])
        {
            tabelaDeHash[indice] = novoDado; // A posição encontrada recebe o dado
            foiExcluido[indice] = false;    // Marca de exclusão removida, pois agora temos um dado naquela posição
            quantidade++;                   // Incrementa a quantidade de dados no vetor 
            return true;
        }

        return false;
    }

    // Procura um elemento na tabela seguindo a mesma sequência de saltos do Incluiu
    public bool Existe(T dado, out int posicao)
    {
        int indiceOriginal = Hash1(dado.Chave);
        int salto = Hash2(dado.Chave);
        int indiceAtual = indiceOriginal;
        int tentativa = 0;

        while (tentativa < tabelaDeHash.Length)
        {
            // Se encontrar um espaço nulo que nunca sofreu exclusão, o dado não está na tabela.
            if (tabelaDeHash[indiceAtual] == null && !foiExcluido[indiceAtual])
            {
                posicao = -1;
                return false;
            }

            // Se o espaço estiver ocupado e for o dado correto, encontrou
            if (!foiExcluido[indiceAtual] && tabelaDeHash[indiceAtual] != null)
            {
                if (tabelaDeHash[indiceAtual].Chave == dado.Chave)
                {
                    posicao = indiceAtual; // Diz onde a posição onde que o dado está, o índice dele no vetor
                    return true;
                }
            }

            // Segue a trilha somando o mesmo salto calculado para o dado
            indiceAtual = (indiceAtual + salto) % tabelaDeHash.Length;
            tentativa++;
        }

        posicao = -1;
        return false;
    }

    // Remove um item do sistema
    public bool Excluiu(T item)
    {
        int onde;
        // Verifica se o item existe. Se existir, a variável 'onde' recebe o índice dele
        if (!Existe(item, out onde))
            return false;

        tabelaDeHash[onde] = default; // Apaga o objeto daquela posição
        foiExcluido[onde] = true;     // Adiciona no vetor de bools, marcando que aquel vaga foi excluída
        quantidade--;                 // Diminui quantos elementos temos no vetor tabelaDeHash
        return true;
    }

    // Retorna uma lista de strings mostrando os índices reais ocupados pelos dados
    public List<string> LocaisDosDados()
    {
        var dados = new List<string>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null && !foiExcluido[i])
                dados.Add($"{i,5} : {tabelaDeHash[i]}");
        return dados;
    }

    // Retorna uma lista limpa contendo apenas os objetos ativos da tabela
    public List<T> Conteudo()
    {
        var dados = new List<T>();
        for (int i = 0; i < tabelaDeHash.Length; i++)
            if (tabelaDeHash[i] != null && !foiExcluido[i])
                dados.Add(tabelaDeHash[i]);
        return dados;
    }

  // Move os dados da tabela antiga para a nova estrutura maior
    private void Rehash(T[] tabelaAntiga)
    {
        for (int i = 0; i < tabelaAntiga.Length; i++)
        {
            // Ignora os espaços nulos e insere apenas dados válidos na tabela nova
            if (tabelaAntiga[i] != null)
            {
                //Inclui utilizando o novo tamanho, com os dados da tabela antiga
                Incluiu(tabelaAntiga[i]);
            }
        }
    }

    // Dobra o tamanho do array, acha o próximo número primo e reorganiza os dados
    private void AmpliarTabela()
    {
        // Começa definindo o novo tamanho como o dobro do atual
        int novoTamanho = tabelaDeHash.Length * 2;
        // Se o dobro for par, adiciona 1 para virar ímpar, pois números primos, com execeção do 2, não são pares
        if (novoTamanho % 2 == 0)
            novoTamanho++;
        bool encontrouPrimo = false;
        //Verifica se o novo tamanho para o vetor é primo, como o anterior
        while (!encontrouPrimo)
        {
            encontrouPrimo = true; // Supõe que é primo
            // Testa divisores ímpares a partir de 3
            for (int i = 3; i * i <= novoTamanho && encontrouPrimo; i += 2)
            {
                if (novoTamanho % i == 0)
                {
                    encontrouPrimo = false; // Se for divisível por alguém, não é primo
                }
            }
            // Se não for primo, pula para o próximo número ímpar (+2) e tenta o loop de novo
            if (!encontrouPrimo)
                novoTamanho += 2;
        }
        //Agora, o novoTamanho é um número primo, igual ao maior ao dobro do tamanho antigo do vetor.

        // Guarda o vetor antigo antes de substituí-lo
        T[] antiga = tabelaDeHash;
        //Limpa os arrays
        tabelaDeHash = new T[novoTamanho];
        foiExcluido = new bool[novoTamanho];
        // Zera a quantidade
        quantidade = 0;
        // Transfere os novos dados para as suas novas posições
        Rehash(antiga);
    }
}