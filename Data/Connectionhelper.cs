using System;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace PaintControl.Data
{
    /// <summary>
    /// Helper para manejar reintentos automáticos de conexión a la base de datos.
    /// Cuando hay una caída momentánea de red entre las terminales y el servidor,
    /// esta clase reintenta la operación automáticamente antes de mostrar un error.
    /// 
    /// IMPORTANTE: Solo reintenta errores de RED/CONEXIÓN (transitorios).
    /// Errores de lógica (datos duplicados, FK inválida, etc.) NO se reintentan.
    /// </summary>
    public static class ConnectionHelper
    {
        // Número máximo de intentos antes de fallar
        private const int MAX_REINTENTOS = 3;

        // Milisegundos de espera entre cada reintento (va aumentando)
        private const int ESPERA_BASE_MS = 500;

        /// <summary>
        /// Ejecuta una operación de base de datos con reintentos automáticos.
        /// Solo reintenta si el error es de conexión/red (transitorio).
        /// </summary>
        public static T EjecutarConReintento<T>(Func<T> operacion)
        {
            Exception ultimaExcepcion = null;

            for (int intento = 1; intento <= MAX_REINTENTOS; intento++)
            {
                try
                {
                    return operacion();
                }
                catch (Exception ex)
                {
                    // Solo reintentar si es un error transitorio de red/conexión
                    if (!EsErrorTransitorio(ex))
                    {
                        // Error de lógica (datos, FK, etc.) - NO reintentar, lanzar inmediato
                        Debug.WriteLine($"[ConnectionHelper] Error NO transitorio: {ex.Message}");
                        throw;
                    }

                    ultimaExcepcion = ex;
                    Debug.WriteLine($"[ConnectionHelper] Intento {intento}/{MAX_REINTENTOS} falló (red): {ex.Message}");

                    if (intento < MAX_REINTENTOS)
                    {
                        // Espera progresiva: 500ms, 1000ms, 1500ms...
                        int espera = ESPERA_BASE_MS * intento;
                        Debug.WriteLine($"[ConnectionHelper] Reintentando en {espera}ms...");
                        Thread.Sleep(espera);
                    }
                }
            }

            // Todos los intentos fallaron por error de red
            Debug.WriteLine($"[ConnectionHelper] Reintentos agotados: {ultimaExcepcion?.Message}");
            throw new ConexionException(
                "No se pudo conectar con el servidor. Verifique que la computadora principal esté encendida y conectada a la red.",
                ultimaExcepcion);
        }

        /// <summary>
        /// Ejecuta una operación sin valor de retorno con reintentos automáticos.
        /// </summary>
        public static void EjecutarConReintento(Action operacion)
        {
            EjecutarConReintento(() =>
            {
                operacion();
                return true;
            });
        }

        /// <summary>
        /// Determina si una excepción es un error transitorio (de red/conexión)
        /// que vale la pena reintentar.
        /// Errores de lógica (FK, duplicados, etc.) retornan false.
        /// </summary>
        private static bool EsErrorTransitorio(Exception ex)
        {
            Exception actual = ex;
            while (actual != null)
            {
                // SqlException con números de error específicos de red
                if (actual is SqlException sqlEx)
                {
                    foreach (SqlError error in sqlEx.Errors)
                    {
                        switch (error.Number)
                        {
                            case -2:     // Timeout expirado
                            case 2:      // Timeout apertura de conexión
                            case 53:     // Servidor no encontrado
                            case 40:     // No se pudo abrir conexión
                            case 233:    // Conexión cerrada por servidor
                            case 10054:  // Conexión cerrada por host remoto
                            case 10053:  // Conexión abortada por software
                            case 10060:  // Timeout de conexión
                            case 11001:  // No se pudo resolver el host
                            case 10928:  // Límite de recursos (temporal)
                            case 10929:  // Servidor ocupado
                            case 40197:  // Error procesando solicitud
                            case 40501:  // Servicio ocupado
                            case 40613:  // Base de datos no disponible
                                return true;
                        }
                    }
                    return false;
                }

                if (actual is System.Net.Sockets.SocketException)
                    return true;

                if (actual is TimeoutException)
                    return true;

                if (actual is InvalidOperationException &&
                    actual.Message != null &&
                    (actual.Message.Contains("connection") ||
                     actual.Message.Contains("provider")))
                    return true;

                actual = actual.InnerException;
            }

            // Verificar mensaje como última opción
            string msg = ex.ToString().ToLowerInvariant();
            return msg.Contains("underlying provider failed") ||
                   msg.Contains("transport-level error") ||
                   msg.Contains("network-related") ||
                   msg.Contains("connection was forcibly closed");
        }
    }

    /// <summary>
    /// Excepción personalizada para problemas de conexión.
    /// </summary>
    public class ConexionException : Exception
    {
        public ConexionException(string mensaje, Exception inner)
            : base(mensaje, inner)
        {
        }
    }
}