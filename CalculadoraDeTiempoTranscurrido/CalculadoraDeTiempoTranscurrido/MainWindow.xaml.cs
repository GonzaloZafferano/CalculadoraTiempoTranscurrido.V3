using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BibliotecaTema;
using BibliotecaTiempoTranscurrido;
using Validaciones;

namespace CalculadoraDeTiempoTranscurrido
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void ModificarControles(Visual visual, Tema tema);

        public MainWindow()
        {
            this.InitializeComponent();
        }

        #region Carga de datos

        /// <summary>
        /// Evento que se dispara al cargar la Ventana Principal
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void ventanaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            _ = Enum.TryParse(Properties.Settings.Default.temaAplicado, out Tema.Temas tema);

            switch(tema)
            {
                case Tema.Temas.Azul:
                    this.rBtnAzul.IsChecked = true;
                    break;
                case Tema.Temas.Verde:
                    this.rbtnVerde.IsChecked = true;
                    break;
                case Tema.Temas.Rosa:
                    this.rBtnRosa.IsChecked = true;
                    break;
            }

            this.AplicarTemaElegido(tema);
            this.Limpiar();
        }

        /// <summary>
        /// Carga la fecha actual en los Textbox recibidos por parametro.
        /// </summary>
        /// <param name="anio">Textbox del año.</param>
        /// <param name="mes">Textbox del mes.</param>
        /// <param name="dia">Textbox del dia.</param>
        private void CargarFechaActual(TextBox anio, TextBox mes, TextBox dia)
        {
            anio.Text = DateTime.Now.Year.ToString();
            mes.Text = DateTime.Now.Month.ToString();
            dia.Text = DateTime.Now.Day.ToString();
        }

        /// <summary>
        /// Carga los datos en el Label
        /// </summary>
        private void CargarLabelConDatos()
        {
            try
            {
                DateTime fechaFinal = new DateTime(int.Parse(this.txtAnioFinal.Text), int.Parse(this.txtMesFinal.Text), int.Parse(this.txtDiaFinal.Text));

                DateTime fechaInicial = new DateTime(int.Parse(this.txtAnioInicial.Text), int.Parse(this.txtMesInicial.Text), int.Parse(this.txtDiaInicial.Text));

                if (fechaInicial <= fechaFinal)
                {
                    this.lblAniosActual.Content = TiempoTranscurrido.ObtenerStringConAnioMesYDia(fechaInicial, fechaFinal);
                }
                else
                {
                    MessageBox.Show($"Un momento! La fecha inicial ({fechaInicial.ToShortDateString()}) no puede ser mayor a la fecha final ({fechaFinal.ToShortDateString()})!", "Alerta, fecha invalida!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Un momento! La/s fecha/s indicada/s no existe/existen!", "Alerta, fecha invalida!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Logica

        /// <summary>
        /// Evento que se dispara al hacer click sobre alguno de los "btnFechaHoy"
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void btnFechaHoy(object sender, RoutedEventArgs e)
        {
            if (object.ReferenceEquals(sender, btnFechaHoyFinal))
            {
                this.CargarFechaActual(this.txtAnioFinal, this.txtMesFinal, this.txtDiaFinal);
            }
            else
            {
                this.CargarFechaActual(this.txtAnioInicial, this.txtMesInicial, this.txtDiaInicial);
            }
        }

        /// <summary>
        /// Evalua si estan cargadas las fechas.
        /// </summary>
        /// <returns>True si estan cargadas, caso contrario False.</returns>
        private bool EstanFechasCargadas()
        {
            return !string.IsNullOrWhiteSpace(this.txtDiaInicial.Text) &&
                   !string.IsNullOrWhiteSpace(this.txtMesInicial.Text) &&
                   !string.IsNullOrWhiteSpace(this.txtAnioInicial.Text) &&
                   !string.IsNullOrWhiteSpace(this.txtDiaFinal.Text) &&
                   !string.IsNullOrWhiteSpace(this.txtMesFinal.Text) &&
                   !string.IsNullOrWhiteSpace(this.txtAnioFinal.Text);
        }

        /// <summary>
        /// Evento que se dispara al hacer click sobre el boton "Calcular"
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (this.EstanFechasCargadas())
            {
                this.CargarLabelConDatos();
            }
            else
            {
                MessageBox.Show("Un momento! Complete los campos 'Día', 'Mes' y 'Año' para poder operar!", "Alerta, campos incompletos!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Escribe en el TextBox de dia un contenido valido (numeros de 1 - 31).
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void txtDia_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = ((TextBox)sender);
            txt.Text = ValidarFecha.ObtenerDiaValido(txt.Text);
            txt.SelectionStart = txt.Text.Length;
        }

        /// <summary>
        /// Escribe en el TextBox de mes un contenido valido (numeros de 1 - 12).
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void txtMes_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = ((TextBox)sender);
            txt.Text = ValidarFecha.ObtenerMesValido(txt.Text);
            txt.SelectionStart = txt.Text.Length;
        }

        /// <summary>
        /// Escribe en el TextBox de año un contenido valido (numeros de 1 - 9999).
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void txtAnio_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = ((TextBox)sender);
            txt.Text = ValidarFecha.ObtenerAnioValido(txt.Text);
            txt.SelectionStart = txt.Text.Length;
        }

        #endregion

        #region Carga de Tema

        /// <summary>
        /// Evento que se dispara al hacer click sobre alguno de los "rBtn" de temas.
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void rbtnTema_Click(object sender, RoutedEventArgs e)
        {
            if (Object.ReferenceEquals(sender, this.rbtnVerde))
            {
                this.AplicarTemaElegido(Tema.Temas.Verde);
            }
            else if (Object.ReferenceEquals(sender, this.rBtnRosa))
            {
                this.AplicarTemaElegido(Tema.Temas.Rosa);
            }
            else
            {
                this.AplicarTemaElegido(Tema.Temas.Azul);
            }
        }

        /// <summary>
        /// Se encarga de gestionar la aplicacion de un tema
        /// </summary>
        /// <param name="tema">Tema a aplicar</param>
        private void AplicarTemaElegido(Tema.Temas tema)
        {
            Tema temaAplicacion = new Tema(tema);
            this.RecorrerControlesVisualesParaAplicarTema(this, this.CargarTema, temaAplicacion);

            Properties.Settings.Default.temaAplicado = tema.ToString();
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Carga el elemento con el tema claro
        /// </summary>
        /// <param name="elemento">Elemento que se modificara, previa evaluacion.</param>
        private void CargarTema(Visual elemento, Tema tema)
        {
            if (elemento is Label || elemento is Button ||
                elemento is GroupBox || elemento is TextBox ||
                elemento is RadioButton)
            {
                ((Control)elemento).Background = tema.ColorDeFondoControl;
                ((Control)elemento).Foreground = tema.ColorDeLetra;
                ((Control)elemento).BorderBrush = tema.ColorBordeBoton;

                if (elemento is TextBox textBox)
                {
                    textBox.Background = Brushes.White;
                    textBox.FontWeight = FontWeights.Bold;
                }
            }
        }

        /// <summary>
        /// Recorre todos los controles visuales HIJOS del objeto recibido por parametro
        /// </summary>
        /// <param name="visual">Objeto visual a recorrer</param>
        private void RecorrerControlesVisualesParaAplicarTema(Visual visual, ModificarControles metodo, Tema tema)
        {
            this.Background = tema.ColorDeFondoAplicacion;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual visualHijo = (Visual)VisualTreeHelper.GetChild(visual, i);

                metodo(visualHijo, tema);

                this.RecorrerControlesVisualesParaAplicarTema(visualHijo, metodo, tema);
            }
        }

        #endregion

        #region Limpiar y Cerrar

        /// <summary>
        /// Evento que se dispara al hacer click sobre el boton "Limpiar"
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            this.Limpiar();
        }

        /// <summary>
        /// Limpia los ComboBoxes y Label de los datos cargados.
        /// </summary>
        private void Limpiar()
        {
            this.txtDiaInicial.Text = string.Empty;
            this.txtMesInicial.Text = string.Empty;
            this.txtAnioInicial.Text = string.Empty;
            this.txtDiaFinal.Text = string.Empty;
            this.txtMesFinal.Text = string.Empty;
            this.txtAnioFinal.Text = string.Empty;
            this.lblAniosActual.Content = string.Empty;
        }

        /// <summary>
        /// Evento que se dispara al hacer cerrar la aplicacion.
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void ventanaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult respuesta = MessageBox.Show("Desea salir de la aplicacion?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        #endregion              
    }
}
