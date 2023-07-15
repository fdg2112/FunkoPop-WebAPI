using Entities;
using Logic.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Logic
{
    public class CollectionLogic : BaseLogic, ICRUD<Collection, int>
    {
        public CollectionLogic() { }

        public Collection Get(int id)
        {
            try
            {
                return _context.Collection.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la dirección.", ex);
            }
        }

        public List<Collection> GetAll()
        {
            try
            {
                return _context.Collection.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de colecciones.", ex);
            }
        }

        public List<Collection> GetList(FilterParams filterParams)
        {
            try
            {
                IQueryable<Collection> query = _context.Collection;

                // Filtrar por término de búsqueda
                if (!string.IsNullOrEmpty(filterParams.Searched))
                {
                    query = query.Where(c => c.Name.Contains(filterParams.Searched) || c.Description.Contains(filterParams.Searched));
                }

                // Ordenar
                OrderByParams orderByParams = filterParams.OrderBy ?? new OrderByParams { Field = "id", Direction = OrderDirection.Ascending };
                switch (orderByParams.Field)
                {
                    case "name":
                        if (orderByParams.Direction == OrderDirection.Descending)
                        {
                            query = query.OrderByDescending(c => c.Name);
                        }
                        else
                        {
                            query = query.OrderBy(c => c.Name);
                        }
                        break;
                    default:
                        query = query.OrderBy(c => c.Id); // Orden por defecto
                        break;
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de colecciones.", ex);
            }
        }

        public List<Collection> GetListAdmin(FilterParams filterParams)
        {
            try
            {
                IQueryable<Collection> query = _context.Collection;

                // Filtrar por término de búsqueda
                if (!string.IsNullOrEmpty(filterParams.Searched))
                {
                    query = query.Where(c => c.Name.Contains(filterParams.Searched) || c.Description.Contains(filterParams.Searched));
                }

                // Ordenar
                OrderByParams orderByParams = filterParams.OrderBy  ?? new OrderByParams { Field = "id", Direction = OrderDirection.Ascending };
                switch (orderByParams.Field)
                {
                    case "name":
                        if (orderByParams.Direction == OrderDirection.Descending)
                        {
                            query = query.OrderByDescending(c => c.Name);
                        }
                        else
                        {
                            query = query.OrderBy(c => c.Name);
                        }
                        break;
                    default:
                        query = query.OrderBy(c => c.Id); // Orden por defecto
                        break;
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de colecciones para el administrador.", ex);
            }
        }

        public bool Finded(int id)
        {
            if (_context.Collection.Find(id) != null)
                return true;
            else
                return false;
        }

        public void Add(Collection collection)
        {
            try
            {
                if (collection == null)
                    throw new ArgumentNullException(nameof(collection), "La colección no puede ser nula.");
                if (string.IsNullOrEmpty(collection.Name))
                    throw new ArgumentException("El campo Nombre de la colección no puede estar vacío.", nameof(collection.Name));
                if (string.IsNullOrEmpty(collection.Description))
                    throw new ArgumentException("El campo Descripción de la colección no puede estar vacío.", nameof(collection.Description));
                if (collection.Name.Length > 50)
                    throw new ArgumentException("El límite del campo Nombre de la colección es de 50 caracteres.", nameof(collection.Name));
                if (collection.Description.Length > 999)
                    throw new ArgumentException("El límite del campo Descripción de la colección es de 999 caracteres.", nameof(collection.Description));
                _context.Collection.Add(collection);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar la colección. " + ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var elementToRemove = _context.Collection.Find(id) ?? throw new Exception($"La colección con ID {id} no existe.");
                _context.Collection.Remove(elementToRemove);
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
                        throw new Exception($"No se puede eliminar la colección porque está relacionada con la tabla {tableName}.");
                    }
                }
                throw new Exception("No se ha podido eliminar la colección.");
            }
            catch (Exception ex)
            {
                throw new Exception($"No se ha podido eliminar la colección. {ex.Message}");
            }
        }

        public void Update(Collection collection)
        {
            try
            {
                if (collection == null)
                    throw new ArgumentNullException(nameof(collection), "La colección no puede ser nula.");
                var existingCollection = _context.Collection.FirstOrDefault(c => c.Id == collection.Id) ?? throw new ArgumentException("La colección no existe en la base de datos.", nameof(collection.Id));
                existingCollection.Name = collection.Name;
                existingCollection.Description = collection.Description;
                existingCollection.Active = collection.Active;
                existingCollection.Url_image = collection.Url_image;
                existingCollection.Ref_image = collection.Ref_image;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar la colección. " + ex.Message, ex);
            }
        }

        public void Reactivate(int id)
        {
            try
            {
                var collectionToReactivate = _context.Collection.FirstOrDefault(collection => collection.Id == id && !collection.Active) ?? throw new Exception($"No se encontró la colección inactiva con ID {id}.");
                collectionToReactivate.Active = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo reactivar la colección con ID {id}.", ex);
            }
        }

        public void AddCollectionToCart(int collectionId, User user)
        {
            try
            {
                Collection collectionToAdd = _context.Collection
                    .Include(c => c.Product)
                    .FirstOrDefault(c => c.Id == collectionId) ?? throw new Exception($"No se encontró la colección con ID {collectionId}.");
                if (collectionToAdd.Product.Count == 0)
                {
                    throw new Exception("La colección no tiene productos.");
                }

                Cart userCart = _context.Cart.FirstOrDefault(cart => cart.User_id == user.Id && cart.Active);

                if (userCart == null)
                {
                    userCart = new Cart
                    {
                        User_id = user.Id,
                        Active = true
                    };

                    _context.Cart.Add(userCart);
                }

                foreach (Product product in collectionToAdd.Product)
                {
                    Line_cart lineCart = new Line_cart
                    {
                        Product_id = product.Id,
                        Quantity = 1,
                        Cart_id = userCart.Id,
                        Active = true
                    };

                    _context.Line_cart.Add(lineCart);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la colección al carrito de compras.", ex);
            }
        }

        public void LinkImageWithCollection(int collectionId, string imageUrl, string refImage)
        {
            try
            {
                Collection collection = _context.Collection.Find(collectionId);
                if (collection != null)
                {
                    collection.Url_image = imageUrl;
                    collection.Ref_image = refImage;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al vincular la imagen con la colección. " + ex.Message, ex);
            }
        }

        public void RemoveRelationProductAndCollection(List<int> productIds)
        {
            try
            {
                List<Product> products = _context.Product.Where(p => productIds.Contains(p.Id)).ToList();
                foreach (Product product in products)
                {
                    product.Collection_id = null;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            { 
                throw new Exception("Ha ocurrido un error al eliminar la relación entre productos y colecciones. " + ex.Message, ex);
            }
        }

    }
}
