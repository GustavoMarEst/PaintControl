using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;

namespace PaintControl.Data
{
    public class CatalogoService
    {
        // ========== TIPOS DE PINTURA ==========

        public List<TipoPintura> ObtenerTiposActivos()
        {
            using (var context = new DOALDbContext())
            {
                return context.TiposPintura
                    .AsNoTracking()
                    .Where(t => t.Activo)
                    .OrderBy(t => t.Nombre)
                    .ToList();
            }
        }

        public List<TipoPintura> ObtenerTodosTipos()
        {
            using (var context = new DOALDbContext())
            {
                return context.TiposPintura
                    .AsNoTracking()
                    .Include(t => t.Lineas)
                    .OrderBy(t => t.Nombre)
                    .ToList();
            }
        }

        public bool AgregarTipo(TipoPintura tipo)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    context.TiposPintura.Add(tipo);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar tipo: {ex.Message}");
                return false;
            }
        }

        public bool ActualizarTipo(TipoPintura tipo)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var existente = context.TiposPintura.Find(tipo.Id);
                    if (existente == null) return false;

                    existente.Nombre = tipo.Nombre;
                    existente.Abreviatura = tipo.Abreviatura;
                    existente.Activo = tipo.Activo;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar tipo: {ex.Message}");
                return false;
            }
        }

        public bool EliminarTipo(int id)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var tipo = context.TiposPintura
                        .Include(t => t.Lineas.Select(l => l.Acabados))
                        .FirstOrDefault(t => t.Id == id);
                    if (tipo == null) return false;

                    // Eliminar en cascada: acabados -> líneas -> tipo
                    foreach (var linea in tipo.Lineas.ToList())
                    {
                        foreach (var acabado in linea.Acabados.ToList())
                        {
                            context.AcabadosPintura.Remove(acabado);
                        }
                        context.LineasPintura.Remove(linea);
                    }
                    context.TiposPintura.Remove(tipo);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar tipo: {ex.Message}");
                return false;
            }
        }

        // ========== LÍNEAS DE PINTURA ==========

        public List<LineaPintura> ObtenerLineasPorTipo(int tipoPinturaId)
        {
            using (var context = new DOALDbContext())
            {
                return context.LineasPintura
                    .AsNoTracking()
                    .Where(l => l.TipoPinturaId == tipoPinturaId && l.Activo)
                    .OrderBy(l => l.Nombre)
                    .ToList();
            }
        }

        public List<LineaPintura> ObtenerTodasLineasPorTipo(int tipoPinturaId)
        {
            using (var context = new DOALDbContext())
            {
                return context.LineasPintura
                    .AsNoTracking()
                    .Include(l => l.Acabados)
                    .Where(l => l.TipoPinturaId == tipoPinturaId)
                    .OrderBy(l => l.Nombre)
                    .ToList();
            }
        }

        public bool AgregarLinea(LineaPintura linea)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    context.LineasPintura.Add(linea);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar línea: {ex.Message}");
                return false;
            }
        }

        public bool ActualizarLinea(LineaPintura linea)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var existente = context.LineasPintura.Find(linea.Id);
                    if (existente == null) return false;

                    existente.Nombre = linea.Nombre;
                    existente.Abreviatura = linea.Abreviatura;
                    existente.Activo = linea.Activo;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar línea: {ex.Message}");
                return false;
            }
        }

        public bool EliminarLinea(int id)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var linea = context.LineasPintura
                        .Include(l => l.Acabados)
                        .FirstOrDefault(l => l.Id == id);
                    if (linea == null) return false;

                    foreach (var acabado in linea.Acabados.ToList())
                    {
                        context.AcabadosPintura.Remove(acabado);
                    }
                    context.LineasPintura.Remove(linea);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar línea: {ex.Message}");
                return false;
            }
        }

        // ========== ACABADOS DE PINTURA ==========

        public List<AcabadoPintura> ObtenerAcabadosPorLinea(int lineaPinturaId)
        {
            using (var context = new DOALDbContext())
            {
                return context.AcabadosPintura
                    .AsNoTracking()
                    .Where(a => a.LineaPinturaId == lineaPinturaId && a.Activo)
                    .OrderBy(a => a.Nombre)
                    .ToList();
            }
        }

        public List<AcabadoPintura> ObtenerTodosAcabadosPorLinea(int lineaPinturaId)
        {
            using (var context = new DOALDbContext())
            {
                return context.AcabadosPintura
                    .AsNoTracking()
                    .Where(a => a.LineaPinturaId == lineaPinturaId)
                    .OrderBy(a => a.Nombre)
                    .ToList();
            }
        }

        public bool AgregarAcabado(AcabadoPintura acabado)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    context.AcabadosPintura.Add(acabado);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar acabado: {ex.Message}");
                return false;
            }
        }

        public bool ActualizarAcabado(AcabadoPintura acabado)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var existente = context.AcabadosPintura.Find(acabado.Id);
                    if (existente == null) return false;

                    existente.Nombre = acabado.Nombre;
                    existente.Activo = acabado.Activo;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar acabado: {ex.Message}");
                return false;
            }
        }

        public bool EliminarAcabado(int id)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var acabado = context.AcabadosPintura.Find(id);
                    if (acabado == null) return false;

                    context.AcabadosPintura.Remove(acabado);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar acabado: {ex.Message}");
                return false;
            }
        }

        // ========== UTILIDADES ==========

        /// <summary>
        /// Genera una abreviatura automática a partir del nombre.
        /// Toma las primeras letras de cada palabra (máximo 3 caracteres).
        /// Si es una sola palabra, toma las primeras 3 letras.
        /// </summary>
        public string GenerarAbreviatura(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "";

            string[] palabras = nombre.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (palabras.Length == 1)
            {
                // Una sola palabra: tomar las primeras 3 letras
                return nombre.Trim().Substring(0, Math.Min(3, nombre.Trim().Length)).ToUpper();
            }

            // Múltiples palabras: tomar la primera letra de cada una
            string abr = "";
            foreach (var palabra in palabras)
            {
                if (!string.IsNullOrEmpty(palabra))
                {
                    abr += palabra[0];
                }
            }

            // Limitar a 5 caracteres máximo
            if (abr.Length > 5)
                abr = abr.Substring(0, 5);

            return abr.ToUpper();
        }

        /// <summary>
        /// Construye la descripción estándar: "ABR_TIPO. ABR_LINEA. ACABADO"
        /// </summary>
        public string ConstruirDescripcion(string abreviaturaTipo, string abreviaturaLinea, string acabado)
        {
            var partes = new List<string>();

            if (!string.IsNullOrWhiteSpace(abreviaturaTipo))
                partes.Add(abreviaturaTipo.Trim());

            if (!string.IsNullOrWhiteSpace(abreviaturaLinea))
                partes.Add(abreviaturaLinea.Trim());

            if (!string.IsNullOrWhiteSpace(acabado))
                partes.Add(acabado.Trim().ToUpper());

            return string.Join(". ", partes);
        }
    }
}