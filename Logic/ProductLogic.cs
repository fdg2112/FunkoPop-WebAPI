using Data;
using Entities;
using Logic.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Logic
{
    public class ProductLogic : BaseLogic, ICRUD<Product, int>
    {
        public ProductLogic() { }

        public Product Get(int id)
        {
            try
            {
                return _context.Product.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto.", ex);
            }
        }

        public List<Product> GetAll()
        {
            try
            {
                return _context.Product.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de productos.", ex);
            }
        }

        public List<Product> GetList(FilterParams filterParams)
        {
            try
            {
                IQueryable<Product> query = _context.Product.Where(p => p.Active && p.Stock > 0);

                if (filterParams != null)
                {
                    if (!string.IsNullOrEmpty(filterParams.Searched))
                    {
                        string searchLower = filterParams.Searched.ToLower();
                        query = query.Where(p => p.Name.ToLower().Contains(searchLower) || p.Description.ToLower().Contains(searchLower));
                    }

                    if (filterParams.Shine.HasValue)
                    {
                        query = query.Where(p => p.Shine == filterParams.Shine.Value);
                    }

                    if (filterParams.OrderBy != null && !string.IsNullOrEmpty(filterParams.OrderBy.Field))
                    {
                        string orderByField = filterParams.OrderBy.Field;
                        bool isAscending = filterParams.OrderBy.Direction == OrderDirection.Ascending;

                        switch (orderByField)
                        {
                            case "name":
                                query = isAscending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                                break;
                            case "price":
                                query = isAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                                break;
                            // Agrega más casos para otros campos de ordenación si es necesario
                            default:
                                // Ordenación por defecto si el campo no coincide con ninguno esperado
                                query = query.OrderBy(p => p.Id);
                                break;
                        }
                    }
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de productos.", ex);
            }
        }

        public List<Product> GetListAdmin(FilterParams filterParams)
        {
            try
            {
                IQueryable<Product> query = _context.Product.Where(p => p.Active);

                // Filtrar por término de búsqueda
                if (!string.IsNullOrEmpty(filterParams.Searched))
                {
                    query = query.Where(p => p.Name.Contains(filterParams.Searched) || p.Description.Contains(filterParams.Searched));
                }

                // Filtrar por brillo
                if (filterParams.Shine.HasValue)
                {
                    query = query.Where(p => p.Shine == filterParams.Shine.Value);
                }

                // Ordenar
                var orderByParams = filterParams.OrderBy ?? new OrderByParams();
                switch (orderByParams.Field)
                {
                    case "name":
                        if (orderByParams.Direction == OrderDirection.Descending)
                        {
                            query = query.OrderByDescending(p => p.Name);
                        }
                        else
                        {
                            query = query.OrderBy(p => p.Name);
                        }
                        break;
                    default:
                        query = query.OrderBy(p => p.Id); // Orden por defecto
                        break;
                }


                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de productos para el administrador.", ex);
            }
        }
 
        public bool Finded(int id)
        {
            if (_context.Product.Find(id) != null)
                return true;
            else
                return false;
        }

        public void Add(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo.");
                if (string.IsNullOrEmpty(product.Name))
                    throw new ArgumentException("El campo Nombre del producto no puede estar vacío.", nameof(product.Name));
                if (string.IsNullOrEmpty(product.Description))
                    throw new ArgumentException("El campo Descripción del producto no puede estar vacío.", nameof(product.Description));
                if (product.Name.Length > 30)
                    throw new ArgumentException("El límite del campo Nombre del producto es de 30 caracteres.", nameof(product.Name));
                if (product.Description.Length > 999)
                    throw new ArgumentException("El límite del campo Descripción del producto es de 999 caracteres.", nameof(product.Description));
                if (product.Price < 0)
                    throw new ArgumentException("El precio del producto no puede ser negativo.", nameof(product.Price));
                if (product.Stock < 0)
                    throw new ArgumentException("El stock del producto no puede ser negativo.", nameof(product.Stock));
                if (string.IsNullOrEmpty(product.Ref_image))
                    throw new ArgumentException("El campo Ref_image del producto no puede estar vacío.", nameof(product.Ref_image));
                _context.Product.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar el producto. " + ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var elementToRemove = _context.Product.Find(id) ?? throw new Exception($"El producto con ID {id} no existe.");
                _context.Product.Remove(elementToRemove);
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
                        throw new Exception($"No se puede eliminar el producto porque está relacionado con la tabla {tableName}.");
                    }
                }
                throw new Exception("No se ha podido eliminar el producto.");
            }
            catch (Exception ex)
            {
                throw new Exception($"No se ha podido eliminar el producto. {ex.Message}");
            }
        }

        public void Update(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo.");
                var existingProduct = _context.Product.FirstOrDefault(p => p.Id == product.Id) ?? throw new ArgumentException("El producto no existe en la base de datos.", nameof(product.Id));
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.Shine = product.Shine;
                existingProduct.Collection_id = product.Collection_id;
                existingProduct.Active = product.Active;
                existingProduct.Url_image = product.Url_image;
                existingProduct.Ref_image = product.Ref_image;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar el producto. " + ex.Message, ex);
            }
        }

        public Product Reactivate(int id)
        {
            var product = _context.Product.Find(id);
            if (product != null && product.Stock > 0)
            {
                product.Active = true;
                _context.SaveChanges();
                return product;
            }
            else
            {
                throw new Exception("No se puede reactivar el producto.");
            }
        }

        public void DiscountStock(int productId, int quantity)
        {
            try
            {
                var product = _context.Product.Find(productId);
                if (product != null)
                {
                    if (product.Stock >= quantity)
                    {
                        product.Stock -= quantity;
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("No hay suficiente stock disponible.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al descontar el stock del producto. " + ex.Message, ex);
            }
        }

        public void AddStock(int productId, int quantity)
        {
            try
            {
                var product = _context.Product.Find(productId);
                if (product != null)
                {
                    product.Stock += quantity;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al reponer el stock del producto. " + ex.Message, ex);
            }
        }

        public void LinkImageWithProduct(int productId, string imageUrl)
        {
            try
            {
                var product = _context.Product.Find(productId);
                if (product != null)
                {
                    product.Url_image = imageUrl;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al vincular la imagen con el producto. " + ex.Message, ex);
            }
        }



    }
}
