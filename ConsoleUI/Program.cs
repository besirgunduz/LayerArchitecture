using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProductTest();

            ProductManager productManager = new ProductManager(new EfProductDal());

            var result = productManager.GetProductDetails();
            if (result.Success == true)
            {
                foreach (var product in productManager.GetProductDetails().Data)
                {
                    Console.WriteLine(product.ProductName + " --- " + product.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }


            Console.ReadKey();
        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal());

            foreach (var product in productManager.GetByUnitPrice(40, 100).Data)
            {
                Console.WriteLine($"{product.ProductName}---{product.UnitPrice}---{product.UnitsInStock}---{product.CategoryId}");
            }
        }
    }
}
