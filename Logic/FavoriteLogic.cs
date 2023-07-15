using Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    internal class FavoriteLogic : BaseLogic
    {
        public FavoriteLogic() { }

        public List<Favorite> GetList(User user)
        {
            try
            {
                return _context.Favorite.Where(f => f.User_id == user.Id).OrderByDescending(f => f.Id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de favoritos.", ex);
            }
        }

        public Favorite Create(int product_id, User user)
        {
            var currentProduct = _context.Product.FirstOrDefault(p => p.Id == product_id && p.Active) ?? throw new Exception("Product not found.");
            var favorite = new Favorite
            {
                Product_id = product_id,
                User_id = user.Id
            };

            _context.Favorite.Add(favorite);
            _context.SaveChanges();

            return favorite;
        }

        public Favorite AddToCart(int id)
        {
            var currentFavorite = _context.Favorite.FirstOrDefault(f => f.Id == id) ?? throw new Exception("Favorite not found.");
            var userCart = _context.Cart.FirstOrDefault(c => c.User_id == currentFavorite.User_id) ?? throw new Exception("User cart not found.");
            var lineCart = new Line_cart
            {
                Quantity = 1,
                Total = currentFavorite.Product.Price,
                Cart_id = userCart.Id,
                Product_id = currentFavorite.Product_id
            };
            _context.Line_cart.Add(lineCart);
            _context.Favorite.Remove(currentFavorite);
            _context.SaveChanges();

            return currentFavorite;
        }

        public Favorite Delete(int id)
        {
            var currentFavorite = _context.Favorite.FirstOrDefault(f => f.Id == id) ?? throw new Exception("Favorite not found.");
            _context.Favorite.Remove(currentFavorite);
            _context.SaveChanges();
            return currentFavorite;
        }
    }
}
