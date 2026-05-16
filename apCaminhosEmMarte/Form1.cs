using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace apCaminhosEmMarte
{
    public partial class FrmCaminhos : Form
    {
        public FrmCaminhos()
        {
            InitializeComponent();
        }

        IHashing<Cidade> tabelaDeCidades;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Método chamado ao clicar no botão para abrir o arquivo de texto
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
                // ler registros do arquivo aberto
                while (!asCidades.EndOfStream)
                {
                    // instanciar um objeto cidade
                    // lê-lo do arquivo para preencher seus atributos
                    // armazenar esse objeto na tabela de Hash
                    // de acordo com a técnica de hash escolhida
                    // pelo usuário
                    Cidade umaCidade = new Cidade().LerRegistro(asCidades);
                    if (umaCidade != null)
                        tabelaDeCidades.Incluiu(umaCidade);

                }
                pbMapa.Invalidate();
                // Desenhar os nomes das cidades no mapa de Marte
                asCidades.Close();  // deixar arquivo fechado
            }
        }

        private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
        {
            // aqui, a tabela de hash deve ser percorrida e os 
            // registros armazenados devem ser gravados no arquivo
            // agora, aberto para saída (StreamWriter).
        }

        // Método que desenha as cidades na tela dentro da PictureBox do mapa
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // Objeto usado para desenhar os elementos gráficos
            if (tabelaDeCidades != null)

            {
                var listaCidades = tabelaDeCidades.Conteudo(); // Pega a lista de cidades da tabela hash
                foreach (var cidade in listaCidades)
                {
                    // Se as coordenadas no arquivo forem de 0 a 1:
                    // Converte os valores decimais em pixels de acordo com o tamanho do mapa
                    int xPixel = (int)(cidade.X * pbMapa.Width);
                    int yPixel = (int)(cidade.Y * pbMapa.Height);

                    // Desenha o círculo vermelho representando a cidade
                    g.FillEllipse(Brushes.Red, xPixel - 3, yPixel - 3, 6, 6);

                    // Desenha o texto com o nome da cidade
                    g.DrawString(cidade.Chave, this.Font, Brushes.Black, xPixel - 10, yPixel + 5);
                }
            }
        }

        // Método chamado ao clicar no botão para inserir uma cidade manualmente
        private void btnInserir_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim(); // Pega o texto digitado tirando espaços em branco
            double x = (double)udX.Value; // Pega o valor X selecionado na tela
            double y = (double)udY.Value; // Pega o valor Y selecionado na tela

            if (!string.IsNullOrEmpty(nome))
            {

                var novaCidade = new Cidade(nome, x, y); // Cria um novo objeto cidade

                if (tabelaDeCidades.Incluiu(novaCidade)) // Tenta inserir o objeto na tabela hash ativa
                {
                    pbMapa.Invalidate(); // Força o mapa a se redesenhar com a nova cidade
                    MessageBox.Show("Incluido com sucesso!");
                }

                MessageBox.Show("Erro ao incluir!");
            }

            MessageBox.Show("Digite um nome para a cidade!");
        }

        // Método chamado ao clicar no botão para remover uma cidade do sistema
        private void btnRemover_Click(object sender, EventArgs e)
        {
            string nomeParaExcluir = txtNome.Text.Trim(); // Pega o nome digitado na tela
            var cidadeExcluida = new Cidade(nomeParaExcluir, 0, 0); // Cria o objeto para busca

            if (tabelaDeCidades.Excluiu(cidadeExcluida)) // Tenta remover da tabela hash ativa
            {
                pbMapa.Invalidate(); // Força o mapa a se redesenhar para sumir com a cidade
                MessageBox.Show("Cidade excluída com sucesso!");
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        // Método chamado ao clicar no botão para buscar uma cidade cadastrada
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nomeProcurado = txtNome.Text.Trim(); // Pega o nome digitado na tela
            var cidadeProcurada = new Cidade(nomeProcurado, 0, 0); // Cria o objeto para busca

            if (tabelaDeCidades.Existe(cidadeProcurada, out int onde)) // Verifica se a cidade existe na tabela
            {
                // Busca o objeto correspondente na lista de conteúdo
                var cidadeAchada = tabelaDeCidades.Conteudo().Find(c => c.Chave == nomeProcurado);

                // Atualiza os campos numéricos na tela com as coordenadas encontradas
                udX.Value = (decimal)cidadeAchada.X;
                udY.Value = (decimal)cidadeAchada.Y;

                MessageBox.Show("Busca realizada com sucesso! ");
            }
            else
                MessageBox.Show("Erro ao buscar cidade!");
        }
    }
}