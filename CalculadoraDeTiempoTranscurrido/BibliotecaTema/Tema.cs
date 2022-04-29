using System;
using System.Windows.Media;

namespace BibliotecaTema
{
    public class Tema
    {
        public enum Temas { Azul, Rosa, Verde };
        private Brush colorLetra;
        private Brush colorDeFondoAplicacion;
        private Brush colorDeFondoControl;
        private Brush colorBordeBoton;
        private Temas temaAplicado;

        public Brush ColorDeLetra { get => this.colorLetra; }
        public Brush ColorDeFondoAplicacion { get => this.colorDeFondoAplicacion; }
        public Brush ColorDeFondoControl { get => this.colorDeFondoControl; }
        public Brush ColorBordeBoton { get => this.colorBordeBoton; }
        public Temas TemaAplicado { get => this.temaAplicado; }

        public Tema(Temas tema)
        {
            this.temaAplicado = tema;
            this.CargarTema();
        }

        /// <summary>
        /// Carga los colores necesarios en el objeto, de acuerdo al tema aplicado.
        /// </summary>
        private void CargarTema()
        {
            switch (temaAplicado)
            {
                case Temas.Rosa:
                    this.colorBordeBoton = Brushes.MediumVioletRed;
                    this.colorDeFondoAplicacion = Brushes.LightPink;
                    this.colorDeFondoControl = Brushes.HotPink;
                    this.colorLetra = Brushes.MediumVioletRed;
                    break;

                case Temas.Verde:
                    this.colorBordeBoton = Brushes.DarkGreen;
                    this.colorDeFondoAplicacion = Brushes.GreenYellow;
                    this.colorDeFondoControl = Brushes.YellowGreen;
                    this.colorLetra = Brushes.DarkGreen;

                    break;
                case Temas.Azul:
                    this.colorBordeBoton = Brushes.DarkBlue;
                    this.colorDeFondoAplicacion = Brushes.LightSteelBlue;
                    this.colorDeFondoControl = Brushes.CornflowerBlue;
                    this.colorLetra = Brushes.DarkBlue;
                    break;
            }
        }
    }
}
