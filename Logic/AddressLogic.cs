using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Logic
{
    public class AddressLogic : BaseLogic, ICRUD<Address, int>
    {
        public AddressLogic() { }

        public Address Get(int id)
        {
            try
            {
                return _context.Address.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la dirección.", ex);
            }
        }

        public List<Address> GetAll()
        {
            try
            {
                return _context.Address.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de direcciones.", ex);
            }
        }

        public bool Finded(int id)
        {
            return _context.Address.Find(id) != null;
        }

        public void Add(Address address)
        {
            try
            {
                if (address == null)
                    throw new ArgumentNullException(nameof(address), "La dirección no puede ser nula.");
                if (string.IsNullOrEmpty(address.Street_name))
                    throw new ArgumentException("El campo Nombre de la calle de la dirección no puede estar vacío.", nameof(address.Street_name));
                if (address.Street_number <= 0)
                    throw new ArgumentException("El número de calle de la dirección debe ser mayor que cero.", nameof(address.Street_number));
                _context.Address.Add(address);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar la dirección. " + ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var elementToRemove = _context.Address.Find(id) ?? throw new Exception($"La dirección con ID {id} no existe.");
                _context.Address.Remove(elementToRemove);
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
                        throw new Exception($"No se puede eliminar la dirección porque está relacionada con la tabla {tableName}.");
                    }
                }
                throw new Exception("No se ha podido eliminar la dirección.");
            }
            catch (Exception ex)
            {
                throw new Exception($"No se ha podido eliminar la dirección. {ex.Message}");
            }
        }

        public void Update(Address address)
        {
            try
            {
                if (address == null) throw new ArgumentNullException(nameof(address), "La dirección no puede ser nula.");
                var existingAddress = _context.Address.FirstOrDefault(a => a.Id == address.Id) ?? throw new ArgumentException("La dirección no existe en la base de datos.", nameof(address.Id));
                existingAddress.Street_name = address.Street_name;
                existingAddress.Street_number = address.Street_number;
                existingAddress.Floor = address.Floor;
                existingAddress.Department = address.Department;
                existingAddress.Location_id = address.Location_id;
                existingAddress.User_id = address.User_id;
                existingAddress.Active = address.Active;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar la dirección. " + ex.Message, ex);
            }
        }
    }
}
