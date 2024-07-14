using OcaJuego;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OcaJuego
{
    public partial class Form1 : Form
    {
        private Jugador jugador1 = new Jugador { Nombre = "Jugador 1" };
        private Jugador jugador2 = new Jugador { Nombre = "Jugador 2" };
        private Jugador jugadorActual;

        RadioButton rbJugador1 = new RadioButton { Name = "jugador1" };
        RadioButton rbJugador2 = new RadioButton { Name = "jugador2"};

        private Random random = new Random();

        private List<Point> posicionesCasillas = new List<Point>();
        private List<Panel> casillasVisuales = new List<Panel>();

        private Label lblValorDado;
        private Label lblMovimiento;
        private Label lblPuntosJugador1;
        private Label lblPuntosJugador2;

        private PictureBox pictureBoxDado;
        private Timer timerDado;
        private int valorDado;
        private int timerCounter;
        private List<Image> dadoImages;

        private Button btnOcaEnOca;
        private Button btnPuenteEnPuente;
        private Button btnDadoADado;
        private Button btnDado;
        private Button btnCagaste;

        private List<int> casillasOca = new List<int> { 5, 9, 14, 18, 23, 27, 32, 36, 41, 45, 50, 54, 59, 63 };
        private List<int> casillasPuente = new List<int> { 6, 12 };
        private List<int> casillasDado = new List<int> { 26, 53 };

        public Form1()
        {
            InitializeComponent();

            lblValorDado = new Label();
            lblMovimiento = new Label();
            lblPuntosJugador1 = new Label();
            lblPuntosJugador2 = new Label();

            lblValorDado.AutoSize = true;
            lblValorDado.Location = new Point(10, 10);
            lblValorDado.Name = "lblValorDado";
            lblValorDado.Size = new Size(100, 23);
            lblValorDado.Text = "Valor del Dado: 0";

            lblMovimiento.AutoSize = true;
            lblMovimiento.Location = new Point(10, 40);
            lblMovimiento.Name = "lblMovimiento";
            lblMovimiento.Size = new Size(100, 23);
            lblMovimiento.Text = "Movimiento: 0";

            lblPuntosJugador1.AutoSize = true;
            lblPuntosJugador1.Location = new Point(10, 70);
            lblPuntosJugador1.Name = "lblPuntosJugador1";
            lblPuntosJugador1.Size = new Size(100, 23);
            lblPuntosJugador1.Text = "Jugador 1: 0";

            lblPuntosJugador2.AutoSize = true;
            lblPuntosJugador2.Location = new Point(10, 100);
            lblPuntosJugador2.Name = "lblPuntosJugador2";
            lblPuntosJugador2.Size = new Size(100, 23);
            lblPuntosJugador2.Text = "Jugador 2: 0";

            this.Controls.Add(rbJugador1);
            this.Controls.Add(rbJugador2);

            jugador1.ControlVisual = rbJugador1;
            jugador2.ControlVisual = rbJugador2;
            this.Controls.Add(lblValorDado);
            this.Controls.Add(lblMovimiento);
            this.Controls.Add(lblPuntosJugador1);
            this.Controls.Add(lblPuntosJugador2);

            jugador1.ControlVisual = this.Controls["jugador1"] as RadioButton;
            jugador2.ControlVisual = this.Controls["jugador2"] as RadioButton;
            rbJugador2.BackColor = Color.Blue;  // Color de fondo
            rbJugador1.BackColor = Color.Red;  // Color de fondo
            rbJugador1.Size = new Size(20, 20);  // Ancho y alto del RadioButton
            rbJugador2.Size = new Size(20, 20);  // Ancho y alto del RadioButton
            rbJugador1.Location = new Point(137, 639);
            rbJugador2.Location = new Point(117, 639);

            jugadorActual = jugador1;

            InicializarPosicionesCasillas();
           // CrearCasillasVisuales();


            pictureBoxDado = new PictureBox();
            pictureBoxDado.Size = new Size(50, 50);
            pictureBoxDado.Location = new Point(729, 70);
            pictureBoxDado.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBoxDado);

            timerDado = new Timer();
            timerDado.Interval = 100;
            timerDado.Tick += TimerDado_Tick;

            btnDado = new Button();
            btnDado.Text = "Tirar Dado";
            btnDado.Location = new Point(729, 130);
            btnDado.Click += BtnDado_Click;
            this.Controls.Add(btnDado);

            btnOcaEnOca = new Button();
            btnOcaEnOca.Text = "De oca en oca";
            btnOcaEnOca.Location = new Point(729, 170);
            btnOcaEnOca.Click += BtnOcaEnOca_Click;
            btnOcaEnOca.Enabled = true;
            btnOcaEnOca.Visible = false;
            this.Controls.Add(btnOcaEnOca);

            btnPuenteEnPuente = new Button();
            btnPuenteEnPuente.Text = "De puente en puente";
            btnPuenteEnPuente.Location = new Point(729, 210);
            btnPuenteEnPuente.Click += btnPuenteEnPuente_Click;
            btnPuenteEnPuente.Enabled = true;
            btnPuenteEnPuente.Visible = false;
            this.Controls.Add(btnPuenteEnPuente);

            btnDadoADado = new Button();
            btnDadoADado.Text = "De dado a dado";
            btnDadoADado.Location = new Point(729, 250);
            btnDadoADado.Click += btnDadoADado_Click;
            btnDadoADado.Enabled = true;
            btnDadoADado.Visible = false;
            this.Controls.Add(btnDadoADado);

            btnCagaste = new Button();
            btnCagaste.Text = "CAGASTE";
            btnCagaste.Location = new Point(729, 290);
            btnCagaste.Click += BtnCagaste_Click;
            btnCagaste.Visible = false;
            this.Controls.Add(btnCagaste);

            dadoImages = new List<Image>
            {
                Properties.Resources.dado1,
                Properties.Resources.dado2,
                Properties.Resources.dado3,
                Properties.Resources.dado4,
                Properties.Resources.dado5,
                Properties.Resources.dado6
            };
        }

        private void BtnDado_Click(object sender, EventArgs e)
        {
            if (jugadorActual.TurnosPerdidos > 0)
            {
                jugadorActual.TurnosPerdidos--;
                AlternarTurno();
                return;
            }

            timerCounter = 0;
            timerDado.Start();
        }

        private void TimerDado_Tick(object sender, EventArgs e)
        {
            if (timerCounter < 10)
            {
                valorDado = random.Next(1, 7);
                pictureBoxDado.Image = dadoImages[valorDado - 1];
                timerCounter++;
            }
            else
            {
                timerDado.Stop();

                jugadorActual.Tirada = valorDado;
                jugadorActual.PosActual += valorDado;

                if (jugadorActual.PosActual > 63)
                {
                    int exceso = jugadorActual.PosActual - 63;
                    jugadorActual.PosActual = 63 - exceso;
                }

                Point nuevaPosicion = posicionesCasillas[jugadorActual.PosActual];
                MoverJugador(jugadorActual, nuevaPosicion);

                if (jugadorActual.PosActual == 63)
                {
                    MostrarVictoria();
                    return;
                }

                if (jugadorActual.PosActual == 58)
                {
                    btnCagaste.Visible = true;
                    btnDado.Visible = false;
                }
                else if (casillasOca.Contains(jugadorActual.PosActual))
                {
                    lblMovimiento.Text = "De oca en oca y tiro porque me toca";
                    btnOcaEnOca.Visible = true;
                    btnDado.Visible = false;
                }
                else if (casillasPuente.Contains(jugadorActual.PosActual))
                {
                    lblMovimiento.Text = "De puente en puente y tiro porque me lleva la corriente";
                    btnPuenteEnPuente.Visible = true;
                    btnDado.Visible = false;
                }
                else if (casillasDado.Contains(jugadorActual.PosActual))
                {
                    lblMovimiento.Text = "De dado a dado y tiro porque me ha tocado";
                    btnDadoADado.Visible = true;
                    btnDado.Visible = false;
                }
                else if (jugadorActual.PosActual == 19)
                {
                    jugadorActual.TurnosPerdidos = 1;
                    AlternarTurno();
                }
                else if (jugadorActual.PosActual == 31)
                {
                    jugadorActual.TurnosPerdidos = 2;
                    AlternarTurno();
                }
                else if (jugadorActual.PosActual == 52)
                {
                    jugadorActual.TurnosPerdidos = 3;
                    AlternarTurno();
                }
                else
                {
                    AlternarTurno();
                }

                lblValorDado.Text = $"Valor del Dado: {valorDado}";
                lblPuntosJugador1.Text = $"Jugador 1: {jugador1.PosActual}";
                lblPuntosJugador2.Text = $"Jugador 2: {jugador2.PosActual}";
            }
        }


        /*  private void CrearCasillasVisuales()
          {
              foreach (Point posicion in posicionesCasillas)
              {
                  Panel casilla = new Panel();
                  casilla.Size = new Size(40, 40);
                  casilla.Location = posicion;
                  casilla.BackColor = Color.LightGray;
                  casilla.BorderStyle = BorderStyle.FixedSingle;

                  this.Controls.Add(casilla);
                  casillasVisuales.Add(casilla);
              }
          }*/

        private void MoverJugador(Jugador jugador, Point nuevaPosicion)
        {
            if (jugador.ControlVisual != null)
            {
                jugador.ControlVisual.Location = new Point(nuevaPosicion.X, nuevaPosicion.Y + 20);
            }
            else
            {
                throw new Exception("El control visual del jugador es nulo.");
            }
        }

        private void BtnOcaEnOca_Click(object sender, EventArgs e)
        {
            int siguienteOca = casillasOca.Find(casilla => casilla > jugadorActual.PosActual);

            // Mover al jugador a la siguiente Oca.
            jugadorActual.PosActual = siguienteOca;

            // Actualizar la posición visual del jugador.
            Point nuevaPosicion = posicionesCasillas[jugadorActual.PosActual];
            MoverJugador(jugadorActual, nuevaPosicion);

            // Si el jugador alcanza la casilla 63 (meta), mostrar victoria.
            if (jugadorActual.PosActual == 63)
            {
                MostrarVictoria();

                return;
            }

            lblMovimiento.Text = "Tiro de nuevo porque estoy en una oca";

            // Actualizar el estado de los botones.
            btnOcaEnOca.Visible = false;
            btnDado.Visible = true;

            // Actualizar los puntos en la interfaz.
            lblPuntosJugador1.Text = $"Jugador 1: {jugador1.PosActual}";
            lblPuntosJugador2.Text = $"Jugador 2: {jugador2.PosActual}";
        }

        private void btnPuenteEnPuente_Click(object sender, EventArgs e)
        {
            if (jugadorActual.PosActual == 6)
            {
                jugadorActual.PosActual = 12; // Si el jugador está en la primera casilla de puente, se mueve a la segunda.
            }
            else if (jugadorActual.PosActual == 12)
            {
                jugadorActual.PosActual = 6; // Si el jugador está en la segunda casilla de puente, se mueve a la primera.
            }

            Point nuevaPosicion = posicionesCasillas[jugadorActual.PosActual];
            MoverJugador(jugadorActual, nuevaPosicion);

            lblMovimiento.Text = "De puente en puente y tiro porque me lleva la corriente";

            btnPuenteEnPuente.Visible = false;
            btnDado.Visible = true;

            lblPuntosJugador1.Text = $"Jugador 1: {jugador1.PosActual}";
            lblPuntosJugador2.Text = $"Jugador 2: {jugador2.PosActual}";
        }


        private void btnDadoADado_Click(object sender, EventArgs e)
        {
            if (jugadorActual.PosActual == 26)
            {
                jugadorActual.PosActual = 53; // Si el jugador está en la primera casilla de dado, se mueve a la segunda.
            }
            else if (jugadorActual.PosActual == 53)
            {
                jugadorActual.PosActual = 26; // Si el jugador está en la segunda casilla de dado, se mueve a la primera.
            }

            Point nuevaPosicion = posicionesCasillas[jugadorActual.PosActual];
            MoverJugador(jugadorActual, nuevaPosicion);

            lblMovimiento.Text = "Tiro de nuevo porque estoy en un dado";

            btnDadoADado.Visible = false;
            btnDado.Visible = true;

            lblPuntosJugador1.Text = $"Jugador 1: {jugador1.PosActual}";
            lblPuntosJugador2.Text = $"Jugador 2: {jugador2.PosActual}";
        }

        private void BtnCagaste_Click(object sender, EventArgs e)
        {
            // Asegúrate de que el jugador actual vuelva al principio.
            jugadorActual.PosActual = 1; // Casilla inicial en lugar de 0.
            Point nuevaPosicion = posicionesCasillas[jugadorActual.PosActual];
            MoverJugador(jugadorActual, nuevaPosicion);

            lblMovimiento.Text = $"{jugadorActual.Nombre} regresa al inicio";

            btnCagaste.Visible = false;
            btnDado.Visible = true;

            lblPuntosJugador1.Text = $"Jugador 1: {jugador1.PosActual}";
            lblPuntosJugador2.Text = $"Jugador 2: {jugador2.PosActual}";
        }


        private void MostrarVictoria()
        {
            MessageBox.Show($"{jugadorActual.Nombre} ha ganado el juego!", "¡Victoria!");
            btnDado.Enabled = false; // Deshabilita el botón de tirar el dado para que el juego no continúe
            btnOcaEnOca.Visible = false;
            btnReiniciar.Visible = true;
            lblVolver.Visible = true;
        }

        private void AlternarTurno()
        {
            jugadorActual = (jugadorActual == jugador1) ? jugador2 : jugador1;
        }

        private void InicializarPosicionesCasillas()
        {
            // Agregar posiciones de las casillas a la lista 
            posicionesCasillas.Add(new Point(137, 619));   // Casilla Inicial
            posicionesCasillas.Add(new Point(187, 619));   // Casilla 1
            posicionesCasillas.Add(new Point(264, 619));   // Casilla 2
            posicionesCasillas.Add(new Point(317, 619));  // Casilla 3
            posicionesCasillas.Add(new Point(368, 619));  // Casilla 4
            posicionesCasillas.Add(new Point(435, 619));  // Casilla 5 OCA
            posicionesCasillas.Add(new Point(513, 619));  // Casilla 6 PUENTE
            posicionesCasillas.Add(new Point(605, 619));  // Casilla 7
            posicionesCasillas.Add(new Point(593, 533));  // Casilla 8
            posicionesCasillas.Add(new Point(599, 473));  // Casilla 9 OCA
            posicionesCasillas.Add(new Point(611, 371));  // Casilla 10
            posicionesCasillas.Add(new Point(616, 324));  // Casilla 11
            posicionesCasillas.Add(new Point(617, 257));  // Casilla 12 PUENTE
            posicionesCasillas.Add(new Point(603, 194));  // Casilla 13
            posicionesCasillas.Add(new Point(608, 123));  // Casilla 14 OCA
            posicionesCasillas.Add(new Point(610, 42));  // Casilla 15 LAGO
            posicionesCasillas.Add(new Point(533, 35));  // Casilla 16
            posicionesCasillas.Add(new Point(477, 35));  // Casilla 17
            posicionesCasillas.Add(new Point(420, 50));  // Casilla 18 OCA
            posicionesCasillas.Add(new Point(333, 61));  // Casilla 19 POSADA
            posicionesCasillas.Add(new Point(261, 59));  // Casilla 20
            posicionesCasillas.Add(new Point(212, 59));  // Casilla 21
            posicionesCasillas.Add(new Point(159, 59));  // Casilla 22
            posicionesCasillas.Add(new Point(82, 63));  // Casilla 23 OCA
            posicionesCasillas.Add(new Point(78, 137));  // Casilla 24
            posicionesCasillas.Add(new Point(78, 188));  // Casilla 25
            posicionesCasillas.Add(new Point(78, 241));  // Casilla 26 DADO
            posicionesCasillas.Add(new Point(78, 316));  // Casilla 27 OCA
            posicionesCasillas.Add(new Point(78, 366));  // Casilla 28
            posicionesCasillas.Add(new Point(78, 412));  // Casilla 29
            posicionesCasillas.Add(new Point(78, 452));  // Casilla 30 
            posicionesCasillas.Add(new Point(110, 531));  // Casilla 31 POZO
            posicionesCasillas.Add(new Point(185, 526));  // Casilla 32 OCA
            posicionesCasillas.Add(new Point(249, 526));  // Casilla 33
            posicionesCasillas.Add(new Point(302, 526));  // Casilla 34
            posicionesCasillas.Add(new Point(348, 526));  // Casilla 35
            posicionesCasillas.Add(new Point(429, 526));  // Casilla 36 OCA
            posicionesCasillas.Add(new Point(509, 534));  // Casilla 37
            posicionesCasillas.Add(new Point(517, 450));  // Casilla 38 LABERINTO
            posicionesCasillas.Add(new Point(537, 375));  // Casilla 39
            posicionesCasillas.Add(new Point(517, 323));  // Casilla 40
            posicionesCasillas.Add(new Point(530, 279));  // Casilla 41 OCA
            posicionesCasillas.Add(new Point(516, 204));  // Casilla 42
            posicionesCasillas.Add(new Point(518, 134));  // Casilla 43
            posicionesCasillas.Add(new Point(451, 134));  // Casilla 44
            posicionesCasillas.Add(new Point(382, 134));  // Casilla 45 OCA
            posicionesCasillas.Add(new Point(315, 134));  // Casilla 46
            posicionesCasillas.Add(new Point(257, 134));  // Casilla 47
            posicionesCasillas.Add(new Point(193, 160));  // Casilla 48
            posicionesCasillas.Add(new Point(175, 200));  // Casilla 49
            posicionesCasillas.Add(new Point(174, 260));  // Casilla 50 OCA
            posicionesCasillas.Add(new Point(172, 307));  // Casilla 51
            posicionesCasillas.Add(new Point(175, 368));  // Casilla 52 CARCEL
            posicionesCasillas.Add(new Point(194, 431));  // Casilla 53 DADO
            posicionesCasillas.Add(new Point(263, 445));  // Casilla 54 OCA
            posicionesCasillas.Add(new Point(317, 445));  // Casilla 55
            posicionesCasillas.Add(new Point(363, 445));  // Casilla 56
            posicionesCasillas.Add(new Point(431, 434));  // Casilla 57
            posicionesCasillas.Add(new Point(450, 361));  // Casilla 58 CAGASTE
            posicionesCasillas.Add(new Point(449, 300));  // Casilla 59 OCA
            posicionesCasillas.Add(new Point(431, 217));  // Casilla 60
            posicionesCasillas.Add(new Point(369, 202));  // Casilla 61
            posicionesCasillas.Add(new Point(313, 233));  // Casilla 62
            posicionesCasillas.Add(new Point(318, 350));  // Casilla 63 OCA FINAL



        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            rbJugador1.Location = new Point(137, 639);
            rbJugador2.Location = new Point(117, 639);
            btnDado.Visible = true;
            btnReiniciar.Visible = false;
            lblVolver.Visible = false;
            btnDado.Enabled = true;
        }
    }
}
