using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Logic
{
    public class CartLogic : BaseLogic, ICRUD<Cart, int>
    {
        public Cart Get(int id)
        {
            return _context.Cart.Find(id);
        }

        public List<Cart> GetAll()
        {
            return _context.Cart.ToList();
        }

        public bool Finded(int id)
        {
            return _context.Cart.Any(c => c.Id == id);
        }

        public void Add(Cart cart)
        {
            try
            {
                if (cart == null) throw new ArgumentNullException(nameof(cart), "El carrito no puede ser nulo.");
                _context.Cart.Add(cart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar el carrito. " + ex.Message, ex);
            }
        }

        public void Update(Cart cart)
        {
            try
            {
                if (cart == null) throw new ArgumentNullException(nameof(cart), "El carrito no puede ser nulo.");
                var existingCart = _context.Cart.Find(cart.Id) ?? throw new ArgumentException("El carrito no existe en la base de datos.", nameof(cart.Id));
                _context.Entry(existingCart).CurrentValues.SetValues(cart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar el carrito. " + ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var cart = _context.Cart.Find(id);
                if (cart != null)
                {
                    _context.Cart.Remove(cart);
                    _context.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("El carrito no existe en la base de datos.", nameof(id));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar eliminar el carrito. " + ex.Message, ex);
            }
        }
    }
}
