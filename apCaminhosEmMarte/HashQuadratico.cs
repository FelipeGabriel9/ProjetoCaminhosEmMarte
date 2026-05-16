using System;
using System.Collections.Generic;
public class HashQuadratico<T> : IHashing<T> where T : IComparable<T>, IRegistro<T>, new()
{

    private const int tamanhoPadrao = 97; // Tamanho inicial padrão da tabela (um número primo ideal
    private T[] tabelaDeHash;             // Vetor que armazena os dados na memória
    private bool[] foiExcluido;           // Vetor booleano para marcar posições onde dados foram deletados, usado para evitar a quebra na busca
    private int quantidade;               //tamanho lógico

    // Construtor padrão da classe
    public HashQuadratico()
    {
        // Instancia os vetores com o tamanho físico padrão
        tabelaDeHash = new T[tamanhoPadrao];
        foiExcluido = new bool[tamanhoPadrao];
        quantidade = 0;
    }
    // Construtor que define um tamanho personalizado para a tabela
    public HashQuadratico(int tamanhoDesejado)
    {
        tabelaDeHash = new T[tamanhoDesejado];
        foiExcluido = new bool[tamanhoDesejado];
        quantidade = 0;
    }
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
        if (tot < 0) tot += tabelaDeHash.Length;

        return (int)tot; // Retorna o índice final
    }

    // Insere um novo elemento na tabela usando o método de sondagem quadrática
    public bool Incluiu(T novoDado)
    {
        // Se atingir metade da capacidade, aumenta a capacidade do vetor. Garantindo assim de forma absoluta, que o dado irá encontrar uma vaga livre
        if (quantidade >= tabelaDeHash.Length / 2)
        {
            AmpliarTabela();
        }

        int indice = Hash(novoDado.Chave); // Descobre o endereço ideal do item
        int tentativa = 0;                 // Contador do número de colisões ocorridas
        int posicaoAtual = indice;         // Posição que está sendo testada no momento

        // Enquanto o espaço atual estiver ocupado por um dado válido, que não está vazio ou foi excluído, continua procurando vaga
        while (tabelaDeHash[posicaoAtual] != null && !foiExcluido[posicaoAtual])
        {
            // Se encontrar a mesma chave na tabela, rejeita a inserção, para evitar chave duplicadas
            if (tabelaDeHash[posicaoAtual].Chave == novoDado.Chave)
                return false;

            tentativa++; // Incrementa a tentativa de salto

            // Se o número de tentativas for igual ou maior que o tamanho do vetor, para para evitar  um loop infinito
            if (tentativa >= tabelaDeHash.Length)
                return false;

            //O deslocamento cresce de forma exponencial com em tentativas²
            posicaoAtual = (indice + (tentativa * tentativa)) % tabelaDeHash.Length;
        }

        // Armazena o dado na vaga livre encontrada 
        tabelaDeHash[posicaoAtual] = novoDado;
        quantidade++; // Incrementa quantos dados temos no vetor
        return true;
    }

    // Procura por um item e verifica se ele existe
    public bool Existe(T item, out int onde)
    {
        int indice = Hash(item.Chave); // Descobre onde o item deveria ter começado a procurar
        onde = -1;                     // Valor padrão de retorno caso não encontre o item

        // Percorre a tabela simulando os mesmos saltos quadráticos usados no método Incluiu
        for (int i = 0; i < tabelaDeHash.Length; i++)
        {
<<<<<<< HEAD
            // Aplica o salto quadrático (i * i) para verificar as posições que foram mapeadas no Incluiu
            int pos = (indice + (i * i)) % tabelaDeHash.Length;

            // Se achar um espaço nulo que nunca foi excluído, o item não existe
            if (tabelaDeHash[pos] == null && !foiExcluido[pos])
                return false;
=======
            int pos = (indice + (i * i)) % tabelaDeHash.Length;

            if (tabelaDeHash[pos] == null && !FoiExcluido[pos])
                 return false;

>>>>>>> 7f4fbe3a4feb140533a1c4d1b590c263c7ccca11

            // Se o espaço estiver ocupado e a chave for igual à procurada, o item foi localizado
            if (tabelaDeHash[pos] != null && tabelaDeHash[pos].Chave.Equals(item.Chave))
            {
                onde = pos;   // Guarda o índice onde o item foi achado
                return true;  // Retorna sucesso na busca
            }
        }
<<<<<<< HEAD
        onde = -1;
        return false; // Percorreu todos os saltos possíveis e não encontrou
=======
        return false;
>>>>>>> 7f4fbe3a4feb140533a1c4d1b590c263c7ccca11
    }

    // Remove um elemento do sistema com base no seu valor
    public bool Excluiu(T dadoParaExcluir)
    {
        // Traz o indice do dado que vai excluir, utilizando do existe para encontrar esse índice
        if (Existe(dadoParaExcluir, out int onde))
        {
            tabelaDeHash[onde] = default; // Apaga o objeto daquel posição
            foiExcluido[onde] = true;     // Adiciona no vetor de bools, marcando que aquel vaga foi excluída
            quantidade--;                 // Diminui quantos elementos temos no vetor tabelaDeHash
            return true;                  // Removeu com sucesso
        }
        return false; // O item não existia na tabela
    }

    // Retorna uma lista de strings mostrando os índices reais ocupados pelos dados
    public List<string> LocaisDosDados()
    {
        var dados = new List<string>();
        // Percorre todo o vetor da tabela
        for (int i = 0; i < tabelaDeHash.Length; i++)
            // Ignora slots vazios ou marcados como excluídos
            if (tabelaDeHash[i] != null && !foiExcluido[i])
                dados.Add($"{i,5} : {tabelaDeHash[i]}"); // Adiciona o índice formatado com 5 espaços
        return dados;
    }

    // Retorna uma lista limpa contendo apenas os objetos ativos da tabela
    public List<T> Conteudo()
    {
        var dados = new List<T>();
        // Percorre todo o vetor da tabela
        for (int i = 0; i < tabelaDeHash.Length; i++)
            // Filtra trazendo apenas os registros que estão ativos e válidos
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