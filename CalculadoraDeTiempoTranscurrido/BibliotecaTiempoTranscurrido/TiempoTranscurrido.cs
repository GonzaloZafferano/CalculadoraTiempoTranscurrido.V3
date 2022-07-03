using System;
using System.Text;

namespace BibliotecaTiempoTranscurrido
{
    public static class TiempoTranscurrido
    {
        #region Obtener Dias, Meses y Años

        private static DateTime fechaFinal;

        /// <summary>
        /// Obtiene un string con la cantidad de Años, meses y dias transcurridos desde una fecha inicial hasta la fecha actual.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial a partir de la cual se desea evaluar la cantidad de años, meses y dias trancurridos hasta la actualidad.</param>
        /// <returns>Un mensaje con la cantidad de años, meses y dias transcurridos.</returns>
        public static string ObtenerStringConAnioMesYDia(DateTime fechaInicial, DateTime fechaFinal)
        {
            StringBuilder sb = new StringBuilder();
            int anios = 0;
            int meses = 0;
            int dias = 0;
            int diasTotales = 0;

            TiempoTranscurrido.fechaFinal = fechaFinal;

            if (TiempoTranscurrido.fechaFinal.ToShortDateString() != fechaInicial.ToShortDateString())
            {
                anios = TiempoTranscurrido.ObtenerAnios(fechaInicial);
                meses = TiempoTranscurrido.ObtenerMeses(fechaInicial);
                dias = TiempoTranscurrido.ObtenerDias(fechaInicial);
                diasTotales = TiempoTranscurrido.ObtenerDiferenciaTotalEnDias(fechaInicial);
            }

            sb.AppendLine(string.Format("{0:N0} años.", anios));
            sb.AppendLine(string.Format("{0} meses.", meses));
            sb.AppendLine(string.Format("{0} días.", dias));
            sb.AppendLine(string.Format("{0:N0} días en total.", diasTotales));

            return sb.ToString();
        }

        /// <summary>
        /// Calcula la cantidad de años transcurridos entre una fecha de inicio y la fecha actual.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial a partir de la cual se desea evaluar la cantidad de años trancurridos hasta la actualidad.</param>
        /// <returns>Cantidad de años transcurridos.</returns>
        private static int ObtenerAnios(DateTime fechaInicial)
        {
            int anios = TiempoTranscurrido.fechaFinal.Year - fechaInicial.Year;

            if (fechaInicial.Month > TiempoTranscurrido.fechaFinal.Month ||
               (fechaInicial.Month == TiempoTranscurrido.fechaFinal.Month && fechaInicial.Day > TiempoTranscurrido.fechaFinal.Day))
            {
                anios--;
            }
            return anios;
        }

        /// <summary>
        /// Calcula la cantidad de dias transcurridos desde el ultimo cumple-mes (de la fecha inicial) hasta la fecha actual.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial para obtener el cumple-mes.</param>
        /// <returns>Cantidad de dias transcurridos desde el ultimo cumple-mes.</returns>
        private static int ObtenerDias(DateTime fechaInicial)
        {
            int diasTranscurridos;

            if (TiempoTranscurrido.fechaFinal.Day > fechaInicial.Day)
            {
                diasTranscurridos = TiempoTranscurrido.fechaFinal.Day - fechaInicial.Day;
            }
            else
            {
                diasTranscurridos = TiempoTranscurrido.ObtenerDiasTranscurridosEntreElMesAnteriorYActual(fechaInicial);
            }
            return diasTranscurridos;
        }

        /// <summary>
        /// Obtiene los dias transcurridos entre la fecha cumple-mes del mes anterior y la fecha actual.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial para obtener el cumple-mes.</param>
        /// <returns>Retorna los dias transcurridos entre la fecha cumple-mes del mes anterior y la fecha actual.</returns>
        private static int ObtenerDiasTranscurridosEntreElMesAnteriorYActual(DateTime fechaInicial)
        {
            int diasTranscurridos = 0;

            if (fechaInicial.Day != TiempoTranscurrido.fechaFinal.Day)
            {
                DateTime fechaMesAnterior = TiempoTranscurrido.ObtenerMesAnteriorAlActual();
                int contadorDiasDelMes = 0;
                int mesAnterior = fechaMesAnterior.Month;

                while (mesAnterior == fechaMesAnterior.Month)
                {
                    fechaMesAnterior = fechaMesAnterior.AddDays(1);
                    contadorDiasDelMes++;
                }
                diasTranscurridos = Math.Abs(fechaInicial.Day - contadorDiasDelMes) + TiempoTranscurrido.fechaFinal.Day;
            }
            return diasTranscurridos;
        }

        /// <summary>
        /// Obtiene una fecha correspondiente al 1ro del mes anterior al mes actual.
        /// </summary>
        /// <returns>Retorna una fecha que se corresponde al 1ro del mes anterior al mes actual.</returns>
        private static DateTime ObtenerMesAnteriorAlActual()
        {
            return TiempoTranscurrido.fechaFinal.Month == 0 ?
                new DateTime(TiempoTranscurrido.fechaFinal.Year - 1, 12, 1) :
                new DateTime(TiempoTranscurrido.fechaFinal.Year, TiempoTranscurrido.fechaFinal.Month - 1, 1);
        }

        /// <summary>
        /// Obtiene la cantidad de meses transcurridos el ultimo año hasta la fecha actual.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial para obtener el cumple-mes.</param>
        /// <returns>Retorna la cantidad de meses transcurridos el ultimo año hasta la fecha actual.</returns>
        private static int ObtenerMeses(DateTime fechaInicial)
        {
            int contadorMeses = 0;

            if (fechaInicial.Month != TiempoTranscurrido.fechaFinal.Month || fechaFinal.Day < fechaInicial.Day)
            {
                contadorMeses = TiempoTranscurrido.fechaFinal.Month - fechaInicial.Month;

                if (TiempoTranscurrido.fechaFinal.Year - fechaInicial.Year > 0)
                {
                    if (fechaInicial.Month >= TiempoTranscurrido.fechaFinal.Month)
                    {
                        contadorMeses = 12 - fechaInicial.Month;

                        contadorMeses += TiempoTranscurrido.fechaFinal.Month;
                    }

                    if (fechaInicial.Day > TiempoTranscurrido.fechaFinal.Day || contadorMeses == 12)
                    {
                        contadorMeses--;
                    }
                }
                else
                {
                    if (contadorMeses != 0 && fechaInicial.Day > TiempoTranscurrido.fechaFinal.Day)
                    {
                        contadorMeses--;
                    }
                }
            }
            return contadorMeses;
        }

        /// <summary>
        /// Obtiene la diferencia total, en dias, entre dos fechas.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial.</param>
        /// <returns>La diferencia total en dias, entre la fecha final y la fecha inicial.</returns>
        private static int ObtenerDiferenciaTotalEnDias(DateTime fechaInicial)
        {
            return (int)((TiempoTranscurrido.fechaFinal - fechaInicial).TotalDays);
        }

        #endregion
    }
}
