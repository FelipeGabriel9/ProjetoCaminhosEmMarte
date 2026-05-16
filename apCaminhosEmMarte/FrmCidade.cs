using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace apCaminhosEmMarte
{
    public partial class FrmCidade : Form
    {
        // Construtor do formulário
        public FrmCidade()
        {
            InitializeComponent();
        }

        // Variável para armazenar a tabela de hash de cidades
        IHashing<Cidade> tabelaDeCidades;

        // Variável para armazenar a cidade que está sendo procurada
        Cidade cidadeProcurada = null;


        // Evento de clique para abrir o arquivo de cidades
        private void btnAbrirArquivo_Click(object sender, EventArgs e)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                // verificamos qual a técnica de Hash escolhida
                // pelo usuário e criamos uma tabela de hash de
                // acordo com essa escolha
                if (rbBucketHash.Checked)
                    tabelaDeCidades = new BucketHash<Cidade>();

                else if (rbSondagemLinear.Checked)
                    tabelaDeCidades = new HashLinear<Cidade>();

                else if (rbSondagemQuadratica.Checked)
                    tabelaDeCidades = new HashQuadratico<Cidade>();

                else
                    tabelaDeCidades = new HashDuplo<Cidade>();

                // abrimos o arquivo escolhido
                var asCidades = new StreamReader(dlgAbrir.FileName);

                // Inicializamos a variável de cidade procurada como null
                cidadeProcurada = null;

                // ler registros do arquivo aberto
                while (!asCidades.EndOfStream)
                {
                    // Lemos uma cidade do arquivo e tentamos incluí-la na tabela de hash
                    var umaCidade = new Cidade().LerRegistro(asCidades);

                    // Se a cidade foi lida corretamente, adicionamos na tabela de hash
                    if (umaCidade != null && !string.IsNullOrEmpty(umaCidade.Chave))
                    {
                        if (!tabelaDeCidades.Incluiu(umaCidade))
                        {
                            // Caso a inclusão falhe, exibimos uma mensagem de erro
                            MessageBox.Show($"Erro ao incluir a cidade: {umaCidade.Chave}");
                        }
                    }

                }

                // Atualizamos o mapa na tela
                pbMapa.Invalidate();

                // Fechamos o arquivo após a leitura
                asCidades.Close();
            }
        }


        // Evento que desenha um ponto no mapa
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics ponto = e.Graphics;

            // Verifica se a tabela de cidades foi carregada
            if (tabelaDeCidades != null)
            {
                // Obtém a lista de cidades da tabela de hash
                var listaCidades = tabelaDeCidades.Conteudo();

                // Percorre todas as cidades e desenha um ponto no mapa para cada uma
                foreach (var cidade in listaCidades)
                {
                    // Calcula a posição X no mapa
                    int xPixel = (int)(cidade.X * pbMapa.Width);

                    // Calcula a posição Y no mapa
                    int yPixel = (int)(cidade.Y * pbMapa.Height);

                    // Verifica se a cidade é a cidade procurada
                    if (cidade == cidadeProcurada)
                    {
                        // Define a cor que será usada para destacar a cidade procurada
                        var corBuscar = new Pen(Color.Yellow, 4);

                        // Desenha um ponto azul no mapa para representar a cidade procurada
                        ponto.DrawEllipse(corBuscar, xPixel - 10, yPixel - 10, 20, 20);

                        // Desenha um ponto vermelho no mapa para representar a cidade
                        ponto.FillEllipse(Brushes.Red, xPixel - 4, yPixel - 4, 8, 8);
                    }
                    else
                    {
                        // Desenha um ponto vermelho no mapa para representar a cidade
                        ponto.FillEllipse(Brushes.Red, xPixel - 4, yPixel - 4, 8, 8);
                    }

                    // Escreve o nome da cidade próximo ao ponto desenhado
                    ponto.DrawString(cidade.Chave, this.Font, Brushes.Black, xPixel - 10, yPixel + 5);
                }
            }
        }

        // Evento do botão de inserir uma cidade
        private void btnInserir_Click(object sender, EventArgs e)
        {
            // Verifica se o arquivo de cidades foi aberto
            if (tabelaDeCidades == null)
            {
                MessageBox.Show("Abra o arquivo primeiro!");
                return;
            }
            else
            {
                // Lê os dados de uma cidade
                string nome = txtNome.Text.Trim();
                double x = (double)udX.Value;
                double y = (double)udY.Value;

                // Inicializa a variável de cidade procurada como null
                cidadeProcurada = null;

                // Verifica se o nome da cidade foi preenchido
                if (string.IsNullOrEmpty(nome))
                {
                    MessageBox.Show("Digite um nome para a cidade!");
                }
                else
                {
                    // Cria um objeto Cidade com os dados lidos
                    var novaCidade = new Cidade(nome, x, y);

                    // Tenta incluir a nova cidade na tabela de hash e atualiza o mapa
                    if (tabelaDeCidades.Incluiu(novaCidade))
                    {
                        pbMapa.Invalidate();
                        MessageBox.Show("Incluido com sucesso!");
                    }
                    else
                    {
                        // Caso a inclusão falhe, atualiza o mapa e exibe uma mensagem de erro
                        pbMapa.Invalidate();
                        MessageBox.Show("Erro ao incluir!");
                    }
                }
            }
        }

        // Evento do botão de remover uma cidade
        private void btnRemover_Click(object sender, EventArgs e)
        {
            // Verifica se o arquivo de cidades foi aberto
            if (tabelaDeCidades == null)
            {
                MessageBox.Show("Abra o arquivo primeiro!");
                return;
            }

            // Cria um objeto Cidade com o nome da cidade a ser excluída
            var cidadeAExcluir = new Cidade(txtNome.Text.Trim(), 0, 0);

            // Marca a cidade procurada como null, visto que ainda não estamos procurando nenhuma cidade
            cidadeProcurada = null;

            // Tenta excluir a cidade da tabela de hash e atualiza o mapa
            if (tabelaDeCidades.Excluiu(cidadeAExcluir))
            {
                pbMapa.Invalidate();
                MessageBox.Show("Cidade excluída com sucesso!");
            }
            else
            {
                // Caso a exclusão falhe, exibe uma mensagem de erro
                MessageBox.Show("Cidade não encontrada!");
            }
        }

        // Evento do botão de buscar uma cidade
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verifica se o campo de nome da cidade foi preenchido
            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencha o nome da cidade!");
                txtNome.Focus();
                pbMapa.Invalidate();
            }

            // Cria um objeto Cidade com o nome da cidade a ser buscada
            cidadeProcurada = new Cidade(txtNome.Text.Trim(), 0, 0);

            // Verifica se a cidade existe na tabela
            if (tabelaDeCidades.Existe(cidadeProcurada, out int onde))
            {
                // Se a cidade existe, encontra a cidade na tabela de hash e atualiza os campos de coordenadas
                var cidadeEncontrada = tabelaDeCidades.Conteudo().Find(c => c.Chave == cidadeProcurada.Chave);

                udX.Value = (decimal)cidadeEncontrada.X;
                udY.Value = (decimal)cidadeEncontrada.Y;

                // Atualiza a variável de cidade procurada para a cidade encontrada e atualiza o mapa
                cidadeProcurada = cidadeEncontrada;
                pbMapa.Invalidate();

                MessageBox.Show("Busca realizada com sucesso! ");
            }
            else
            {
                // Caso a cidade não seja encontrada, exibe uma mensagem de erro e atualiza o mapa
                pbMapa.Invalidate();
                MessageBox.Show("Erro ao buscar cidade!");
            }
        }

        // Evento acionado ao fechar o programa
        private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Verifica se a tabela de cidades foi carregada e se possui pelo menos uma cidade
            if (tabelaDeCidades != null && tabelaDeCidades.Conteudo().Count > 0)
            {
                // Cria um arquivo para salvar as cidades, utilizando o mesmo nome do arquivo aberto
                using (var salvarArquivo = new StreamWriter(dlgAbrir.FileName))
                {
                    // Obtém a lista de cidades da tabela de hash e escreve cada cidade no arquivo
                    var listaCidades = tabelaDeCidades.Conteudo();
                    foreach (var cidade in listaCidades)
                    {
                        cidade.EscreverRegistro(salvarArquivo);
                    }
                }
                // Aqui, o arquivo é automaticamente fechado após a escrita devido ao uso do bloco 'using'
            }
        }

        // Evento do botão de listar as cidades
        private void btnListar_Click(object sender, EventArgs e)
        {
            // Verifica se o arquivo de cidades foi aberto
            if (tabelaDeCidades == null)
            {
                MessageBox.Show("Abra o arquivo antes de listar!");
                return;
            }

            // Obtém a lista de todas as cidades da tabela hash
            List<Cidade> listaCidades = tabelaDeCidades.Conteudo();

            // 3. Verifica se a tabela não está vazia
            if (listaCidades.Count == 0)
            {
                MessageBox.Show("A tabela está vazia!");
                return;
            }

            // Limpa o ListBox antes de listar
            lsbListagem.Items.Clear();

            //Percorre a lista e adiciona no ListBox
            foreach (var cidade in listaCidades)
            {
                string nomeCidade = cidade.Chave + ";";
                // Aqui formatamos como a linha vai aparecer na tela para o usuário
                string linha = $"{nomeCidade,-20} X: {cidade.X:0.00000}; Y: {cidade.Y:0.00000};";

                lsbListagem.Items.Add(linha);
            }

            MessageBox.Show($"Listagem concluída!");
        }
    }
}
