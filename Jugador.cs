using System.Windows.Forms;

namespace OcaJuego
{
    public class Jugador
    {
        public string Nombre { get; set; }
        public int PosActual { get; set; } = 0;
        public int Tirada { get; set; }
        public int PosActual_X { get; set; }
        public int PosActual_Y { get; set; }
        public int Puntos { get; set; } = 0;
        public RadioButton ControlVisual { get; set; }
        public int TurnosPerdidos { get; set; } = 0;

        public bool PierdeTurno { get; set; }
    }
}
