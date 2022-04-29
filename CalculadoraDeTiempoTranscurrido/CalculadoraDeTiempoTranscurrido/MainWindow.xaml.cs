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

            this.AplicarTemaElegido(tema);
            this.CargarAnios();
            this.CargarMeses();
            this.CargarDias();
            this.Limpiar();
        }

        /// <summary>
        /// Carga una lista de años en el comboBox de años.
        /// </summary>
        private void CargarAnios()
        {
            int anioFinal = DateTime.Now.Year + 100;

            for (int i = 1900; i <= anioFinal; i++)
            {
                this.cmbAniosInicial.Items.Add(i);
                this.cmbAniosFinal.Items.Add(i);
            }
        }

        /// <summary>
        /// Carga una lista de meses en el comboBox de meses.
        /// </summary>
        private void CargarMeses()
        {
            string cadena;

            for (int i = 1; i <= 12; i++)
            {
                cadena = "";

                if (i < 10)
                {
                    cadena = "0";
                }

                cadena += i;
                this.cmbMesFinal.Items.Add(cadena);
                this.cmbMesInicial.Items.Add(cadena);
            }
        }

        /// <summary>
        /// Carga una lista de dias en el comboBox de dias.
        /// </summary>
        private void CargarDias()
        {
            string cadena;
            for (int i = 1; i <= 31; i++)
            {
                cadena = "";

                if (i < 10)
                {
                    cadena = "0";
                }
                cadena += i;
                this.cmbDiasFinal.Items.Add(cadena);
                this.cmbDiaInicial.Items.Add(cadena);
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
                this.CargarFechaActual(this.cmbDiasFinal, this.cmbMesFinal, this.cmbAniosFinal);
            }
            else
            {
                this.CargarFechaActual(this.cmbDiaInicial, this.cmbMesInicial, this.cmbAniosInicial);
            }
        }

        /// <summary>
        /// Carga la fecha actual en los comboBox recibidos por parametro.
        /// </summary>
        /// <param name="dia">Combobox de dias</param>
        /// <param name="mes">Combobox de meses</param>
        /// <param name="anio">Combobox de años</param>
        private void CargarFechaActual(ComboBox dia, ComboBox mes, ComboBox anio)
        {
            dia.SelectedIndex = DateTime.Now.Day - 1;
            mes.SelectedIndex = DateTime.Now.Month - 1;
            anio.SelectedIndex = this.ObtenerIndiceAño();
        }

        /// <summary>
        /// Obtiene el indice correspondiente al año actual, dentro del ComboBox de años.
        /// </summary>
        /// <returns>Un numero entero, que representa el indice del año actual dentro del comboBox de años</returns>
        private int ObtenerIndiceAño()
        {
            int retorno = 0;
            for (int i = 0; i < this.cmbAniosInicial.Items.Count; i++)
            {
                if ((int)this.cmbAniosInicial.Items[i] == DateTime.Now.Year)
                {
                    retorno = i;
                    break;
                }
            }
            return retorno;
        }

        /// <summary>
        /// Evento que se dispara al hacer click sobre el boton "Calcular"
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Objeto que contiene los datos del evento</param>
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (this.EstanComboBoxesCargados())
            {
                this.CargarLabelConDatos();
            }
            else
            {
                MessageBox.Show("Un momento! Complete los campos 'Dia', 'Mes' y 'Año' para poder operar!", "Alerta, campos incompletos!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Evalua que todos los ComboBoxes esten Cargados.
        /// </summary>
        /// <returns>True si los ComboBoxes estan cargados, caso contrario False.</returns>
        private bool EstanComboBoxesCargados()
        {
            return this.cmbDiaInicial.SelectedItem != null && this.cmbMesInicial.SelectedItem != null &&
                   this.cmbAniosInicial.SelectedItem != null && this.cmbDiasFinal.SelectedItem != null &&
                   this.cmbMesFinal.SelectedItem != null && this.cmbAniosFinal.SelectedItem != null;
        }

        /// <summary>
        /// Carga los datos en el Label
        /// </summary>
        private void CargarLabelConDatos()
        {
            try
            {
                DateTime fechaFinal = new DateTime(int.Parse(this.cmbAniosFinal.SelectedItem.ToString()), int.Parse(this.cmbMesFinal.SelectedItem.ToString()), int.Parse(this.cmbDiasFinal.SelectedItem.ToString()));

                DateTime fechaInicial = new DateTime(int.Parse(this.cmbAniosInicial.SelectedItem.ToString()), int.Parse(this.cmbMesInicial.SelectedItem.ToString()), int.Parse(this.cmbDiaInicial.SelectedItem.ToString()));

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
                elemento is GroupBox || elemento is ComboBox ||
                elemento is RadioButton)
            {
                ((Control)elemento).Background = tema.ColorDeFondoControl;
                ((Control)elemento).Foreground = tema.ColorDeLetra;
                ((Control)elemento).BorderBrush = tema.ColorBordeBoton;

                if (elemento is ComboBox)
                {
                    ((ComboBox)elemento).Opacity = 0.4;
                }
            }
        }

        /// <summary>
        /// Recorre todos los controles visuales HIJOS del objeto recibido por parametro
        /// </summary>
        /// <param name="myVisual">Objeto visual a recorrer</param>
        private void RecorrerControlesVisualesParaAplicarTema(Visual myVisual, ModificarControles metodo, Tema tema)
        {
            this.Background = tema.ColorDeFondoAplicacion;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);

                metodo(childVisual, tema);

                this.RecorrerControlesVisualesParaAplicarTema(childVisual, metodo, tema);
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
            this.cmbAniosFinal.SelectedItem = null;
            this.cmbAniosInicial.SelectedItem = null;
            this.cmbDiaInicial.SelectedItem = null;
            this.cmbDiasFinal.SelectedItem = null;
            this.cmbMesFinal.SelectedItem = null;
            this.cmbMesInicial.SelectedItem = null;
            this.lblAniosActual.Content = "";
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
