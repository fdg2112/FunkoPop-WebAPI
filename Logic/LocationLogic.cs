using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Logic
{
    public class LocationLogic : BaseLogic, ICRUD<Location, int>
    {
        public LocationLogic() { }
        public Location Get(int id)
        {
            try
            {
                return _context.Location.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la ubicacion.", ex);
            }
        }
        public List<Location> GetAll()
        {
            try
            {
                return _context.Location.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de ubicaciones.", ex);
            }
        }
        public bool Finded(int id)
        {
            if (_context.Location.Find(id) != null)
                return true;
            else
                return false;
        }
        public void Add(Location location)
        {
            try
            {
                if (location == null)
                    throw new ArgumentNullException(nameof(location), "La ubicación no puede ser nula.");
                if (string.IsNullOrEmpty(location.Postal_code))
                    throw new ArgumentException("El campo Código Postal de la ubicación no puede estar vacío.", nameof(location.Postal_code));
                if (string.IsNullOrEmpty(location.Name))
                    throw new ArgumentException("El campo Nombre de la ubicación no puede estar vacío.", nameof(location.Name));
                if (location.Postal_code.Length > 8)
                    throw new ArgumentException("El límite del campo Código Postal de la ubicación es de 8 caracteres.", nameof(location.Postal_code));
                if (location.Name.Length > 50)
                    throw new ArgumentException("El límite del campo Nombre de la ubicación es de 50 caracteres.", nameof(location.Name));
                _context.Location.Add(location);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar la ubicación. " + ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var elementToRemove = _context.Location.Find(id) ?? throw new Exception($"La ubicación con ID {id} no existe.");
                _context.Location.Remove(elementToRemove);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException.InnerException;
                if (innerException != null && innerException.Message.Contains("FK_"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(innerException.Message, @"(?<=tabla ')([^']*)");
                    if (match.Success)
                    {
                        string tableName = match.Value;
                        throw new Exception($"No se puede eliminar la ubicación porque está relacionada con la tabla {tableName}.");
                    }
                }
                throw new Exception("No se ha podido eliminar la ubicación.");
            }
            catch (Exception ex)
            {
                throw new Exception($"No se ha podido eliminar la ubicación. {ex.Message}");
            }
        }

        public void Update(Location location)
        {
            try
            {
                if (location == null)
                    throw new ArgumentNullException(nameof(location), "La ubicación no puede ser nula.");
                var existingLocation = _context.Location.FirstOrDefault(l => l.Id == location.Id) ?? throw new ArgumentException("La ubicación no existe en la base de datos.", nameof(location.Id));
                existingLocation.Postal_code = location.Postal_code;
                existingLocation.Name = location.Name;
                existingLocation.Province_id = location.Province_id;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar la ubicación. " + ex.Message, ex);
            }
        }
    }
}
