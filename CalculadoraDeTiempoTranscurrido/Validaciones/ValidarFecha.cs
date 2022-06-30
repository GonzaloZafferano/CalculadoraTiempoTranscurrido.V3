using System;

namespace Validaciones
{
    public class ValidarFecha
    {
        /// <summary>
        /// Obtiene un dia valido (1-31), a partir de un string con numeros recibido por parametro.
        /// </summary>
        /// <param name="dia">String con el numero a evaluar.</param>
        /// <returns>Un dia valido, o string.Empty si el string recibido por parametro no es numerico.</returns>
        public static string ObtenerDiaValido(string dia)
        {
            if(!string.IsNullOrWhiteSpace(dia) && int.TryParse(dia, out int diaNumerico) && diaNumerico > 0)
            {
                if (diaNumerico <= 31)
                {
                    return diaNumerico.ToString();
                }
                return ValidarFecha.ObtenerDiaValido(diaNumerico.ToString().Substring(0, diaNumerico.ToString().Length - 1));
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene un mes valido (1-12), a partir de un string con numeros recibido por parametro.
        /// </summary>
        /// <param name="mes">String con el numero a evaluar.</param>
        /// <returns>Un mes valido, o string.Empty si el string recibido por parametro no es numerico.</returns>
        public static string ObtenerMesValido(string mes)
        {
            if (!string.IsNullOrWhiteSpace(mes) && int.TryParse(mes, out int mesNumerico) && mesNumerico > 0)
            {
                if (mesNumerico <= 12)
                {
                    return mesNumerico.ToString();
                }
                return ValidarFecha.ObtenerMesValido(mesNumerico.ToString().Substring(0, mesNumerico.ToString().Length - 1));
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene un año valido, a partir de un string con numeros recibido por parametro.
        /// </summary>
        /// <param name="anio">String con el numero a evaluar.</param>
        /// <returns>Un año valido, o string.Empty si el string recibido por parametro no es numerico.</returns>
        public static string ObtenerAnioValido(string anio)
        {
            if (!string.IsNullOrWhiteSpace(anio) && int.TryParse(anio, out int anioNumerico) && anioNumerico > 0)
            {
                if (anioNumerico <= 9999)
                {
                    return anioNumerico.ToString();
                }
                return ValidarFecha.ObtenerAnioValido(anioNumerico.ToString().Substring(0, anioNumerico.ToString().Length - 1));
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
