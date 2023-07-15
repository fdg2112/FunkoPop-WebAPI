using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class SaleLogic: BaseLogic
    {
        public Sale Get(int id, string saleStatus, string saleType)
        {
            var sale = _context.Sale
                .FirstOrDefault(s => s.Id == id && s.Sale_status == saleStatus && s.Sale_type == saleType && s.Active);

            return sale ?? throw new Exception("Sale not found");
        }

        public List<Sale> GetList(User user)
        {
            var sales = _context.Sale
                .Where(s => s.Active && (user.Role != "Customer" || s.User_id == user.Id))
                .OrderByDescending(s => s.Operation_date)
                .ToList();
            return sales;
        }

        public List<Sale> GetListAdmin()
        {
            var sales = _context.Sale
                .Where(s => s.Active)
                .OrderByDescending(s => s.Operation_date)
                .ToList();
            return sales;
        }

        public Sale Create(int paymentId, int addressId, string saleStatus, User user)
        {
            var current_user = _context.User.FirstOrDefault(u => u.Id == user.Id) ?? throw new Exception("El usuario actual no existe.");
            var lineCart = current_user.Cart.FirstOrDefault()?.Line_cart;
            if (lineCart == null || !lineCart.Any())
            {
                throw new Exception("No hay productos en el carrito de compras.");
            }

            var address = _context.Address.FirstOrDefault(a => a.Id == addressId && a.User_id == user.Id) ?? throw new Exception("La dirección especificada no existe o no pertenece al usuario actual.");
            DateTime? paymentDate = saleStatus == "Success" ? DateTime.Now : (DateTime?)null;
            var sale = new Sale
            {
                Operation_date = DateTime.Now,
                Payment_date = paymentDate,
                Sale_type = saleStatus,
                Address_id = addressId,
                User_id = user.Id,
                Active = true,
                Payment_id = paymentId,
                Payment_total = CalculatePaymentTotal(lineCart.ToList()),
                Sale_status = saleStatus
            };

            _context.Sale.Add(sale);
            _context.SaveChanges();

            // Eliminar productos del carrito
            _context.Line_cart.RemoveRange(lineCart);
            _context.SaveChanges();

            // Restar stock de productos
            foreach (var line in lineCart)
            {
                var product = _context.Product.FirstOrDefault(p => p.Id == line.Product_id);
                if (product != null) product.Stock -= (int)line.Quantity;
            }
            _context.SaveChanges();

            return sale;
        }

        private double CalculatePaymentTotal(List<Line_cart> lineCart)
        {
            double? paymentTotal = 0;

            foreach (var line in lineCart)
            {
                paymentTotal += line.Total;
            }

            return (double)paymentTotal;
        }

        public Sale Reanalyze(int id, string newSaleStatus)
        {
            if (newSaleStatus != "Success" && newSaleStatus != "Failure" && newSaleStatus != "Pending")
            {
                throw new Exception("Invalid new sale status");
            }

            var currentSale = _context.Sale.FirstOrDefault(s => s.Id == id && s.Sale_status == "Pending") ?? throw new Exception("Sale not found or not in pending status");
            Sale sale = currentSale;

            if (newSaleStatus == "Success")
            {
                // Actualizar el estado de la venta y otras propiedades...
            }
            else if (newSaleStatus == "Failure")
            {
                // Lógica para manejar el estado de venta "Failure"...
            }

            return sale;
        }

        public List<Sale_product> SetSaleProducts(List<Line_cart> lineCart)
        {
            var saleProducts = new List<Sale_product>();

            foreach (var line in lineCart)
            {
                var saleProduct = new Sale_product
                {
                    Quantity = (int)line.Quantity,
                    Product_id = line.Product_id,
                    Unitary_total = line.Product.Price
                };

                saleProducts.Add(saleProduct);
            }

            return saleProducts;
        }

    }
}
