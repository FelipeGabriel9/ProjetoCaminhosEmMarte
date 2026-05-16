using apCaminhosEmMarte;
using System;
using System.Collections.Generic;

public class HashLinear<T> : IHashing<T>
  where T : IComparable<T>, IRegistro<T>, new()
{
    
    private const int tamanhoPadrao = 97; // Tamanho inicial padrão da tabela (um número primo ideal
    private T[] tabelaDeHash;             // Vetor que armazena os dados na memória
    private bool[] foiExcluido;           // Vetor booleano para marcar posições onde dados foram deletados, usado para evitar a quebra na busca
    private int quantidade;               //tamanho lógico

    // Construtor que define um tamanho personalizado para a tabela
    public HashLinear(int tamanhoDesejado)
    {
        tabelaDeHash = new T[tamanhoDesejado];
        foiExcluido = new bool[tamanhoDesejado];
        quantidade = 0; 
    }

    // Construtor padrão que usa o tamanho predefinido, reserva
    public HashLinear() : this(tamanhoPadrao) { }

    // Função Hash, usada para transforma a string da chave em um índice válido para o array
    private int Hash(string chave)
    {
        long tot = 0;

        // Multiplica por 37 e soma o valor ASCII, um número que representa um caractere no computador
        // de cada letra para determinar a ordem dos índices no vetor tabelaDeHash e evitar muitas colisões, pois cada caractere possui um código diferente

        for (int i = 0; i < chave.Length; i++)
            tot = 37 * tot + (int)chave[i];

        // Limita que o número total(tot) seja gerado até o valor máximo estabelicido
        tot = tot % tabelaDeHash.Length;

        //Caso o número vire negativo durante o processo de hashing, retomamos ele para o intervalo positivo
        if (tot < 0)
            tot += tabelaDeHash.Length;

        return (int)tot; // Retorna o índice final
    }

    // Insere um dado na tabela
    public bool Incluiu(T item)
    {
        // Se atingir 80% de ocupação, dobra o tamanho
        if ((double)quantidade / tabelaDeHash.Length >= 0.8)
        {
            AmpliarTabela();
        }

        int indiceAtual = Hash(item.Chave); // Descobre o endereço ideal do item
        int posicao = indiceAtual;

        // Percorre a tabela indice por indice em caso de colisão
        for (int salto = 0; salto < tabelaDeHash.Length; salto++)
        {
            // Anda para a frente e volta para o início caso chegue ao fim da tabela (%)
            posicao = (indiceAtual + salto) % tabelaDeHash.Length;

            // Se achar uma indice livre, inclui
            if (tabelaDeHash[posicao] == null || foiExcluido[posicao])
            {
                tabelaDeHash[posicao] = item;  // Salva o item na vaga
                foiExcluido[posicao] = false; // Limpa o marcador de exclusão antigo da vaga
                quantidade++;                  // Registra que mais um elemento entrou
                return true;
            }

       }

        return false;
    }

    // Procura por um item e verifica se ele existe
    public bool Existe(T item, out int onde)
    {
        int indice = Hash(item.Chave); // Descobre onde a busca deve começar
        onde = -1;                     // Valor padrão caso o item não exista

        for (int i = 0; i < tabelaDeHash.Length; i++)
        {
            int pos = (indice + i) % tabelaDeHash.Length; 

            // Se achar um espaço vazio que nunca foi preenchido, o item não existe
            if (tabelaDeHash[pos] == null && !foiExcluido[pos])
                return false;

            // Se o espaço estiver ocupado e for o dado correto, encontrou
            if (tabelaDeHash[pos] != null && tabelaDeHash[pos].Equals(item))
            {
                onde = pos;   // Guarda a posição encontrada
                return true;
            }
        }

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