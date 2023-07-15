using Data;
using System.Data.Entity;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Logic
{
    public class LineCartLogic : BaseLogic, ICRUD<Line_cart, int>
    {
        public Line_cart Get(int id)
        {
            return _context.Line_cart.Find(id);
        }

        public List<Line_cart> GetAll()
        {
            return _context.Line_cart.ToList();
        }

        public bool Finded(int id)
        {
            return _context.Line_cart.Any(lc => lc.Id == id);
        }

        public void Add(Line_cart lineCart)
        {
            try
            {
                if (lineCart == null) throw new ArgumentNullException(nameof(lineCart), "La línea de carrito no puede ser nula.");
                _context.Line_cart.Add(lineCart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar la línea de carrito. " + ex.Message, ex);
            }
        }

        public void Update(Line_cart lineCart)
        {
            try
            {
                if (lineCart == null)
                    throw new ArgumentNullException(nameof(lineCart), "El objeto Line_cart no puede ser nulo.");
                var existingLineCart = _context.Line_cart.Find(lineCart.Id) ?? throw new ArgumentException("La línea de carrito no existe en la base de datos.", nameof(lineCart.Id));
                _context.Entry(existingLineCart).CurrentValues.SetValues(lineCart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar la línea de carrito. " + ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var lineCart = _context.Line_cart.Find(id);
                if (lineCart != null)
                {
                    _context.Line_cart.Remove(lineCart);
                    _context.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("La línea de carrito no existe en la base de datos.", nameof(id));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar eliminar la línea de carrito. " + ex.Message, ex);
            }
        }

        public Line_cart UpdateLineCart(int productId, int quantity, User user)
        {
            try
            {
                var product = _context.Product.SingleOrDefault(p => p.Id == productId) ?? throw new ArgumentException("Product not found.", nameof(productId));
                var cart = _context.Cart.Include(c => c.Line_cart).SingleOrDefault(c => c.User_id == user.Id) ?? throw new ArgumentException("Cart not found.", nameof(user.Id));
                var lineCart = cart.Line_cart.FirstOrDefault(lc => lc.Product_id == productId);
                if (lineCart != null)
                {
                    var newQuantity = lineCart.Quantity + quantity;
                    if (newQuantity > product.Stock)
                    {
                        throw new ArgumentException("Exceeded stock limit.", nameof(quantity));
                    }

                    lineCart.Quantity = newQuantity;
                    lineCart.Total = newQuantity * product.Price;
                }
                else
                {
                    if (quantity > product.Stock)
                    {
                        throw new ArgumentException("Exceeded stock limit.", nameof(quantity));
                    }

                    lineCart = new Line_cart
                    {
                        Quantity = quantity,
                        Total = quantity * product.Price,
                        Cart_id = cart.Id,
                        Product_id = productId
                    };

                    cart.Line_cart.Add(lineCart);
                }

                _context.SaveChanges();

                return lineCart;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Product not found.", nameof(productId));
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating or inserting line cart.", ex);
            }
        }

        public Line_cart UpdateQuantityOfProduct(int productId, int quantity, User user)
        {
            try
            {
                var product = _context.Product.SingleOrDefault(p => p.Id == productId) ?? throw new ArgumentException("Product not found.", nameof(productId));
                var cart = _context.Cart.SingleOrDefault(c => c.User_id == user.Id) ?? throw new ArgumentException("Cart not found.");
                var lineCartWithThisProduct = cart.Line_cart.SingleOrDefault(lc => lc.Product_id == productId) ?? throw new ArgumentException("Product in cart not found.");
                if (quantity > product.Stock)
                {
                    throw new ArgumentException("Exceeded stock limit.");
                }

                lineCartWithThisProduct.Quantity = quantity;
                lineCartWithThisProduct.Total = quantity * product.Price;

                _context.SaveChanges();

                return lineCartWithThisProduct;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product quantity in cart.", ex);
            }
        }

        public Line_cart DeleteProduct(int id, User user)
        {
            try
            {
                var cart = _context.Cart.SingleOrDefault(c => c.User_id == user.Id) ?? throw new ArgumentException("Cart not found.");
                var lineCartWithThisProduct = cart.Line_cart.SingleOrDefault(lc => lc.Id == id) ?? throw new ArgumentException("Product in cart not found.");

                _context.Line_cart.Remove(lineCartWithThisProduct);
                _context.SaveChanges();

                return lineCartWithThisProduct;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product from cart.", ex);
            }
        }

    }
}