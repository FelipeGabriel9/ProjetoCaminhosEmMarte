using System;
using System.Collections;
using System.Collections.Generic;

public class BucketHash<T> : IHashing<T>
             where T : IRegistro<T>, IEquatable<T>, new()
{
    private const int SIZE = 97;    // Tamanho fixo da tabela de hash
    ArrayList[] dados;              // Vetor utilizado para armazenar os dados


    // Construtor que inicializa o vetor de ArrayLists
    public BucketHash()
    {
        dados = new ArrayList[SIZE]; // Cria o vetor com o tamanho definido

        // cada posição do vetor recebe um ArrayList
        for (int i = 0; i < SIZE; i++)
            dados[i] = new ArrayList(4);    
    }

    // Método que calcula o índice de hash a partir da chave do dado
    private int Hash(string chave)
    {
        long tot = 0;

        // Percorre cada caractere da chave, multiplicando o total acumulado por 37 e somando o valor do caractere
        for (int i = 0; i < chave.Length; i++)
            tot += 37 * tot + (int)chave[i];

        // Garante que o índice fique dentro do tamanho do vetor
        tot = tot % dados.Length;

        // Se o total for negativo, ajusta para garantir um índice positivo
        if (tot < 0)
            tot += dados.Length;

        // Retorna o índice calculado
        return (int)tot;
    }

    // Método para incluir um novo dado na tabela de hash
    public bool Incluiu(T novoDado)
    {
        // Calcula o índice de hash para o novo dado
        int indiceDeHash = Hash(novoDado.Chave);

        // Verifica se o dado já existe no bucket
        if (!dados[indiceDeHash].Contains(novoDado))
        {
            // Se o dado não existe, adiciona ao bucket correspondente
            dados[indiceDeHash].Add(novoDado);
            return true;
        }

        // Se o dado já existe, não é possível incluir novamente
        return false;
    }

    // Método que verifica se um dado existe na tabela de hash
    public bool Existe(T dadoAProcurar, out int onde)
    {
        // Calcula o índice de hash para o dado a procurar
        onde = Hash(dadoAProcurar.Chave);

        // Verifica se o bucket correspondente contém o dado a procurar
        return dados[onde].Contains(dadoAProcurar);
    }

    // Método para excluir um dado da tabela de hash
    public bool Excluiu(T dadoAExcluir)
    {
        int onde = 0;

        // Verifica se o dado a excluir existe na tabela de hash
        if (!Existe(dadoAExcluir, out onde))
            return false;

        // Se o dado existe, remove do bucket correspondente
        dados[onde].Remove(dadoAExcluir);
        return true;
    }
    
    // Método que retorna os locais dos dados na tabela de hash
    public List<string> LocaisDosDados()
    {
        List<string> saida = new List<string>();

        // Percorre o vetor de buckets para encontrar os dados armazenados
        for (int i = 0; i < dados.Length; i++)

            // Verifica se o bucket atual contém dados
            if (dados[i].Count > 0)     
            {

                // Se o bucket contém dados, constrói uma string com o índice do bucket e os dados armazenados
                string linha = $"{i,5} : ";

                // Adiciona cada dado do bucket à string
                foreach (T dado in dados[i])
                    linha += " | " + dado;

                // Adiciona a linha construída à lista de saída
                saida.Add(linha);
            }

        // Retorna a lista de locais dos dados
        return saida;
    }

    // Método que retorna o conteúdo de todos os dados armazenados na tabela de hash
    public List<T> Conteudo()
    {
        List<T> saida = new List<T>();

        // Percorre o vetor de buckets para coletar os dados armazenados e insere na lista de saída
        for (int i = 0; i < dados.Length; i++) 
            foreach (T dado in dados[i])
                saida.Add(dado);

        // Retorna a lista de saída
        return saida;
    }
}

